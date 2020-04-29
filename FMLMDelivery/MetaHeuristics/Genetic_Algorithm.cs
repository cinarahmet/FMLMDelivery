using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Genetic_Algorithm : Heuristic
    {
        private List<List<Double>> population = new List<List<double>>();
        private List<List<Double>> new_population = new List<List<double>>();
        private Double population_size = 10;
        private List<Score> Score_List = new List<Score>();
        private List<Int32> already_open_list = new List<Int32>();
        private List<xDocks> open_xDocks = new List<xDocks>();


        public Genetic_Algorithm(List<Double> solution, List<List<Double>> assignments, List<xDocks> _xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock) : base(solution,assignments,_xDocks, demandPoints, parameters, lm_coverage, num_xdock)
        {

        }


        private List<Double> Create_New_Choromosome()
        {
            open_xDocks.Clear();
            var chromosome_length = _xDocks.Count;
            List<Double> new_choromosome = Enumerable.Repeat(0.0, chromosome_length).ToList();
            if (already_open_list.Count > 0)
            {
                for (int a = 0; a < already_open_list.Count; a++)
                {
                    new_choromosome[already_open_list[a]] = 1;
                    open_xDocks.Add(_xDocks[already_open_list[a]]);
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
                open_xDocks.Add(_xDocks[index]);
            }

            return new_choromosome;
        }

        private void Create_Initial_Population()
        {
            Create_Objective();
            var score = new Score(objective_value);
            Score_List.Add(score);
            population.Add(_solution);
            for (int k = 0; k < _xDocks.Count; k++) if (_xDocks[k].If_Already_Opened()) already_open_list.Add(k); ;
            for (int i = 0; i < population_size - 1; i++)
            {
                var feasible_sol = false;
                var new_choromosome = new List<Double>();
                while (!feasible_sol)
                {
                    Console.WriteLine("Looking for feasible solution");
                    new_choromosome = Create_New_Choromosome();
                    Console.WriteLine("Chromosome created");
                    _solution = new_choromosome;
                    var model = new DemandxDockModel(_demand_Points, open_xDocks, "ANKARA", true, false, _lm_coverage, 1250, true, _num_xDock, false, 0.01, true);
                    model.Run();

                    Console.WriteLine("Assignments completed");
                    feasible_sol = Check_Feasibility();
                    Console.WriteLine("Feasibilty check finished");
                }
                var model1 = new DemandxDockModel(_demand_Points, open_xDocks, "ANKARA", true, false, _lm_coverage, 1250, true, _num_xDock, false, 0.01, true);
                model1.Run();
                var obj_val=model1.GetObjVal();
                Create_Objective();
                score = new Score(obj_val);
                Score_List.Add(score);
                population.Add(new_choromosome);
            }  
        }

        private void Evaluation()
        {
            var max_obj = Score_List.Max(x => x.Get_Obj_Value());
            var min_obj = Score_List.Min(x => x.Get_Obj_Value());
            var difference = max_obj - min_obj;
            for (int i = 0; i < population_size; i++)
            {
                var fitness_score = (max_obj - Score_List[i].Get_Obj_Value()) / difference;
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
            
        }

        protected override void Optimize()
        {
            Console.WriteLine("I'm genetic algorithm :)");
            Create_Initial_Population();
            Evaluation();
            Check_Feasibility();
        }
    }

    internal class Score
    {
        private Double obj_score ;
        private Double fitness_score = 0;
        private Double probability = 0;
        private Double cum_probability = 0;
        public Score(Double objective_value)
        {
            obj_score = objective_value;
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
