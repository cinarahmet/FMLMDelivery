﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Device.Location;

namespace FMLMDelivery
{
    class xDockHubModel
    {
        /// <summary>
        /// The maximum distance that a XDock can be assigned to a Hub 
        /// </summary>
        private Double distance_threshold = 100;

        /// <summary>
        /// Maximum number of XDock that can be assigned to a single Hub.
        /// </summary>
        private Double max_num_xdock_assigned = 400;

        private Double min_num_xdock_assigned = 4;

        /// <summary>
        /// Min number of XDock that can be assigned to a single Hub.
        /// </summary>
        private Double min_hub_capacity = 10000;

        /// <summary>
        /// Total amount of demand for each hub
        /// </summary>
        private Double max_hub_capaticity = 300000;

        /// <summary>
        /// The maximum distance that a XDock can be assigned to a Hub in the west side
        /// </summary>
        private Double distance_thresholdwest = 200;

        /// <summary>
        /// The maximum distance that a XDock can be assigned to a Hub in the middle side
        /// </summary>
        private Double distance_thresholdmiddle = 250;

        /// <summary>
        /// The maximum distance that a XDock can be assigned to a Hub in the east side
        /// </summary>
        private Double distance_thresholdeast = 500;


        /// <summary>
        /// Number of Xdocks
        /// </summary>
        private readonly Int32 _numOfXdocks;

        /// <summary>
        /// Number of Hubs
        /// </summary>
        private readonly Int32 _numOfHubs;

        /// <summary>
        /// Number of Seller
        /// </summary>
        private readonly Int32 _numOfSeller;

        /// <summary>
        /// List of xDocks 
        /// </summary>
        private List<xDocks> _xDocks;

        /// <summary>
        /// List of potential hubs
        /// </summary>
        private List<Hub> _hubs;
        /// <summary>
        /// List of sellers
        /// </summary>
        private List<Seller> _sellers;

        /// <summary>
        /// x[i, j] € {0,1} denotes whether Xdock i is assigned to hub j
        /// </summary>
        private List<List<INumVar>> x;
        /// <summary>
        /// s[i,j]  € {0,1} denotes whether seller i is assigned to Hub j
        /// </summary>
        private List<List<INumVar>> s;

        /// <summary>
        /// y[j] € {0,1} denotes whether opened hub on location j
        /// </summary>
        private List<INumVar> y;

        /// <summary>
        /// z[i] € {0,1} denotes whether xDock i is covered 
        /// </summary>
        private List<INumVar> z;

        /// <summary>
        /// a[i,j] € {0,1} denotes whether xDock i is in the range of hub j.
        /// </summary>
        private List<List<Double>> a;
        /// <summary>
        /// aseller[i,j] € {0,1} whether seller i is in the range of Hub j.
        /// </summary>
        private List<List<Double>> a_seller;
        
        /// <summary>
        /// Linearization variable: Created in order to eliminate arg min cost func. for distances of un-covered xDocks. 
        /// </summary>
        private List<List<INumVar>> f;

        /// <summary>
        /// Minimum distance of unassigned dock i from hubs
        /// </summary>
        private List<INumVar> mu;

        /// <summary>
        /// Total Cost for unassigned xDocks
        /// </summary>
        private List<INumVar> k;

        /// <summary>
        /// d[i,j] € {0,1} is the distance matrix for all xDock i's and hub j's
        /// </summary>
        private List<List<Double>> d;

        /// <summary>
        /// dseller[i,j] is the distance matrix for all seller i's and Hub j's
        /// </summary>
        private List<List<Double>> d_seller;
        /// <summary>
        /// Revised distance matrix with notion of adding smalle sellers the minimum distance of xdocks and recalculating the seller to hub distance matrix.
        /// </summary>
        private List<List<Double>> d_sellerhub;
        /// <summary>
        /// 
        /// </summary>
        private List<double> min_distance;
        /// <summary>
        /// 
        /// </summary>
        private List<double> xdock_chosen;

