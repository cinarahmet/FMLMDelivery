using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FMLMDelivery.Classes;

namespace FMLMDelivery.MetaHeuristics
{
    
    public class Simulated_Annealing : Heuristic
    {
        private Double iteration = 5000;
        private Double Temperature=1000000;
        private Double count_temp = 10;
        private Double nofeas_iteration = 500;
        private double alpha_temp=0.99;
        private double alpha_km = 0.05;
        private double diversification_km = 30;
        private Double intensification_km=5;
        private Double lb_intens = 2;
        private List<xDock_Demand_Point_Pairs> candidate_pairs = new List<xDock_Demand_Point_Pairs>();
        private double best_objective = new double();
        private double objective = new Double();
        private bool status = new bool();
        private List<xDocks> opened_xdocks = new List<xDocks>();
        private List<Double> score_table = new List<Double>();
        private List<Double> old_soln = new List<Double>();
        private List<Double> heuristic_assignments = new List<double>();
        private List<Double> Solution_to_send = new List<Double>();
        private List<Double> assignment_to_send = new List<Double>();

        public Simulated_Annealing(List<Double> solution, List<List<Double>> assignments, List<xDocks> xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock,String key) : base(solution, assignments, xDocks, demandPoints, parameters, lm_coverage, num_xdock,key)
        {

        }
        protected override void Optimize()
        {
            Algorithm();
        }
        public Tuple<List<Double>,List<Double>> Return_Heuristic_Results()
        {
            return Tuple.Create(Solution_to_send, assignment_to_send);
            
        }
        private void Temperature_Revision(int Count)
        {
            if (Count% count_temp == 0) Temperature = Temperature * alpha_temp;
        }

        private List<xDocks> Get_Information_Xdocks(List<Double> best_solution)
        {
            for (int i = 0; i < best_solution.Count; i++)
            {
                if (best_solution[i] == 1) opened_xdocks.Add(_pairs[i].Get_xDock());
            }
            return opened_xdocks;
        }
        protected override void Swap()
        {
            throw new NotImplementedException();    
        }
        private Tuple<Double,Boolean,List<Double>> Run_xDock_Demand_Point()
        {   var located_xdocks = new List<xDocks>();
            var min_cap = new Double();
            var _objective = new Double();
            var _assignments = new List<Double>();
            var _status = new bool();
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    located_xdocks.Add(_pairs[i].Get_xDock());
                }

            }
            for (int i = 0; i < _parameters.Count; i++)
            {
                if (_parameters[i].Get_Key() == located_xdocks[0].Get_City())
                {
                    min_cap = _parameters[i].Get_Min_Cap();
                }
            }

