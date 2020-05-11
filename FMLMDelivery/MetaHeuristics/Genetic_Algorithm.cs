using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Genetic_Algorithm : Heuristic
    {
        private List<List<Double>> population = new List<List<double>>();
        private Double population_size = 20;
        private List<Score> Score_List = new List<Score>();
        private List<Score> new_score_list = new List<Score>();
        private List<Int32> already_open_list = new List<Int32>();
        private Double crossover_probability = 0.40;
        private Double mutation_probability = 0.40;
        private Int32 elitist_size = 4;
        private Random rand = new Random();
        private Int32 chromosome_length;
        private Int32 iteration_count = 100;
        private Double infeasible_acceptance_percentage = 0.30;
        private Double alpha = 0.005;
        private Dictionary<Int32,Double> best_score_matrix = new Dictionary<Int32, Double>();
        private Dictionary<Int32, List<Double>> best_solution_matrix = new Dictionary<int, List<double>>();
        private List<Double> final_assignments = new List<double>();
        private List<Double> final_chromosome = new List<double>();




        public Genetic_Algorithm(List<Double> solution, List<List<Double>> assignments, List<xDocks> _xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock, String key) : base(solution,assignments,_xDocks, demandPoints, parameters, lm_coverage, num_xdock, key)
        {
            chromosome_length = _xDocks.Count;
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
            var model = new DemandxDockModel(_demand_Points, opened_xDocks, _key,false, false, _lm_coverage, 1250,false, _num_xDock,false, 0.01,3600, true);
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
                        var model1 = new DemandxDockModel(_demand_Points, xDocks, _key, false, false, _lm_coverage, 1250, false, _num_xDock, false, 0.01,3600, true);
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
            Score_List.Add(score);
            for (int k = 0; k < _xDocks.Count; k++) if (_xDocks[k].If_Already_Opened()) already_open_list.Add(k); ;
            for (int i = 1; i < population_size; i++)
            {
                var selected_chromosome = new List<Double>();
                var new_choromosome = new List<Double>();
                (new_choromosome,xDocks)= Create_New_Choromosome();
                (selected_chromosome, objective_value, chromosome_status) = Suitability_Check(new_choromosome, Score_List.Count, current_infeasible_count,xDocks);
                population.Add(selected_chromosome);
                score = new Score(i, objective_value);
                Score_List.Add(score);
                if (!chromosome_status)
                {
                    current_infeasible_count += 1;
                }
            }  
        }

        private void Evaluation()
        {
            var max_obj = Score_List.Max(x => x.Get_Obj_Value());
            var min_obj = Score_List.Min(x => x.Get_Obj_Value());
            var difference = max_obj - min_obj;
            for (int i = 0; i < population_size; i++)
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
            for (int j = 0; j < population_size; j++)
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
                (final_chromosome, objective_value, chromosome_status) = Suitability_Check(selected_chromosome, new_population.Count, current_infeasible_count,new_xDocks);
                var score = new Score(i, objective_value);
                new_scores.Add(score);
                new_population.Add(final_chromosome);

                if (!chromosome_status)
                {
                    current_infeasible_count += 1;
                }
            }
            population = new_population;
            Score_List = new_scores;
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
                    selected_chromosome = population[i];
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
            Create_Initial_Population();
            Evaluation();
            Run_Algorithm();
        }

        private void Run_Algorithm()
        {
            
            for (int i = 0; i < iteration_count; i++)
            {
                Console.WriteLine("Iteration Count: {0}\n", i);
                Select_Population();
                Evaluation();
                var best_obj = Score_List.Min(x => x.Get_Obj_Value());
                var sorted_list = Score_List.OrderByDescending(x => x.Get_Fitness_Score()).ToList();
                var index = sorted_list[0].Get_ID();
                best_solution_matrix.Add(i, population[index]);
                best_score_matrix.Add(i, best_obj);
                Console.WriteLine("Best objective: {0}\n",best_obj);
                if (infeasible_acceptance_percentage > 0.01)
                {
                    infeasible_acceptance_percentage -= alpha;
                }
            }

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
