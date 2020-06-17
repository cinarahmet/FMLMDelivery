using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using FMLMDelivery.Classes;
using System.Device.Location;

namespace FMLMDelivery.Classes
{
    public class Courier_Assignment
    {
        private xDocks _xDock;
        private List<Mahalle> _mahalle_list;
        private Double _courier_max_cap;
        private Double _courier_min_cap;
        private Double _threshold;
        private List<Courier> courier_list=new List<Courier>();
        private List<List<Double>> _distance_matrix = new List<List<double>>();
        private List<Remaining_Demand_List> _remaining_demand_list = new List<Remaining_Demand_List>();

        public Courier_Assignment(xDocks xDock, List<Mahalle> mahalles, Double courier_max_cap, Double courier_min_cap, Double threshold)
        {
            _xDock = xDock;
            //Sort Descending Order
            _mahalle_list = mahalles.OrderByDescending(x => x.Return_Mahalle_Demand()).ToList(); 
            _courier_max_cap = courier_max_cap;
            _courier_min_cap = courier_min_cap;
            _threshold = threshold;
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() >= _threshold) _mahalle_list[i].Set_Type_of_Mahalle(true);
            }
            
        }
        private Double Calculate_Distances(double long_1, double lat_1, double long_2, double lat_2)
        {
            var sCoord = new GeoCoordinate(lat_1, long_1);
            var eCoord = new GeoCoordinate(lat_2, long_2);

            return sCoord.GetDistanceTo(eCoord) / 1000;
        }
        private void Create_Distance_Matrix()
        {
            
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                var d_i = new List<Double>();
                for (int j = 0; j < _mahalle_list.Count; j++)
                {
                    var distance = Calculate_Distances(_mahalle_list[i].Return_Longitude(), _mahalle_list[i].Return_Lattitude(), _mahalle_list[j].Return_Longitude(), _mahalle_list[j].Return_Lattitude());
                    d_i.Add(distance);
                }
                _distance_matrix.Add(d_i);
            }
        }
        private void Assign_Big_Mahalle_Partially()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() > _threshold)
                {   var loop_until = Math.Floor(_mahalle_list[i].Return_Mahalle_Demand() / _threshold);
                    var demand_count = 0.0;
                    for (int j = 0; j < loop_until; j++)
                    {
                        demand_count += _threshold;
                        var courier_mahalle = _mahalle_list[i];
                        var mahalle_demand = _threshold;
                        var courier_total = _threshold;
                        var number_courier = courier_list.Count;
                        var new_courier = new Courier("");
                        new_courier.Add_Mahalle_To_Courier(courier_mahalle);
                        new_courier.Add_To_Total(courier_total);
                        new_courier.Demand_From_Mahalle(mahalle_demand);
                        courier_list.Add(new_courier);
                    }
                    var mahalle_id = _mahalle_list[i].Return_Mahalle_Id();
                    var min_demand = _mahalle_list[i].Return_Mahalle_Demand() - demand_count;
                    var max_demand = 0.0;
                    if((_mahalle_list[i].Return_Mahalle_Demand() - loop_until * _courier_min_cap) > _threshold)
                    {
                        max_demand = _threshold;
                    }
                    else
                    {
                        max_demand = _mahalle_list[i].Return_Mahalle_Demand() - loop_until * _courier_min_cap;
                    }
                    var remaining = new Remaining_Demand_List(mahalle_id, min_demand, max_demand);
                    _remaining_demand_list.Add(remaining);
                }
                else
                {
                    var mahalle_id = _mahalle_list[i].Return_Mahalle_Id();
                    var max_demand = _mahalle_list[i].Return_Mahalle_Demand();
                    var min_demand = _mahalle_list[i].Return_Mahalle_Demand();
                    var remaining = new Remaining_Demand_List(mahalle_id, min_demand, max_demand);
                    _remaining_demand_list.Add(remaining);
                }
                
            }
        }

        private void Assign_Big_Mahalle_Completely()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
            }            
        }

        private void Assign_Remaining_Mahalle()
        {

        }

        private void Complete_Final_Assignments()
        {
            
        }

        public void Run_Assignment_Procedure()
        {
            Create_Distance_Matrix();
            Assign_Big_Mahalle_Partially();
            Assign_Big_Mahalle_Completely();
            Assign_Remaining_Mahalle();
            Complete_Final_Assignments();
        }


    }

    internal class Remaining_Demand_List
    {
        private string ID;
        private Double _min_demand;
        private Double _max_demand;

        public Remaining_Demand_List(string id,double min_demand,double max_demand)
        {
            ID = id;
            _max_demand = max_demand;
            _min_demand = min_demand;
        }

        public Double Get_Max_Demand()
        {
            return _max_demand;
        }
        public Double Get_Min_Demand()
        {
            return _min_demand;
        }
    }
}
