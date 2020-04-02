using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using System.IO;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Device.Location;
using System.Linq;
using FMLMDelivery;

/// <summary>
/// IMPORTANT !!!!!!!!!
/// This model does not differ from other model file, the only difference is this model is modified for county and xDocks rather than xDocks and hubs.
/// </summary>
public class DemandxDockModel
{
 
    /// <summary>
    /// Maximum number of County that can be assigned to a single xDock.
    /// </summary>
    private Double max_num_demand_point_assigned = 400;

    

    private Double max_hub_capacity = 4000000;
    

    /// <summary>
    /// Cplex object
    /// </summary>  
    private readonly Cplex _solver;

    /// <summary>
    /// Number of Xdocks
    /// </summary>
    private readonly Int32 _numOfXdocks;

    /// <summary>
    /// Number of County
    /// </summary>
    private readonly Int32 _num_of_demand_point;

    /// <summary>
    /// List of posible xDocks 
    /// </summary>
    private List<xDocks> _xDocks;

    /// <summary>
    /// List of County
    /// </summary>
    private List<DemandPoint> _demandpoint;

    /// <summary>
    /// x[i, j] € {0,1} denotes whether county i is assigned to xDock j
    /// </summary>
    private List<List<INumVar>> x;

    /// <summary>
    /// y[j] € {0,1} denotes whether opened xDock on location j
    /// </summary>
    private List<INumVar> y;

    /// <summary>
    /// z[i] € {0,1} denotes whether county i is covered 
    /// </summary>
    private List<INumVar> z;

    /// <summary>
    /// a[i,j] € {0,1} denotes whether county i is in the range of xDock j.
    /// </summary>
    private List<List<Double>> a;

    /// <summary>
    /// Linearization variable: Created in order to eliminate arg min cost func. for distances of un-covered county. 
    /// </summary>
    private List<List<INumVar>> f;

    /// <summary>
    /// Minimum distance of unassigned county i from xDocks
    /// </summary>
    private List<INumVar> mu;

    /// <summary>
    /// Total Cost for unassigned county
    /// </summary>
    private List<INumVar> k;

    /// <summary>
    /// d[i,j] € {0,1} is the distance matrix for all county i's and xDock j's
    /// </summary>
    private List<List<Double>> d;

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
    /// Opening cost for xDock j
    /// </summary>
    private List<Double> c;

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
    private readonly long _timeLimit =3600;
    /// <summary>
    /// The starting time of the model
    /// </summary>
    private readonly DateTime Start;

    /// <summary>
    /// Gap limit is given in percentage
    /// </summary>
    private readonly double _gap ;

    /// <summary>
    /// if cost is incurred
    /// </summary>
    private Boolean _cost_incurred;

    /// <summary>
    /// if capacity constraint is incurred
    /// </summary>
    private Boolean _capacity_incurred;

    /// <summary>
    /// If minimum xDock model is incurred
    /// </summary>
    private Boolean _min_xDock_model;

    /// <summary>
    /// Objective Value
    /// </summary>
    private Double _objVal;

    private Dictionary<Int32, String> xDock_names;

    /// <summary>
    /// total number of xDock opened
    /// </summary>
    private Double p;

    /// <summary>
    /// Demand of each county
    /// </summary>
    private List<Double> demand_of_demand_point;

    /// <summary>
    /// Weigted demand for counties
    /// </summary>
    private Boolean _demand_weighted;

    /// <summary>
    /// List of opened xDocks latitude and longitude
    /// </summary>
    private List<xDocks> new_XDocks;

    /// <summary>
    /// List of potential Hub locations
    /// </summary>
    private List<Hub> potential_Hubs;

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

    private Int32 xDock_count = 0;

    private List<Double> _initial_xDocks;

    private List<Double> _initial_assignments;

    private Boolean _second_part;

    private List<Double> opened_xdocks = new List<Double>();

    private  List<String> record_list = new List<String>();

    private Double _min_xDock_cap;

    private List<String> record_stats = new List<String>();

    private String _location;


