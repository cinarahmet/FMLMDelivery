using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public class Genetic_Algorithm : Heuristic
    {
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
