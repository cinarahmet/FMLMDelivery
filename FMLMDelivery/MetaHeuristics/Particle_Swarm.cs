using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Particle_Swarm : Heuristic
    {
        private Int32 swarm_size = 20;
        private List<Particle> personalBests; 
        private Particle globalBest;
        private List<Particle> swarm = new List<Particle>();
        private List<Int32> already_open_list = new List<Int32>();
        private Int32 dimension;
        private double infeasible_acceptance_percentage = 0.0;
        private List<List<Double>> particle_positions = new List<List<double>>();
        private List<List<Double>> particle_velocities = new List<List<double>>();
        private List<Double> particle_objectives = new List<double>();
        private List<Score> personal_bests = new List<Score>();
        private List<Double> global_best_position = new List<double>();
        private List<Double> global_best_velocity = new List<double>();
        private Int32 max_iterations = 100;
        private Double starting_score = 10;

        public Particle_Swarm(List<double> solution, List<xDocks> xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, double lm_coverage, double num_xdock, string key) : base(solution, xDocks, demandPoints, parameters, lm_coverage, num_xdock, key)
        {
            dimension = xDocks.Count;
            globalBest = new Particle(dimension);
            personalBests=new List<Particle>(5);

        }

        private Tuple<Particle, List<xDocks>> Create_New_Particle()
        {
            var new_xDocks = new List<xDocks>();
            var particle = new Particle(dimension);

            if (already_open_list.Count > 0)
            {
                for (int i = 0; i < already_open_list.Count; i++)
                {
                    particle.Position[already_open_list[i]] = 1;
                    particle.Velocity[already_open_list[i]] = 0;
                    new_xDocks.Add(_xDocks[already_open_list[i]]);
                }
            }

            Random rand = new Random();
            var remaining_xDock_Need = _num_xDock - already_open_list.Count;
            for (int j = 0; j < remaining_xDock_Need; j++)
            {
                var index = rand.Next(dimension);
                while (particle.Position[index] == 1)
                {
                    index = rand.Next(dimension);
                }
                particle.Position[index] = 1;
                particle.Velocity[index] = 0;
                new_xDocks.Add(_xDocks[index]);
            }

            double vmin = 0.0;
            double vmax = 10.0;

            for (int k = 0; k < particle.Velocity.Count; k++)
            {
                var velocity = vmin + (vmax - vmin) * rand.NextDouble();
                particle.Velocity[k] = velocity;
            }

            if (already_open_list.Count > 0)
            {
                for (int l = 0; l < already_open_list.Count; l++)
                {
                    particle.Velocity[already_open_list[l]] = 0;
                }
            }

            return Tuple.Create(particle, new_xDocks);
        }

        private Particle Suitability_Check(Particle particle, Double current_population_size, Double infeasible_sol_count, List<xDocks> opened_xDocks)
        {
            var xDocks = new List<xDocks>();
            Console.WriteLine("sc burdayım1");
            var model = new DemandxDockModel(_demand_Points, opened_xDocks, _key, false, false, _lm_coverage, 1250, false, _num_xDock, false, 0.01, 3600, true);
            model.Run();
            Console.WriteLine("sc burdayım2");
            var is_feasible = model.Return_Status();
            if (!is_feasible)
            {
                double infeasible_ratio = (infeasible_sol_count + 1) / (current_population_size + 1);
                if (infeasible_ratio > infeasible_acceptance_percentage)
                {
                    var new_particle = new Particle(dimension);

                    while (!is_feasible)
                    {
                        (new_particle, xDocks) = Create_New_Particle();
                        var model1 = new DemandxDockModel(_demand_Points, xDocks, _key, false, false, _lm_coverage, 1250, false, _num_xDock, false, 0.01, 3600, true);
                        model1.Run();
                        new_particle.IsFeasible = model1.Return_Status();
                        new_particle.Fitness = model1.GetObjVal();
                        is_feasible = new_particle.IsFeasible;
                    }
                    return new_particle;
                }
                else
                {
                    particle.Fitness = model.GetObjVal();
                    particle.IsFeasible = model.Return_Status();
                    return particle;
                }
            }
            else
            {
                particle.Fitness = model.GetObjVal();
                particle.IsFeasible = model.Return_Status();
                return particle;
            }
        }

        private void Create_Initial_Population()
        {
            var initial_particle = new Particle(dimension);
            var xDocks = new List<xDocks>();
            var current_infeasible_count = 0.0;
            //Burayı düzelt
            Console.WriteLine("Geldim 1");
            for (int i = 0; i < _solution.Count; i++)
            {
                if (_solution[i] == 1)
                {
                    xDocks.Add(_xDocks[i]);
                }
            }
            initial_particle.Position = _solution;
            initial_particle.IsFeasible = true;
            Console.WriteLine("Geldim 2");
            initial_particle = Suitability_Check(initial_particle, personal_bests.Count, current_infeasible_count, xDocks);
            Console.WriteLine("Geldim 3");
            //new_particle.Position = _solution;
            //new_particle.Fitness = objective_value;
            swarm.Add(initial_particle);
            var score = new Score(0, initial_particle.Fitness);
            personal_bests.Add(score);
            for (int k = 0; k < _xDocks.Count; k++) if (_xDocks[k].If_Already_Opened()) already_open_list.Add(k); ;
            for (int i = 1; i < swarm_size; i++)
            {
                var new_particle2 = new Particle(dimension);
                (new_particle2, xDocks) = Create_New_Particle();
                var selected_particle = Suitability_Check(new_particle2, personal_bests.Count, current_infeasible_count, xDocks);
                score = new Score(i, objective_value);
                personal_bests.Add(score);
                swarm.Add(selected_particle);
                if (!selected_particle.IsFeasible)
                {
                    current_infeasible_count += 1;
                }
            }
            
            //personalBests = swarm.GetRange(0, swarm.Count);
            
            Console.WriteLine("Geldim 4");
        }

        private List<Particle> GetPersonalBests(List<Particle> swarm, List<Particle> personalBests)
        {
            var result = new List<Particle>();
            for (var k = 0; k < swarm.Count; k++)
            {
                if (swarm[k].Fitness < personalBests[k].Fitness)
                {
                    var particle = new Particle(swarm[k]);
;                    result.Add(particle); 
                    //personalBests[k]=swarm[k];
                }
                else
                {
                    var particle = new Particle(personalBests[k]);
                    result.Add(particle);
                }
            }
            return result;
        }

        private Particle GetGlobalBest(List<Particle> personalBests, Particle globalBest)
        {
            var min = personalBests.Min();
            return min.CompareTo(globalBest) < 0 ? new Particle(min) : globalBest;
        }

        private double SigmoidFunction(double velocity)
        {
            var prob = new double();
            prob = 1.0 / (1.0 + (double)Math.Exp(-velocity));
            return prob;
        }

        private void CalculateNewVelocity(List<Particle> swarm)
        {
            //var w = 0.5;
            //var c1 = 1.0;
            //var c2 = 1.0;

            //Random rand = new Random();

            for (var i = 0; i < swarm.Count; i++)
            {
                for (var j = 0; j < swarm[i].Velocity.Count; j++)
                {
                    var prob = SigmoidFunction(swarm[i].Velocity[j]);
                    var new_velocity = 2 * Math.Abs(prob - 0.5);
                    swarm[i].Velocity[j] = new_velocity;
                }

                if (already_open_list.Count > 0)
                {
                    for (int k = 0; k < already_open_list.Count; k++)
                    {
                        swarm[i].Velocity[already_open_list[k]] = 0;
                    }
                }
                //    var congnitive = personalBests[k].Position
                //        .Zip(swarm[k].Position, (pb, cb) => pb - cb)
                //        .Select(v => v * c1 * rand.NextDouble());
                //
                //    var social = globalBest.Position
                //        .Zip(swarm[k].Position, (gb, cb) => gb - cb)
                //        .Select(v => v * c2 * rand.NextDouble());
                //
                //    var inertia = swarm[k].Velocity.Select(v => v * w);
                //
                //    swarm[k].Velocity = congnitive.Zip(social, (c, s) => c + s)
                //        .Zip(inertia, (i, c) => i + c).ToList();
            }
        }

        private void CalculateNewPosition(List<Particle> swarm)
        {
            for (var i = 0; i < swarm.Count; i++)
            {
                for (int j = 0; j < swarm[i].Position.Count; j++)
                {
                    Random rand = new Random();
                    var random_number = rand.NextDouble();
                    if (random_number < swarm[i].Velocity[j])
                    {
                        Console.WriteLine("If Kısmı");
                        swarm[i].Position[j] = 1 - swarm[i].Position[j];
                    }

                    else
                    {
                        Console.WriteLine("Else Kısmı");
                        swarm[i].Position[j] = swarm[i].Position[j];
                    }

                }

                //swarm[k].Position = swarm[k].Position
                //    .Zip(swarm[k].Velocity, (p, v) => p + v)
                //    .ToList();

            }
        }
        
       private void New_Calculation(List<Particle> swarm)
        {   
            for (int i = 0; i < swarm.Count; i++)
            {
                for (int j = 0; j < swarm[i].Position.Count; j++)
                {
                    if(j != already_open_list[0] && swarm[i].Position[j] == 1)
                    {
                        var random = new Random();
                        var new_index = random.Next(j);
                        swarm[i].Position[j] = 0;
                        swarm[i].Position[new_index]=1;
                        
                    }
                }
            }
        }
        private List<xDocks> Update_Open_xDock(Particle particle)
        {
            var new_xDocks = new List<xDocks>();
            for (int i = 0; i < particle.Position.Count; i++)
            {
                if (particle.Position[i] == 1)
                {
                    new_xDocks.Add(_xDocks[i]);
                }
            }
            return new_xDocks;
        }

        private List<Particle> CalculateFitness(List<Particle> swarm)
        {
            foreach (var particle in swarm)
            {

                var new_xDocks_2 = Update_Open_xDock(particle);
                var model = new DemandxDockModel(_demand_Points, new_xDocks_2, _key, false, false, _lm_coverage, 1250, false, _num_xDock, false, 0.01, 3600, true);
                model.Run();
                particle.Fitness = model.GetObjVal();
                particle.IsFeasible = model.Return_Status();

                while (!particle.IsFeasible)
                {
                    Console.WriteLine("***********************");
                    Console.WriteLine("***********************");
                    Console.WriteLine("***********************");
                    Console.WriteLine("***********************");
                    Console.WriteLine("***********************");
                    Suitability_Check(particle, swarm.Count, 0.0, new_xDocks_2);
                }
            }

            return swarm;
        }

        private void Run_Algorithm()
        {
            //personalBests = swarm;
            var swarm2 = new List<Particle>();
            var personal_b = new List<Particle>();
            for (int i = 0; i < swarm.Count; i++)
            {
                var particle = Copy(swarm[i]);
                personal_b.Add(particle); 
            }

            for (var iteration = 0; iteration < max_iterations; iteration++)
            {
                swarm = CalculateFitness(swarm);

                personal_b = GetPersonalBests(swarm, personal_b);
                globalBest = GetGlobalBest(personal_b, globalBest);

                CalculateNewVelocity(swarm);//, personalBests, globalBest);
                New_Calculation(swarm);
                Console.WriteLine("Global Best of the Particle Swarm is : {0}", globalBest.Fitness);
                //Console.WriteLine(swarm[1].Fitness);
                //Console.WriteLine(personalBests[1].Fitness);
            }
            Console.WriteLine("Finished:)");
        }
        private Particle Copy(Particle old)
        {
            var new_p = new Particle(old);

            return new_p;
        }

        protected override void Swap()
        {
            throw new NotImplementedException();
        }

        protected override void Optimize()
        {
            Console.WriteLine("I'm particle swarm algorithm :)");
            Create_Initial_Population();
            Run_Algorithm();
        }

        internal class Particle : IComparable
        {
            //public Boolean IsFeasible = new Boolean();
            //public double Fitness = new double();
            //public List<Double> Position = new List<double>();
            //public List<Double> Velocity = new List<double>();
            public List<Double> Position { get; set; }
            public List<Double> Velocity { get; set; }
            public double Fitness { get; set; }
            public Boolean IsFeasible { get; set; }

            public Particle(int n)
            {
                Position = Enumerable.Repeat(0.0, n).ToList();
                Velocity = Enumerable.Repeat(0.0, n).ToList();
                Fitness = double.MaxValue;
                IsFeasible = false;
            }

            public Particle(Particle p)
            {
                this.Position = p.Position;
                this.Velocity = p.Velocity;
                this.Fitness = p.Fitness;
                this.IsFeasible = p.IsFeasible;
            }
            public void Copy_to(Particle p)
            {
                Position = p.Position.GetRange(0, p.Position.Count-1);
                Velocity = p.Position.GetRange(0, p.Position.Count-1);
                Fitness = p.Fitness;
                IsFeasible = p.IsFeasible;
            }

            public int CompareTo(object obj)
            {
                if (obj is null)
                {
                    return -1;
                }

                if (obj is Particle p)
                {
                    if (Fitness < p.Fitness)
                        return -1;

                    if (Fitness > p.Fitness)
                        return 1;

                    return 0;
                }

                throw new Exception("Invalid type");
            }
        }

        internal class Score
        {
            private Int32 ID;
            private Boolean solution_status;
            private Double obj_score;
            private Double fitness_score = 0;
            private Double probability = 0;
            private Double cum_probability = 0;
            public Score(Int32 id, Double objective_value)
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
}