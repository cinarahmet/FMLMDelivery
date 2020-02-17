using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery;
using FMLMDelivery.Classes;

namespace FMLMDelivery
{
    class Runner
    {
        private List<xDocks> xDocks;
        private List<DemandPoint> county;
        private List<xDocks> agency;
        private List<Seller> _prior_small_seller;
        private List<Seller> _prior_big_seller;
        private List<Seller> _regular_small_seller;
        private List<Seller> _regular_big_seller;
        private double total_demand;


        public Runner(List<DemandPoint> _counties, List<xDocks> _xDocks, List<xDocks> _agency, List<Seller> prior_small, List<Seller> regular_small, List<Seller> prior_big, List<Seller> regular_big)
        {
            xDocks = _xDocks;
            county = _counties;
            agency = _agency;
            _prior_big_seller = prior_big;
            _prior_small_seller = prior_small;
            _regular_big_seller = regular_big;
            _regular_small_seller = regular_small;
        }

       

        public Tuple<List<xDocks>,List<Hub>> Run()
        {
            /* This method firstly calls Demand-xDock model with the minimum xDock objective in given demand covarage. After solving the model with this object, the method takes the number of xDock
             * and re-solved the model with demand-distance weighted objective given the number of xDocks and identifies the optimal locations for xDocks. After xDocks are identified, xDock-Hub model
             * is called with the minimum hub objective and after the model is solved, with the given numer of hub the model is resolved in order to obtain demand-distance weighted locations for hubs. 
             */
            var min_model_model = true;
            var demand_weighted_model = false;
            //Phase 2 takes the solution of min_model as an input and solve same question with demand weighted objective
            var phase_2 = false;
            var demand_covarage = 0.95;
            var objVal = 0.0;
            var new_xDocks = new List<xDocks>();
            var potential_Hubs = new List<Hub>();
            var p = 0;
            var first_phase = new DemandxDockModel(county, xDocks, demand_weighted_model, min_model_model, demand_covarage, phase_2, p);

            first_phase.Run();
            objVal = first_phase.GetObjVal();
            new_xDocks = first_phase.Return_XDock();
            var min_num = first_phase.Return_Num_Xdock();
            

            //Part 2 for county-xDock pair
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            first_phase = new DemandxDockModel(county, xDocks, demand_weighted_model, min_model_model, demand_covarage, phase_2, min_num);
            first_phase.Run();
            objVal = first_phase.GetObjVal();
            //xDocks are assigned
            new_xDocks = first_phase.Return_XDock();
            Modify_xDocks(new_xDocks);
            potential_Hubs = first_phase.Return_Potential_Hubs();


            //Seller-xDock Assignment
            total_demand = 0;
            for (int i = 0; i < _prior_small_seller.Count; i++)
            {
                total_demand += _prior_small_seller[i].Get_Demand();
            }

            var second_phase = new SmallSellerxDockModel(_prior_small_seller,new_xDocks,true);
            second_phase.Run();
            var assigned_prior_sellers = second_phase.Return_Assigned_Seller();
            var covered_demand = second_phase.Return_Covered_Demand();
            var remaining_demand = total_demand - covered_demand;

            second_phase = new SmallSellerxDockModel(_regular_small_seller, new_xDocks, false, remaining_demand);
            second_phase.Run();
            var assigned_regular_sellers = second_phase.Return_Assigned_Seller();
            var cov_demand = second_phase.Return_Covered_Demand();


            demand_covarage = 0.90;
            min_model_model = true;
            demand_weighted_model = false;
            phase_2 = false;

            //xDock-Seller-Hub Assignment
            var third_phase = new xDockHubModel(new_xDocks, potential_Hubs, _prior_big_seller, demand_weighted_model, min_model_model, demand_covarage, phase_2, p);
            third_phase.Run();
            var num_clusters = third_phase.Return_num_Hubs();
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            third_phase = new xDockHubModel(new_xDocks, potential_Hubs, _prior_big_seller, demand_weighted_model, min_model_model, demand_covarage, phase_2, num_clusters);
            third_phase.Run();
            objVal = third_phase.GetObjVal();
            var new_hubs = third_phase.Return_New_Hubs();
            
            

            String csv = String.Join(Environment.NewLine, new_hubs.Select(d => $"{d.Get_Latitude()};{d.Get_Longitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\test2.csv", csv);

            String csv2 = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_Id()};{d.Get_Demand()};{d.Get_Longitude()};{d.Get_Latitude()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\new_XDocks.csv", csv2);
            
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
