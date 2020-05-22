using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Genetic_Algorithm : Heuristic
    {
        private List<List<Double>> population = new List<List<double>>();
        private Double population_size = 10;
        private List<Score> Score_List = new List<Score>();
        private List<Score> new_score_list = new List<Score>();
        private List<Int32> already_open_list = new List<Int32>();
        private Double crossover_probability = 0.70;
        private Double mutation_probability = 0.70;
        private Int32 elitist_size = 2;
        private Random rand = new Random();
        private Int32 chromosome_length;
        private Int32 iteration_count = 1000;
        private Double infeasible_acceptance_percentage = 0.00;
        private Double alpha = 0.005;
        private Dictionary<Int32,Double> best_score_matrix = new Dictionary<Int32, Double>();
        private Dictionary<Int32, Double> time_matrix = new Dictionary<Int32, Double>();
        private List<Double> best_solution = new List<double>();
        private List<Double> final_assignments = new List<double>();
        private List<Double> final_chromosome = new List<double>();
        private List<Double> heuristic_pairs = new List<double>();
        private Double min_cap = new double();
        private Double covered_demand = new double();
        private Dictionary<String, Double> solution_list = new Dictionary<string, double>();
        private String heuristic_name = "Genetic Algorithm";



        public Genetic_Algorithm(List<Double> solution, List<xDocks> _xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock, String key) : base(solution,_xDocks, demandPoints, parameters, lm_coverage, num_xdock, key)
        {
            chromosome_length = _xDocks.Count;
            var index = parameters.FindIndex(x => x.Get_Key().Equals(key));
            //min_cap = parameters[index].Get_Min_Cap();
        }


        private Tuple<List<Double>,List<xDocks>> Create_New_Choromosome()
        {
            var new_xDocks = new List<xDocks>();
            List<Double> new_choromosome = Enumerable.Repeat(0.0, chromosome_length).ToList();
            if (already_open_list.Count > 0)
            {
                for (int a = 0; a < already_open_list.Count; a++)
                {
                    new_choromosome[already_open_list[a]] = 1;
                    new_xDocks.Add(_xDocks[already_open_list[a]]);
                }
            }
            Random rand = new Random();
            var remaining_xDock_Need = _num_xDock - already_open_list.Count;
            for (int j = 0; j < remaining_xDock_Need; j++)
            {
                var index = rand.Next(chromosome_length);
                while (new_choromosome[index] == 1)
                {
                    index = rand.Next(chromosome_length);
                }
                new_choromosome[index] = 1;
                new_xDocks.Add(_xDocks[index]);
            }

            return Tuple.Create(new_choromosome,new_xDocks);
        }

        private Tuple<List<Double>, Double, Boolean> Suitability_Check(List<Double> chromosome,Double current_population_size,Double infeasible_sol_count,List<xDocks> opened_xDocks)
        {
            var xDocks = new List<xDocks>();
            var obj_value_for_chromosome = 0.0;
            var model = new DemandxDockModel(_demand_Points, opened_xDocks, _key,false, false, _lm_coverage,false, _num_xDock,false, 0.01,10, true);
            model.Run();
            var is_feasible = model.Return_Status();
            if (!is_feasible)
            {
                double infeasible_ratio = (infeasible_sol_count + 1) / (current_population_size + 1);
                if (infeasible_ratio > infeasible_acceptance_percentage)
                {
                    var new_choromosome = new List<Double>();
                    while (!is_feasible)
                    {
                        (new_choromosome,xDocks) = Create_New_Choromosome();
                        var model1 = new DemandxDockModel(_demand_Points, xDocks, _key, false, false, _lm_coverage, false, _num_xDock, false, 0.01,10, true);
                        model1.Run();
                        is_feasible = model1.Return_Status();
                        obj_value_for_chromosome = model1.GetObjVal();
                    }
                    return Tuple.Create(new_choromosome, obj_value_for_chromosome, is_feasible);
                }
                else
                {
                    obj_value_for_chromosome = model.GetObjVal();
                    return Tuple.Create(chromosome,obj_value_for_chromosome,is_feasible);
                }
            }
            else
            {
                obj_value_for_chromosome = model.GetObjVal();
                return Tuple.Create(chromosome, obj_value_for_chromosome, is_feasible);
            }
        } 

        private void Create_Initial_Population()
        {
            var xDocks = new List<xDocks>();
            var current_infeasible_count = 0.0;
            var chromosome_status = true;
            var objective_value = 0.0;
            //Burayı düzelt
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    xDocks.Add(_xDocks[i]);
                }
            }
            (_solution, objective_value, chromosome_status) = Suitability_Check(_solution, Score_List.Count, current_infeasible_count,xDocks);
            population.Add(_solution);
            var score = new Score(0, objective_value);
            var string_sol = String.Join(',', _solution);
            solution_list.Add(string_sol, objective_value);
            Score_List.Add(score);
            for (int k = 0; k < _xDocks.Count; k++) if (_xDocks[k].If_Already_Opened()) already_open_list.Add(k); ;
            for (int i = 1; i < population_size; i++)
            {
                var selected_chromosome = new List<Double>();
                var new_choromosome = new List<Double>();
                (new_choromosome,xDocks)= Create_New_Choromosome();
                string_sol = String.Join(',', new_choromosome);
                if (solution_list.ContainsKey(string_sol))
                {
                    solution_list.TryGetValue(string_sol,out objective_value);
                    population.Add(new_choromosome);
                }
                else
                {
                    (selected_chromosome, objective_value, chromosome_status) = Suitability_Check(new_choromosome, Score_List.Count, current_infeasible_count, xDocks);
                    population.Add(selected_chromosome);
                    string_sol = String.Join(',', selected_chromosome);
                    solution_list.Add(string_sol, objective_value);
                }
                score = new Score(i, objective_value);
                Score_List.Add(score);
                if (!chromosome_status)
                {
                    current_infeasible_count += 1;
                }
            }  
        }

        private void Eliminate_Initial_Population()
        {
            var sorted_score_list = Score_List.OrderBy(x => x.Get_Fitness_Score()).ToList();
            var initial_population = new List<List<Double>>(); 
            for (int i = 0; i < population_size; i++)
            {
                var index = sorted_score_list[i].Get_ID();
                initial_population.Add(population[index]);
            }
            population = initial_population;
            Score_List = sorted_score_list.GetRange(0, Convert.ToInt32( population_size));
            for (int i = 0; i < population_size; i++)
            {
                Score_List[i].Set_ID(i);
            }
        }

        private void Evaluation()
        {
            var max_obj = Score_List.Max(x => x.Get_Obj_Value());
            var min_obj = Score_List.Min(x => x.Get_Obj_Value());
            var difference = max_obj - min_obj;
            for (int i = 0; i < population.Count; i++)
            {
                var fitness_score = 0.0;
                if (difference == 0)
                {
                    fitness_score = 1;
                }
                else
                {
                    fitness_score = (max_obj - Score_List[i].Get_Obj_Value()) / difference;
                }
                Score_List[i].Set_Fitness_Score(fitness_score);
            }

            var cumulative_fitness_score = Score_List.Sum(x => x.Get_Fitness_Score());
            var cumulative_probability = 0.0;
            for (int j = 0; j < population.Count; j++)
            {
                var probability = Score_List[j].Get_Fitness_Score() / cumulative_fitness_score;
                Score_List[j].Set_Prob(probability);
                cumulative_probability += probability;
                Score_List[j].Set_Cum_Prob(cumulative_probability);
            }
        }

        protected override void Swap()
        {

        }

        private void Select_Population()
        {
            //Take best n choromosome to next population
            var new_population = new List<List<Double>>();
            var new_scores = new List<Score>();
            (new_population, new_scores) = Elitist_Policy();
            //Current Infeasible solutions in the population
            var current_infeasible_count = 0.0;
            var chromosome_status = true;
            var objective_value = 0.0;
            for (int i = elitist_size; i < population_size; i++)
            {
                var selected_chromosome = Roulette_Wheel();
                if (selected_chromosome.Count == 0)
                {
                    Console.WriteLine("sad");
                }
                var probability_for_crossover = rand.Next(1001) / 1000.0;
                if (probability_for_crossover < crossover_probability)
                {
                    var crossover_chromosome = Roulette_Wheel();
                    if (crossover_chromosome.Count == 0)
                    {
                        Console.WriteLine("sad");
                    }
                    var updated_chromosome = Crossover_Population(selected_chromosome, crossover_chromosome);
                    selected_chromosome = updated_chromosome;
                }
                var probability_for_mutation = rand.Next(1001) / 1000.0;
                if (probability_for_mutation < mutation_probability)
                {
                    var mutated_chromosome = Mutate_Population(selected_chromosome);
                    selected_chromosome = mutated_chromosome;
                }
                var num_xDock_in_chromosome = selected_chromosome.Where(x => x.Equals(1)).Count();
                if (num_xDock_in_chromosome == 0)
                {
                    Console.WriteLine("sad");
                }
                if (_num_xDock != num_xDock_in_chromosome)
                {
                    selected_chromosome = Repair_Chromosome(selected_chromosome, num_xDock_in_chromosome);
                }
                var new_xDocks = Update_Open_xDock(selected_chromosome);
                var final_chromosome = new List<Double>();
                //Buranın konuşulması gerek
                var string_sol = String.Join(',', selected_chromosome);
                if (solution_list.ContainsKey(string_sol))
                {
                    solution_list.TryGetValue(string_sol, out objective_value);
                    new_population.Add(selected_chromosome);
                }
                else
                {
                    (final_chromosome, objective_value, chromosome_status) = Suitability_Check(selected_chromosome, new_population.Count, current_infeasible_count, new_xDocks);
                    new_population.Add(final_chromosome);
                    string_sol = String.Join(',', final_chromosome);
                    solution_list.Add(string_sol, objective_value);
                }
                var score = new Score(i, objective_value);
                new_scores.Add(score);
                

                if (!chromosome_status)
                {
                    current_infeasible_count += 1;
                }
            }
            population.Clear();
            Score_List.Clear();
            population.AddRange(new_population);
            Score_List.AddRange(new_scores);
        }

        private List<Double> Repair_Chromosome(List<Double> chromosome,Int32 xDock_count)
        {
            var index_matrix = new List<Int32>();
            for (int i = 0; i < chromosome_length; i++)
            {
                if (chromosome[i] == 1 && !_xDocks[i].If_Already_Opened())
                {
                    index_matrix.Add(i);
                }
            }
            //Extra xDock Case
            if (xDock_count>_num_xDock)
            {
                var difference = xDock_count - _num_xDock;
                for (int i = 0; i < difference; i++)
                {
                    var rand_index = rand.Next(index_matrix.Count);
                    var extracted_index = index_matrix[rand_index];
                    chromosome[extracted_index] = 0;
                    index_matrix.Remove(extracted_index);
                }
            }
            else
            {
                var difference = _num_xDock - xDock_count;
                for (int i = 0; i < difference; i++)
                {
                    var added_index = rand.Next(chromosome_length);
                    if (chromosome[added_index] == 1)
                    {
                        var correct_index = false;
                        while (!correct_index)
                        {
                            added_index = rand.Next(chromosome_length);
                            if (chromosome[added_index] == 1)
                            {
                                correct_index = true;
                            }
                        }
                    }
                    chromosome[added_index] = 1;
                }
            }
            return chromosome;
        }

        private List<xDocks> Update_Open_xDock(List<Double> chromosome)
        {
            var new_xDocks = new List<xDocks>();
            for (int i = 0; i < chromosome.Count; i++)
            {
                if (chromosome[i] == 1)
                {
                    new_xDocks.Add(_xDocks[i]);
                }
            }
            return new_xDocks;
        }


        private List<Double> Mutate_Population(List<Double> chromosome)
        {
            var mutated_chromosome = new List<Double>();
            var selection = rand.Next(1, 3);
            if (selection == 1) mutated_chromosome= Swap_Mutation(chromosome);
            if (selection == 2) mutated_chromosome = Bit_Mutation(chromosome);
            return mutated_chromosome;
        }

        private List<Double> Bit_Mutation(List<Double> chromosome)
        {
            var location = rand.Next(chromosome_length);
            if (_xDocks[location].If_Already_Opened())
            {
                var false_loc = true;
                while (false_loc)
                {
                    location = rand.Next(chromosome_length);
                    if (!(_xDocks[location].If_Already_Opened())) false_loc = false;
                }
            }
            if (chromosome[location] == 1) chromosome[location] = 0;
            else chromosome[location] = 1;
            return chromosome;
        }

        private List<Double> Swap_Mutation(List<Double> chromosome)
        {
            var location1 = rand.Next(chromosome_length);
            var location2 = rand.Next(chromosome_length);
            bool suitable = true;
            if (_xDocks[location1].If_Already_Opened() || _xDocks[location2].If_Already_Opened())
            {
                suitable = false;
            }
            while (!suitable)
            {
                suitable = true;
                location1 = rand.Next(chromosome_length);
                location2 = rand.Next(chromosome_length);
                if (_xDocks[location1].If_Already_Opened() || _xDocks[location2].If_Already_Opened())
                {
                    suitable = false;
                }
            }
            var value1 = 0;
            var value2 = 0;
            if (chromosome[location1] == 1) value1 = 1;
            if (chromosome[location2] == 1) value2 = 1;
            chromosome[location1] = value2;
            chromosome[location2] = value1;

            return chromosome;
        }

        private List<Double> Crossover_Population(List<Double> c_1, List<Double> c_2)
        {
            var updated_chromosome = new List<Double>();
            var selection = rand.Next(1, 3);
            if(selection == 1) updated_chromosome = One_Point_Crossover(c_1, c_2);
            if (selection == 2) updated_chromosome = Two_Point_Crossover(c_1, c_2);
            return updated_chromosome;
        }

        private List<Double> One_Point_Crossover(List<Double> c_1, List<Double> c_2)
        {
            var division_point = rand.Next(c_1.Count-1);
            var chromosome_part1 = c_1.GetRange(0, division_point);
            var chromosome_part2 = c_2.GetRange(division_point, c_2.Count-division_point);
            chromosome_part1.AddRange(chromosome_part2);
            return chromosome_part1;
        }

        private List<Double> Two_Point_Crossover(List<Double> c_1, List<Double> c_2)
        {
            var division_point1 = rand.Next(chromosome_length-1);
            var division_point2 = rand.Next(division_point1+1,chromosome_length);
            var chromosome_part1 = c_1.GetRange(0, division_point1);
            var chromosome_part2 = c_2.GetRange(division_point1, division_point2 - division_point1);
            var chromosome_part3 = c_1.GetRange(division_point2, chromosome_length - division_point2);
            chromosome_part1.AddRange(chromosome_part2);
            chromosome_part1.AddRange(chromosome_part3);
            return chromosome_part1;
        }

        private List<Double> Roulette_Wheel()
        {
            var selected_chromosome = new List<Double>();
            double probability = rand.Next(1000) / 1000.0;
            var found = false;
            for (int i = 0; i < population_size && !found; i++)
            {
                if (Score_List[i].Get_Cum_Prob() > probability)
                {
                    found = true;
                    selected_chromosome.AddRange(population[i]);
                }
            }
            return selected_chromosome;
        }

        private Tuple<List<List<Double>>,List<Score>> Elitist_Policy()
        {
            var new_list = new List<Score>();
            var sorted_list = Score_List.OrderByDescending(x => x.Get_Fitness_Score()).ToList();
            var new_population = new List<List<Double>>();
            for (int i = 0; i < elitist_size; i++)
            {
                var index = sorted_list[i].Get_ID();
                new_population.Add(population[index]);
                var score = new Score(i, sorted_list[i].Get_Obj_Value());
                new_list.Add(score);
            }
            return Tuple.Create(new_population,new_list);
        }

        protected override void Optimize()
        {
            Console.WriteLine("I'm genetic algorithm :)");
            List<Double> new_choromosome = Enumerable.Repeat(0.0, chromosome_length).ToList();
            new_choromosome[11] = 1;
            new_choromosome[100] = 1;
            var value=Test_Chromosome_Evaluation(new_choromosome);
            Create_Initial_Population();
            Evaluation();
           // Eliminate_Initial_Population();
            Run_Algorithm();
        }

        private Double Test_Chromosome_Evaluation(List<Double> chromosome)
        {
            var xDocks = Update_Open_xDock(chromosome);
            var model = new DemandxDockModel(_demand_Points, xDocks, _key, false, false, _lm_coverage, false, _num_xDock, false, 0.01, 30 ,true);
            model.Run();
            var a = model.GetObjVal();
            return a;
        }
        private Double Find_Covered_Demand(List<xDocks> xDocks) 
        {
            var whole_demand = _demand_Points.Sum(x => x.Get_Demand());
            var assigned_demand = 0.0;
            for (int i = 0; i < xDocks.Count; i++)
            {
                assigned_demand += xDocks[i].Get_LM_Demand();
            }
            var covered_demand = assigned_demand / whole_demand;
            return covered_demand;

        }

        public Tuple<List<Double>,List<Double>> Return_Best_Solution()
        {
            return Tuple.Create(best_solution, heuristic_pairs);
        }

        public Double Return_Covered_Demand()
        {
            return covered_demand;
        }

        private void Run_Algorithm()
        {
            var start_time = DateTime.Now;
            var best_obj = Double.MaxValue;
            for (int i = 0; i < iteration_count; i++)
            {
                Console.WriteLine("Iteration Count: {0}\n", i);
                Select_Population();
                Evaluation();
                var new_best_obj = Score_List.Min(x => x.Get_Obj_Value());
                if (new_best_obj<best_obj)
                {
                    best_obj = new_best_obj;
                    var score = Score_List.Where(x => x.Get_Obj_Value().Equals(new_best_obj)).First();
                    best_solution = population[score.Get_ID()];
                }
                var current_time = DateTime.Now;
                var time_difference = (current_time - start_time).TotalSeconds;
                if(time_difference > 600)
                {
                    start_time = DateTime.Now;
                    time_matrix.Add(i, best_obj);
                }
                best_score_matrix.Add(i, best_obj);
                Console.WriteLine("Best objective: {0}\n",best_obj);
                //if (infeasible_acceptance_percentage > 0.01)
                //{
                //    infeasible_acceptance_percentage -= alpha;
                //}
                //if (population_size > 100)
                //{
                //    if (iteration_count % 10 == 0)
                //    {
                //        population_size -= 1;
                //    }
                //}
            }
            Console.WriteLine("Finish");
            var xDocks = Update_Open_xDock(best_solution);
            var model = new DemandxDockModel(_demand_Points, xDocks, _key, false, false, _lm_coverage, false, _num_xDock, false, 0.01, 30 ,true);
            model.Run();
            var a = model.GetObjVal();
            var final_xDocks = model.Return_XDock();
            covered_demand = Find_Covered_Demand(final_xDocks);
            var model_assignments = model.Return_Heuristic_Assignment();
            heuristic_pairs = Create_Initial_Solution_Procedure(best_solution, model_assignments);
            Console.WriteLine("Finish");
        }
    }

    internal class Score
    {
        private Int32 ID;
        private Boolean solution_status;
        private Double obj_score ;
        private Double fitness_score = 0;
        private Double probability = 0;
        private Double cum_probability = 0;
        public Score(Int32 id,Double objective_value)
        {
            ID = id;
            obj_score = objective_value;
        }

        public Boolean Get_Status()
        {
            return solution_status;
        }

        public void Set_Status(Boolean Status)
        {
            solution_status = Status;
        }

        public Int32 Get_ID()
        {
            return ID;
        }

        public void Set_ID(Int32 id)
        {
            ID = id;
        }

        public Double Get_Fitness_Score()
        {
            return fitness_score;
        }

        public Double Get_Probability()
        {
            return probability;
        }

        public Double Get_Cum_Prob()
        {
            return cum_probability;
        }

        public Double Get_Obj_Value()
        {
            return obj_score;
        }

        public void Set_Fitness_Score(double score)
        {
            fitness_score = score;
        }

        public void Set_Prob(double prob)
        {
            probability = prob;
        }

        public void Set_Cum_Prob(double cum_prob)
        {
            cum_probability = cum_prob;
        }



    }
}
