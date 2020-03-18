using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Device.Location;

namespace FMLMDelivery.Classes
{
    class SmallSellerxDockModel
    {
        private List<Seller> _small_seller;

        private List<Seller> _assigned_seller;

        private List<xDocks> _xDocks;
        
        private Double distance_threshold = 40;

        private List<List<INumVar>> x;

        private List<List<Double>> a;

        private List<INumVar> availability;

        private List<List<Double>> distance_matrix;

        private List<Double> demand_matrix;

        private Double _objVal;

        private double covered_demand = 0;

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
        private readonly double _gap = 0.0000001;

        private Int32 _num_of_xDocks;

        private Int32 _num_of_Seller;

        private Double M = 1000;

        private Boolean _prior;

        private Double _remaining_demand;

        private List<String> record = new List<String>();

        private List<String> record_stats = new List<String>();


        public SmallSellerxDockModel(List<Seller> small_seller, List<xDocks> xDocks,Boolean prior,double remaining_demand = 0)
        {
            _solver = new Cplex();
            _solver.SetParam(Cplex.Param.TimeLimit, val: _timeLimit);
            _solver.SetParam(Cplex.Param.MIP.Tolerances.AbsMIPGap, _gap);

            _small_seller = small_seller;
            _xDocks = xDocks;

            x = new List<List<INumVar>>();
            a = new List<List<double>>();
            availability = new List<INumVar>();
            distance_matrix = new List<List<double>>();
            demand_matrix = new List<double>();
            _assigned_seller = new List<Seller>();
            _remaining_demand = remaining_demand;
            record = new List<String>();
            _num_of_xDocks = _xDocks.Count;
            _num_of_Seller = _small_seller.Count;
            record_stats = new List<String>();
            _prior = prior;
        }