    public DemandxDockModel(List<DemandPoint> Demand_Points, List<xDocks> xDocks, string key, Boolean Demandweight, Boolean min_hub_model, Double Demand_Covarage,Double min_xdock_cap, Boolean Phase2, Double P,Boolean second_part, double Gap, Boolean cost_incurred = false, Boolean capacity_incurred=false)
	{
        _gap = Gap;
        _solver = new Cplex();
        _solver.SetParam(Cplex.DoubleParam.TiLim, val: _timeLimit);
        _solver.SetParam(Cplex.DoubleParam.EpGap, _gap);
        _xDocks = xDocks;
        _demandpoint = Demand_Points;
        _numOfXdocks = xDocks.Count;
        _num_of_demand_point = Demand_Points.Count;
        _cost_incurred = cost_incurred;
        _capacity_incurred = capacity_incurred;
        _min_xDock_model = min_hub_model;
        phase_2 = Phase2;
        p = P;
        _demand_weighted = Demandweight;
        _demand_covarage = Demand_Covarage;
        _initial_xDocks = new List<double>();
        _initial_assignments = new List<double>();
        _second_part = second_part;
        _min_xDock_cap = min_xdock_cap;
        

        x = new List<List<INumVar>>();
        y = new List<INumVar>();
        z = new List<INumVar>();
        a = new List<List<Double>>();
        f = new List<List<INumVar>>();
        mu = new List<INumVar>();
        k = new List<INumVar>();
        d = new List<List<double>>();
        c = new List<double>();

        record_list = new List<String>();
        xDock_names = new Dictionary<int, string>();
        demand_of_demand_point = new List<double>();
        new_XDocks = new List<xDocks>();
        potential_Hubs = new List<Hub>();
        normalized_demand = new List<double>();
        record_stats = new List<String>();
        _location = key;
    }

    public Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
    {
        var sCoord = new GeoCoordinate(lat_1, long_1);
        var eCoord = new GeoCoordinate(lat_2, long_2);

        return sCoord.GetDistanceTo(eCoord) / 1000;
    }

