using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public abstract class Heuristic
    {
        protected Double time_limit = 360.0;
        protected List<List<DemandPoint>> _initial_solution;

        protected Heuristic()
        {

        }

        protected bool Check_Feasibility()
        {
            var is_feasible = true;
            is_feasible=Capacity_Constraint(_initial_solution);
            if (!is_feasible)
            {
                return is_feasible;
            }

            return is_feasible;
        }



        private void Construct_Initial_Solution()
        {
            
        }

        private Boolean Capacity_Constraint(List<List<DemandPoint>> _solution)
        {
            var is_feasible = true;
            foreach (var item in _solution)
            {
                var sum = 0.0;
                foreach (var demand_point in item)
                {
                    sum += demand_point.Get_Demand();
                }
                if (sum > 4000)
                {
                    is_feasible = false;
                    return is_feasible;
                }
            }
            return is_feasible;
        }

        protected abstract void Optimize();
        

        private Double Objective_Value()
        {
            return 0;
        }

        protected abstract void Swap();

        public void Run()
        {
            Construct_Initial_Solution();
            Optimize();
        }
        

    }

}
