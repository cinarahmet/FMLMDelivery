using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Mahalle
    {
        private readonly String  _mahalle_id;

        private readonly Double _mahalle_long;

        private readonly Double _mahalle_lat;

        private Double _mahalle_demand;

        private bool _big_mahalle = false;

        public Mahalle(String mahalle_id, Double mahalle_long, Double mahalle_lat, Double mahalle_demand)
        {
            _mahalle_lat = mahalle_lat;
            _mahalle_long = mahalle_long;
            _mahalle_id = mahalle_id;
            _mahalle_demand = mahalle_demand;
        }

        public String Return_Mahalle_Id()
        {
            return _mahalle_id;
        }

        public void Set_Remaning_Demand(Double extracted)
        {   
            _mahalle_demand = (_mahalle_demand - extracted);
        }

        public Double Return_Mahalle_Demand()
        {
            return _mahalle_demand;
        }
        public Double Return_Lattitude()
        {
            return _mahalle_lat;
        }
        public Double Return_Longitude()
        {
            return _mahalle_long;
        }
        public Boolean Return_Type()
        {
            return _big_mahalle;
        }
        public void Set_Type_of_Mahalle(Boolean type)
        {
            _big_mahalle = true;
        }
    }
}
