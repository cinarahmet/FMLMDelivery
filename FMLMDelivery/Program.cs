using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery;
using FMLMDelivery.Classes;

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
            var potential_hubs = new List<Hub>();
            var partial_xDocks = new List<xDocks>();
            var parameters = new List<Parameters>();

            //This variable decides which solution methıd will be run. If true; every city individually assigned, else regions are assigned as a whole
            var discrete_solution = true;
            
            //Provide the month index (1-January, 12-December)
            var month = 10;
            var reader = new CSVReader("Demand_Points.csv", "2020_Potential_xDocks_Case3.csv", "2020_FM_Ekim.csv", "Parameters.csv", month);
            reader.Read();
            demand_point = reader.Get_County();
            potential_xDocks = reader.Get_XDocks();
            agency = reader.Get_Agency();
            var prior_small_sellers = reader.Get_Prior_Small_Sellers();
            var regular_small_sellers = reader.Get_Regular_Small_Sellers();
            var prior_big_sellers = reader.Get_Prior_Big_Sellers();
            var regular_big_sellers = reader.Get_Regular_Big_Sellers();
            var parameter_list = reader.Get_Parameter_List();
            var partial_solution = false;
            if (!partial_solution)
            {
                var runner = new Runner(demand_point, potential_xDocks,partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers,parameter_list,partial_solution,discrete_solution);
                (xDocks, hubs) = runner.Run();
                Console.ReadKey();
            }
            else
            {
                var partial_reader = new CSVReader("", "Output/Temporary_xDocks.csv", "","", month);
                partial_reader.Read_Partial_Solution_Xdocks();
                partial_xDocks = partial_reader.Get_Partial_Solution_Xdocks();
                //partial_xDocks = partial_reader.Get_();
                var runner_partial = new Runner(demand_point, potential_xDocks,partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers,parameter_list,partial_solution,discrete_solution);
                (xDocks, hubs) = runner_partial.Run();
                Console.ReadKey();
            }

          

            


            

        }
    }
}
  