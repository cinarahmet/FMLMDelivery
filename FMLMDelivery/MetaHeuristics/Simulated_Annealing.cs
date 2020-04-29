using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMLMDelivery.Classes;

namespace FMLMDelivery.MetaHeuristics
{
    

    public class Simulated_Annealing : Heuristic
    {
        private Double iteration = 100000;
        private Double Temperature=1000000;
        private double alpha_temp=0.001;
        private double alpha_km = 0.01;
        private double diversification_km = 20;
        private Double intensification_km=10;
        private List<xDock_Demand_Point_Pairs> candidate_pairs = new List<xDock_Demand_Point_Pairs>();
        private double best_objective = new double();
        

        public Simulated_Annealing(List<Double> solution, List<List<Double>> assignments, List<xDocks> xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock) : base(solution, assignments, xDocks, demandPoints, parameters, lm_coverage, num_xdock)
        {

        }
        protected override void Optimize()
        {
            Iteration_Solution_Selection();
        }
        protected override void Swap()
        {

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
            var old_objective = new double();
            var new_objective = new double();
            var diversification = true;
            var best_objective = Double.MaxValue;
            _best_pairs.AddRange(_pairs);
            _best_solution.AddRange(_solution);
            old_objective = Get_Objective(_pairs, _solution);
            for (int i = 0; i < iteration; i++)
            {
                var list = Neighborhood_Generation(diversification);
                Neighborhood_Selection(list);
                Assignment_Procedure();
                if (Check_Feasibility())
                {   
                    //Console.WriteLine("Feasible Solution Found");
                    new_objective = Get_Objective(_pairs,_solution);
                    var difference = new_objective - best_objective;
                    
                    if (best_objective <= new_objective)
                    {
                        var propa = random.NextDouble();
                        var exp_temp = Math.Exp(-difference / Temperature);
                        diversification = true;
                        if (propa > exp_temp)
                        {
                            _pairs.Clear();
                            _solution.Clear();
                            _pairs.AddRange(_best_pairs);
                            _solution.AddRange(_best_solution);
                            diversification = false;
                        }
                    }
                    else
                    {
                        _best_pairs = new List<xDock_Demand_Point_Pairs>();
                        _best_solution = new List<double>();
                        //old_objective = new_objective;
                        best_objective = new_objective;
                        Console.WriteLine("İmrpovement found with new objective{0}", new_objective);
                        _best_pairs.AddRange(_pairs);
                        _best_solution.AddRange(_solution);
                        intensification_km = intensification_km - intensification_km * alpha_km;
                        diversification = false;
                    }
                }
                Temperature = Temperature - Temperature * alpha_temp;
            }
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
