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

        private bool _big_mahalle;

        private double _threshold = 120;
        public Mahalle(String mahalle_id, Double mahalle_demand,double threshold)
        {
            _mahalle_id = mahalle_id;
            _mahalle_demand = mahalle_demand;
            _threshold = threshold;
            if (_mahalle_demand >= _threshold)
            {
                _big_mahalle = true;
            }
            else
            {
                _big_mahalle = false;
            }
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

    }
}
