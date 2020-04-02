using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Parameters
    {
        private String _key ;

        private Double _dist_thres ;

        private Double _min_xDock_cap ;

        private Double _gap;
        

        public Parameters(string key, double dist_thres, double min_cap, double gap)
        {
            _key = key;
            _dist_thres = dist_thres;
            _min_xDock_cap = min_cap;
            _gap = gap;
        }
        public Double Get_Dist_Thres()
        {
            return _dist_thres;
        }
        public Double Get_Min_Cap()
        {
            return _min_xDock_cap;
        }
        public Double Get_Gap()
        {
            return _gap;
        }
        public String Get_Key()
        {
            return _key;
        }
    }
}
