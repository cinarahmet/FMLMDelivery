﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Linq;

namespace FMLMDelivery.Classes
{
    class PointEliminator
    {
        private List<INumVar> x;

        private List<DemandPoint> _whole_demand_points;

        private List<xDocks> _whole_xDocks;

        private List<List<double>> d;

        private List<List<double>> a;

        private double _distance_threshold;

        private double _threshold_demand;

        private double _num_of_demand_points;

        /// <summary>
        /// Objective instance which stores the objective function
        /// </summary>
        private ILinearNumExpr _objective;

        /// <summary>
        /// How many seconds the solver worked..
        /// </summary>
        private double _solutionTime;

        /// <summary>
        /// Solution status: 0 - Optimal; 1 - Feasible...
        /// </summary>
        private Cplex.Status _status;

        private List<Double> demand_list;

        private Double _objVal;

        public PointEliminator(List<DemandPoint> demandPoints, List<xDocks> xDocks, double distance_threshold, double threshold_demand)
        {

            _whole_demand_points = demandPoints;
            _whole_xDocks = xDocks;

            d = new List<List<double>>();
            a = new List<List<double>>();
            x = new List<INumVar>();
            demand_list = new List<double>();

            _distance_threshold = distance_threshold;
            _threshold_demand = threshold_demand;

            _num_of_demand_points = _whole_demand_points.Count;
        }

        private void Uptade_Candidate_xDocks()
        {
            var count = 0;
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                if (demand_list[i] <= _threshold_demand)
                {
                    count += 1;
                    var item_to_remove = _whole_xDocks.SingleOrDefault(x => (x.Get_Id() == _whole_demand_points[i].Get_Id()) && (x.Get_District() == _whole_demand_points[i].Get_District()) && x.Get_City() == _whole_demand_points[i].Get_City());
                    _whole_xDocks.Remove(item_to_remove);
                }
            }
        }

        public List<xDocks> Return_Filtered_xDocx_Locations()
        {
            return _whole_xDocks;
        }

        private void Get_Total_Demand()
        {
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                var demand = 0.0;
                for (int j = 0; j < _num_of_demand_points; j++)
                {
                    if (a[i][j] > 0.9)
                    {
                        demand += _whole_demand_points[j].Get_Demand();
                    }
                }
                demand_list.Add(demand);        
            }
        }

        public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord) / 1000;
        }

        public void Run()
        {
            Get_Distance_Matrix();
            Create_Distance_Threshold_Matrix();
            Get_Total_Demand();
            Uptade_Candidate_xDocks();
        }
        
        private void Get_Distance_Matrix()
        {
            //Calculating the distance matrix
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                var count = 0;
                var d_i = new List<double>();
                for (int j = 0; j < _num_of_demand_points; j++)
                {
                    var long_1 = _whole_demand_points[i].Get_Longitude();
                    var lat_1 = _whole_demand_points[i].Get_Latitude();
                    var long_2 = _whole_demand_points[j].Get_Longitude();
                    var lat_2 = _whole_demand_points[j].Get_Latitude();
                    count += 1;
                    var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);
                    d_i.Add(d_ij);
                }
                d.Add(d_i);
            }
        }

        private void Create_Distance_Threshold_Matrix()
        {
            //Create a[i,j] matrix
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                var threshold = _whole_demand_points[i].Get_Distance_Threshold();
                var a_i = new List<Double>();
                for (int j = 0; j < _num_of_demand_points; j++)
                {
                    if (d[i][j] <= threshold)
                    {
                        var a_ij = 1;
                        a_i.Add(a_ij);
                    }
                    else
                    {
                        var a_ij = 0;
                        a_i.Add(a_ij);
                    }
                }
                a.Add(a_i);
            }
        }
    }
}
