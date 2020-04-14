using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery;
using FMLMDelivery.Classes;
using System.IO;


namespace FMLMDelivery.Classes
{
    public class Runner
    {
        private List<xDocks> xDocks;
        private List<DemandPoint> demand_Points;
        private List<xDocks> agency;
        private List<Seller> _prior_small_seller;
        private List<Seller> _prior_big_seller;
        private List<Seller> _regular_small_seller;
        private List<Seller> _regular_big_seller;
        private double total_demand;
        private List<String> partial_run_cities = new List<string>(new string[] { "ANKARA", "İSTANBUL AVRUPA", "İSTANBUL ASYA", "İZMİR", "BURSA","ANTALYA","HATAY","YALOVA" });
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

        public Runner(List<DemandPoint> _demand_points, List<xDocks> _xDocks,List<xDocks> _partial_xdocks, List<xDocks> _agency, List<Seller> prior_small, List<Seller> regular_small, List<Seller> prior_big, List<Seller> regular_big,List<Parameters> parameters,Boolean _partial_run,Boolean discrete_solution)
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
            var first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, p, false, gap);
            first_phase.Run();
            var _status_check = first_phase.Return_Status();
            while (!_status_check)
            {
                demand_covarage -= 0.01;
                first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, p, false, gap);
                first_phase.Run();
                _status_check = first_phase.Return_Status();
            }
            
            objVal = first_phase.GetObjVal();
            new_xDocks = first_phase.Return_XDock();
            stats.AddRange(first_phase.Get_Model_Stats_Info());
            var min_num = first_phase.Return_Num_Xdock();
            var opened_xDocks = first_phase.Return_Opened_xDocks();
            var assignments = first_phase.Return_Assignments();



            //Part 2 for county-xDock pair
            min_model_model = false;
            demand_weighted_model = true;
            phase_2 = true;
            first_phase = new DemandxDockModel(_demand_points, _pot_xDocks, _key, demand_weighted_model, min_model_model, demand_covarage, min_xDock_cap, phase_2, min_num, true,gap);
            first_phase.Provide_Initial_Solution(opened_xDocks,assignments);
            first_phase.Run();
            objVal = first_phase.GetObjVal();
            //xDocks are assigned
            new_xDocks = first_phase.Return_XDock();
            potential_Hubs = first_phase.Return_Potential_Hubs();
            stats.AddRange(first_phase.Get_Model_Stats_Info());
            return Tuple.Create(new_xDocks, potential_Hubs, first_phase.Get_Xdock_County_Info(),stats);
        }

        private Tuple<List<DemandPoint>, List<xDocks>> Get_City_Information(string key,bool other_cities )
        {
            var city_points = new List<DemandPoint>();
            var pot_xDock_loc = new List<xDocks>();
            if (!other_cities)
            {
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
            }
            else
            {
                for (int i = 0; i < demand_Points.Count; i++)
                {
                    var already_assigned = false;
                    for (int j = 0; j < partial_run_cities.Count; j++)
                    {
                        if (demand_Points[i].Get_City() == partial_run_cities[j])
                        {
                            already_assigned = true;
                        }
                    }
                    if (demand_Points[i].Get_Region() == key && !already_assigned)
                    {
                        city_points.Add(demand_Points[i]);
                    }
                }
                for (int j = 0; j < xDocks.Count; j++)
                {
                    var already_assigned = false;
                    for (int k = 0; k < partial_run_cities.Count; k++)
                    {
                        if (xDocks[j].Get_City() == partial_run_cities[k])
                        {
                            already_assigned = true;
                        }
                    }
                    if (xDocks[j].Get_Region() == key && !already_assigned)
                    {
                        pot_xDock_loc.Add(xDocks[j]);
                    }
                }
            }
            
            return Tuple.Create(city_points, pot_xDock_loc);
        }

        private void Partial_Run(string key, bool partial_city_run, double distance_threshold, double min_xDock_capacity, double demand_coverage, double gap)
        {
            (city_points, pot_xDock_loc) = Get_City_Information(key, partial_city_run);
            var elimination_phase = new PointEliminator(city_points, pot_xDock_loc, distance_threshold, min_xDock_capacity);
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
                if (xDocks[i].Get_District() == "ÇAYIROVA" && xDocks[i].Get_Id() == "AKSE")
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

        private List<String> Create_City_List()
        {
            var city_list = new List<String>();
            for (int i = 0; i < demand_Points.Count; i++)
            {
                if (!(city_list.Contains(demand_Points[i].Get_City())))
                {
                    city_list.Add(demand_Points[i].Get_City());
                }
            }
            return city_list;
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
            var hub_demand_covarage = 0.97;
            if (!partial_run)
            {
                if (_discrete_solution)
                {
                    for (int i = 0; i < _parameters.Count; i++)
                    {
                        if (_parameters[i].Get_Activation()) 
                        {
                            Partial_Run(_parameters[i].Get_Key(), false, _parameters[i].Get_Dist_Thres(), _parameters[i].Get_Min_Cap(), 1.0, _parameters[i].Get_Gap());
                        }
                        
                    }
                }
                else
                {
                    gap_list = new List<double>(new double[] { 0.0001, 0.001, 0.01, 0.02, 0.025 });
                    //Partial_Run("ANTALYA", false, 20, 1250, 1.0, gap_list[2]);
                    //Partial_Run("HATAY", false, 20, 1250, 1.0, gap_list[2]);
                    Partial_Run("Akdeniz", true, 30, 1250, 1.0, gap_list[2]);
                    //Partial_Run("ANKARA", false, 20, 2500, 1.0, gap_list[3]);
                    //Partial_Run("İSTANBUL AVRUPA", false, 20, 2500, 1.0, gap_list[4]);
                    //Partial_Run("İSTANBUL ASYA", false, 20, 2500, 1.0, gap_list[2]);
                    //Partial_Run("İZMİR", false, 20, 2500, 1.0, gap_list[2]);
                    //Partial_Run("BURSA", false, 20, 1250, 1.0, gap_list[2]);
                    Partial_Run("İç Anadolu", true, 30, 1250, 1.0, gap_list[2]);
                    Partial_Run("Ege", true, 30, 1250, 1.0, gap_list[2]);
                    Partial_Run("Güneydoğu Anadolu", true, 30, 1250, 1.0, gap_list[2]);
                    //Partial_Run("Karadeniz", true, 30, 1250, 0.90);
                    //Partial_Run("YALOVA", false, 30, 1250, 1.0, gap_list[2]);
                    Partial_Run("Marmara", true, 30, 1250, 1.0, gap_list[2]);
                }

                var header_xdock_demand_point = "#Xdock,xDocks İl,xDocks İlçe,xDock Mahalle,xDocks_Lat,xDokcs_long,Talep Noktası İl,Talep Noktası ilçe,Talep Noktası Mahalle,Uzaklık,İlçe_Demand";
                var write_the_xdocks = new Csv_Writer(writer_xdocks, "xDock_County", header_xdock_demand_point);
                write_the_xdocks.Write_Records();

                Modify_xDocks(new_xDocks);
                potential_hub_locations = Convert_to_Potential_Hubs(new_xDocks);
                Add_Already_Open_Main_Hubs();

                String csv_new = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_City()},{d.Get_District()},{d.Get_Id()},{d.Get_Region()},{d.If_Agency()},{d.Get_Longitude()},{d.Get_Latitude()},{d.If_Already_Opened()},{d.Get_Distance_Threshold()},{d.Get_LM_Demand()},{d.Get_FM_Demand()}"));
                System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\Temporary_xDocks.csv", csv_new, Encoding.UTF8);

                //Seller-xDock Assignment



                total_demand = 0;
                for (int i = 0; i < _prior_small_seller.Count; i++)
                {
                    total_demand += _prior_small_seller[i].Get_Demand();
                }
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
                writer_seller.AddRange(second_phase.Get_Seller_Xdock_Info());
                stats_writer.AddRange(second_phase.Get_Small_Seller_Model_Stat());
                var header = "#Xdock,xDocks İl,xDocks İlçe,xDocks Mahalle,xDocks_Lat,xDokcs_long, Seller İsmi,Seller İl,Seller İlçe,Uzaklık,Seller Gönderi Adeti";
                var writer_small_seller = new Csv_Writer(writer_seller, "Small_Seller_xdock", header);
                writer_small_seller.Write_Records();

                
                var min_model_model = true;
                var demand_weighted_model = false;
                var phase_2 = false;



                //xDock-Seller-Hub Assignment
                var third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, hub_demand_covarage, phase_2, 0);
                third_phase.Run();
                var num_clusters = third_phase.Return_num_Hubs();
                min_model_model = false;
                demand_weighted_model = true;
                phase_2 = true;
                third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, hub_demand_covarage, phase_2, num_clusters);
                third_phase.Run();
                var objVal = third_phase.GetObjVal();
                new_hubs = third_phase.Return_New_Hubs();
                assigned_big_sellers = third_phase.Return_Assigned_Big_Sellers();
                var header_hub = "#Hub,Hub İl,Hub İlçe,Hub Mahalle,Hub_Long,Hub_Lat,Type_of_Assignment,Distinct Id,İl,İlçe,Mahalle,LM Talep/Gönderi,FM Gönderi,Distance";
                var stats_header = "Part,Model,Demand Coverage,Status,Time,Gap";
                stats_writer.AddRange(third_phase.Get_Xdock_Hub_Stats());
                var writer_hub_seller = new Csv_Writer(third_phase.Get_Hub_Xdock_Seller_Info(), "Seller_xDock_Hub", header_hub);
                var stat_writer = new Csv_Writer(stats_writer, "Stats", stats_header);
                writer_hub_seller.Write_Records();
                stat_writer.Write_Records();

            }
            else
            {

                //Seller-xDock Assignment
                potential_hub_locations =Convert_to_Potential_Hubs(partial_xdocks);
                Add_Already_Open_Main_Hubs();
                total_demand = 0;
                for (int i = 0; i < _prior_small_seller.Count; i++)
                {
                    total_demand += _prior_small_seller[i].Get_Demand();
                }
                var second_phase = new SmallSellerxDockModel(_prior_small_seller, partial_xdocks, true);
                second_phase.Run();
                assigned_prior_sellers = second_phase.Return_Assigned_Seller();
                partial_xdocks = second_phase.Return_Updated_xDocks();
                var covered_demand = second_phase.Return_Covered_Demand();
                var remaining_demand = total_demand - covered_demand;
                writer_seller.AddRange(second_phase.Get_Seller_Xdock_Info());
                stats_writer.AddRange(second_phase.Get_Small_Seller_Model_Stat());

                second_phase = new SmallSellerxDockModel(_regular_small_seller, partial_xdocks, false, remaining_demand);
                second_phase.Run();
                assigned_regular_sellers = second_phase.Return_Assigned_Seller();
                partial_xdocks = second_phase.Return_Updated_xDocks();
                var cov_demand = second_phase.Return_Covered_Demand();
                writer_seller.AddRange(second_phase.Get_Seller_Xdock_Info());
                stats_writer.AddRange(second_phase.Get_Small_Seller_Model_Stat());
                var header = "#Xdock,xDocks İl,xDocks İlçe,xDocks Mahalle,xDocks_Lat,xDokcs_long, Seller İsmi,Seller İl,Seller İlçe,Uzaklık,Seller Gönderi Adeti";
                var writer_small_seller = new Csv_Writer(writer_seller, "Small_Seller_xdock", header);
                writer_small_seller.Write_Records();
                
                var min_model_model = true;
                var demand_weighted_model = false;
                var phase_2 = false;


                new_xDocks = partial_xdocks;
                //xDock-Seller-Hub Assignment
                var third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, hub_demand_covarage, phase_2, 0);
                third_phase.Run();
                var num_clusters = third_phase.Return_num_Hubs();
                min_model_model = false;
                demand_weighted_model = true;
                phase_2 = true;
                third_phase = new xDockHubModel(new_xDocks, potential_hub_locations, _prior_big_seller, demand_weighted_model, min_model_model, hub_demand_covarage, phase_2, num_clusters);
                third_phase.Run();
                var objVal = third_phase.GetObjVal();
                new_hubs = third_phase.Return_New_Hubs();
                assigned_big_sellers = third_phase.Return_Assigned_Big_Sellers();
                var header_hub = "#Hub,Hub İl,Hub İlçe,Hub Mahalle,Hub_Long,Hub_Lat,Type_of_Assignment,Distinct Id,İl,İlçe,Mahalle,LM Talep/Gönderi,FM Gönderi,Distance";
                var stats_header = "Part,Model,Status,Time,Gap";
                stats_writer.AddRange(third_phase.Get_Xdock_Hub_Stats());
                var writer_hub_seller = new Csv_Writer(third_phase.Get_Hub_Xdock_Seller_Info(), "Seller_xDock_Hub", header_hub);
                var stat_writer = new Csv_Writer(stats_writer, "Stats", stats_header);
                writer_hub_seller.Write_Records();
                stat_writer.Write_Records();
            }
            
            //Seller-xDock Assignment
            
            String csv7 = String.Join(Environment.NewLine, new_hubs.Select(d => $"{d.Get_Id()},{d.Get_City()},{d.Get_District()},{d.Get_Latitude()},{d.Get_Longitude()},{d.Get_LM_Capacity()},{d.Get_FM_Capacity()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\new_hubs.csv", csv7, Encoding.UTF8);

            String csv2 = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_Id()},{d.Get_City()},{d.Get_District()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_LM_Demand()},{d.Get_FM_Demand()},{d.If_Already_Opened()},{d.If_Agency()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\new_XDocks.csv", csv2, Encoding.UTF8);

            String csv3 = String.Join(Environment.NewLine, assigned_prior_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\assigned_prior_small_sellers.csv", csv3, Encoding.UTF8);

            String csv4 = String.Join(Environment.NewLine, assigned_regular_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\assigned_regular_small_sellers.csv", csv4, Encoding.UTF8);

            String csv5 = String.Join(Environment.NewLine, assigned_big_sellers.Select(d => $"{d.Get_Name()},{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"C:\Workspace\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\Output\assigned_big_sellers.csv", csv5, Encoding.UTF8);


            Console.WriteLine("Hello World!");


                return Tuple.Create(new_xDocks, new_hubs);

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
 