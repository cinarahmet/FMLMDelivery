using System;
using System.Collections.Generic;
using System.Linq;
using FMLMDelivery;

namespace FMLMDelivery
{
    class Program   
    {
        static void Main(string[] args)
        {
            var county_xDock_model = 1;
            var countinues_model = true;
            var county = new List<County>();
            var xDocks = new List<xDocks>();
            var hubs = new List<Hub>();
            if (county_xDock_model == 1 && !countinues_model)
            {
                var reader2 = new CSVReader2("District_Demand.csv", "District_Demand.csv");
                reader2.Read();
                county = reader2.Get_County();
                xDocks = reader2.Get_XDocks();
            }

            else if(county_xDock_model ==0)
            {
                var reader = new CSVReader("District_Demand.csv", "hub_test_81_country.csv");
                reader.Read();

                xDocks = reader.Get_XDocks();
                hubs = reader.Get_Hubs();
            }
            else if(countinues_model)
            {
                var reader = new CSVReader("District_Demand.csv", "District_Demand.csv");
                reader.Read();
                hubs = reader.Get_Hubs();
                var reader2 = new CSVReader2("District_Demand.csv", "District_Demand.csv");
                reader2.Read();
                county = reader2.Get_County();
                xDocks = reader2.Get_XDocks();

            }
            

            var cost_inccurred_model = false;
            var capacity_incured_model = false;
            var min_model_model = true;
            var demand_weighted_model = false;
            //Phase 2 takes the solution of min_model as an input and solve same question with demand weighted objective
            var phase_2 = false;
            var best_k = 0;
            double best_obj = 100000000000000;
            var score_list = new Dictionary<Int32,Double>();
            var name_list = new Dictionary<Int32,Dictionary< Int32, String>>();
            var latlongdisplay = new Dictionary<Int32, List<Latitude_Longtitude>>();
            var BestLatLong = new List<Latitude_Longtitude>();
            var demand_covarage = 0.9;

            var objVal = 0.0;
            var latlong = new List<Latitude_Longtitude>();
            var new_xDocks = new List<xDocks>();

            if (county_xDock_model == 1)
            {
                var first_phase = new Model2(county, xDocks, cost_inccurred_model, capacity_incured_model, 0, demand_weighted_model, min_model_model, demand_covarage, phase_2);
                first_phase.Run();
                objVal = first_phase.GetObjVal();
                name_list.Add(0, first_phase.Get_XDock_Names());
                new_xDocks = first_phase.Return_XDock();
                var min_num = first_phase.Return_Num_Xdock();
                //Part 2 for county-xDock pair
                var num_clusters = min_num;
                min_model_model= false;
                demand_weighted_model = true;
                phase_2 = true;
                first_phase = new Model2(county, xDocks, cost_inccurred_model, capacity_incured_model, num_clusters, demand_weighted_model, min_model_model, demand_covarage, phase_2);
                first_phase.Run();
                objVal = first_phase.GetObjVal();
                name_list.Add(1, first_phase.Get_XDock_Names());
                new_xDocks = first_phase.Return_XDock();
                String csv_2 = String.Join(Environment.NewLine, new_xDocks.Select(d => $"{d.Get_Id()},{d.Get_Longitude()},{d.Get_Latitude()},{d.Get_Demand()}"));
                System.IO.File.WriteAllText(@"C:\Users\can.boyacioglu\Desktop\FMLMDelivery\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\test_xDocks.csv", csv_2);
            }

            if (county_xDock_model == 0 || countinues_model)
            {
                if (countinues_model)
                {
                    xDocks = new_xDocks;
                    var reader = new CSVReader("District_Demand.csv", "test_xDocks.csv");
                    reader.Read_new_Location();
                    hubs = reader.Get_New_Hubs();
                }
                demand_covarage = 1.0;
                min_model_model = true;
                demand_weighted_model = false;
                phase_2 = false;
                var second_phase = new Model(xDocks, hubs, cost_inccurred_model, capacity_incured_model, 0, demand_weighted_model, min_model_model, demand_covarage, phase_2);
                second_phase.Run();
                var num_clusters = second_phase.Return_Hubs();
                min_model_model = false;
                demand_weighted_model = true;
                phase_2 = true;
                second_phase = new Model(xDocks, hubs, cost_inccurred_model, capacity_incured_model, num_clusters, demand_weighted_model, min_model_model, demand_covarage, phase_2);
                second_phase.Run();
                objVal = second_phase.GetObjVal();
                name_list.Add(num_clusters, second_phase.Get_Country_Names());
                latlong = second_phase.Read_Latitude_Longtitude();

                if (objVal < best_obj && !(objVal == 0))
                {
                    best_obj = objVal;
                    best_k = num_clusters;
                }
                score_list.Add(num_clusters, objVal);
                latlongdisplay.Add(num_clusters, latlong);
                
            }
            BestLatLong = latlongdisplay[best_k];

            String csv = String.Join(Environment.NewLine, BestLatLong.Select(d => $"{d.Latitude};{d.Longtitude}"));
            System.IO.File.WriteAllText(@"C:\Users\can.boyacioglu\Desktop\FMLMDelivery\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\test2.csv", csv);

             Console.WriteLine("Hello World!");
        }
        }
}
  