        /// <summary>
        /// A sufficiently big number
        /// </summary>
        private Int32 M_1 = 100000;

        /// <summary>
        /// A sufficiently big number
        /// </summary>
        /// 
        private Int32 M_2 = 100000;

        /// <summary>
        /// A sufficiently big number
        /// </summary>
        /// 
        private Int32 M_3 = 100000;

        /// <summary>
        /// Opening cost for hub j
        /// </summary>
        private List<Double> c;

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

        /// <summary>
        /// if cost is incurred
        /// </summary>
        private Boolean _cost_incurred;

        /// <summary>
        /// if capacity constraint is incurred
        /// </summary>
        private Boolean _capacity_incurred;

        /// <summary>
        /// If minimum hub model is incurred
        /// </summary>
        private Boolean _min_hub_model;

        /// <summary>
        /// Objective Value
        /// </summary>
        private Double _objVal;

        private Dictionary<Int32,String> country_names;

        /// <summary>
        /// total number of hubs opened
        /// </summary>
        private Int32 p;

        /// <summary>
        /// Demand of each xDock
        /// </summary>
        private List<Double> x_dock_demand;

        /// <summary>
        /// Demand of each seller
        /// </summary>
        private List<Double> seller_demand;

        /// <summary>
        /// Weigted demand for 
        /// </summary>
        private Boolean _demand_weighted;

        /// <summary>
        /// List of opened hubs latitude and longitude
        /// </summary>
        //private List<Latitude_Longtitude> lat_long;

        /// <summary>
        /// demand normalization proportion 
        /// </summary>
        private List<Double> normalized_demand;

        /// <summary>
        /// Total demand of the xdocks
        /// </summary>
        private Double total_demand = 0;

        /// <summary>
        /// Total demand coverage that is required from the system
        /// </summary>
        private Double _demand_covarage;

        private Boolean phase_2;

        private int num_hubs = 0;

        private List<Hub> new_hubs;
        private double total_demand_seller;

        public xDockHubModel(List<xDocks> xDocks, List<Hub> hubs,List<Seller> sellers, Boolean Demandweight,Boolean min_hub_model,Double Demand_Covarage,Boolean Phase2, Int32 P , Boolean cost_incurred = false, Boolean capacity_incurred = false)
        {

            _solver = new Cplex();
            _solver.SetParam(Cplex.DoubleParam.TiLim, val: _timeLimit);
            _solver.SetParam(Cplex.DoubleParam.EpGap, _gap);
            _xDocks = xDocks;
            _hubs = hubs;
            _sellers = sellers;
            _numOfXdocks = xDocks.Count;
            _numOfHubs = hubs.Count;
            _numOfSeller = _sellers.Count;
            _cost_incurred = cost_incurred;
            _capacity_incurred = capacity_incurred;
            _min_hub_model = min_hub_model;
            p = P;
            _demand_weighted = Demandweight;
            _demand_covarage = Demand_Covarage;
            phase_2 = Phase2;

            x = new List<List<INumVar>>();
            s = new List<List<INumVar>>();
            y = new List<INumVar>();
            z = new List<INumVar>();
            a = new List<List<Double>>();
            a_seller = new List<List<double>>();
            f = new List<List<INumVar>>();
            mu = new List<INumVar>();
            k = new List<INumVar>();
            d = new List<List<double>>();
            d_seller = new List<List<double>>();
            d_sellerhub = new List<List<double>>();
            min_distance = new List<Double>();
            xdock_chosen = new List<Double>();
            c = new List<double>();
            country_names = new Dictionary<int, string>();
            x_dock_demand = new List<double>();
            seller_demand = new List<double>();
            new_hubs = new List<Hub>();
            normalized_demand = new List<double>();
        }

