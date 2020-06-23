using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using FMLMDelivery.Classes;
using System.Device.Location;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FMLMDelivery.Classes
{

    public class Courier_Assignment
    {
        private xDocks _xDock;
        private List<Mahalle> _mahalle_list;
        private Double _courier_min_cap;
        private Double _threshold;
        private Double _compensation_parameter;
        private List<Courier> courier_list = new List<Courier>();
        private List<List<Double>> _distance_matrix = new List<List<double>>();
        private List<Distance_Class> _all_distances = new List<Distance_Class>();
        private List<Remaining_Demand> _remaining_demand_list = new List<Remaining_Demand>();
        private List<String> _assigned_courier_list = new List<string>();
        

        public Courier_Assignment(xDocks xDock, List<Mahalle> mahalles, Double courier_min_cap, Double threshold,Double compansation)
        {
            _xDock = xDock;
            //Sort Descending Order
            _mahalle_list = mahalles;
            //_mahalle_list = Filter_Run(_mahalle_list);
            _mahalle_list = _mahalle_list.OrderByDescending(x => x.Return_Mahalle_Demand()).ToList();
            _courier_min_cap = courier_min_cap;
            _threshold = threshold;
            _compensation_parameter = compansation;
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
                    var remaining = new Remaining_Demand(mahalle_id, min_demand, max_demand);
                    _remaining_demand_list.Add(remaining);
                }
                else
                {
                    var mahalle_id = _mahalle_list[i].Return_Mahalle_Id();
                    var max_demand = _mahalle_list[i].Return_Mahalle_Demand();
                    var min_demand = _mahalle_list[i].Return_Mahalle_Demand();
                    var remaining = new Remaining_Demand(mahalle_id, min_demand, max_demand);
                    _remaining_demand_list.Add(remaining);
                }

            }
        }
        


        private Tuple<Courier,Double> Nearest_Neighbour_Assignment(Mahalle mahalle, Courier courier, Double upper_bound,bool test)
        {
            var found = true;
            var total_distance_covered = 0.0;
            while (found)
            {
                found = false;
                var remaining_demand_list = new List<Remaining_Demand>();
                var mahalle_list = new List<Mahalle>();
                if (test)
                {
                    
                    for (int a = 0; a < _mahalle_list.Count; a++)
                    {
                        var mahalle_id = _mahalle_list[a].Return_Mahalle_Id();
                        var mahalle_long = _mahalle_list[a].Return_Longitude();
                        var mahalle_lat = _mahalle_list[a].Return_Lattitude();
                        var mahalle_demand = _mahalle_list[a].Return_Mahalle_Demand();
                        var new_mahalle = new Mahalle(mahalle_id, mahalle_long, mahalle_lat, mahalle_demand);
                        mahalle_list.Add(new_mahalle);
                    }
                    
                    for (int b = 0; b < _remaining_demand_list.Count; b++)
                    {
                        var id = _remaining_demand_list[b].Get_ID();
                        var min_demand = _remaining_demand_list[b].Get_Min_Demand();
                        var max_demand = _remaining_demand_list[b].Get_Max_Demand();
                        var rem_dem = new Remaining_Demand(id, min_demand, max_demand);
                        remaining_demand_list.Add(rem_dem);
                    }
                }

                
                var filtered_distance_list = _all_distances.Where(x => x.Get_From() == mahalle.Return_Mahalle_Id());
                var sorted_distance_list = filtered_distance_list.OrderBy(x => x.Get_Distance()).ToList();
                for (int j = 1; j < sorted_distance_list.Count && !found; j++)
                {
                    var candidate_mahalle = _mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[j].Get_To());
                    if (test)
                    {
                        candidate_mahalle = mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[j].Get_To());
                    }
                    if (!candidate_mahalle.Return_Type() && candidate_mahalle.Return_Mahalle_Demand() != 0)
                    {
                        if (upper_bound - courier.Return_Total_Demand() - candidate_mahalle.Return_Mahalle_Demand() > 0)
                        {
                            if (_compensation_parameter * sorted_distance_list[j].Get_Distance() <= candidate_mahalle.Return_Mahalle_Demand())
                            {
                                found = true;
                                courier.Add_Mahalle_To_Courier(candidate_mahalle);
                                courier.Demand_From_Mahalle(candidate_mahalle.Return_Mahalle_Demand());
                                courier.Set_Total_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                if (test)
                                {
                                    remaining_demand_list.Find(x => x.Get_ID() == candidate_mahalle.Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                                    mahalle_list.Find(x => x.Return_Mahalle_Id() == candidate_mahalle.Return_Mahalle_Id()).Set_Remaning_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                }
                                else
                                {
                                    _remaining_demand_list.Find(x => x.Get_ID() == candidate_mahalle.Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                                    _mahalle_list.Find(x => x.Return_Mahalle_Id() == candidate_mahalle.Return_Mahalle_Id()).Set_Remaning_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                }
                                mahalle = candidate_mahalle;
                                total_distance_covered += sorted_distance_list[j].Get_Distance();
                            }
                        }
                    }
                }
            }
            return Tuple.Create(courier,total_distance_covered);
        }
        private void Assign_Big_Mahalle_Completely()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Type())
                {
                    var total_dist = 0.0;
                    var courier = new Courier($"{"Courier "}{courier_list.Count}");
                    var upper_bound = _threshold - _remaining_demand_list[i].Get_Min_Demand();
                    (courier,total_dist) = Nearest_Neighbour_Assignment(_mahalle_list[i], courier, upper_bound,false);
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
            var neighborhood_found = _mahalle_list.Find(x => x.Return_Mahalle_Id() == starting_xdock.Return_Mahalle_Id());
            if (Object.Equals(neighborhood_found,null))
            {
                for (int j = 0; j < _mahalle_list.Count; j++)
                {
                    var distance = Calculate_Distances(starting_xdock.Return_Longitude(), starting_xdock.Return_Lattitude(), _mahalle_list[j].Return_Longitude(), _mahalle_list[j].Return_Lattitude());
                    var Distance = new Distance_Class(starting_xdock.Return_Mahalle_Id(), _mahalle_list[j].Return_Mahalle_Id(), distance);
                    _all_distances.Add(Distance);
                }
            }
            var keep_on = true;
            while (keep_on)
            {
                var filtered_list = _mahalle_list.Where(x => x.Return_Mahalle_Demand() != 0).ToList();
                var best_potential_ratio = 0.0;
                Courier selected_courier = null;
                var selected_index = 0;
                if (Object.Equals(filtered_list.Count,null))
                {
                    keep_on = false;
                }
                else
                {
                    for (int i = 0; i < filtered_list.Count; i++)
                    {
                        var potential_ratio = 0.0;
                        var total_dist_covered = 0.0;
                        var courier_ıd = $"{"Courier "}{courier_list.Count}";
                        var new2_courier = new Courier(courier_ıd);
                        (new2_courier, total_dist_covered) = Nearest_Neighbour_Assignment(filtered_list[i], new2_courier, _threshold-filtered_list[i].Return_Mahalle_Demand(),true);
                        new2_courier.Add_Mahalle_To_Courier(filtered_list[i]);
                        new2_courier.Demand_From_Mahalle(filtered_list[i].Return_Mahalle_Demand());
                        new2_courier.Set_Total_Demand(filtered_list[i].Return_Mahalle_Demand());
                        var dist = _all_distances.Where(x => x.Get_From() == starting_xdock.Return_Mahalle_Id() && x.Get_To() == filtered_list[i].Return_Mahalle_Id());
                        total_dist_covered += dist.ElementAt(0).Get_Distance();
                        potential_ratio = new2_courier.Return_Total_Demand() / total_dist_covered;
                        if (potential_ratio > best_potential_ratio && new2_courier.Return_Total_Demand()>=_courier_min_cap)
                        {
                            selected_courier = new2_courier;
                            best_potential_ratio = potential_ratio;
                            selected_index = i;
                        }
                    }

                    if (!Object.Equals(selected_courier, null))
                    {
                        if (selected_courier.Return_Total_Demand() <_courier_min_cap)
                        {
                            keep_on = false;
                        }
                        else
                        {
                            var dist_cov = 0.0;
                            var courier_ıd = $"{"Courier "}{courier_list.Count}";
                            var selected_c = new Courier(courier_ıd);
                            selected_c.Add_Mahalle_To_Courier(filtered_list[selected_index]);
                            selected_c.Demand_From_Mahalle(filtered_list[selected_index].Return_Mahalle_Demand());
                            selected_c.Set_Total_Demand(filtered_list[selected_index].Return_Mahalle_Demand());
                            _remaining_demand_list.Find(x => x.Get_ID() == filtered_list[selected_index].Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                            _mahalle_list.Find(x => x.Return_Mahalle_Id() == filtered_list[selected_index].Return_Mahalle_Id()).Set_Remaning_Demand(filtered_list[selected_index].Return_Mahalle_Demand());
                            (selected_c, dist_cov) = Nearest_Neighbour_Assignment(filtered_list[selected_index], selected_c, _threshold, false);
                            
                            courier_list.Add(selected_c);
                        }
                    }
                    else
                    {
                        keep_on = false;
                    }
                }
                //var courier_id = $"{"Courier "}{courier_list.Count}";
                //var new_courier = new Courier(courier_id);
                //new_courier = Nearest_Neighbour_Assignment(starting_xdock, new_courier, _threshold);
                //if (new_courier.Return_Total_Demand() != 0)
                //{
                //    courier_list.Add(new_courier);

                //}
                //else
                //{
                //    keep_on = false;
                //}

            }
        }

        private Mahalle Potential_Check(List<Mahalle> mahalles)
        {
            Mahalle selected_mahalle = null;
            var best_potential = 0.0;
            for (int i = 0; i < mahalles.Count; i++)
            {
                var potential = 0.0;
                var index = _mahalle_list.FindIndex(x => x.Return_Mahalle_Id() == mahalles[i].Return_Mahalle_Id());
                var distances = _distance_matrix[index];
                for (int j = 0; j < _mahalle_list.Count; j++)
                {
                    if (_mahalle_list[j].Return_Mahalle_Demand() != 0 && distances[j] != 0)
                    {
                        potential += Math.Floor(_mahalle_list[j].Return_Mahalle_Demand() / (_compensation_parameter * distances[j]))* _mahalle_list[j].Return_Mahalle_Demand();
                    }
                }
                if (potential>best_potential)
                {
                    best_potential = potential;
                    selected_mahalle = mahalles[i];
                }
            }
            return selected_mahalle;
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
                    var neighborhood_assigned = false;
                    for (int d = 1; d < sorted_distance_list.Count && !neighborhood_assigned; d++)
                    {
                        var demand = _mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[d].Get_To()).Return_Mahalle_Demand();
                        var neighborhood_found = false;
                        if (demand == 0)
                        {
                            neighborhood_found = true;
                            neighborhood = sorted_distance_list[d];
                        }


                        var go_in = true;
                        neighborhood_assigned = false;

                        for (int j = 0; j < courier_list.Count && go_in; j++)
                        {
                            for (int k = 0; k < courier_list[j].Return_Assigned_Mahalle().Count && go_in && neighborhood_found; k++)
                            {
                                if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == neighborhood.Get_To() && courier_list[j].Return_Total_Demand() < 2 * _threshold - _mahalle_list[i].Return_Mahalle_Demand())
                                {
                                    neighborhood_assigned = true;
                                    courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                                    courier_list[j].Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                                    courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                    _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                    go_in = false;
                                }
                            }
                        }

                        //if (demand == 0)
                        //{
                        //    assigned = false;
                        //    neighborhood = sorted_distance_list[d];
                        //}
                    }

                    //go_in = true;
                    //for (int j = 0; j < courier_list.Count&& go_in; j++)
                    //{
                    //    for (int k = 0; k < courier_list[j].Return_Assigned_Mahalle().Count && go_in; k++)
                    //    {   
                    //        if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == neighborhood.Get_To() && courier_list[j].Return_Total_Demand() < 2*_threshold - _mahalle_list[i].Return_Mahalle_Demand()) 
                    //        {   
                    //            courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                    //            courier_list[j].Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                    //            courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    //            _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    //            go_in = false;
                    //        }
                    //    }
                    //}
                }
            }
            
        }
        private void Courier_Assignments()
        {
            var xdock_city = _xDock.Get_City();
            var xdock_district = _xDock.Get_District();
            var xdock_id = _xDock.Get_Id();
            for (int i = 0; i < courier_list.Count; i++)
            {
                for (int j = 0; j < courier_list[i].Return_Assigned_Mahalle().Count; j++)
                {
                    var courier_id = courier_list[i].Return_Courier_Id();
                    var courier_mahalle_demand = courier_list[i].Return_Demand_At_Mahalle()[j];
                    var courier_mahalle_name = courier_list[i].Return_Assigned_Mahalle()[j].Return_Mahalle_Id();
                    var list= $"{xdock_city },{xdock_district},{xdock_id},{courier_id},{courier_mahalle_name},{courier_mahalle_demand}";
                    _assigned_courier_list.Add(list);
                }
            }

        }

        public List<String> Return_Courier_Assignments()
        {
            return _assigned_courier_list;
        }
        public void Run_Assignment_Procedure()
        {
            Create_Distance_Matrix();
            Assign_Big_Mahalle_Partially();
            Assign_Big_Mahalle_Completely();
            Assign_Remaining_Mahalle();
            Complete_Final_Assignments();
            Termination_Phase();
            Courier_Assignments();
        }

    }
    internal class Remaining_Demand
    {
        private string ID;
        private Double _min_demand;
        private Double _max_demand;

        public Remaining_Demand(string id, double min_demand, double max_demand)
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


