using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class xDock_Demand_Point_Pairs
    {
        private xDocks _xDock;

        private List<DemandPoint> _demand_Points;

        private List<Double> _pair_distance;

        public xDock_Demand_Point_Pairs(xDocks xDocks, List<DemandPoint> demandPoints,List<Double> pair_dist)
        {
            _xDock = xDocks;
            _demand_Points = demandPoints;
            _pair_distance = pair_dist;
        }

        public xDocks Get_xDock()
        {
            return _xDock;
        }

        public List<DemandPoint> Get_Demand_Point_List()
        {
            return _demand_Points;
        }

        public List<Double> Get_Distance_List()
        {
            return _pair_distance;
        }

        public void Extracted_Demand_Point(DemandPoint demand_p)
        {
            var index = _demand_Points.FindIndex(x => x.Get_Id() == demand_p.Get_Id() && x.Get_District() == demand_p.Get_District() && x.Get_City() == demand_p.Get_City());
            _demand_Points.Remove(demand_p);
            _pair_distance.RemoveAt(index);
        }

        public void Add_Demand_Point(DemandPoint demand_p, Double dist)
        {
            _demand_Points.Add(demand_p);
            _pair_distance.Add(dist);
        }
    }
}