        public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord) / 1000;
        }

        private void Get_Distance_Matrix()
        {
            //Calculating the distance matrix
            for (int i = 0; i < _num_of_Seller; i++)
            {
                var d_i = new List<double>();
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    var long_1 = _small_seller[i].Get_Longitude();
                    var lat_1 = _small_seller[i].Get_Latitude();
                    var long_2 = _xDocks[j].Get_Longitude();
                    var lat_2 = _xDocks[j].Get_Latitude();
                    var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);
                    d_i.Add(d_ij);
                }
                distance_matrix.Add(d_i);
            }
        }
        public Double GetObjVal()
        {
            return _objVal;
        }

        private void Create_Distance_Threshold_Matrix()
        {
            //Create a[i,j] matrix
            for (int i = 0; i < _num_of_Seller; i++)
            {
                var longtitude = _small_seller[i].Get_Longitude();
                //var threshold = _prior_small_seller[i].Get_Distance_Threshold();
                var a_i = new List<Double>();
                for (int j = 0; j < _num_of_xDocks; j++)
                {

                    if (distance_matrix[i][j] <= distance_threshold)
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

        private void Get_Demand_Parameters()
        {
            for (int i = 0; i < _num_of_Seller; i++)
            {
                var d_i = _small_seller[i].Get_Demand();
                demand_matrix.Add(d_i);
            }
        }

        private void Get_Parameters()
        {
            Get_Distance_Matrix();
            Create_Distance_Threshold_Matrix();
            Get_Demand_Parameters();
        }

        public void Run()
        {
            Get_Parameters();
            Build_Model();
            Solve();
            if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
            {
                Get_Status();
                Calculate_Covered_Demand();
                Assigned_Seller_List();
                Get_Seller_Csv();
            }  
            Print();
            ClearModel();
        }

        public List<Seller> Return_Assigned_Seller()
        {
            return _assigned_seller;
        }

        private void Assigned_Seller_List()
        {

            for (int i = 0; i < _num_of_Seller; i++)
            {
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    if (_solver.GetValue(x[i][j]) > 0.9)
                    {
                        _assigned_seller.Add(_small_seller[i]);
                    }
                }
            }
        }

        public void Calculate_Covered_Demand()
        {
            if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
            {
                for (int i = 0; i < _num_of_Seller; i++)
                {
                    for (int j = 0; j < _num_of_xDocks; j++)
                    {
                        if (_solver.GetValue(x[i][j]) > 0.9)
                        {
                            covered_demand += _small_seller[i].Get_Demand();
                        }
                    }
                }
            }
            
        }

        public double Return_Covered_Demand()
        {
            return covered_demand;
        }

        public void Get_Seller_Csv()

        {   
            var count = 0;
            for (int j = 0; j < _num_of_xDocks; j++)
            {
                count += 1;
                for (int i = 0; i < _num_of_Seller; i++)
                {
                    if (_solver.GetValue(x[i][j])>0.9)
                    {
                        var xdock_rank = "Xdock" + count ;
                        var xdock_city = _xDocks[j].Get_City();
                        var xdock_district = _xDocks[j].Get_District();
                        var xdock_lat = _xDocks[j].Get_Latitude();
                        var xdock_long = _xDocks[j].Get_Longitude();
                        var xdock_id = _xDocks[j].Get_Id();
                        var seller_name = _small_seller[i].Get_Name();
                        var seller_city = _small_seller[i].Get_City();
                        var seller_district = _small_seller[i].Get_District();
                        var seller_id = _small_seller[i].Get_Id();
                        var seller_demand = _small_seller[i].Get_Demand();
                        var seller_distance = distance_matrix[i][j];
                        var result = $"{xdock_rank},{xdock_city},{xdock_district},{xdock_id},{xdock_lat},{xdock_long},{seller_name},{seller_city},{seller_district},{seller_distance},{seller_demand}";
                        record.Add(result);
                    }
                }
                
                
            }
        }
        public List<String> Get_Seller_Xdock_Info()
        {
            return record;
        }
        private void Get_Status()
        {
            var part = "All";
            var model= "";
            var time = _solutionTime;
            var status = _status;
            var gap_to_optimal = _solver.GetMIPRelativeGap();
            if (_prior == true)
            {
                model = "Prior Small Seller Model";
            }
            else
            {
                model = "Other Small Seller Model";
            }
            var result = $"{part},{model},{status},{time},{gap_to_optimal}";
            record_stats.Add(result);
        }

        public List<String> Get_Small_Seller_Model_Stat()
        {
            return record_stats;
        }        

        public void ClearModel()
        {
            _solver.ClearModel();
            _solver.Dispose();
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

            if (_prior)
            {
                for (int i = 0; i < _num_of_Seller; i++)
                {
                    if (_solver.GetValue(availability[i]) > 0.9)
                    {
                        Console.WriteLine("availability[{0}] = {1}", i, _solver.GetValue(availability[i]));
                    }
                }

            }

            for (int i = 0; i < _num_of_Seller; i++)
            {
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    if (_solver.GetValue(x[i][j]) > 0.9)
                    {
                        Console.WriteLine("x[{0},{1}] = {2}", i, j, _solver.GetValue(x[i][j]));
                    }
                }
            }

        }

        private void Build_Model()
        {
            Console.WriteLine("Model construction starts at {0}", DateTime.Now);
            Create_Decision_Variables();
            Create_Objective();
            Create_Constraints();
            Console.WriteLine("Model construction ends at {0}", DateTime.Now);
        }

        private void Create_Decision_Variables()
        {
            for (int i = 0; i < _num_of_Seller; i++)
            {
                var x_i = new List<INumVar>();
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    var name = $"x[{i + 1}][{(j + 1)}]";
                    var x_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
                    x_i.Add(x_ij);
                }
                x.Add(x_i);
            }

            for (int i = 0; i < _num_of_Seller; i++)
            {
                var name = $"x[{i + 1}]";
                var availability_i = _solver.NumVar(0, 1, NumVarType.Bool, name);
                availability.Add(availability_i);
            }
        }

        private void Create_Objective()
        {
            _objective = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_Seller; i++)
            {
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    _objective.AddTerm(x[i][j], distance_matrix[i][j] * demand_matrix[i]);
                }
            }
            _solver.AddMinimize(_objective);
        }

        private void Demand_Coverage_Constraint()
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_Seller; i++)
            {
                for (int j = 0; j < _num_of_xDocks; j++)
                {
                    constraint.AddTerm(x[i][j], a[i][j] * demand_matrix[i]);
                }
            }
            _solver.AddGe(constraint, _remaining_demand);
        }

        private void Create_Constraints()
        {
            if (_prior)
            {
                for (int i = 0; i < _num_of_Seller; i++)
                {
                    var constraint = _solver.LinearNumExpr();
                    constraint.AddTerm(availability[i], M);
                    var count = 0;
                    for (int j = 0; j < _num_of_xDocks; j++)
                    {
                        if (a[i][j] > 0)
                        {
                            count += 1;
                        }
                    }
                    _solver.AddGe(constraint, count);
                }

                for (int i = 0; i < _num_of_Seller; i++)
                {
                    var constraint = _solver.LinearNumExpr();
                    for (int j = 0; j < _num_of_xDocks; j++)
                    {
                        constraint.AddTerm(x[i][j], a[i][j]);
                    }
                    constraint.AddTerm(availability[i], -1);
                    _solver.AddEq(constraint, 0);

                }
            }
            else
            {
                for (int i = 0; i < _num_of_Seller; i++)
                {
                    var constraint = _solver.LinearNumExpr();
                    for (int j = 0; j < _num_of_xDocks; j++)
                    {
                        constraint.AddTerm(x[i][j], a[i][j]);
                    }
                    _solver.AddLe(constraint, 1);
                }
                Demand_Coverage_Constraint();
            }
            
        }
    }

}
