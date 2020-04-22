using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.MetaHeuristics
{
    public abstract class Heuristic
    {
        protected Double time_limit = 360.0;
        protected List<Double> _solution;
        private List<xDocks> _xDocks;
        private List<DemandPoint> _demand_Points;
        private List<xDock_Demand_Point_Pairs> _pairs;


        protected Heuristic()
        {

        }

        protected bool Check_Feasibility()
        {
            var is_feasible = true;
            is_feasible=Capacity_Constraint(_solution,_pairs);
            if (!is_feasible)
            {
                return is_feasible;
            }

            return is_feasible;
        }



        private void Construct_Initial_Solution()
        {
            
        }

        private Boolean Capacity_Constraint(List<Double> solution, List<xDock_Demand_Point_Pairs> pair)
        {
            var is_feasible = true;

            for (int i = 0; i < solution.Count; i++)
            {
                if (solution[i] == 1)
                {
                    var demand = 0.0;
                    for (int j = 0; j < pair[i].Get_Demand_Point_List().Count; j++)
                    {
                        demand += pair[i].Get_Demand_Point_List()[j].Get_Demand();
                    }
                    if (demand > pair[i].Get_xDock().Get_LM_Demand())
                    {
                        is_feasible = false;
                        return is_feasible;
                    }
                }
            }
            return is_feasible;

            /*
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
            */
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
