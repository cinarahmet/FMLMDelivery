﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery;
using FMLMDelivery.Classes;
using System.IO;
using FMLMDelivery.MetaHeuristics;

namespace FMLMDelivery.Classes
{
    public class Runner
    {
        private Boolean is_genetic = true;
        private List<xDocks> xDocks;
        private List<DemandPoint> demand_Points;
        private List<xDocks> agency;
        private List<Seller> _prior_small_seller;
        private List<Seller> _prior_big_seller;
        private List<Seller> _regular_small_seller;
        private List<Seller> _regular_big_seller;
        private double total_demand;
        private List<String> writer_seller = new List<String>();
        private List<xDocks>  new_xDocks = new List<xDocks>();
        private List<Hub> potential_hub_locations = new List<Hub>();
        private List<String> writer_xdocks = new List<string>();
        private List<DemandPoint> city_points = new List<DemandPoint>();
        private List<xDocks> pot_xDock_loc = new List<xDocks>();
        private List<String> temp_writer = new List<String>();
        private List<xDocks> temp_xDocks = new List<xDocks>();
        private List<Hub> temp_hubs = new List<Hub>();       
        private List<String> temp_stats = new List<String>();
        private List<String> stats_writer = new List<String>();
        private List<Double> gap_list = new List<double>();
        private List<xDocks> partial_xdocks = new List<xDocks>();
        private Boolean partial_run = new Boolean();
        private List<Parameters> _parameters;
        private Boolean _discrete_solution;
        private String _output_files;
        private double _hub_demand_coverage;
        private bool _only_cities;

        public Runner(List<DemandPoint> _demand_points, List<xDocks> _xDocks,List<xDocks> _partial_xdocks, List<xDocks> _agency, List<Seller> prior_small, List<Seller> regular_small, List<Seller> prior_big, List<Seller> regular_big,List<Parameters> parameters,Boolean _partial_run,Boolean discrete_solution, string Output_files,double hub_demand_coverage,Boolean only_cities)
        {
            partial_xdocks=_partial_xdocks;
            xDocks = _xDocks;
            demand_Points = _demand_points;
            agency = _agency;
            _prior_big_seller = prior_big;
            _prior_small_seller = prior_small;
            _regular_big_seller = regular_big;
            _regular_small_seller = regular_small;
            _parameters = parameters;
            partial_run = _partial_run;
            _discrete_solution = discrete_solution;
            _output_files = Output_files;
            _hub_demand_coverage = hub_demand_coverage;
            _only_cities = only_cities;
        }

        private Tuple<List<xDocks>, List<Hub>, List<String>,List<String>> Run_Demand_Point_xDock_Model(List<DemandPoint> demandPoints, List<xDocks> xDocks,Double demand_cov, Double min_xDock_cap, String key,double gap)
        {   var stats = new List<String>();
            var _demand_points = demandPoints;
            var _pot_xDocks = xDocks;
            var _key = key;
            var min_model_model = true;
            var demand_weighted_model = false;
            //Phase 2 takes the solution of min_model as an input and solve same question with demand weighted objective
            var phase_2 = false;
            var demand_covarage = demand_cov;
            var objVal = 0.0;
            var new_xDocks = new List<xDocks>();
            var potential_Hubs = new List<Hub>(); 
            var p = 0;
            var first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, p,false, gap, 3600);
            first_phase.Run();
            var _status_check = first_phase.Return_Status();
            while (!_status_check)
            {
                demand_covarage -= 0.01;
                first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, p, false, gap,3600);
                first_phase.Run();
                _status_check = first_phase.Return_Status();
            }
            
            objVal = first_phase.GetObjVal(); 
            new_xDocks = first_phase.Return_XDock();
            stats.AddRange(first_phase.Get_Model_Stats_Info());
            var min_num = first_phase.Return_Num_Xdock();
            var opened_xDocks = first_phase.Return_Opened_xDocks();
            var assignments = first_phase.Return_Assignments();
            var heuristic_assignments = first_phase.Return_Heuristic_Assignment();


            //if (is_genetic)
            //{
            //    var heuristic = new Genetic_Algorithm(opened_xDocks, _pot_xDocks, _demand_points, _parameters, demand_covarage, min_num, key);
            //    heuristic.Run();
            //    (opened_xDocks, assignments) = heuristic.Return_Best_Solution();
            //    demand_covarage = heuristic.Return_Covered_Demand();
            //}

            //if (!is_genetic)
            //{
            //    var heuristic1 = new Simulated_Annealing(opened_xDocks, _pot_xDocks, _demand_points, _parameters, demand_covarage, min_num, key);
            //    heuristic1.Run();
            //    (opened_xDocks, assignments, demand_covarage) = heuristic1.Return_Heuristic_Results();
            //}

