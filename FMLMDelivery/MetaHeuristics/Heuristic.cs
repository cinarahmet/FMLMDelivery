using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public abstract class Heuristic
    {
        protected Double time_limit = 360.0;
        protected List<Double> _solution;
        private List<xDocks> _xDocks;
        private List<DemandPoint> _demand_Points;
        protected List<xDock_Demand_Point_Pairs> _pairs;
        private Double total_lm_demand;
        private Double _lm_coverage;
        private List<Parameters> _parameters;
        private Double _num_xDock;
        private List<List<Double>> _assignments;
        protected List<Double> _best_solution = new List<double>();
        protected List<xDock_Demand_Point_Pairs> _best_pairs = new List<xDock_Demand_Point_Pairs>();



        protected Heuristic(List<Double> solution, List<List<Double>> assignments, List<xDocks> xDocks, List<DemandPoint> demandPoints,List<Parameters> parameters, Double lm_coverage,Double num_xdock)
        {
            _xDocks = xDocks;
            _demand_Points = demandPoints;
            _pairs = new List<xDock_Demand_Point_Pairs>();
            _solution = solution;
            _parameters = parameters;
            total_lm_demand = _demand_Points.Sum(x => x.Get_Demand());
            _lm_coverage = lm_coverage;
            _num_xDock = num_xdock;
            _assignments = assignments;
            Initialize_Pairs();
            Console.WriteLine("Working");
        }



        protected void Initialize_Pairs()
        {
            for (int i = 0; i < _xDocks.Count; i++)
            {
                var demand_list = new List<DemandPoint>();
                var distance_list = new List<Double>();
                var _pair = new xDock_Demand_Point_Pairs(_xDocks[i], demand_list, distance_list);
                _pairs.Add(_pair);
            }
        }


        

        protected bool Check_Feasibility()
        {
            var is_feasible = true;
            is_feasible=Capacity_Constraint();
            if (!is_feasible) return is_feasible;
            is_feasible = Coverage_Constraint();
            if (!is_feasible) return is_feasible;
            is_feasible = Already_Open();
            if (!is_feasible) return is_feasible;
            is_feasible = Total_xDock_Constraint();

            return is_feasible;
        }

        private void Assignment_Procedure(List<Double> solution)
        {
            var index_list = new List<Int32>();
            for (int k = 0; k < solution.Count; k++)
            {
                if (solution[k] == 1)
                {
                    index_list.Add(k);
                }
            }
            var distance_matrix = new List<List<Double>>();
            var sorted_list = new List<DemandPoint>();
            sorted_list = _demand_Points.OrderByDescending(x => x.Get_Demand()).ToList();
            for (int j = 0; j < sorted_list.Count; j++)
            {
                var best_distance = 100000000.0;
                var best_index = 0;
                var dist_matrix = new List<Double>();
                for (int i = 0; (i < index_list.Count) ; i++)
                {
                    var index = index_list[i];
                    var dist = Calculate_Distances(_xDocks[index].Get_Longitude(), _xDocks[index].Get_Latitude(), sorted_list[j].Get_Longitude(), sorted_list[j].Get_Latitude());
                    if (dist < best_distance)
                    {
                        best_distance = dist;
                        best_index = index;
                    }
                }
                _pairs[best_index].Add_Demand_Point(sorted_list[j], best_distance);
            }
        }

        private void Construct_Initial_Solution()
        {
            for (int i = 0; i < _solution.Count; i++)
            {
                for (int j = 0; (j < _assignments[0].Count) && (_solution[i] == 1); j++)
                {
                    if (_assignments[i][j] == 1)
                    {
                        var dist = Calculate_Distances(_xDocks[i].Get_Longitude(), _xDocks[i].Get_Latitude(), _demand_Points[j].Get_Longitude(), _demand_Points[j].Get_Latitude());
                        _pairs[i].Add_Demand_Point(_demand_Points[j],dist);
                    }
                    
                }
            }
        }

        private Boolean Total_xDock_Constraint()
        {
            var is_feasible = true;
            var count = 0;
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1) count += 1;
            }
            if (_num_xDock != count)
            {
                is_feasible = false;
            }
            return is_feasible;
        }

        private Boolean Already_Open()
        {
            var is_feasible = true;
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_pairs[i].Get_xDock().If_Already_Opened())
                {
                    if (_solution[i] == 0)
                    {
                        is_feasible = false;
                        return is_feasible;
                    }
                }
            }
            return is_feasible;
        }


        private Boolean Coverage_Constraint()
        {
            var is_feasible = true;
            var lm_demand = 0.0;
            for (int i = 0; i < _solution.Count; i++)
            {
                lm_demand += _pairs[i].Get_xDock().Get_LM_Demand();
            }
            if (lm_demand < _lm_coverage*total_lm_demand)
            {
                is_feasible = false;
            }

            return is_feasible;
        }


        private Boolean Capacity_Constraint()
        {
            var is_feasible = true;

            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    var demand = 0.0;
                    for (int j = 0; j < _pairs[i].Get_Demand_Point_List().Count; j++)
                    {
                        demand += _pairs[i].Get_Demand_Point_List()[j].Get_Demand();
                    }
                    if (demand > _pairs[i].Get_xDock().Get_LM_Demand())
                    {
                        is_feasible = false;
                        return is_feasible;
                    }
                    for (int k = 0; k < _parameters.Count; k++)
                    {
                        if (_parameters[k].Get_Key() == _pairs[i].Get_xDock().Get_City())
                        {
                            if (_parameters[k].Get_Min_Cap() > demand) is_feasible = false;
                        }
                    }
                }
            }
            return is_feasible;

        }

        protected abstract void Optimize();
        

        private Double Objective_Value()
        {
            return 0;
        }

        protected abstract void Swap();

        public List<Double> Run()
        {
            Construct_Initial_Solution();
            Assignment_Procedure(_solution);
            Optimize();
            return _solution;
        }

        public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord) / 1000;
        }




    }

}