            var assignment = new DemandxDockModel(_demand_Points, located_xdocks, _key, true, false, _lm_coverage, min_cap, true, _num_xDock, false, 0.05, 60, true);
            assignment.Run();
            _objective = assignment.GetObjVal();
            _status = assignment.Return_Status();
            _assignments = assignment.Return_Assignments();
            return Tuple.Create(_objective, _status,_assignments);
        }
        private List<List<int>> Neighborhood_Generation(Boolean diversification)
        {   var list_of_possible_points = new List<List<int>>();
            
            for (int i = 0; (i < _solution.Count) ; i++)
            {
                var list_index = new List<int>();
                if (_solution[i] == 1 && !diversification)
                {
                    for (int j = 0; (j < _solution.Count); j++)
                    {
                        var distance = Calculate_Distances(_pairs[i].Get_xDock().Get_Longitude(), _pairs[i].Get_xDock().Get_Latitude(), _pairs[j].Get_xDock().Get_Longitude(),_pairs[j].Get_xDock().Get_Latitude());
                        if (distance < intensification_km && !_pairs[i].Get_xDock().If_Already_Opened())
                        {
                            list_index.Add(j);
                            _solution[i] = 0;
                        }
                    }
                    list_of_possible_points.Add(list_index);
                }
                else if(_solution[i] == 1 && diversification)
                {
                    for (int j = 0; (j < _solution.Count); j++)
                    {
                        var distance = Calculate_Distances(_pairs[i].Get_xDock().Get_Longitude(), _pairs[i].Get_xDock().Get_Latitude(), _pairs[j].Get_xDock().Get_Longitude(), _pairs[j].Get_xDock().Get_Latitude());
                        if (distance < diversification_km && !_pairs[i].Get_xDock().If_Already_Opened())
                        {
                            list_index.Add(j);
                            _solution[i] = 0;
                        }
                    }
                    list_of_possible_points.Add(list_index);

                }
            }
           
            return list_of_possible_points;
        }
        private void Neighborhood_Selection(List<List<int>> generated_list)
        {   var random = new Random();
            for (int i = 0; i < generated_list.Count; i++)
            {
                var solution_found = false;
                while (solution_found == false && generated_list[i].Count != 0)
                {
                    var index = random.Next(generated_list[i].Count);
                    if (_solution[generated_list[i][index]] == 0)
                    {
                        _solution[generated_list[i][index]] = 1;
                        solution_found = true;
                    }

                }
            }
        }
        private void Algorithm()
        {
            var random = new Random();
            var results = new Results();
            var new_objective = Double.MaxValue;
            var best_objective = Double.MaxValue;
            var old_objective = Double.MaxValue;
            var diversification = true;
            var model_run = true;
            var best_model_objective = new List<Double>();
            var time = DateTime.Now;
            var feasible_count = 0;
            var temp_count = 0;
            var call_back = 0;
            for (int i = 0; i < iteration; i++)
            {
                temp_count += 1;
                call_back += 1;
                if (i != 0)
                {
                    var list = Neighborhood_Generation(diversification);
                    Neighborhood_Selection(list);
                }
                (objective,status,heuristic_assignments)=Run_xDock_Demand_Point();
                if (status==true)
                {
                    feasible_count += 1;
                    call_back = 1;
                    Console.WriteLine("Feasible Solution Found");
                    new_objective=objective;
                    var difference = new_objective - best_objective;
                    if (best_objective <= new_objective)
                    {
                        if (old_objective <= new_objective)
                        {
                            var propa = random.NextDouble();
                            difference = new_objective - old_objective;
                            var exp_temp = Math.Exp(-difference / Temperature);
                            diversification = true;
                            if (propa > exp_temp)
                            {
                                _solution.Clear();
                                _solution.AddRange(old_soln);
                                diversification = false;
                            }
                        }
                        else
                        {
                            diversification = false;
                        }
                        //var propa = random.NextDouble();
                        //var exp_temp = Math.Exp(-difference / Temperature);
                        //diversification = true;
                        //if (propa > exp_temp)
                        //{
                        //    _solution.Clear();
                        //    _solution.AddRange(_best_solution);
                        //    diversification = false;
                        //}
                    }
                    else
                    {
                        Console.WriteLine("Improvement found with new objective{0}", new_objective);
                        _best_solution = new List<double>();
                        var best_heu_assignments = new List<List<double>>();
                        best_objective = new_objective;
                        _best_solution.AddRange(_solution);
                        results.Add_Stats(best_objective, heuristic_assignments, _best_solution, Temperature);
                        if (intensification_km > lb_intens) intensification_km = intensification_km - intensification_km * alpha_km;
                        diversification = false;
                    }
                    old_soln.Clear();
                    old_soln.AddRange(_solution);
                    old_objective = new_objective;
                }
                else
                {
                    diversification = true;
                    if (call_back % nofeas_iteration == 0)
                    {
                        _solution.Clear();
                        _solution.AddRange(_best_solution);
                        old_objective = best_objective;
                        diversification = false;
                    }
                }
                Temperature_Revision(temp_count);
            }
            var xdocks = new List<xDocks>();
            xdocks = Get_Information_Xdocks(_best_solution);
            var endtime = DateTime.Now;
            Console.WriteLine("Time Passed{0}", (endtime - time));
            Solution_to_send = results.Return_Best_Solution_Array();
            assignment_to_send = results.Return_Best_Assignments();
           
        }
        public Double Get_Objective(List<xDock_Demand_Point_Pairs> pairs, List<Double> solution)
        {
            var objective = 0.0;
            var cap = 0.0;
            for (int i = 0; i < _pairs.Count; i++)
            {
                if (solution[i] == 1)
                {
                    for (int j = 0; j < pairs[i].Get_Demand_Point_List().Count; j++)
                    {
                        objective += pairs[i].Get_Demand_Point_List()[j].Get_Demand() * pairs[i].Get_Distance_List()[j];
                        cap += pairs[i].Get_Demand_Point_List()[j].Get_Demand();
                    }
                }
            }
            return objective;
        }

        internal class Results
        {
            private List<Double> _Best_Score=new List<double>();
            private List<List<Double>> _Best_Assignments=new List<List<double>>();
            private List<List<Double>> _Best_Solution=new List<List<double>>();
            private List<Double> _temp=new List<double>();
            public Results()
            {
            }

            public void Add_Stats(Double best_score, List<Double> best_assignments, List<Double> best_solution, Double temperature)
            {
                _Best_Score.Add(best_score);
                _Best_Assignments.Add(best_assignments);
                _Best_Solution.Add(best_solution);
                _temp.Add(temperature);
            }
            public Double Return_Best_Score()
            {
                return _Best_Score[_Best_Score.Count - 1];
            }
            public List<Double> Return_Best_Assignments()
            {
                return _Best_Assignments[_Best_Assignments.Count - 1];
            }
            public List<Double> Return_Best_Solution_Array()
            {
                return _Best_Solution[_Best_Solution.Count - 1];
            }
            public Double Current_Temp()
            {
                return _temp[_temp.Count - 1];
            }
        }
    }
}
