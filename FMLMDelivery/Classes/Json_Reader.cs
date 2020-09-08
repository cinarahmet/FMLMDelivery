﻿using ChoETL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FMLMDelivery.Classes
{   
    public class Json_Reader
    {   
        /// <summary>
        /// Json input of all the input files and parameters converted to dictionary
        /// </summary>
        private Dictionary<String, String[]> total_json_input = new Dictionary<string, string[]>();
        /// <summary>
        /// 
        /// </summary>
        private string _Json_Path;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<xDocks, List<Mahalle>> _xDock_neighborhood_assignments = new Dictionary<xDocks, List<Mahalle>>();
        /// <summary>
        /// 
        /// </summary>
        private List<DemandPoint> _demand_point = new List<DemandPoint>();
        /// <summary>
        /// 
        /// </summary>
        private List<xDocks> _potential_xdocks = new List<xDocks>();
        /// <summary>
        /// 
        /// </summary>
        private List<xDocks> _partial_xdocks = new List<xDocks>();
        /// <summary>
        /// 
        /// </summary>
        private List<Seller> _regular_small_seller = new List<Seller>();
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Seller> _prior_big_seller = new List<Seller>();
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Seller> _prior_small_seller = new List<Seller>();
        /// <summary>
        /// 
        /// </summary>
        private List<Seller> _regular_big_seller = new List<Seller>();
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Seller> _total_seller = new List<Seller>();
        /// <summary>
        /// 
        /// </summary>
        private List<Parameters> _parameters = new List<Parameters>();
        /// <summary>
        /// 
        /// </summary>
        private int month_to_run;
        /// <summary>
        /// 
        /// </summary>
        private List<Boolean> run_type = new List<Boolean>();
        /// <summary>
        /// 
        /// </summary>
        private List<Double> courier_parameters = new List<Double>();
        /// <summary>
        /// 
        /// </summary>
        private double hub_coverage;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, Double> region_county_threshold = new Dictionary<string, double>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, Double> region_xDock_threshold = new Dictionary<string, double>();
        /// <summary>
        /// 
        /// </summary>
        private Double scope_out_threshold = 10;

        public Json_Reader(String Json_Path)
        {
            _Json_Path = Json_Path;

        }
        private void Create_Demand_Point_Region_Threshold()
        {
            var s_1 = "Akdeniz";
            var value = 30;
            var s_2 = "Marmara";
            var value_2 = 30;
            var s_3 = "İç Anadolu";
            var value_3 = 30;
            var s_4 = "Ege";
            var value_4 = 30;
            var s_5 = "Güneydoğu";
            var value_5 = 30;
            var s_6 = "Karadeniz";
            var value_6 = 30;
            var s_7 = "Güneydoğu Anadolu";
            var value_7 = 30;
            var s_8 = "Doğu Anadolu";
            var value_8 = 30;
            region_county_threshold.TryAdd(s_1, value);
            region_county_threshold.TryAdd(s_2, value_2);
            region_county_threshold.TryAdd(s_3, value_3);
            region_county_threshold.TryAdd(s_4, value_4);
            region_county_threshold.TryAdd(s_5, value_5);
            region_county_threshold.TryAdd(s_6, value_6);
            region_county_threshold.TryAdd(s_7, value_7);
            region_county_threshold.TryAdd(s_8, value_8);
        }

        private void Create_xDock_Region_Threshold()
        {
            var s_1 = "Akdeniz";
            var value = 180;
            var s_2 = "Marmara";
            var value_2 = 150;
            var s_3 = "İç Anadolu";
            var value_3 = 180;
            var s_4 = "Ege";
            var value_4 = 220;
            var s_5 = "Güneydoğu";
            var value_5 = 200;
            var s_6 = "Karadeniz";
            var value_6 = 200;
            var s_7 = "Güneydoğu Anadolu";
            var value_7 = 200;
            var s_8 = "Doğu Anadolu";
            var value_8 = 200;
            region_xDock_threshold.TryAdd(s_1, value);
            region_xDock_threshold.TryAdd(s_2, value_2);
            region_xDock_threshold.TryAdd(s_3, value_3);
            region_xDock_threshold.TryAdd(s_4, value_4);
            region_xDock_threshold.TryAdd(s_5, value_5);
            region_xDock_threshold.TryAdd(s_6, value_6);
            region_xDock_threshold.TryAdd(s_7, value_7);
            region_xDock_threshold.TryAdd(s_8, value_8);
        }
        public void Convert_To_Dictionary(String file_path)
        {
            string json = "";
            using (StreamReader r = new StreamReader(file_path))
            {
                json = r.ReadToEnd();
            }
            total_json_input = JsonConvert.DeserializeObject<Dictionary<String, String[]>>(json);
        }

        private void Create_Parameters_To_Run()
        {
            month_to_run = (int)Int64.Parse(total_json_input["Run Month"].ToList()[0]);
            hub_coverage= Convert.ToDouble(total_json_input["Hub Coverage"].ToList()[0]);
            courier_parameters = total_json_input["Courier Parameters"].ToList().Select(x => double.Parse(x)).ToList();
            run_type = total_json_input["Run Type"].ToList().Select(x => Boolean.Parse(x)).ToList();
        }

        private void Create_Demand_List(int month)
        {
            string[] demand_list;
            if(total_json_input.ContainsKey("Demand Points"))
            {
                demand_list = total_json_input["Demand Points"];
                demand_list = demand_list.Skip(1).ToArray();
;                foreach (var item in demand_list)
                 {
                    var line = item.Split(",");

                    var demand_point_City = line[0];
                    var demand_point_district = line[1];
                    var demand_point_ID = line[2];
                    var demand_point_region = line[3];
                    var demand_point_lat = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                    var demand_point_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                    var demand_point_dis_thres = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                    if (demand_point_dis_thres == 0.0)
                    {
                        demand_point_dis_thres = region_county_threshold[demand_point_region];
                    }
                    var demand_point_Demand = Convert.ToDouble(line[month+6]);
                    if (demand_point_Demand != 0.0)
                    {
                        demand_point_Demand = Convert.ToDouble(line[month+6]) / Math.Ceiling(Convert.ToDouble(line[month+6]) / 4000);
                    }
                    if (demand_point_Demand > 10.0)
                    {
                        var demand_point = new DemandPoint(demand_point_City, demand_point_district, demand_point_ID, demand_point_region, demand_point_long, demand_point_lat, demand_point_dis_thres, demand_point_Demand);
                        _demand_point.Add(demand_point);

                        if (Math.Ceiling(Convert.ToDouble(line[month + 6]) / 4000) > 1)
                        {
                            for (int i = 2; i <= Math.Ceiling(Convert.ToDouble(line[month + 6]) / 4000); i++)
                            {
                                var demand_point_City_ = line[0];
                                var demand_point_district_ = line[1];
                                var demand_point_ID_ = line[2] + " " + i;
                                var demand_point_Region_ = line[3];
                                var demand_point_long_ = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                                var demand_point_lat_ = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                                var demand_point_dis_thres_ = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                                var demand_point_Demand_ = Convert.ToDouble(line[month + 6]) / Math.Ceiling(Convert.ToDouble(line[month + 6]) / 4000);
                                if (demand_point_Demand_ > scope_out_threshold)
                                {
                                    var demand_point_ = new DemandPoint(demand_point_City_, demand_point_district_, demand_point_ID_, demand_point_Region_, demand_point_long_, demand_point_lat_, demand_point_dis_thres_, demand_point_Demand_);
                                    _demand_point.Add(demand_point_);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Create_Potential_xDocks(int month)
        {
            string[] pot_xdocks;
            if (total_json_input.ContainsKey("Potential_xDocks"))
            {
                pot_xdocks = total_json_input["Potential_xDocks"];
                pot_xdocks = pot_xdocks.Skip(1).ToArray();

                foreach (var item in pot_xdocks)
                {
                    var line = item.Split(",");
                    var xDock_City = line[0];
                    var xDock_District = line[1];
                    var xDock_Id = line[2];
                    var xDock_region = line[3];
                    var xDock_lat = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_dist_threshold = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_min_cap = Convert.ToDouble(line[8], System.Globalization.CultureInfo.InvariantCulture);
                    var hub_point = Convert.ToDouble(line[9], System.Globalization.CultureInfo.InvariantCulture);
                    if (hub_point == 0)
                    {
                        hub_point = 2;
                    }
                    else
                    {
                        var log_value = Math.Log(hub_point, 10);
                        hub_point = (2 - log_value);
                    }
                    if (xDock_dist_threshold == 0.0)
                    {
                        xDock_dist_threshold = region_xDock_threshold[xDock_region];
                    }
                    var Already_Opened = Convert.ToDouble(line[6]);
                    var xDock_Already_Opened = false;
                    if (Already_Opened == 1.0)
                    {
                        xDock_Already_Opened = true;
                    }
                    var xDock_Capacity = Convert.ToDouble(line[month + 9]);

                    if (xDock_Capacity > scope_out_threshold)
                    {
                        var type_value = false;
                        var xDock = new xDocks(xDock_City, xDock_District, xDock_Id, xDock_region, xDock_long, xDock_lat, xDock_dist_threshold, xDock_min_cap,hub_point ,xDock_Capacity, xDock_Already_Opened, type_value);
                        _potential_xdocks.Add(xDock);

                    }
                }
            }
        }

        private void Create_Parameters_File()
        {
            string[] parameterlist;

            if (total_json_input.ContainsKey("Parameters"))
            {
                parameterlist = total_json_input["Parameters"];
                parameterlist = parameterlist.Skip(1).ToArray();

                foreach (var item in parameterlist)
                {
                    var line = item.Split(",");

                    var distinct_city = line[0];
                    var size = line[1];
                    var active = Convert.ToBoolean(Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture));
                    var parameter = new Parameters(distinct_city, size, active);
                    _parameters.Add(parameter);
                }
            }
        }

        private void Create_Seller()
        {
            string[] sellerlist;
            if (total_json_input.ContainsKey("Sellers"))
            {
                sellerlist = total_json_input["Sellers"];
                sellerlist = sellerlist.Skip(1).ToArray();
                foreach (var item in sellerlist)
                {
                    var line = item.Split(",");

                    var seller_name = line[0];
                    var seller_id = line[1];
                    var seller_city = line[2];
                    var seller_district = line[3];
                    var seller_priority = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                    var seller_lat = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                    var seller_long = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                    var seller_demand = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                    var seller_dist = Convert.ToDouble(line[8], System.Globalization.CultureInfo.InvariantCulture);
                    var seller_size = line[9];
                    if (seller_size == "Small")
                    {
                        var small_seller = new Seller(seller_name, seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_demand, seller_dist, seller_size);
                        if (seller_priority == 1)
                        {
                            _prior_small_seller.Add(small_seller);
                        }
                        else
                        {
                            _regular_small_seller.Add(small_seller);
                        }
                    }
                    else
                    {
                        var big_seller = new Seller(seller_name, seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_demand, seller_dist, seller_size);
                        if (seller_priority == 1)
                        {
                            _prior_big_seller.Add(big_seller);
                        }
                        _regular_big_seller.Add(big_seller);
                    }
                    var total_seller = new Seller(seller_name, seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_demand, seller_dist, seller_size);
                    _total_seller.Add(total_seller);
                }
            }
        }

        private void Create_Partial_Solution_xDocks()
        {
            string[] partial_solution_xDocks;

            if (total_json_input.ContainsKey("Partial Solution xDocks"))
            {
                partial_solution_xDocks = total_json_input["Partial Solution xDocks"];
                partial_solution_xDocks = partial_solution_xDocks.Skip(1).ToArray();
                foreach (var item in partial_solution_xDocks)
                {
                    var line = item.Split(',');

                    var xDock_City = line[0];
                    var xDock_District = line[1];
                    var xDock_Id = line[2];
                    var xDock_region = line[3];
                    var type_value = false;
                    var xDock_lat = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                    var Already_Opened = Convert.ToBoolean(line[6], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_dist_threshold = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                    var xDock_min_cap = Convert.ToDouble(line[8], System.Globalization.CultureInfo.InvariantCulture);
                    var xdock_demand = Convert.ToDouble(line[9], System.Globalization.CultureInfo.InvariantCulture);
                    var empty_hub_point = 0.0;
                    var xDock = new xDocks(xDock_City, xDock_District, xDock_Id, xDock_region, xDock_long, xDock_lat, xDock_dist_threshold, xDock_min_cap,empty_hub_point,xdock_demand, Already_Opened, type_value);
                    _partial_xdocks.Add(xDock);
                }
            }
        }

        private void Create_xDocks_Neighborhood_Assignment()
        {
            string[] xdock_neighborhood;
            if (total_json_input.ContainsKey("Xdock Neighbourhood Assignments"))
            {
                xdock_neighborhood = total_json_input["Xdock Neighbourhood Assignments"];
                xdock_neighborhood = xdock_neighborhood.Skip(1).ToArray();
                foreach (var item in xdock_neighborhood)
                {
                    var line = item.Split(',');
                    if(line[0]!= "Atanmayan Talep Noktası")
                    {
                        var xdock_city = line[0];
                        var xdock_district = line[1];
                        var xdock_id = line[2];
                        var xdock_lat = Convert.ToDouble(line[3], System.Globalization.CultureInfo.InvariantCulture);
                        var xdock_long = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                        var demand_point_city = line[5];
                        var demand_point_district = line[6];
                        var demand_point_id = line[7];
                        var demand_point_lat = Convert.ToDouble(line[8], System.Globalization.CultureInfo.InvariantCulture);
                        var demand_point_long = Convert.ToDouble(line[9], System.Globalization.CultureInfo.InvariantCulture);
                        var distance_xdock_county = Convert.ToDouble(line[10], System.Globalization.CultureInfo.InvariantCulture);
                        var demand = Convert.ToDouble(line[11], System.Globalization.CultureInfo.InvariantCulture);
                        var dummy_xDock = new xDocks(xdock_city, xdock_district, xdock_id, "a", xdock_long, xdock_lat, 30, 1250,0,4000, false, false);
                        var neighborhood = new Mahalle(demand_point_id, demand_point_district, demand_point_long, demand_point_lat, demand);
                        var neighborhood_list = new List<Mahalle>();
                        var list_contains = _xDock_neighborhood_assignments.Keys.Where(x => x.Get_City() == xdock_city && x.Get_District() == xdock_district && x.Get_Id() == xdock_id).ToList();

                        if (list_contains.Count > 0)
                        {
                            _xDock_neighborhood_assignments[list_contains[0]].Add(neighborhood);

                        }
                        else
                        {
                            neighborhood_list.Add(neighborhood);
                            _xDock_neighborhood_assignments.Add(dummy_xDock, neighborhood_list);
                        }
                    }
                }
            }
        }
        public void Stable_Parameter_Read()
        {
            Create_Demand_Point_Region_Threshold();
            Create_xDock_Region_Threshold();
            Convert_To_Dictionary(_Json_Path);
            Create_Parameters_To_Run();
            Create_Demand_List(month_to_run);
            Create_Potential_xDocks(month_to_run);
            Create_Seller();
            Create_Parameters_File();
            Create_Partial_Solution_xDocks();
            Create_xDocks_Neighborhood_Assignment();

        }

        public void Different_Parameter_Read(int month)
        {
            Create_Demand_Point_Region_Threshold();
            Create_xDock_Region_Threshold();
            Convert_To_Dictionary(_Json_Path);
            Create_Parameters_To_Run();
            Create_Demand_List(month);
            Create_Potential_xDocks(month);
            Create_Seller();
            Create_Parameters_File();
            Create_Partial_Solution_xDocks();
            Create_xDocks_Neighborhood_Assignment();
        }

        public List<DemandPoint> Get_Demand_Points()
        {
            return _demand_point;
        }

        public List<xDocks> Get_Potential_xDocks()
        {
            return _potential_xdocks;

        }

        public Dictionary<xDocks,List<Mahalle>> Get_xDock_Neighborhood()
        {
            return _xDock_neighborhood_assignments;
        }
        public List<xDocks> Get_Partial_Solution_xDocks()
        {
            return _partial_xdocks;
        }

        public List<Seller> Get_Prior_Small_Sellers()
        {
            return _prior_small_seller;
        }

        public List<Seller> Get_Regular_Small_Sellers()
        {
            return _regular_small_seller;
        }

        public List<Seller> Get_Prior_Big_Sellers()
        {
            return _prior_big_seller;
        }

        public List<Seller> Get_Regular_Big_Sellers()
        {
            return _regular_big_seller;
        }

        public List<Parameters> Get_Parameter_List()
        {
            return _parameters;
        }

        public List<Boolean> Get_Run_Type()
        {
            return run_type;
        }

        public List<Double> Get_Courier_Parameters()
        {
            return courier_parameters;
        }

        public Double Get_Hub_Coverage()
        {
            return hub_coverage;
        }

        public int Get_Run_Month()
        {
            return month_to_run;
        }
    }
}
