using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Mahalle
    {
        private readonly String  _mahalle_id;

        private Double _mahalle_demand;

        public Mahalle(String mahalle_id, Double mahalle_demand )
        {
            _mahalle_id = mahalle_id;
            _mahalle_demand = mahalle_demand;            
        }

        public String Return()
    }
}
