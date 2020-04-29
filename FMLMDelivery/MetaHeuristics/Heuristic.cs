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
        protected List<xDocks> _xDocks;
        protected List<DemandPoint> _demand_Points;
        protected List<xDock_Demand_Point_Pairs> _pairs;
        protected Double total_lm_demand;
        protected Double _lm_coverage;
        protected List<Parameters> _parameters;
        protected Double _num_xDock;
        protected List<List<Double>> _assignments;
        protected List<Double> _best_solution = new List<double>();
        protected List<xDock_Demand_Point_Pairs> _best_pairs = new List<xDock_Demand_Point_Pairs>();
        protected List<xDock_Demand_Point_Pairs> _old_pairs = new List<xDock_Demand_Point_Pairs>();
        protected Double objective_value = 0.0;
        protected List<Double> _old_solution=new List<double>();

        



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

        protected void Assignment_Procedure()
        {
            _pairs.Clear();
            Initialize_Pairs();
            var index_list = new List<Int32>();
            for (int k = 0; k < _solution.Count; k++)
            {
                if (_solution[k] == 1)
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
                var best_index = -1;
                var dist_matrix = new List<Double>();
                for (int i = 0; (i < index_list.Count) ; i++)
                {
                    var index = index_list[i];
                    var dist = Calculate_Distances(_xDocks[index].Get_Longitude(), _xDocks[index].Get_Latitude(), sorted_list[j].Get_Longitude(), sorted_list[j].Get_Latitude());
                    if (dist < best_distance && dist < sorted_list[j].Get_Distance_Threshold())
                    {
                        var remaining_demand = _pairs[index].Get_xDock().Get_LM_Demand() - _pairs[index].Get_Occupied_Capacity();
                        if (remaining_demand >= sorted_list[j].Get_Demand())
                        {
                            best_distance = dist;
                            best_index = index;
                        }
                    }
                }
                if (best_index != -1)
                {
                    _pairs[best_index].Add_Demand_Point(sorted_list[j], best_distance);
                    _pairs[best_index].Add_to_Occupied_Capacity(sorted_list[j].Get_Demand());
                }
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
            if (_num_xDock > count)
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
                lm_demand += _pairs[i].Get_Occupied_Capacity();
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
                    var demand = _pairs[i].Get_Occupied_Capacity();
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
        

        public Double Objective_Value()
        {
            return objective_value;
        }

        protected abstract void Swap();

        public List<Double> Run()
        {
            Construct_Initial_Solution();
            //Assignment_Procedure();
            Optimize();
            return _solution;
        }

        public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord) / 1000;
        }

        public void Create_Objective()
        {
            objective_value = 0.0;
            for (int i = 0; i < _pairs.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    for (int j = 0; j < _pairs[i].Get_Demand_Point_List().Count; j++)
                    {
                        objective_value += _pairs[i].Get_Demand_Point_List()[j].Get_Demand() * _pairs[i].Get_Distance_List()[j];
                    }
                }
            }
        }




    }

}
