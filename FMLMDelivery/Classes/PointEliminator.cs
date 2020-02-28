using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;

namespace FMLMDelivery.Classes
{
    class PointEliminator
    {
        private List<INumVar> x;

        private List<DemandPoint> _whole_demand_points;

        private List<DemandPoint> _candidate_demand_points;

        private List<xDocks> _whole_xDocks;

        private List<xDocks> _candidate_xDocks;

        private List<List<double>> d;

        private List<List<double>> a;

        private double _distance_threshold;

        private double _threshold_demand;

        private double _num_of_demand_points;

        /// <summary>
        /// Cplex object
        /// </summary>
        private readonly Cplex _solver;

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

        /// <summary>
        /// Time limit is given in seconds.
        /// </summary>
        private readonly long _timeLimit = 6000;

        /// <summary>
        /// Gap limit is given in percentage
        /// </summary>
        private readonly double _gap = 0.00001;

        private List<Double> demand_list;

        private Double _objVal;

        public PointEliminator(List<DemandPoint> demandPoints, List<xDocks> xDocks, double distance_threshold, double threshold_demand)
        {
            _solver = new Cplex();
            _solver.SetParam(Cplex.Param.TimeLimit, val: _timeLimit);
            _solver.SetParam(Cplex.Param.MIP.Tolerances.AbsMIPGap, _gap);

            _whole_demand_points = demandPoints;
            _whole_xDocks = xDocks;

            _candidate_demand_points = new List<DemandPoint>();
            _candidate_xDocks = new List<xDocks>();
            d = new List<List<double>>();
            a = new List<List<double>>();
            x = new List<INumVar>();
            demand_list = new List<double>();

            _distance_threshold = distance_threshold;
            _threshold_demand = threshold_demand;

            _num_of_demand_points = _whole_demand_points.Count;
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
            Build_Model();
            Solve();
            Print();
            Get_Candidate_Demand_Points();
        }

        public List<DemandPoint> Return_Candidate_Demand_Points()
        {
            return _candidate_demand_points;
        }

        private void Get_Candidate_Demand_Points()
        {
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                if (_solver.GetValue(x[i]) > 0.9)
                {
                    _candidate_demand_points.Add(_whole_demand_points[i]);
                }
                
            }

        }

        private void Print()
        {
            if (!(_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
            {
                Console.WriteLine("Solution is neither optimal nor feasible!");
                return;

            }
            _objVal = Math.Round(_solver.GetObjValue(), 2);
            var stats = _solver.GetStatus();
            Console.WriteLine("Objective value is {0}\n", _objVal);
            Console.WriteLine("Solution status is {0}\n", stats);
            var n_var = _solver.NbinVars;
            Console.WriteLine("Number of variables : {0}", n_var);

            //for (int i = 0; i < _num_of_demand_points; i++)
            //{
            //    for (int j = 0; j < _num_of_demand_points; j++)
            //    {
            //        if (_solver.GetValue(x[i][j]) > 0.9)
            //        {
            //            Console.WriteLine("x[{0},{1}] = {2}", i, j, _solver.GetValue(x[i][j]));
            //        }
            //    }
            //}
        }

        private void Build_Model()
        {
            Create_Decision_Variables();
            Create_Objective();
            Create_Constraints();
        }

        private void Solve()
        {
            Console.WriteLine("Algorithm starts running at {0}", DateTime.Now);
            var startTime = DateTime.Now;

            _solver.Solve();
            _solutionTime = (DateTime.Now - startTime).Seconds;
            _status = _solver.GetStatus();
            Console.WriteLine("Algorithm stops running at {0}", DateTime.Now);
        }

        private void Create_Constraints()
        {
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(x[i], demand_list[i]);
                _solver.AddGe(constraint,_threshold_demand);
            }
        }

        private void Create_Objective()
        {
            _objective = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                _objective.AddTerm(x[i], 1);
            }
            _solver.AddMaximize(_objective);
        }

        private void Create_Decision_Variables()
        {
            //for (int i = 0; i < _num_of_demand_points; i++)
            //{
            //    var x_i = new List<INumVar>();
            //    for (int j = 0; j < _num_of_demand_points; j++)
            //    {
            //        var name = $"x[{i + 1}][{(j + 1)}]";
            //        var x_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
            //        x_i.Add(x_ij);
            //    }
            //    x.Add(x_i); 
            //}
            for (int i = 0; i < _num_of_demand_points; i++)
            {
                var name = $"x[{i + 1}]";
                var x_i = _solver.NumVar(0, 1, NumVarType.Bool, name);
                x.Add(x_i);
            }
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