            //var heuristic_particle = new Particle_Swarm(opened_xDocks, _pot_xDocks, _demand_points, _parameters, demand_covarage, min_num, key);
            //heuristic_particle.Run();

            //Loop_For_Simulated_Annealing(opened_xDocks, _demand_points, _pot_xDocks, demand_covarage, min_xDock_cap, key, gap,min_num);

            //Part 2 for county-xDock pair
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, min_num, true,gap,3600,false,true);
            first_phase.Provide_Initial_Solution(opened_xDocks, assignments);
            first_phase.Run();
            objVal = first_phase.GetObjVal();
            //xDocks are assigned
            new_xDocks = first_phase.Return_XDock();  
            potential_Hubs = first_phase.Return_Potential_Hubs();
            stats.AddRange(first_phase.Get_Model_Stats_Info());


            return Tuple.Create(new_xDocks, potential_Hubs, first_phase.Get_Xdock_County_Info(),stats);
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

        private void Partial_Run(string key, double min_xDock_capacity, double demand_coverage, double gap)
        {
            (city_points, pot_xDock_loc) = Get_City_Information(key);
            var elimination_phase = new PointEliminator(city_points, pot_xDock_loc, min_xDock_capacity);
            elimination_phase.Run();
            pot_xDock_loc = elimination_phase.Return_Filtered_xDocx_Locations();
            (temp_xDocks, temp_hubs, temp_writer, temp_stats) = Run_Demand_Point_xDock_Model(city_points, pot_xDock_loc, demand_coverage, min_xDock_capacity, key, gap);
            stats_writer.AddRange(temp_stats);
            new_xDocks.AddRange(temp_xDocks);
            potential_hub_locations.AddRange(temp_hubs);
            writer_xdocks.AddRange(temp_writer);
        }

        private void Add_Already_Open_Main_Hubs()
        {
            for (int i = 0; i < xDocks.Count; i++)
            {
                if (xDocks[i].Get_District() == "TUZLA" && xDocks[i].Get_Id() == "KİRAZ")
                {
                    var city = xDocks[i].Get_City();
                    var district = xDocks[i].Get_District();
                    var id = xDocks[i].Get_Id();
                    var region = xDocks[i].Get_Region();
                    var longitude = xDocks[i].Get_Longitude();
                    var latitude = xDocks[i].Get_Latitude();
                    var dist_thres = xDocks[i].Get_Distance_Threshold();
                    var capacity = 1000000;
                    var already_opened = true;
                    var open_hub = new Hub(city, district, id, region, longitude, latitude, dist_thres, capacity, already_opened);
                    potential_hub_locations.Add(open_hub);

                }
                else if (xDocks[i].Get_District() == "YÜREĞİR" && xDocks[i].Get_Id() == "İNCİRLİK CUMHURİYET")
                {
                    var city = xDocks[i].Get_City();
                    var district = xDocks[i].Get_District();
                    var id = xDocks[i].Get_Id();
                    var region = xDocks[i].Get_Region();
                    var longitude = xDocks[i].Get_Longitude();
                    var latitude = xDocks[i].Get_Latitude();
                    var dist_thres = xDocks[i].Get_Distance_Threshold();
                    var capacity = 400000;
                    var already_opened = true;
                    var open_hub = new Hub(city, district, id, region, longitude, latitude, dist_thres, capacity, already_opened);
                    potential_hub_locations.Add(open_hub);
                }
                else if (xDocks[i].Get_District() == "BAŞAKŞEHİR" && xDocks[i].Get_Id() == "İKİTELLİ OSB")
                {
                    var city = xDocks[i].Get_City();
                    var district = xDocks[i].Get_District();
                    var id = xDocks[i].Get_Id();
                    var region = xDocks[i].Get_Region();
                    var longitude = xDocks[i].Get_Longitude();
                    var latitude = xDocks[i].Get_Latitude();
                    var dist_thres = xDocks[i].Get_Distance_Threshold();
                    var capacity = 200000;
                    var already_opened = true;
                    var open_hub = new Hub(city, district, id, region, longitude, latitude, dist_thres, capacity, already_opened);
                    potential_hub_locations.Add(open_hub);
                }
            }
           
        }

        private Tuple<List<Seller>,List<Seller>> Second_Phase()
        {
            total_demand = 0;
            for (int i = 0; i < _prior_small_seller.Count; i++)
            {
                total_demand += _prior_small_seller[i].Get_Demand();
            }
            var assigned_prior_sellers = new List<Seller>();
            var assigned_regular_sellers = new List<Seller>();
            var second_phase = new SmallSellerxDockModel(_prior_small_seller, new_xDocks, true);
            second_phase.Run();
            assigned_prior_sellers = second_phase.Return_Assigned_Seller();
            new_xDocks = second_phase.Return_Updated_xDocks();
            var covered_demand = second_phase.Return_Covered_Demand();
            var remaining_demand = total_demand - covered_demand;
            writer_seller.AddRange(second_phase.Get_Seller_Xdock_Info());
            stats_writer.AddRange(second_phase.Get_Small_Seller_Model_Stat());

            second_phase = new SmallSellerxDockModel(_regular_small_seller, new_xDocks, false, remaining_demand);
            second_phase.Run();
            assigned_regular_sellers = second_phase.Return_Assigned_Seller();
            new_xDocks = second_phase.Return_Updated_xDocks();
            var cov_demand = second_phase.Return_Covered_Demand();
            //writer_seller.AddRange(second_phase.Get_Seller_Xdock_Info());
            //stats_writer.AddRange(second_phase.Get_Small_Seller_Model_Stat());
            //var header = "xDocks İl,xDocks İlçe,xDocks Mahalle,xDocks Enlem,xDokcs Boylam, Seller İsmi,Seller İl,Seller İlçe,Seller Uzaklık,Seller Gönderi Adeti";
            //var writer_small_seller = new Csv_Writer(writer_seller, "Küçük Tedarikçi xDock Atamaları", header, _output_files);
            //writer_small_seller.Write_Records();

            return Tuple.Create(assigned_prior_sellers, assigned_regular_sellers);
        }

        private Tuple<List<Hub>, List<Seller>> Third_Phase()
        {
            var new_hubs = new List<Hub>();
            var assigned_big_sellers = new List<Seller>();
            var min_model_model = true;
            var demand_weighted_model = false;
            var phase_2 = false;
            //xDock-Seller-Hub Assignment
            var third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, _hub_demand_coverage, phase_2, 0);
            third_phase.Run();
            var num_clusters = third_phase.Return_num_Hubs();
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, _hub_demand_coverage, phase_2, num_clusters);
            third_phase.Run();
            var objVal = third_phase.GetObjVal();
            new_hubs = third_phase.Return_New_Hubs();
            assigned_big_sellers = third_phase.Return_Assigned_Big_Sellers();
            //var header_hub = "Hub İl,Hub İlçe,Hub Mahalle,Hub Boylam,Hub Enlem,Atanma Türü,Unique Id,İl,İlçe,Mahalle,LM Talep/Gönderi,FM Gönderi,Distance";
            //var stats_header = "Part,Model,Demand Coverage,Status,Time,Gap";
            //stats_writer.AddRange(third_phase.Get_Xdock_Hub_Stats());
            //var writer_hub_seller = new Csv_Writer(third_phase.Get_Hub_Xdock_Seller_Info(), "Büyük Tedarikçi xDock Hub Atamaları", header_hub, _output_files);
            //var stat_writer = new Csv_Writer(stats_writer, "Statü", stats_header, _output_files);
            //writer_hub_seller.Write_Records();
            //stat_writer.Write_Records();

            return Tuple.Create(new_hubs, assigned_big_sellers);
        }

        public Tuple<List<xDocks>, List<Hub>> Run()
        {   var new_hubs = new List<Hub>();
            var assigned_prior_sellers = new List<Seller>();
            var assigned_regular_sellers = new List<Seller>();
            var assigned_big_sellers = new List<Seller>();
            /* This method firstly calls Demand-xDock model with the minimum xDock objective in given demand covarage. After solving the model with this object, the method takes the number of xDock
             * and re-solved the model with demand-distance weighted objective given the number of xDocks and identifies the optimal locations for xDocks. After xDocks are identified, xDock-Hub model
             * is called with the minimum hub objective and after the model is solved, with the given numer of hub the model is resolved in order to obtain demand-distance weighted locations for hubs. 
             */
            
            if (!partial_run)
            {
                for (int i = 0; i < _parameters.Count; i++)
                {
                    if (_parameters[i].Get_Activation())
                    {
                        Partial_Run(_parameters[i].Get_Key(), _parameters[i].Get_Min_Cap(), 1.0, Gap_Converter_1(_parameters[i].Get_Size()));
                    }
                }
                var header_xdock_demand_point = "xDocks İl,xDocks İlçe,xDock Mahalle,xDocks Enlem,xDokcs Boylam,Talep Noktası İl,Talep Noktası ilçe,Talep Noktası Mahalle,Uzaklık,Talep Noktası Talebi";
                var write_the_xdocks = new Csv_Writer(writer_xdocks, "Mahalle xDock Atamaları", header_xdock_demand_point,_output_files);
                write_the_xdocks.Write_Records();

                String csv_new = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_City()},{d.Get_District()},{d.Get_Id()},{d.Get_Region()},{d.If_Agency()},{d.Get_Longitude()},{d.Get_Latitude()},{d.If_Already_Opened()},{d.Get_Distance_Threshold()},{d.Get_LM_Demand()},{d.Get_FM_Demand()}"));
                System.IO.File.WriteAllText(@"" + _output_files + "\\Kısmi Çalıştırma Dosyası.csv", csv_new, Encoding.UTF8);

                Modify_xDocks(new_xDocks);
                potential_hub_locations = Convert_to_Potential_Hubs(new_xDocks);
                Add_Already_Open_Main_Hubs();

                //Seller-xDock Assignment
                (assigned_prior_sellers,assigned_regular_sellers) = Second_Phase();
                if (!_only_cities)
                {
                    //xDock-Seller-Hub Assignment
                    (new_hubs, assigned_big_sellers) = Third_Phase();
                }
            }
            else
            {
                potential_hub_locations = Convert_to_Potential_Hubs(partial_xdocks);
                Add_Already_Open_Main_Hubs();
                new_xDocks = partial_xdocks;
                //Seller-xDock Assignment
                (assigned_prior_sellers, assigned_regular_sellers) = Second_Phase();
                //xDock-Seller-Hub Assignment
                (new_hubs, assigned_big_sellers) = Third_Phase();
            }

            if (!_only_cities)
            {
                //Seller-xDock Assignment
                string[] new_hubs_headers = { "İl", "İlçe", "Mahalle", "Boylam", "Enlem", "LM Talep", "FM Gönderi" };
                String headers_hub = String.Join(",", new_hubs_headers) + Environment.NewLine;
                String csv7 = headers_hub + String.Join(Environment.NewLine, new_hubs.Select(d => $"{d.Get_City()},{d.Get_District()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_LM_Capacity()},{d.Get_FM_Capacity()}"));
                System.IO.File.WriteAllText(@"" + _output_files + "\\Açılmış Hublar Listesi.csv", csv7, Encoding.UTF8);

                string[] big_seller = { "İsim", "ID", "Boylam", "Enlem", "Gönderi" };
                String big_s = String.Join(",", big_seller) + Environment.NewLine;
                String csv5 = big_s + String.Join(Environment.NewLine, assigned_big_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
                System.IO.File.WriteAllText(@"" + _output_files + "\\Atanmış Büyük Tedarikçiler.csv", csv5, Encoding.UTF8);

            }

            string[] new_xdocks_headers_2 = { "İl", "İlçe", "Mahalle", "Boylam", "Enlem", "LM Talep", "FM Gönderi","Önceden Açılmış","Acente"};
            String headers_xdock_2 = String.Join(",", new_xdocks_headers_2) + Environment.NewLine;
            String csv2 = headers_xdock_2 + String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_City()},{d.Get_District()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_LM_Demand()},{d.Get_FM_Demand()},{d.If_Already_Opened()},{d.If_Agency()}"));
            System.IO.File.WriteAllText(@"" + _output_files +"\\Açılmış xDocklar Listesi.csv", csv2, Encoding.UTF8);


            string[] prior_small_seller = { "İsim", "ID", "Boylam", "Enlem", "Gönderi"};
            String p_small = String.Join(",", prior_small_seller) + Environment.NewLine;
            String csv3 = p_small + String.Join(Environment.NewLine, assigned_prior_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"" + _output_files +"\\Atanmış Öncelikli Küçük Tedarikçiler.csv", csv3, Encoding.UTF8);


            string[] regular_small_seller = { "İsim", "ID", "Boylam", "Enlem", "Gönderi" };
            String r_small = String.Join(",", regular_small_seller) + Environment.NewLine;
            String csv4 = r_small + String.Join(Environment.NewLine, assigned_regular_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"" + _output_files +"\\Atanmış Sıradan Küçük Tedarikçiler.csv", csv4, Encoding.UTF8);


           

            Console.WriteLine("Hello World!");


                return Tuple.Create(new_xDocks, new_hubs);

        }
        private Double Gap_Converter_1(String Size)
        {   var gap = 0.0;
            if (Size == "Small")
            {
                gap = 0.01;
            }
            else
            {
                gap = 0.025;
            }

            return gap;
        }
        
        private List<Hub> Convert_to_Potential_Hubs(List<xDocks> new_XDocks)
        {
            var potential_Hubs = new List<Hub>();
            for (int i = 0; i < new_XDocks.Count; i++)
            {
                var city = new_XDocks[i].Get_City();
                var district = new_XDocks[i].Get_District();
                var id = new_XDocks[i].Get_Id();
                var region = new_XDocks[i].Get_Region();
                var longitude = new_XDocks[i].Get_Longitude();
                var latitude = new_XDocks[i].Get_Latitude();
                var dist_thres = new_XDocks[i].Get_Distance_Threshold();
                var capacity = 4000000;
                var already_opened = false;
                var potential_hub = new Hub(city, district, id, region, longitude, latitude, dist_thres, capacity, already_opened);
                potential_Hubs.Add(potential_hub);
            }

            return potential_Hubs;
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
 