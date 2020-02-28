using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery;
using FMLMDelivery.Classes;
using System.IO;


namespace FMLMDelivery
{
    class Runner
    {
        private List<xDocks> xDocks;
        private List<DemandPoint> demand_Points;
        private List<xDocks> agency;
        private List<Seller> _prior_small_seller;
        private List<Seller> _prior_big_seller;
        private List<Seller> _regular_small_seller;
        private List<Seller> _regular_big_seller;
        private double total_demand;


        public Runner(List<DemandPoint> _demand_points, List<xDocks> _xDocks, List<xDocks> _agency, List<Seller> prior_small, List<Seller> regular_small, List<Seller> prior_big, List<Seller> regular_big)
        {
            xDocks = _xDocks;
            demand_Points = _demand_points;
            agency = _agency;
            _prior_big_seller = prior_big;
            _prior_small_seller = prior_small;
            _regular_big_seller = regular_big;
            _regular_small_seller = regular_small; 
        }

        private Tuple<List<xDocks>, List<Hub>> Run_Demand_Point_xDock_Model(List<DemandPoint> demandPoints, List<xDocks> xDocks)
        {
            var _demand_points = demandPoints;
            var _pot_xDocks = xDocks;
            var min_model_model = true;
            var demand_weighted_model = false;
            //Phase 2 takes the solution of min_model as an input and solve same question with demand weighted objective
            var phase_2 = false;
            var demand_covarage = 0.88;
            var objVal = 0.0;
            var new_xDocks = new List<xDocks>();
            var potential_Hubs = new List<Hub>();
            var p = 0;
            var first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, demand_weighted_model, min_model_model, demand_covarage, phase_2, p, false);

            first_phase.Run();
            objVal = first_phase.GetObjVal();
            new_xDocks = first_phase.Return_XDock();
            var min_num = first_phase.Return_Num_Xdock();
            var opened_xDocks = first_phase.Return_Opened_xDocks();



            //Part 2 for county-xDock pair
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, demand_weighted_model, min_model_model, demand_covarage, phase_2, min_num, true);
            first_phase.Provide_Initial_Solution(opened_xDocks);
            first_phase.Run();
            objVal = first_phase.GetObjVal();
            //xDocks are assigned
            new_xDocks = first_phase.Return_XDock();
            Modify_xDocks(new_xDocks);
            potential_Hubs = first_phase.Return_Potential_Hubs();