    private void Get_Demand_Weight()
    {
        double totalweight = 0;
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            totalweight = demand_of_demand_point[i] + totalweight;
        }
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var dp2 = new List<Double>();
            double proportion = demand_of_demand_point[i] / totalweight;
            normalized_demand.Add(proportion);
        }
    }

    private void Get_xDock()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {

            if (_status == Cplex.   Status.Feasible || _status == Cplex.Status.Optimal)
            {
                if (_solver.GetValue(y[j]) > 0.9)
                {
                    var city = _xDocks[j].Get_City();
                    var district = _xDocks[j].Get_District();
                    var county = _xDocks[j].Get_Id();
                    var region = _xDocks[j].Get_Region();
                    var valueslat = _xDocks[j].Get_Latitude();
                    var valueslong = _xDocks[j].Get_Longitude();
                    var distance_threshold = _xDocks[j].Get_Distance_Threshold();
                    var demand = 0.0;
                    var already_opened = _xDocks[j].If_Already_Opened();
                    var is_agency = _xDocks[j].If_Agency();
                    for (int i = 0; i < _num_of_demand_point; i++)
                    {
                        if (_solver.GetValue(x[i][j])>0.9)
                        {
                            demand += _demandpoint[i].Get_Demand();
                        }
                    }
                    var x_Dock =new xDocks(city,district,county,region,valueslong,valueslat,distance_threshold,demand,already_opened,is_agency);
                    new_XDocks.Add(x_Dock);

                }
            }

        }
    }

    private void Get_Potential_Hubs()
    {
        if (_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal)
        {
            for (int i = 0; i < new_XDocks.Count; i++)
            {
                var city = new_XDocks[i].Get_City();
                var district = new_XDocks[i].Get_District();
                var id = new_XDocks[i].Get_Id();
                var region = new_XDocks[i].Get_Region();
                var longitude = new_XDocks[i].Get_Longitude();
                var latitude = new_XDocks[i].Get_Latitude();
                var dist_thres = new_XDocks[i].Get_Distance_Threshold();
                var capacity = max_hub_capacity;
                var already_opened = false;
                var potential_hub = new Hub(city,district, id, region, longitude, latitude, dist_thres, capacity, already_opened);
                potential_Hubs.Add(potential_hub);

            }
        }

    }

    private void Get_Opened_xDocks()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {
            var value = Math.Round(_solver.GetValue(y[j]));
            opened_xdocks.Add(value);
        }
    }

    private void Get_Assignments()
    {
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var value = Math.Round(_solver.GetValue(x[i][j]));
                _initial_assignments.Add(value);
            }
        }
    }

    public void Provide_Initial_Solution(List<Double> opened_xDocks, List<Double> assignments)
    {
        _initial_xDocks = opened_xDocks;
        _initial_assignments = assignments;

    }

    public List<Double> Return_Opened_xDocks()
    {
        return opened_xdocks;
    }

    public List<Double> Return_Assignments()
    {
        return _initial_assignments;
    }

    public List<Hub> Return_Potential_Hubs()
    {
        return potential_Hubs;
    }

    public List<xDocks> Return_XDock()
    {
        return new_XDocks;
    }

    private void Get_Distance_Matrix()
    {
        
        //Calculating the distance matrix
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var count = 0;
            var d_i = new List<double>();
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var long_1 = _demandpoint[i].Get_Longitude();
                var lat_1 = _demandpoint[i].Get_Latitude();
                var long_2 = _xDocks[j].Get_Longitude();
                var lat_2 = _xDocks[j].Get_Latitude();
                count += 1;
                var d_ij = Calculate_Distances(long_1, lat_1, long_2, lat_2);
                d_i.Add(d_ij);
            }
            d.Add(d_i);
        }
    }

    private void Get_Num_XDocks()
    {
        
        for (int j = 0; j < _numOfXdocks; j++)
        {
            if (_solver.GetValue(y[j])>0.9)
            {
                xDock_count += 1;
            }
        }
    }

    public Double GetObjVal()
    {
        return _objVal;
    }

    private void Get_Cost_Parameters()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {
            var c_j = _xDocks[j].Get_LM_Demand();
            c.Add(c_j);
        }
    }

    private void Create_Distance_Threshold_Matrix()
    {
        //Create a[i,j] matrix
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var longtitude = _demandpoint[i].Get_Longitude();
            var threshold = _demandpoint[i].Get_Distance_Threshold();
            var a_i = new List<Double>();
            for (int j = 0; j < _numOfXdocks; j++)
            {
                if (d[i][j]<= threshold)
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

    private void Get_Model_Info()
    {
        var type = "";
        if (phase_2 == false)
        {
            type = "Min model";
        }
        else
        {
            type = "Demand Weighted";
        }
        Console.WriteLine("The model runs for the key {0}, and the model is {1}", _location, type);
    }

    public void Run()
    {
        Get_Model_Info();
        Get_Parameters();
        Build_Model();
        if (_second_part)
        {
            Add_Initial_Solution();
        }
        Solve();
        if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
        {
            Get_Opened_xDocks();
            Get_Assignments();
            Create_XDock_Names();
            Get_xDock();
            Get_Potential_Hubs();
            Get_Num_XDocks();
            Get_Csv_Information();
            Get_Model_Status();

        }
        Print();
        ClearModel();
    }

    public Boolean Return_Status()
    {
        if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Add_Initial_Solution()
    {
        if (_initial_xDocks.Count > 0)
        {
            _solver.AddMIPStart(y.ToArray(), _initial_xDocks.ToArray());
        }

        var xArray = new List<INumVar>();
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                xArray.Add(x[i][j]);
            }
        }

        if (_initial_assignments.Count > 0)
        {
            _solver.AddMIPStart(xArray.ToArray(), _initial_assignments.ToArray());
        }


    }

    private void Create_XDock_Names()
    {
        var count = 0;
        for (int j = 0; j < _numOfXdocks; j++)
        {
            if ((_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
            {
                if (_solver.GetValue(y[j]) > 0.9)
                {
                    count += 1;
                    xDock_names.Add(j, _xDocks[j].Get_Id());
                }
            }

        }

    }
    public Dictionary<Int32, String> Get_XDock_Names()
    {
        return xDock_names;
    }
    private void Get_Csv_Information()
    {                      
        var count = 0;
        
        for (int j = 0; j < _xDocks.Count; j++)
        {
            if (_solver.GetValue(y[j]) > 0.9)
            {
                count += 1;
                var xdock_city = _xDocks[j].Get_City();
                var xdock_district = _xDocks[j].Get_District();
                var xdock_lat = _xDocks[j].Get_Latitude();
                var xdock_long = _xDocks[j].Get_Longitude();
                var xdock_id = _xDocks[j].Get_Id();
                for (int i = 0; i < _demandpoint.Count; i++)
                { 
                    if (_solver.GetValue(x[i][j]) > 0.9)
                    {   var x_dock_ranking = "Xdock" + count ;
                        var demand_point_city = _demandpoint[i].Get_City();
                        var demand_point_district = _demandpoint[i].Get_District();
                        var demand_point_id = _demandpoint[i].Get_Id();
                        var demand = _demandpoint[i].Get_Demand();
                        var distance_xdock_county = d[i][j];
                        var result = $"{x_dock_ranking},{xdock_city},{xdock_district},{xdock_id},{xdock_lat},{xdock_long},{demand_point_city},{demand_point_district},{demand_point_id},{distance_xdock_county},{demand}";                                            
                        record_list.Add(result);
                    }                    
                }
            }
        }
                        
    }
    public List<String> Get_Xdock_County_Info()
    {
        return record_list;
    }

    private void Get_Model_Status()
    {
        var location = _location;
        var status = _status;
        var time = _solutionTime;
        var gap_to_optimal = (_solver.GetMIPRelativeGap())*100;
        var type = "";
        if (phase_2 == false)
        {
            type = "Min model";
        }
        else
        {
            type = "Demand Weighted";
        }
        var result = $"{location},{type},{_demand_covarage},{status},{time},{gap_to_optimal}";
        record_stats.Add(result);
    }
    
    public List<String> Get_Model_Stats_Info()
    {
        return record_stats;
    }
    private void Print()
    {
        
        if (!(_status == Cplex.Status.Feasible || _status == Cplex.Status.Optimal))
        {
            Console.WriteLine("Solution is neither optimal nor feasible!");
            return;

        }
        _objVal = Math.Round(_solver.GetObjValue(), 2);
        var status = _solver.GetStatus();
        Console.WriteLine("Objective value is {0}\n", _objVal);
        Console.WriteLine("Solution status is {0}\n", status);
        var n_var = _solver.NbinVars;
        Console.WriteLine("Number of variables : {0}", n_var);

        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                if (_solver.GetValue(x[i][j]) > 0.9)
                {
                  //  Console.WriteLine("x[{0},{1}] = {2}", i, j, _solver.GetValue(x[i][j]));
                }

            }
        }


        for (int j = 0; j < _numOfXdocks; j++)
        {
            if (_solver.GetValue(y[j]) > 0.9)
            {
               // Console.WriteLine("y[{0}] = {1}", j, _solver.GetValue(y[j]));
            }

        }

        if (_cost_incurred)
        {
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                if (_solver.GetValue(z[i]) > 0.9)
                {
                    Console.WriteLine("z[{0}] = {1}", i, _solver.GetValue(z[i]));
                }

                Console.WriteLine("k[{0}] = {1}", i, _solver.GetValue(k[i]));
                Console.WriteLine("mu[{0}] = {1}", i, _solver.GetValue(mu[i]));

            }
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                for (int j = 0; j < _numOfXdocks; j++)
                {
                    Console.WriteLine("f[{0},{1}]] = {2}", i, j, _solver.GetValue(f[i][j]));

                }
            }
        }

    }

    private void Get_Parameters()
    {
        Get_Distance_Matrix();
        Get_Cost_Parameters();
        Create_Distance_Threshold_Matrix();
        Get_Demand_Parameters();
        Get_Demand_Weight();
        Get_Total_Demand();
    }

    private void Get_Total_Demand()
    {
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            total_demand = demand_of_demand_point[i] + total_demand;
        }
    }

    private void Get_Demand_Parameters()
    {
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var d_i = _demandpoint[i].Get_Demand();
            demand_of_demand_point.Add(d_i);
        }
    }

    public Double Return_Num_Xdock()
    {
        return xDock_count;
    }

    private void Solve()
    {
        Console.WriteLine("Algorithm starts running at {0}", DateTime.Now);
        var startTime = DateTime.Now;
        _solver.Solve();
        _solutionTime =( (DateTime.Now - startTime).Hours*60*60 + (DateTime.Now - startTime).Minutes*60 + (DateTime.Now - startTime).Seconds);
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
        Already_Opened();
        if (_cost_incurred)
        {
            UnAssigned_XDock_Constraints();
        }
        if (_capacity_incurred)
        {
            TotalXDockConstraint();
            Capacity_Constraint();
            Min_County_Constraint();
        }
        if (_demand_weighted)
        {
            if (!(_capacity_incurred))
            {
                TotalXDockConstraint();
                Capacity_Constraint();
                Min_County_Constraint();
            }
            if (phase_2)
            {
                Demand_Coverage_Constraint();
                
            }

        }
        if (_min_xDock_model)
        {
            Demand_Coverage_Constraint();
            Capacity_Constraint();
            Min_County_Constraint();
        }

    }
    private void Already_Opened()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {
            if (_xDocks[j].If_Already_Opened())
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(y[j], 1);
                _solver.AddEq(constraint, 1);
            }
        }
    }

    private void Min_County_Constraint()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                constraint.AddTerm(x[i][j], a[i][j] * demand_of_demand_point[i]);
            }
            
            constraint.AddTerm(y[j], -_min_xDock_cap);
           
            _solver.AddGe(constraint, 0);
        }

        //for (int j = 0; j < _numOfXdocks; j++)
        //{
        //    var constraint = _solver.LinearNumExpr();
        //    for (int i = 0; i < _numOfCounty; i++)
        //    {
        //        constraint.AddTerm(x[i][j], a[i][j]);
        //    }
        //    constraint.AddTerm(y[j], -min_num_county_assigned);
        //    _solver.AddGe(constraint, 0);
        //}
    }

    private void Demand_Coverage_Constraint()
    {
        var constraint = _solver.LinearNumExpr();
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                constraint.AddTerm(x[i][j], demand_of_demand_point[i] * a[i][j]);
            }
        }
        _solver.AddGe(constraint, total_demand * _demand_covarage);
    }

    private void TotalXDockConstraint()
    {
        var constraint = _solver.LinearNumExpr();
        for (int j = 0; j < _numOfXdocks; j++)
        {
            constraint.AddTerm(y[j], 1);
        }
        _solver.AddEq(constraint, p);
    }

    private void MainHubConstraint()
    {
        //  var constraint = _solver.LinearNumExpr();
        //  constraint.AddTerm(y[39], 1);
        //  constraint.AddTerm(y[40], 1);
        //  _solver.AddEq(constraint, 2);
    }

    private void Capacity_Constraint()
    {
        for (int j = 0; j < _numOfXdocks; j++)
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                var demand_included = demand_of_demand_point[i] * a[i][j];
                constraint.AddTerm(x[i][j], demand_included);
            }
            constraint.AddTerm(y[j], -_xDocks[j].Get_LM_Demand());
            _solver.AddLe(constraint, 0);
        }

        for (int j = 0; j < _numOfXdocks; j++)
        {
            var constraint = _solver.LinearNumExpr();
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                constraint.AddTerm(x[i][j], a[i][j]);
            }
            constraint.AddTerm(y[j], -max_num_demand_point_assigned);
            _solver.AddLe(constraint, 0);
        }

    }

    private void UnAssigned_XDock_Constraints()
    {
        //mu[i] <= y[j]*d[i][j]
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(mu[i], 1);
                constraint.AddTerm(y[j], -d[i][j] + M_1);
                _solver.AddLe(constraint, M_1);
            }
        }

        //mu[i] >= y[j]*d[i][j]-(1-f[i][j])*M_1
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var constraint = _solver.LinearNumExpr();
                constraint.AddTerm(mu[i], 1);
                constraint.AddTerm(y[j], -d[i][j] + M_1);
                constraint.AddTerm(f[i][j], -M_1);
                _solver.AddGe(constraint, -M_1 + M_1);
            }
        }

        //∑f[i][j] >= 1
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();

            for (int j = 0; j < _numOfXdocks; j++)
            {
                constraint.AddTerm(f[i][j], 1);
            }
            _solver.AddGe(constraint, 1);
        }

        //k[i] <= M_2*(1-z[i])
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();
            constraint.AddTerm(k[i], 1);
            constraint.AddTerm(z[i], M_2);
            _solver.AddLe(constraint, M_2);
        }

        //k[i] <= mu[i]
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();
            constraint.AddTerm(k[i], 1);
            constraint.AddTerm(mu[i], -1);
            _solver.AddLe(constraint, 0);
        }

        //k[i] >= mu[i]-z[i]*M_3
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();
            constraint.AddTerm(k[i], 1);
            constraint.AddTerm(mu[i], -1);
            constraint.AddTerm(z[i], M_3);
            _solver.AddGe(constraint, 0);
        }

        //k[i] >= 0
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();
            constraint.AddTerm(k[i], 1);
            _solver.AddGe(constraint, 0);
        }
    }

    private void CoverageConstraints()
    {

        //∑x[i,j]*a[i,j] = z[i]/1/<=1
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var constraint = _solver.LinearNumExpr();
            for (int j = 0; j < _numOfXdocks; j++)
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
            else if (_min_xDock_model)
            {
                _solver.AddLe(constraint, 1);
            }

        }
        if (_cost_incurred)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var constraint = _solver.LinearNumExpr();
                for (int i = 0; i < _num_of_demand_point; i++)
                {
                    constraint.AddTerm(x[i][j], a[i][j]);
                }
                constraint.AddTerm(y[j], -max_num_demand_point_assigned);
                _solver.AddLe(constraint, 0);
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

            for (int j = 0; j < _numOfXdocks; j++)
            {
                _objective.AddTerm(y[j], c[j]);
            }

            for (int i = 0; i < _num_of_demand_point; i++)
            {
                _objective.AddTerm(k[i], 1);
            }
        }
        if (_capacity_incurred)
        {
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                for (int j = 0; j < _numOfXdocks; j++)
                {
                    _objective.AddTerm(x[i][j], d[i][j]);
                }
            }
        }
        if (_demand_weighted)
        {
            for (int i = 0; i < _num_of_demand_point; i++)
            {
                for (int j = 0; j < _numOfXdocks; j++)
                {
                    _objective.AddTerm(x[i][j], d[i][j] * demand_of_demand_point[i]);

                }
            }

        }
        if (_min_xDock_model)
        {
            for (int j = 0; j < _numOfXdocks; j++)
            {
                _objective.AddTerm(y[j], 1);
            }
        }


        _solver.AddMinimize(_objective);

    }

    private void CreateDecisionVariables()
    {
        // Create x[i,j]-variables
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var x_i = new List<INumVar>();
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var name = $"x[{i + 1}][{(j + 1)}]";
                var x_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
                x_i.Add(x_ij);
            }
            x.Add(x_i);
        }

        //Create y[j] variables
        for (int j = 0; j < _numOfXdocks; j++)
        {
            var name = $"y[{(j + 1)}]";
            var y_j = _solver.NumVar(0, 1, NumVarType.Bool, name);
            y.Add(y_j);
        }

        //Create z[i] variables
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var name = $"z[{(i + 1)}]";
            var z_i = _solver.NumVar(0, 1, NumVarType.Bool, name);
            z.Add(z_i);
        }

        //Create f[i,j] variables
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var f_i = new List<INumVar>();
            for (int j = 0; j < _numOfXdocks; j++)
            {
                var name = $"f[{i + 1}][{(j + 1)}]";
                var f_ij = _solver.NumVar(0, 1, NumVarType.Bool, name);
                f_i.Add(f_ij);
            }
            f.Add(f_i);
        }

        //Create k[i] variables
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var name = $"k[{(i + 1)}]";
            var k_i = _solver.NumVar(0, Int32.MaxValue, NumVarType.Float, name);
            k.Add(k_i);
        }

        //Create mu[i] variables
        for (int i = 0; i < _num_of_demand_point; i++)
        {
            var name = $"mu[{(i + 1)}]";
            var mu_i = _solver.NumVar(0, Int32.MaxValue, NumVarType.Float, name);
            mu.Add(mu_i);
        }
    }

    public void ClearModel()
    {
        _solver.ClearModel();
        _solver.Dispose();
    }







}
