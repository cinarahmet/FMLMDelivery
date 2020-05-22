using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Parameters
    {
        private String _key ;  

        private Boolean _activation;

        private String _size;

        public Parameters(string key,string size,Boolean activation)
        {
            _key = key; 
            _size = size;
            _activation = activation;

        }        
        public String Get_Key()
        {
            return _key;
        }
        public Boolean Get_Activation()
        {
            return _activation;
        }
        public String Get_Size()
        {
            return _size;
        }
    }
}
