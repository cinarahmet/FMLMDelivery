using System;
using System.Collections.Generic;
using System.Text;

namespace FMLMDelivery.Classes
{
    public class Courier
    {
        private string _Id;

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

        public void Demand_From_Mahalle(Double demand)
        {
            _demand_from_mahalle.Add(demand);
        }
        public Double Return_Total_Demand()
        {
            return _total_demand;
        }
        public List<Mahalle> Return_Assigned_Mahalle()
        {
            return _mahalle_list;
        }

        public List<Double> Return_Demand_At_Mahalle()
        {
            return _demand_from_mahalle;
        }
        public String Return_Courier_Id()
        {
            return _Id;
        }
        public void Revise_Courier_Id(String revised_id)
        {
            _Id = revised_id;
        }
    }
}
