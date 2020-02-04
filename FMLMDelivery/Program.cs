﻿using System;
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
            var small_seller = new List<Seller>();
            var big_seller = new List<Seller>();
            //Provide the month index (1-January, 12-December)
            var month = 10;
            var reader = new CSVReader("Demand_Data.csv", "Potential_Xdock_Data.csv","Potential_Seller_Data.csv", month);
            reader.Read();
            demand_point = reader.Get_County();
            potential_xDocks = reader.Get_XDocks();
            agency = reader.Get_Agency();
            small_seller = reader.Get_Small_Sellers();
            big_seller = reader.Get_Big_Sellers();

            String csv = String.Join(Environment.NewLine, demand_point.Select(d => $"{d.Get_Latitude()};{d.Get_Longitude()};{d.Get_Demand()}"));
            System.IO.File.WriteAllText(@"C:\NETWORK DESIGN\FMLMDelivery\FMLMDelivery\bin\Debug\netcoreapp2.1\test3.csv", csv);



            var runner = new Runner(demand_point,potential_xDocks,agency, small_seller, big_seller);
            (xDocks , hubs)=runner.Run();
            Console.ReadKey();
        }
    }
}
  