        public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord)/1000;
        }

        //Currently unused. The proportion of the demand of xDock i to whole demand.
        private void Get_Demand_Weight()
        {
            double totalweight = 0;
            for (int i = 0; i < _numOfXdocks; i++)
            {
                totalweight = x_dock_demand[i] + totalweight;
            }
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var dp2 = new List<Double>();
                double proportion = x_dock_demand[i]/totalweight;
                normalized_demand.Add(proportion);
            }
        }

       

        private void Get_Distance_Matrix()
        {
            
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var d_i = new List<double>();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    var long_1 = _xDocks[i].Get_Longitude();
                    var lat_1 = _xDocks[i].Get_Latitude();
                    var long_2 = _hubs[j].Get_Longitude();
                    var lat_2 = _hubs[j].Get_Latitude();
                    var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);
                    d_i.Add(d_ij);
                }
                d.Add(d_i);
            }
        }
        private void Get_Revised_Distance()
        {
            for (int i = 0; i < _numOfSeller; i++)
            {
                if (_sellers[i].Get_Size() == "Small")
                {
                    for (int e = 0; e < _numOfXdocks; e++)
                    {
                        if (xdock_chosen[i] ==e )
                        {
                            var d_i = new List<double>();
                            for (int k = 0; k < _numOfHubs; k++)
                            {
                                var long_1 = _xDocks[Convert.ToInt32(xdock_chosen[i])].Get_Longitude();
                                var lat_1 = _xDocks[Convert.ToInt32(xdock_chosen[i])].Get_Latitude();
                                var long_2 = _hubs[k].Get_Longitude();
                                var lat_2 = _hubs[k].Get_Latitude();
                                var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2) + min_distance[i];

                                d_i.Add(d_ij);
                            }
                            d_sellerhub.Add(d_i);
                        }
                    }
                   
                }
                else
                {
                    var d_i = new List<double>();
                    for (int k = 0; k < _numOfHubs; k++)
                    {
                        var long_1 = _sellers[i].Get_Longitude();
                        var lat_1 = _sellers[i].Get_Latitude();
                        var long_2 = _hubs[k].Get_Longitude();
                        var lat_2 = _hubs[k].Get_Latitude();
                        var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);

                        d_i.Add(d_ij);
                    }
                    d_sellerhub.Add(d_i);
                }


            }
        }
        private void Get_Distance_Matrix_Seller()
        {   //Calculating distance matrix for sellers
            for (int i = 0; i < _numOfSeller; i++)
            {
                var d_k = new List<double>();
                for (int j = 0; j < _numOfXdocks; j++)
                {
                    var long_1 = _sellers[i].Get_Longitude();
                    var lat_1 = _sellers[i].Get_Latitude();
                    var long_2 = _xDocks[j].Get_Longitude();
                    var lat_2 = _xDocks[j].Get_Latitude();
                    var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);

                    d_k.Add(d_ij);
                }
                d_seller.Add(d_k);
            }

        }
        private void Min_Distance()
        {
           
            var dist = new Double();
            var index = new Double();
            for (int i = 0; i < _numOfSeller; i++)
            {
                dist = 1000000;
                for (int j = 0; j < _numOfXdocks; j++)
                {
                    if (d_seller[i][j] < dist)
                    {
                        dist = d_seller[i][j];
                        index = j;
                    }
                }
                min_distance.Add(dist);
                xdock_chosen.Add(index);
            }

        }


        public Double GetObjVal()
        {
            return _objVal;
        }


        //For cost incurred model
        private void Get_Cost_Parameters()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                var c_j = _hubs[j].Get_Capacity();
                c.Add(c_j);
            }
        }


        private void Create_Distance_Threshold_Matrix()
        {
            //Create a[i,j] matrix
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var longtitude = _xDocks[i].Get_Longitude();
                var threshold = _xDocks[i].Get_Distance_Threshold();
                var a_i = new List<Double>();
                for (int j = 0; j < _numOfHubs; j++)
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
        private void Create_Distance_Threshold_Seller()
        {   //Create a_seller[i,j] matrix
            for (int i = 0; i < _numOfSeller; i++)
            {
                
                var threshold = _sellers[i].Get_Distance_Threshold();
                var a_k = new List<Double>();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    if (d_seller[i][j] <= threshold)
                    {
                        var a_ij = 1;
                        a_k.Add(a_ij);
                    }
                    else
                    {
                        var a_ij = 0;
                        a_k.Add(a_ij);
                    }
                }
                a_seller.Add(a_k);
            }

        }

        public void Run()
        {
            Get_Parameters();
            Build_Model();
           // AddInitialSolution();
            Solve();
            Create_Country_Names();
            Get_Num_Hubs();
            Get_new_Hubs();
            Print();
            ClearModel();
            
        }

        public List<Hub> Return_New_Hubs()
        {
            return new_hubs;
        }

        private void Get_new_Hubs()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                if (_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal)
                {
                    if (_solver.GetValue(y[j]) > 0.9)
                    {
                        var city = _hubs[j].Get_City();
                        var id = _hubs[j].Get_Id();
                        var region = _hubs[j].Get_Region();
                        var valueslat = _hubs[j].Get_Latitude();
                        var valueslong = _hubs[j].Get_Longitude();
                        var dist = _hubs[j].Get_Dsitance_Threshold();
                        var capacity = _hubs[j].Get_Capacity();
                        var already_opened = _hubs[j].If_Already_Opened();

                        var new_hub = new Hub(city, id, region, valueslong, valueslat, dist, capacity, already_opened);
                        new_hubs.Add(new_hub);
                    }
                }

            }
        }

        //In order to return to minimum number of hubs that gives a feasible solution
        private void Get_Num_Hubs()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                if (_solver.GetValue(y[j])>0.9)
                {
                    num_hubs += 1;
                }
            }
        }

        //In order to return to minimum number of hubs that gives a feasible solution
        public Int32 Return_num_Hubs()
        {
            return num_hubs;
        }

        //For returning the Id's of the solution
        private void Create_Country_Names()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
                {
                    if (_solver.GetValue(y[j]) > 0.9)
                    {
                        country_names.Add(j, _hubs[j].Get_Id());
                    }
                }
                
            }

        }

        public Dictionary<Int32, String> Get_Country_Names()
        {
            return country_names;
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

            for (int i = 0; i < _numOfXdocks; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    if (_solver.GetValue(x[i][j] ) > 0.9)
                    {
                        //Console.WriteLine("x[{0},{1}] = {2}", i, j, _solver.GetValue(x[i][j]));
                    }
                   
                }
            }
            for (int i = 0; i < _numOfSeller; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    if (_solver.GetValue(s[i][j])>0.9)
                    {
                        Console.WriteLine("s[{0},{1}] = {2}", i, j, _solver.GetValue(s[i][j]));
                    }
                }
            }
            for (int i = 0; i < _numOfSeller; i++)
            {
                Console.WriteLine("mindist[{0}] = {1}", i, xdock_chosen[i]);
            }
           
            for (int j = 0; j < _numOfHubs; j++)
            {
                if (_solver.GetValue(y[j])>0.9)
                {
                    Console.WriteLine("y[{0}] = {1}", j, _solver.GetValue(y[j]));
                }
                
            }

            if (_cost_incurred)
            {
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    if (_solver.GetValue(z[i]) > 0.9)
                    {
                        Console.WriteLine("z[{0}] = {1}", i, _solver.GetValue(z[i]));
                    }

                    Console.WriteLine("k[{0}] = {1}", i, _solver.GetValue(k[i]));
                    Console.WriteLine("mu[{0}] = {1}", i, _solver.GetValue(mu[i]));

                }
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    for (int j = 0; j < _numOfHubs; j++)
                    {
                        Console.WriteLine("f[{0},{1}]] = {2}", i, j, _solver.GetValue(f[i][j]));

                    }
                }
            }
           
        }

        private void Get_Parameters()
        {
            Get_Distance_Matrix_Seller();
            Min_Distance();
            Get_Distance_Matrix();
            Get_Revised_Distance();
            Get_Cost_Parameters();
            Create_Distance_Threshold_Matrix();
            Create_Distance_Threshold_Seller();
            Get_Demand_Parameters();
            Get_Demand_Weight();
            Get_Total_Demand();
        }

        private void Get_Total_Demand()
        {
            for (int i = 0; i < _numOfXdocks; i++)
            {
                total_demand = x_dock_demand[i] + total_demand;
            }
            for (int j = 0; j < _numOfSeller; j++)
            {
                total_demand_seller += _sellers[j].Get_Demand();
            }
        }
        private void Get_Demand_Parameters()
        {
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var d_i = _xDocks[i].Get_Demand();
                x_dock_demand.Add(d_i);
            }
            for (int i = 0; i < _numOfSeller; i++)
            {
                var d_k = _sellers[i].Get_Demand();
                seller_demand.Add(d_k);
            }
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

        private void Build_Model()
        {
            Console.WriteLine("Model construction starts at {0}", DateTime.Now);
            CreateDecisionVariables();
            CreateObjective();
            CreateConstraints();
            Console.WriteLine("Model construction ends at {0}", DateTime.Now);
        }



        private void CreateConstraints()
        {
            CoverageConstraints();
            MainHubConstraint();
            Nonnegativity_Constraint();
            
            if (_cost_incurred)
            {
                UnAssigned_XDock_Constraints();
                Capacity_Constraint();
            }
            if (_capacity_incurred)
            {
                TotalHubConstraint();
                Capacity_Constraint();
                Min_X_Dock_Constraint();
            }
            if (_demand_weighted)
            {
                if (!(_capacity_incurred))
                {
                    TotalHubConstraint();
                    Capacity_Constraint();
                   // Min_X_Dock_Constraint();
                }
                if (phase_2)
                {
                    Demand_Coverage_Constraint();
                    Seller_Capacity_Constraint();
                    Seller_Assignment_Constraint();
                    Seller_Demand_Satisfaction_Constraint();


                }

            }
            if (_min_hub_model)
            {
                Seller_Capacity_Constraint();
                Seller_Assignment_Constraint();
                Demand_Coverage_Constraint();
                Capacity_Constraint();
                Seller_Demand_Satisfaction_Constraint();
            }

        }

        private void Nonnegativity_Constraint()
        {
            for (int i = 0; i < _numOfXdocks; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    _solver.AddGe(x[i][j], 0);
                }
            }
            for (int j = 0; j < _numOfHubs; j++)
            {
                _solver.AddGe(y[j], 0);
            }
            for (int i = 0; i < _numOfSeller; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    _solver.AddGe(s[i][j], 0);
                }
            }
        }

        //y[j]*beta <= ∑x[i,j]*a[i,j]*demand[i]
        private void Min_X_Dock_Constraint()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    constraint.AddTerm(x[i][j], a[i][j]*x_dock_demand[i]);
                }
                constraint.AddTerm(y[j], -min_hub_capacity);
                _solver.AddGe(constraint,0);
            }

            for (int j = 0; j < _numOfHubs; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    constraint.AddTerm(x[i][j], a[i][j]);
                }
                constraint.AddTerm(y[j], -min_num_xdock_assigned);
                _solver.AddGe(constraint, 0);
            }
        }
      
        private void Seller_Assignment_Constraint()
        {
            for (int i = 0; i < _numOfSeller; i++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    constraint.AddTerm(s[i][j], a_seller[i][j]);
                }
                _solver.AddLe(constraint, 1);
            }
        }
        private void Seller_Capacity_Constraint()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _numOfSeller; i++)
                {
                    constraint.AddTerm(s[i][j], a_seller[i][j]);
                    constraint.AddTerm(y[j], -200000);
                }
                _solver.AddLe(constraint, 0);
            }
        }
        private void Seller_Demand_Satisfaction_Constraint()
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _numOfSeller; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    constraint.AddTerm(s[i][j], a_seller[i][j]*seller_demand[i]);
                    
                }
            }
            _solver.AddGe(constraint, total_demand_seller * 0.76);
        }
        //∑∑x[i,j]*a[i,j]*d[i] >= covarage_percentage*demand
        private void Demand_Coverage_Constraint()
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _numOfXdocks; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    constraint.AddTerm(x[i][j], x_dock_demand[i]*a[i][j]);
                }
            }
            _solver.AddGe(constraint, total_demand*_demand_covarage);
        }
        //∑y[j]=num_of_cluster
        private void TotalHubConstraint()
        {
            var constraint = _solver.LinearNumExpr();
            for (int j = 0; j < _numOfHubs; j++)
            {
                constraint.AddTerm(y[j], 1);
            }
            _solver.AddEq(constraint, p);
        }

        //If any hub is already open
        private void MainHubConstraint()
        {
          //  var constraint = _solver.LinearNumExpr();
          //  constraint.AddTerm(y[39], 1);
          //  constraint.AddTerm(y[40], 1);
          //  _solver.AddEq(constraint, 2);
        }

        //y[j]*alfa >= ∑x[i,j]*a[i,j]*demand[i]
        private void Capacity_Constraint()
        {
            for (int j = 0; j < _numOfHubs; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    var demand_included = x_dock_demand[i] * a[i][j];
                    constraint.AddTerm(x[i][j], demand_included);
                }
                constraint.AddTerm(y[j], -max_hub_capaticity);
                _solver.AddLe(constraint, 0);
            }

            for (int j = 0; j < _numOfHubs; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    constraint.AddTerm(x[i][j], a[i][j]);
                }
                constraint.AddTerm(y[j], -max_num_xdock_assigned);
                _solver.AddLe(constraint, 0);
            }
        }


        private void UnAssigned_XDock_Constraints()
        {
            //mu[i] <= y[j]*d[i][j]
            for (int i = 0; i < _numOfXdocks; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                     var constraint = _solver.LinearNumExpr();
                    constraint.AddTerm(mu[i], 1);
                    constraint.AddTerm(y[j], -d[i][j]+M_1);
                    _solver.AddLe(constraint, M_1);
                }
            }

            //mu[i] >= y[j]*d[i][j]-(1-f[i][j])*M_1
            for (int i = 0; i < _numOfXdocks; i++)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    var constraint = _solver.LinearNumExpr();
                    constraint.AddTerm(mu[i], 1);
                    constraint.AddTerm(y[j], -d[i][j]+M_1);
                    constraint.AddTerm(f[i][j], -M_1);
                    _solver.AddGe(constraint, -M_1+M_1);
                }
            }

            //∑f[i][j] >= 1
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
            
                for (int j = 0; j < _numOfHubs; j++)
                {
                    constraint.AddTerm(f[i][j], 1);
                }
                _solver.AddGe(constraint, 1);
            }

            //k[i] <= M_2*(1-z[i])
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(k[i], 1);
                constraint.AddTerm(z[i], M_2);
                _solver.AddLe(constraint, M_2);
            }

            //k[i] <= mu[i]
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(k[i], 1);
                constraint.AddTerm(mu[i], -1);
                _solver.AddLe(constraint, 0);
            }

            //k[i] >= mu[i]-z[i]*M_3
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(k[i], 1);
                constraint.AddTerm(mu[i], -1);
                constraint.AddTerm(z[i], M_3);
                _solver.AddGe(constraint, 0);
            }

            //k[i] >= 0
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(k[i], 1);
                _solver.AddGe(constraint, 0);
            }
        }

        private void CoverageConstraints()
        {

            //∑x[i,j]*a[i,j] = z[i]/1/<=1
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    constraint.AddTerm(x[i][j], a[i][j]);
                }
                if (_cost_incurred)
                {
                    _solver.AddEq(constraint, z[i]);
                }
                else if (_capacity_incurred || _demand_weighted)
                {
                    if (!phase_2)
                    {
                        _solver.AddEq(constraint, 1);
                    }
                    else
                    {
                        _solver.AddLe(constraint, 1);
                    }
                    
                }
                else if (_min_hub_model)
                {
                    _solver.AddLe(constraint, 1);
                }
                
            }
            

           
        }


        /// <summary>
        /// 
        /// </summary>
        private void CreateObjective()
        {
            _objective = _solver.LinearNumExpr();
            /// <summary>
            /// Create objective function which tries to minimizes number of hubs while also tries to minimizes unassigned XDocks.
            /// </summary>
            if (_cost_incurred)
            {
                
                for (int j = 0; j < _numOfHubs; j++)
                {
                    _objective.AddTerm(y[j], c[j]);
                }

                for (int i = 0; i < _numOfXdocks; i++)
                {
                    _objective.AddTerm(k[i], 1);
                }
            }
            //Minimizes distance between hub j and assigned xDock i
            if (_capacity_incurred)
            {
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    for (int j = 0; j < _numOfHubs; j++)
                    {
                        _objective.AddTerm(x[i][j], d[i][j]);
                    }
                }
            }
            //Minizes the distance, between hub j and assigned xDock i  considering demand
            if (_demand_weighted)
            {
                for (int i = 0; i < _numOfXdocks; i++)
                {
                    for (int j = 0; j < _numOfHubs; j++)
                    {
                        _objective.AddTerm(x[i][j], d[i][j] * x_dock_demand[i]);

                    }
                }
                for (int i = 0; i < _numOfSeller; i++)
                {
                    for (int j = 0; j < _numOfHubs; j++)
                    {
                        _objective.AddTerm(s[i][j], d_sellerhub[i][j] * seller_demand[i]);
                    }
                }


            }
            //Minimizes the total number of hub j
            if (_min_hub_model)
            {
                for (int j = 0; j < _numOfHubs; j++)
                {
                    _objective.AddTerm(y[j], 1);
                }
            }
            _solver.AddMinimize(_objective);

        }


        private void CreateDecisionVariables()
        {
            // Create x[i,j]-variables
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var x_i = new List<INumVar>();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    var name = $"x[{i + 1}][{(j + 1)}]";
                    var x_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
                    x_i.Add(x_ij);
                }
                x.Add(x_i);
            }
            for (int i = 0; i < _numOfSeller; i++)
            {
                var s_i = new List<INumVar>();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    var name = $"s[{i + 1}][{(j + 1)}]";
                    var s_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
                    s_i.Add(s_ij);
                }
                s.Add(s_i);
            }

            //Create y[j] variables
            for (int j = 0; j < _numOfHubs; j++)
            {
                var name = $"x[{(j + 1)}]";
                var y_j = _solver.NumVar(0, 1, NumVarType.Bool, name);
                y.Add(y_j);
            }
            
            //Create z[i] variables
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var name = $"z[{(i + 1)}]";
                var z_i = _solver.NumVar(0, 1, NumVarType.Bool, name);
                z.Add(z_i);
            }

            //Create f[i,j] variables
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var f_i = new List<INumVar>();
                for (int j = 0; j < _numOfHubs; j++)
                {
                    var name = $"f[{i + 1}][{(j + 1)}]";
                    var f_ij = _solver.NumVar(0, 1,NumVarType.Bool, name);
                    f_i.Add(f_ij);
                }
                f.Add(f_i);
            }

            //Create k[i] variables
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var name = $"k[{(i + 1)}]";
                var k_i = _solver.NumVar(0, Int32.MaxValue, NumVarType.Float, name);
                k.Add(k_i);
            }

            //Create mu[i] variables
            for (int i = 0; i < _numOfXdocks; i++)
            {
                var name = $"mu[{(i + 1)}]";
                var mu_i = _solver.NumVar(0, Int32.MaxValue, NumVarType.Float, name);
                mu.Add(mu_i);
            }


        }

        private void AddInitialSolution(Double[] sol = null)
        {
            _solver.AddMIPStart(y.ToArray(), sol);
        }

        public void ClearModel()
        {
            _solver.ClearModel();
            _solver.Dispose();
        }

    }
    
}