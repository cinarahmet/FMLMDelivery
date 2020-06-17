using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Courier
    {
        private readonly string _Id;

        private List<Mahalle> _mahalle_list=new List<Mahalle>();

        private List<Double> _demand_from_mahalle=new List<Double>();

        private Double _total_demand= new double();

        public Courier(String ıd)
        { 
            _Id = ıd;
        }

        public void Set_Total_Demand(Double addition)
        {
            _total_demand = _total_demand + addition;
        }

        public void Add_Mahalle_To_Courier(Mahalle mahalle)
        {
            _mahalle_list.Add(mahalle);
        }

        public void Add_To_Total(Double addition)
        {
            _total_demand = _total_demand + addition;
        }
        public void Demand_From_Mahalle(Double demand)
        {
            _demand_from_mahalle.Add(demand);
        }
    }
}
