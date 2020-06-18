using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using FMLMDelivery.Classes;
using System.Device.Location;
using System.Security.Cryptography;

namespace FMLMDelivery.Classes
{
    public class Courier_Assignment
    {
        private xDocks _xDock;
        private List<Mahalle> _mahalle_list;
        private Double _courier_max_cap;
        private Double _courier_min_cap;
        private Double _threshold;
        private Double compensation_parameter = 2;
        private List<Courier> courier_list = new List<Courier>();
        private List<List<Double>> _distance_matrix = new List<List<double>>();
        private List<Distance_Class> _all_distances = new List<Distance_Class>();
        private List<Remaining_Demand_List> _remaining_demand_list = new List<Remaining_Demand_List>();

        public Courier_Assignment(xDocks xDock, List<Mahalle> mahalles, Double courier_max_cap, Double courier_min_cap, Double threshold)
        {
            _xDock = xDock;
            //Sort Descending Order
            _mahalle_list = mahalles;
            _mahalle_list = Filter_Run(_mahalle_list);
            _mahalle_list = _mahalle_list.OrderByDescending(x => x.Return_Mahalle_Demand()).ToList();
            _courier_max_cap = courier_max_cap;
            _courier_min_cap = courier_min_cap;
            _threshold = threshold;
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() >= _threshold) _mahalle_list[i].Set_Type_of_Mahalle(true);
            }

        }
        private List<Mahalle> Filter_Run(List<Mahalle> mahalle_list)
        {
            var revised_mahalle_list = new List<Mahalle>();
            var desired_list =new List<String> { "Güzeloba", "GÜZELOLUK", "GÜZELBAĞ","HÜSNÜ KARAKAŞ","ALTINOVA SİNAN","Merkez","ALTINOVA ORTA","ŞELALE","MENDERES","BARAJ","GÜNEŞ", "Altıntaş","Soğucaksu", "Pınarlı", "Macun", "Barbaros", "Kadriye" };
            for (int i = 0; i < desired_list.Count; i++)
            {
                var found = mahalle_list.Find(x => x.Return_Mahalle_Id() == desired_list[i]);
                if (found != null) revised_mahalle_list.Add(found);
            }
            return revised_mahalle_list;
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
                    var Distance = new Distance_Class(_mahalle_list[i].Return_Mahalle_Id(), _mahalle_list[j].Return_Mahalle_Id(), distance);
                    _all_distances.Add(Distance);
                    d_i.Add(distance);
                }
                _distance_matrix.Add(d_i);
            }
        }
        private void Assign_Big_Mahalle_Partially()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Type())
                { var loop_until = Math.Floor(_mahalle_list[i].Return_Mahalle_Demand() / _threshold);
                    var demand_count = 0.0;
                    for (int j = 0; j < loop_until; j++)
                    {
                        demand_count += _threshold;
                    }
                    var mahalle_id = _mahalle_list[i].Return_Mahalle_Id();
                    var min_demand = _mahalle_list[i].Return_Mahalle_Demand() - demand_count;
                    var max_demand = 0.0;
                    if ((_mahalle_list[i].Return_Mahalle_Demand() - loop_until * _courier_min_cap) > _threshold)
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


        private Courier Nearest_Neighbour_Assignment(Mahalle mahalle, Courier courier, Double upper_bound)
        {
            var found = true;
            while (found)
            {
                found = false;
                var filtered_distance_list = _all_distances.Where(x => x.Get_From() == mahalle.Return_Mahalle_Id());
                var sorted_distance_list = filtered_distance_list.OrderBy(x => x.Get_Distance()).ToList();
                for (int j = 1; j < sorted_distance_list.Count && !found; j++)
                {
                    var candidate_mahalle = _mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[j].Get_To());
                    if (!candidate_mahalle.Return_Type() && candidate_mahalle.Return_Mahalle_Demand() != 0)
                    {
                        if (upper_bound - courier.Return_Total_Demand() - candidate_mahalle.Return_Mahalle_Demand() > 0)
                        {
                            if (compensation_parameter * sorted_distance_list[j].Get_Distance() <= candidate_mahalle.Return_Mahalle_Demand())
                            {
                                found = true;
                                courier.Add_Mahalle_To_Courier(candidate_mahalle);
                                courier.Demand_From_Mahalle(candidate_mahalle.Return_Mahalle_Demand());
                                courier.Set_Total_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                _remaining_demand_list.Find(x => x.Get_ID() == candidate_mahalle.Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                                _mahalle_list.Find(x => x.Return_Mahalle_Id() == candidate_mahalle.Return_Mahalle_Id()).Set_Remaning_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                mahalle = candidate_mahalle;
                            }
                        }
                    }
                }
            }
            return courier;
        }
        private void Assign_Big_Mahalle_Completely()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Type())
                {
                    var courier = new Courier($"{"Courier "}{courier_list.Count}");
                    var upper_bound = _threshold - _remaining_demand_list[i].Get_Min_Demand();
                    courier = Nearest_Neighbour_Assignment(_mahalle_list[i], courier, upper_bound);
                    var total_demand_of_neighborhood = _mahalle_list[i].Return_Mahalle_Demand();
                    var assigned_demand_from_big_neighborhood = Math.Min(_remaining_demand_list[i].Get_Max_Demand(), _threshold - courier.Return_Total_Demand());
                    var remaining_demand_big_neighborhood = total_demand_of_neighborhood - assigned_demand_from_big_neighborhood;
                    var count = Convert.ToDouble(Math.Floor(total_demand_of_neighborhood / _threshold));
                    var demand_of_courier = remaining_demand_big_neighborhood / count;
                    for (int j = 0; j < count; j++)
                    {
                        var courier_mahalle = _mahalle_list[i];
                        var number_courier = $"{"Courier "}{courier_list.Count}";
                        var new_courier = new Courier(number_courier);
                        new_courier.Add_Mahalle_To_Courier(courier_mahalle);
                        new_courier.Demand_From_Mahalle(demand_of_courier);
                        new_courier.Set_Total_Demand(demand_of_courier);
                        courier_list.Add(new_courier);
                    }
                    courier.Revise_Courier_Id($"{"Courier "}{courier_list.Count}");
                    courier.Add_Mahalle_To_Courier(_mahalle_list[i]);
                    courier.Demand_From_Mahalle(assigned_demand_from_big_neighborhood);
                    courier.Set_Total_Demand(assigned_demand_from_big_neighborhood);
                    courier_list.Add(courier);
                    _mahalle_list[i].Set_Remaning_Demand(total_demand_of_neighborhood);
                    _remaining_demand_list[i].Set_Min_Max_Demand(0);
                    Console.WriteLine("dcsv");
                }
            }
        }

        private void Assign_Remaining_Mahalle()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() >= _courier_min_cap && _mahalle_list[i].Return_Mahalle_Demand() <= _threshold)
                {
                    var courier_ıd = $"{"Courier "}{courier_list.Count}";
                    var new_courier = new Courier(courier_ıd);
                    new_courier.Add_Mahalle_To_Courier(_mahalle_list[i]);
                    new_courier.Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    new_courier.Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                    _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    _remaining_demand_list[i].Set_Min_Max_Demand(0);
                    courier_list.Add(new_courier);
                }
            }
        }

        private void Complete_Final_Assignments()
        {
            var starting_xdock = new Mahalle(_xDock.Get_Id(), _xDock.Get_Longitude(), _xDock.Get_Latitude(), 0);
            var keep_on = true;
            while (keep_on)
            {
                var courier_id = $"{"Courier "}{courier_list.Count}";
                var new_courier = new Courier(courier_id);
                new_courier = Nearest_Neighbour_Assignment(starting_xdock,new_courier,_threshold);
                if (new_courier.Return_Total_Demand() != 0)
                {
                    courier_list.Add(new_courier);
                    
                }
                else
                {
                    keep_on = false;
                }
                
            }
        }

        private void Termination_Phase()
        {
            var list_of_remove = new List<Courier>();
            for (int i = 0; i < courier_list.Count; i++)
            {
                if (courier_list[i].Return_Total_Demand() < _courier_min_cap)
                {
                    for (int j = 0; j < courier_list[i].Return_Assigned_Mahalle().Count; j++)
                    {
                        _mahalle_list.Find(x => x.Return_Mahalle_Id() == courier_list[i].Return_Assigned_Mahalle()[j].Return_Mahalle_Id()).Set_Remaning_Demand(-courier_list[i].Return_Demand_At_Mahalle()[j]);
                    }
                    list_of_remove.Add(courier_list[i]);
                }
            }
            for (int i = 0; i < list_of_remove.Count; i++)
            {
                courier_list.RemoveAll(x => x.Return_Courier_Id()==list_of_remove[i].Return_Courier_Id());
            }

            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() > 0)
                {
                    var filtered_distance_list = _all_distances.Where(x => x.Get_From() == _mahalle_list[i].Return_Mahalle_Id());
                    var sorted_distance_list = filtered_distance_list.OrderBy(x => x.Get_Distance()).ToList();
                    var neighborhood = sorted_distance_list[1];
                    var assigned = true;
                    for (int d = 1; d < sorted_distance_list.Count && assigned; d++)
                    {
                        var demand = _mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[d].Get_To()).Return_Mahalle_Demand();
                        if (demand == 0)
                        {
                            assigned = false;
                            neighborhood = sorted_distance_list[d];
                        }
                    }
                    var go_in = true;
                    for (int j = 0; j < courier_list.Count&& go_in; j++)
                    {
                        for (int k = 0; k < courier_list[j].Return_Assigned_Mahalle().Count && go_in ; k++)
                        {   
                            if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == neighborhood.Get_To()) 
                            {   
                                courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                                courier_list[j].Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                                courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                go_in = false;
                            }
                        }
                    }
                }
            }
            var count = 0;
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand()>0)
                {
                    count = count + 1;
                }
            }
        }

        public void Run_Assignment_Procedure()
        {
            Create_Distance_Matrix();
            Assign_Big_Mahalle_Partially();
            Assign_Big_Mahalle_Completely();
            Assign_Remaining_Mahalle();
            Complete_Final_Assignments();
            Termination_Phase();
        }

    }
    internal class Remaining_Demand_List
    {
        private string ID;
        private Double _min_demand;
        private Double _max_demand;

        public Remaining_Demand_List(string id, double min_demand, double max_demand)
        {
            ID = id;
            _max_demand = max_demand;
            _min_demand = min_demand;
        }

        public String Get_ID()
        {
            return ID;
        }

        public Double Get_Max_Demand()
        {
            return _max_demand;
        }
        public Double Get_Min_Demand()
        {
            return _min_demand;
        }

        public void Set_Min_Max_Demand(Double demand)
        {
            _max_demand = demand;
            _min_demand = demand;
        }
    }

    internal class Distance_Class
    {
        private string _from;
        private string _to;
        private Double _distance;
        public Distance_Class(string from, string to, Double distance)
        {
            _from = from;
            _to = to;
            _distance = distance;
        }

        public string Get_From()
        {
            return _from;
        }
        public string Get_To()
        {
            return _to;
        }
        public Double Get_Distance()
        {
            return _distance;
        }
    }


}

