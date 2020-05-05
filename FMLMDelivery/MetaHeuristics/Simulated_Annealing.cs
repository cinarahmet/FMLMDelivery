using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery.Classes;

namespace FMLMDelivery.MetaHeuristics
{
    

    public class Simulated_Annealing : Heuristic
    {
        private Double iteration = 5000;
        private Double Temperature=1000000;
        private double alpha_temp=0.99;
        private double alpha_km = 0.05;
        private double diversification_km = 30;
        private Double intensification_km=5;
        private Double lb_intens = 2;
        private List<xDock_Demand_Point_Pairs> candidate_pairs = new List<xDock_Demand_Point_Pairs>();
        private double best_objective = new double();
        private double objective = new Double();
        private List<xDocks> opened_xdocks = new List<xDocks>();
        private List<Double> score_table = new List<Double>();
        private List<Double> old_soln = new List<Double>();

        public Simulated_Annealing(List<Double> solution, List<List<Double>> assignments, List<xDocks> xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock) : base(solution, assignments, xDocks, demandPoints, parameters, lm_coverage, num_xdock)
        {

        }
        protected override void Optimize()
        {
            Iteration_Solution_Selection();
        }
        private void Initialize_Score_Table()
        {
            for (int i = 0; i < _solution.Count; i++)
            {
                score_table.Add(1);
            }
        }

        private void Weighing_of_Points(List<Double> solution, List<Double> score_table)
        {
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    var increase = score_table[i];
                    increase = increase + 1;
                    score_table[i] = increase;
;               }
            }
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
        private Double Run_xDock_Demand_Point(Boolean Model_run)
        {   var located_xdocks = new List<xDocks>();
            var city_name ="";
            var min_cap = new Double();
            
            if (Model_run)
            {
                for (int i = 0; i < _solution.Count; i++)
                {
                    if (_solution[i] == 1)
                    {
                        located_xdocks.Add(_pairs[i].Get_xDock());
                    }
                    
                }
                for (int i = 0; i < _parameters.Count; i++)
                {
                    city_name = located_xdocks[0].Get_City();
                    if (_parameters[i].Get_Key() == located_xdocks[0].Get_City())
                    {
                        min_cap = _parameters[i].Get_Min_Cap();
                    }
                }

                var assignment = new DemandxDockModel(_demand_Points, located_xdocks, city_name, true, false, _lm_coverage, min_cap, true, _num_xDock, false, 0.05, true);
                assignment.Run();
                objective = assignment.GetObjVal();
            }
            else
            {
                Assignment_Procedure();
            }

            return objective;
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
        private void Iteration_Solution_Selection()
        {
            var random = new Random();
            var new_objective = Double.MaxValue;
            var diversification = true;
            var model_run = true;
            var best_objective = Double.MaxValue;
            var old_objective= Double.MaxValue;
            var best_model_objective = new List<Double>();
            var time = DateTime.Now;
            var count = 0;
            for (int i = 0; i < iteration; i++)
            {
                if (i != 0)
                {
                    var list = Neighborhood_Generation(diversification);
                    Neighborhood_Selection(list);
                }
                Run_xDock_Demand_Point(model_run);
                if (objective != 0)
                {
                    count += 1;
                    Console.WriteLine("Feasible Solution Found");
                    new_objective = objective;
                    var difference = new_objective - best_objective;
                    if (best_objective <= new_objective)
                    {
                        //if (old_objective <= new_objective)
                        //{
                        //    var propa = random.NextDouble();
                        //    difference = new_objective - old_objective;
                        //    var exp_temp = Math.Exp(-difference / Temperature);
                        //    diversification = true;
                        //    if (propa > exp_temp)
                        //    {   
                        //        _solution.Clear();
                        //        _solution.AddRange(_old_solution);
                        //        diversification = false;
                        //    }
                        //}
                        //else
                        //{
                        //    diversification = false;
                        //}
                        var propa = random.NextDouble();
                        var exp_temp = Math.Exp(-difference / Temperature);
                        diversification = true;
                        if (propa > exp_temp)
                        {   //new if condition will be added
                            _solution.Clear();
                            _solution.AddRange(_best_solution);
                            diversification = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Improvement found with new objective{0}", new_objective);
                        _best_solution = new List<double>();
                        best_objective = new_objective;
                        _best_solution.AddRange(_solution);
                        best_model_objective.Add(best_objective);
                        if (intensification_km > lb_intens) intensification_km = intensification_km - intensification_km * alpha_km;
                        diversification = false;
                    }
                    old_soln.Clear();
                    old_soln.AddRange(_solution);
                    old_objective = new_objective;
                }
                Temperature = Temperature * alpha_temp;
                Console.WriteLine("Tempereature:{0} \t objective value:{1}", Temperature, best_model_objective[best_model_objective.Count - 1]);
                
            }
            var xdocks = new List<xDocks>();
            xdocks = Get_Information_Xdocks(_best_solution);
            var endtime = DateTime.Now;
            Console.WriteLine("Time Passed{0}", (endtime - time));
            Console.WriteLine("Number of Feasible Solution Found{0}", count);
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
    }
}
