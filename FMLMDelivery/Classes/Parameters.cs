using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Parameters
    {
        private String _key ;        

        private Double _min_xDock_cap ;        

        private Boolean _activation;

        public Parameters(string key, double min_cap,Boolean activation)
        {
            _key = key;            
            _min_xDock_cap = min_cap;            
            _activation = activation;

        }        
        public Double Get_Min_Cap()
        {
            return _min_xDock_cap;
        }       
        public String Get_Key()
        {
            return _key;
        }
        public Boolean Get_Activation()
        {
            return _activation;
        }
    }
}
