using System;
using System.Collections.Generic;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;
using System.Linq;

namespace FMLMDelivery.MetaHeuristics
{
    public class Call_Back : Cplex.MIPCallback
    {
        private Double[] _x { get; set; }
        private Double[][] population { get; set; }
        private List<INumVar> x;
        private DateTime start;
        private List<Double> gap_list;
        private Dictionary<string, Double> sol_list = new Dictionary<string, Double>();
        private Int32 population_size = 10;
        private int revision = 0;
        private Double Time_thresh = 300;
        
        public Call_Back(List<INumVar> x, double[] incumb)
        {
            this.x = x;
            _x = incumb;
            start = DateTime.Now;
            gap_list = new List<double>();
            population = new double[10][];
        }

       public override void Main()
       {    
            if (HasIncumbent())
            {
                //revision += 1;
                //var time_passed = 0;
                //var a = GetBestObjValue();
                //gap_list.Add(Math.Round(GetMIPRelativeGap() * 100));
                //if (revision > 10 && gap_list[gap_list.Count - 2] != gap_list[gap_list.Count - 1])
                //{
                //    start = DateTime.Now;
                //}
                //time_passed = (DateTime.Now - start).Seconds + 60 * (DateTime.Now - start).Minutes;

                //if (time_passed > Time_thresh)
                //{
                //    _x = GetIncumbentValues(x.ToArray());
                //    Abort();
                //    return;
                //}

                var current_gap = Math.Round(GetMIPRelativeGap() * 100);
                var obj = GetBestObjValue();
                var sol = string.Join(',', GetIncumbentValues(x.ToArray()));
                if (!(gap_list.Contains(current_gap)) && !sol_list.ContainsKey(sol))
                {
                    sol_list.Add(sol, obj);
                    gap_list.Add(current_gap);
                    revision += 1;
                    _x = GetIncumbentValues(x.ToArray());
                    population[revision - 1] = _x;
                }
                if (revision >= population_size)
                {
                    Abort();
                    return;
                }

            }
        }

        public double[][] Get_Population()
        {
            return population;
        }

        public double[] Get_Partial_Solution()
        {
            return _x;
        }
        public Double Get_Gap()
        {
            return gap_list[gap_list.Count-1];
        }
    }

    
}
