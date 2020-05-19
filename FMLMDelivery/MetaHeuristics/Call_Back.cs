using System;
using System.Collections.Generic;
using System.Text;
using ILOG.CPLEX;
using ILOG.Concert;

namespace FMLMDelivery.MetaHeuristics
{
    public class Call_Back : Cplex.MIPCallback
    {
        private Double[] _x { get; set; }
        private List<INumVar> x;
        private DateTime start;
        private List<Double> _gap;
        private int revision;
        private Double Time_thresh = 300;
        private String _heuristic ="";
        public Call_Back(List<INumVar> x, double[] incumb,string heuristic)
        {
            this.x = x;
            _x = incumb;
            start = DateTime.Now;
            _gap = new List<double>();
            _heuristic = heuristic;
        }

       public override void Main()
       {    
            if (HasIncumbent())
            {
                if (_heuristic == "Simulated Annealing")
                {
                    revision += 1;
                    var time_passed = 0;
                    var a = GetBestObjValue();
                    _gap.Add(Math.Round(GetMIPRelativeGap() * 100));
                    if (revision > 10 && _gap[_gap.Count - 2] != _gap[_gap.Count - 1])
                    {
                        start = DateTime.Now;
                    }
                    time_passed = (DateTime.Now - start).Seconds + 60 * (DateTime.Now - start).Minutes;

                    if (time_passed > Time_thresh)
                    {
                        _x = GetIncumbentValues(x.ToArray());
                        Abort();
                        return;
                    }
                }
                if (_heuristic == "Genetic Algorithm")
                {

                }
                
            } 
       }
        public double[] Get_Partial_Solution()
        {
            return _x;
        }
        public Double Get_Gap()
        {
            return _gap[_gap.Count-1];
        }
    }

    
}