            return Tuple.Create(new_xDocks, potential_Hubs);
        }

        private Tuple<List<DemandPoint>, List<xDocks>> Get_City_Information(string key)
        {
            var city_points = new List<DemandPoint>();
            var pot_xDock_loc = new List<xDocks>();
            for (int i = 0; i < demand_Points.Count; i++)
            {
                if (demand_Points[i].Get_City() == key)
                {
                    city_points.Add(demand_Points[i]);
                }
            }
            for (int j = 0; j < xDocks.Count; j++)
            {
                if (xDocks[j].Get_City() == key)
                {
                    pot_xDock_loc.Add(xDocks[j]);
                }
            }

            return Tuple.Create(city_points, pot_xDock_loc);
        }

        public Tuple<List<xDocks>, List<Hub>> Run()
        {
            /* This method firstly calls Demand-xDock model with the minimum xDock objective in given demand covarage. After solving the model with this object, the method takes the number of xDock
             * and re-solved the model with demand-distance weighted objective given the number of xDocks and identifies the optimal locations for xDocks. After xDocks are identified, xDock-Hub model
             * is called with the minimum hub objective and after the model is solved, with the given numer of hub the model is resolved in order to obtain demand-distance weighted locations for hubs. 
             */

            var elimination_phase = new PointEliminator(demand_Points, xDocks, 30, 1250);
            elimination_phase.Run();
            var eliminated_demand_points = elimination_phase.Return_Candidate_Demand_Points();

            var new_xDocks = new List<xDocks>();
            var potential_hub_locations = new List<Hub>();
            var key = "İSTANBUL";
            var city_points = new List<DemandPoint>();
            var pot_xDock_loc = new List<xDocks>();

            (city_points, pot_xDock_loc) = Get_City_Information(key);
            (new_xDocks, potential_hub_locations) = Run_Demand_Point_xDock_Model(city_points, pot_xDock_loc);

            var temp_xDocks = new List<xDocks>();
            var temp_hubs = new List<Hub>();

            key = "ANKARA";
            (city_points, pot_xDock_loc) = Get_City_Information(key);
            (temp_xDocks, temp_hubs) = Run_Demand_Point_xDock_Model(city_points, pot_xDock_loc);
            new_xDocks.AddRange(temp_xDocks);
            potential_hub_locations.AddRange(temp_hubs);

            key = "İZMİR";
            (city_points, pot_xDock_loc) = Get_City_Information(key);
            (temp_xDocks, temp_hubs) = Run_Demand_Point_xDock_Model(city_points, pot_xDock_loc);
            new_xDocks.AddRange(temp_xDocks);
            potential_hub_locations.AddRange(temp_hubs);





            //Seller-xDock Assignment
            total_demand = 0;
            for (int i = 0; i < _prior_small_seller.Count; i++)
            {
                total_demand += _prior_small_seller[i].Get_Demand();
            }
            var second_phase = new SmallSellerxDockModel(_prior_small_seller, new_xDocks, true);
            second_phase.Run();
            var assigned_prior_sellers = second_phase.Return_Assigned_Seller();
            var covered_demand = second_phase.Return_Covered_Demand();
            var remaining_demand = total_demand - covered_demand;

            second_phase = new SmallSellerxDockModel(_regular_small_seller, new_xDocks, false, remaining_demand);
            second_phase.Run();
            var assigned_regular_sellers = second_phase.Return_Assigned_Seller();
            var cov_demand = second_phase.Return_Covered_Demand();


            var demand_covarage = 0.90;
            var min_model_model = true;
            var demand_weighted_model = false;
            var phase_2 = false;

            //xDock-Seller-Hub Assignment
            var third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, demand_covarage, phase_2, 0);
            third_phase.Run();
            var num_clusters = third_phase.Return_num_Hubs();
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, demand_covarage, phase_2, num_clusters);
            third_phase.Run();
            var objVal = third_phase.GetObjVal();
            var new_hubs = third_phase.Return_New_Hubs();
            var assigned_big_sellers = third_phase.Return_Assigned_Big_Sellers();

            String csv = String.Join(Environment.NewLine, potential_hub_locations.Select(d => $"{d.Get_Id()};{d.Get_Capacity()};{d.Get_Latitude()};{d.Get_Longitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\new_hubs.csv", csv, Encoding.UTF8);

            String csv2 = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_Id()};{d.Get_Demand()};{d.Get_Longitude()};{d.Get_Latitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\new_XDocks.csv", csv2, Encoding.UTF8);

            String csv3 = String.Join(Environment.NewLine, assigned_prior_sellers.Select(d => $"{d.Get_Id()};{d.Get_Demand()};{d.Get_Longitude()};{d.Get_Latitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\assigned_prior_small_sellers.csv", csv3, Encoding.UTF8);

            String csv4 = String.Join(Environment.NewLine, assigned_regular_sellers.Select(d => $"{d.Get_Id()};{d.Get_Demand()};{d.Get_Longitude()};{d.Get_Latitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\assigned_regular_small_sellers.csv", csv4, Encoding.UTF8);

            String csv5 = String.Join(Environment.NewLine, assigned_big_sellers.Select(d => $"{d.Get_Id()};{d.Get_Demand()};{d.Get_Longitude()};{d.Get_Latitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\assigned_big_sellers.csv", csv5, Encoding.UTF8);


            Console.WriteLine("Hello World!");


            return Tuple.Create(new_xDocks, new_hubs);

        }

        private void Modify_xDocks(List<xDocks> new_xDocks)
        {
            for (int i = 0; i < agency.Count; i++)
            {
                new_xDocks.Add(agency[i]);
            }
        }
    }
}
