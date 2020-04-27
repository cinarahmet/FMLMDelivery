using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Genetic_Algorithm : Heuristic
    {
        
        public Genetic_Algorithm(List<Double> solution, List<List<Double>> assignments, List<xDocks> xDocks, List<DemandPoint> demandPoints, List<Parameters> parameters, Double lm_coverage, Double num_xdock) : base(solution,assignments,xDocks, demandPoints, parameters, lm_coverage, num_xdock)
        {

        }

        protected override void Swap()
        {
            
        }

        protected override void Optimize()
        {
            Console.WriteLine("I'm genetic algorithm :)");
            Check_Feasibility();
        }
    }
}
