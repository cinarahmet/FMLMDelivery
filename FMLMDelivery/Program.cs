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
            var demand_point = new List<DemandPoint>();
            var potential_xDocks = new List<xDocks>();
            var xDocks = new List<xDocks>();
            var agency = new List<xDocks>();
            var hubs = new List<Hub>();
           

            //Provide the month index (1-January, 12-December)
            var month = 10;
            var reader = new CSVReader("Demand_Points.csv", "Potential_Xdocks.csv", "Potential_Seller_Data_New.csv", month);
            reader.Read();
            demand_point = reader.Get_County();
            potential_xDocks = reader.Get_XDocks();
            agency = reader.Get_Agency();
            var prior_small_sellers = reader.Get_Prior_Small_Sellers();
            var regular_small_sellers = reader.Get_Regular_Small_Sellers();
            var prior_big_sellers = reader.Get_Prior_Big_Sellers();
            var regular_big_sellers = reader.Get_Regular_Big_Sellers();


            var runner = new Runner(demand_point,potential_xDocks,agency,prior_small_sellers,regular_small_sellers,prior_big_sellers,regular_big_sellers);
            (xDocks , hubs)=runner.Run();
            Console.ReadKey();
        }
    }
}
  