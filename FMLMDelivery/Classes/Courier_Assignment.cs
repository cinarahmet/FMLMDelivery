using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Courier_Assignment
    {
        private xDocks _xDock;
        private List<Mahalle> _mahalle_list;
        private Double _courier_max_cap;
        private Double _courier_min_cap;
        private Double _threshold;


        public Courier_Assignment(xDocks xDock, List<Mahalle> mahalles, Double courier_max_cap, Double courier_min_cap, Double threshold)
        {
            _xDock = xDock;
            //Sort Descending Order
            _mahalle_list = mahalles.OrderByDescending(x => x.Return_Mahalle_Demand()).ToList(); 
            _courier_max_cap = courier_max_cap;
            _courier_min_cap = courier_min_cap;
            _threshold = threshold;
            
        }

        private void Assign_Big_Mahalle_Partially()
        {

        }

        private void Assign_Big_Mahalle_Completely()
        {
            
        }

        private void Assign_Remaining_Mahalle()
        {

        }

        private void Complete_Final_Assignments()
        {
            
        }



    }

    internal class Remaining_Demand_List
    {
        private string ID;
        private Double _min_demand;
        private Double _max_demand;

        public Remaining_Demand_List(string id,double min_demand,double max_demand)
        {
            ID = id;
            _max_demand = max_demand;
            _min_demand = min_demand;
        }

        public Double Get_Max_Demand()
        {
            return _max_demand;
        }
        public Double Get_Min_Demand()
        {
            return _min_demand;
        }
    }
}
