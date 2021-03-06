﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Device.Location;



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
        private Double _courier_max_cap;
        private Mahalle starting_xdock = null;
        private Double deleted_courier_count = 0;

        public Courier_Assignment(xDocks xDock, List<Mahalle> mahalles, Double courier_min_cap, Double courier_max_cap, Double threshold,Double compansation)
        {
            _xDock = xDock;
            //Sort Descending Order
            _mahalle_list = mahalles;
            Return_Duplicated_Mahalle(mahalles);
            //_mahalle_list = Filter_Run(_mahalle_list);
            _mahalle_list = _mahalle_list.OrderByDescending(x => x.Return_Mahalle_Demand()).ToList();
            _courier_min_cap = courier_min_cap;
            _courier_max_cap = courier_max_cap;
            _threshold = courier_max_cap;
            _compensation_parameter = compansation;
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() >= Math.Max(2*_courier_min_cap,_courier_max_cap)) _mahalle_list[i].Set_Type_of_Mahalle(true);
            }

        }

        private void Return_Duplicated_Mahalle(List<Mahalle> mahalle_list)
        {   
            for (int i = 0; i < mahalle_list.Count; i++)
            {
                var count = 0;
                for (int j = 0; j < mahalle_list.Count; j++)
                {   
                    if (mahalle_list[i].Return_Mahalle_Id() == mahalle_list[j].Return_Mahalle_Id())
                    {
                        count += 1;
                        if (count >= 2)
                        {
                            var mahalle = new Mahalle(mahalle_list[j].Return_Mahalle_Id() +" "+ count.ToString(), mahalle_list[j].Return_Mahalle_District(), mahalle_list[j].Return_Longitude(), mahalle_list[j].Return_Lattitude(), mahalle_list[j].Return_Mahalle_Demand());
                            mahalle_list[j] = mahalle;
                        }
                    }
                }
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
                { 
                    var loop_until = Math.Floor(_mahalle_list[i].Return_Mahalle_Demand() / _threshold);
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
            var remaining_demand_list = new List<Remaining_Demand>();
            var mahalle_list = new List<Mahalle>();
            if (test)
            {

                for (int a = 0; a < _mahalle_list.Count; a++)
                {
                    var mahalle_id = _mahalle_list[a].Return_Mahalle_Id();
                    var mahalle_district = _mahalle_list[a].Return_Mahalle_District();
                    var mahalle_long = _mahalle_list[a].Return_Longitude();
                    var mahalle_lat = _mahalle_list[a].Return_Lattitude();
                    var mahalle_demand = _mahalle_list[a].Return_Mahalle_Demand();
                    var new_mahalle = new Mahalle(mahalle_id, mahalle_district, mahalle_long, mahalle_lat, mahalle_demand);
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
                remaining_demand_list.Find(x => x.Get_ID() == mahalle.Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                mahalle_list.Find(x => x.Return_Mahalle_Id() == mahalle.Return_Mahalle_Id()).Set_Remaning_Demand(mahalle.Return_Mahalle_Demand());
            }
            while (found)
            {
                found = false;
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
                        if (upper_bound - courier.Return_Total_Demand() - candidate_mahalle.Return_Mahalle_Demand() >= 0)
                        {
                            if (_compensation_parameter * sorted_distance_list[j].Get_Distance() <= candidate_mahalle.Return_Mahalle_Demand())
                            {
                                found = true;
                                courier.Add_Mahalle_To_Courier(candidate_mahalle);
                                courier.Add_Demand_From_Mahalle(candidate_mahalle.Return_Mahalle_Demand());
                                courier.Set_Total_Demand(candidate_mahalle.Return_Mahalle_Demand());
                                courier.Add_Distance_to_Courier(sorted_distance_list[j].Get_Distance());
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
                    var demand_of_courier = 0.0;
                    var count = 0.0;
                    var total_dist = 0.0;
                    var assigned_demand_from_big_neighborhood = 0.0;
                    var total_demand_of_neighborhood = 0.0;

                    var courier = new Courier($"{"Courier "}{courier_list.Count}");
                    var upper_bound = _threshold - _remaining_demand_list[i].Get_Min_Demand();
                    (courier,total_dist) = Nearest_Neighbour_Assignment(_mahalle_list[i], courier, upper_bound,false);
                    var dist = _all_distances.Where(x => x.Get_From() == starting_xdock.Return_Mahalle_Id() && x.Get_To() == _mahalle_list[i].Return_Mahalle_Id());
                    total_dist += dist.ElementAt(0).Get_Distance();
                    courier.Add_Distance_to_Courier(dist.ElementAt(0).Get_Distance());
                    count = Convert.ToDouble(Math.Floor(_mahalle_list[i].Return_Mahalle_Demand() / _threshold));
                    if (courier.Return_Assigned_Mahalle().Count > 0)
                    {
                        total_demand_of_neighborhood = _mahalle_list[i].Return_Mahalle_Demand();
                        assigned_demand_from_big_neighborhood = Math.Min(_remaining_demand_list[i].Get_Max_Demand(), _threshold - courier.Return_Total_Demand());
                        var remaining_demand_big_neighborhood = total_demand_of_neighborhood - assigned_demand_from_big_neighborhood;
                        demand_of_courier = remaining_demand_big_neighborhood / count;
                    }
                    else
                    {
                        var number_of_courier = Math.Ceiling(_mahalle_list[i].Return_Mahalle_Demand() / _threshold);
                        assigned_demand_from_big_neighborhood = _mahalle_list[i].Return_Mahalle_Demand() / number_of_courier;
                        if (assigned_demand_from_big_neighborhood < _courier_min_cap)
                        {
                            assigned_demand_from_big_neighborhood = _mahalle_list[i].Return_Mahalle_Demand() / Math.Floor(_mahalle_list[i].Return_Mahalle_Demand() / _threshold);
                            count -= 1;
                        }
                        demand_of_courier = assigned_demand_from_big_neighborhood;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        var courier_mahalle = _mahalle_list[i];
                        var number_courier = $"{"Courier "}{courier_list.Count}";
                        var new_courier = new Courier(number_courier);
                        new_courier.Add_Mahalle_To_Courier(courier_mahalle);
                        new_courier.Add_Demand_From_Mahalle(demand_of_courier);
                        new_courier.Set_Total_Demand(demand_of_courier);
                        new_courier.Add_Distance_to_Courier(dist.ElementAt(0).Get_Distance());
                        courier_list.Add(new_courier);
                    }
                    courier.Revise_Courier_Id($"{"Courier "}{courier_list.Count}");
                    courier.Add_Mahalle_To_Courier(_mahalle_list[i]);
                    courier.Add_Demand_From_Mahalle(assigned_demand_from_big_neighborhood);
                    courier.Set_Total_Demand(assigned_demand_from_big_neighborhood);
                    courier_list.Add(courier);
                    _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    _remaining_demand_list[i].Set_Min_Max_Demand(0);
                }
            }
        }

        private void Assign_Remaining_Mahalle()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() >= _courier_min_cap && _mahalle_list[i].Return_Mahalle_Demand() <= Math.Max(2*_courier_min_cap,_courier_max_cap))
                {
                    var courier_ıd = $"{"Courier "}{courier_list.Count}";
                    var new_courier = new Courier(courier_ıd);
                    new_courier.Add_Mahalle_To_Courier(_mahalle_list[i]);
                    new_courier.Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    new_courier.Add_Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                    var dist = _all_distances.Where(x => x.Get_From() == starting_xdock.Return_Mahalle_Id() && x.Get_To() == _mahalle_list[i].Return_Mahalle_Id());
                    new_courier.Add_Distance_to_Courier(dist.ElementAt(0).Get_Distance());
                    _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                    _remaining_demand_list[i].Set_Min_Max_Demand(0);
                    courier_list.Add(new_courier);
                }
            }
        }
     

        private void Complete_Final_Assignments()
        {
            
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
                        var courier_ıd = $"{"Courier "}{courier_list.Count+deleted_courier_count}";
                        var new2_courier = new Courier(courier_ıd);
                        (new2_courier, total_dist_covered) = Nearest_Neighbour_Assignment(filtered_list[i], new2_courier, _threshold-filtered_list[i].Return_Mahalle_Demand(),true);
                        new2_courier.Add_Mahalle_To_Courier(filtered_list[i]);
                        new2_courier.Add_Demand_From_Mahalle(filtered_list[i].Return_Mahalle_Demand());
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
                            var courier_ıd = $"{"Courier "}{courier_list.Count + deleted_courier_count}";
                            var selected_c = new Courier(courier_ıd);
                            selected_c.Add_Mahalle_To_Courier(filtered_list[selected_index]);
                            selected_c.Add_Demand_From_Mahalle(filtered_list[selected_index].Return_Mahalle_Demand());
                            selected_c.Set_Total_Demand(filtered_list[selected_index].Return_Mahalle_Demand());
                            _remaining_demand_list.Find(x => x.Get_ID() == filtered_list[selected_index].Return_Mahalle_Id()).Set_Min_Max_Demand(0);
                            _mahalle_list.Find(x => x.Return_Mahalle_Id() == filtered_list[selected_index].Return_Mahalle_Id()).Set_Remaning_Demand(filtered_list[selected_index].Return_Mahalle_Demand());
                            (selected_c, dist_cov) = Nearest_Neighbour_Assignment(filtered_list[selected_index], selected_c, _threshold, false);
                            var dist = _all_distances.Where(x => x.Get_From() == starting_xdock.Return_Mahalle_Id() && x.Get_To() == filtered_list[selected_index].Return_Mahalle_Id());
                            dist_cov += dist.ElementAt(0).Get_Distance();
                            selected_c.Add_Distance_to_Courier(dist.ElementAt(0).Get_Distance());
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

        private void New_Termination_Phase()
        {
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() > 0)
                {
                    var filtered_distance_list = _all_distances.Where(x => x.Get_From() == _mahalle_list[i].Return_Mahalle_Id());
                    var sorted_distance_list = filtered_distance_list.OrderBy(x => x.Get_Distance()).ToList();
                    var neighbor = sorted_distance_list[1];
                    var mah = _mahalle_list.Find(x => x.Return_Mahalle_Id() == neighbor.Get_To());
                    var go_in = true;
                    var assigned = true;
                    if (mah.Return_Mahalle_District() == _mahalle_list[i].Return_Mahalle_District())
                    {
                        for (int j = 0; j < courier_list.Count & assigned; j++)
                        {
                            for (int k = 0; k < courier_list[j].Return_Assigned_Mahalle().Count & assigned; k++)
                            {
                                if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == mah.Return_Mahalle_Id() )
                                {
                                    if (courier_list[j].Return_Total_Demand() + _mahalle_list[i].Return_Mahalle_Demand() <= Math.Max(2 * _courier_min_cap, _courier_max_cap))
                                    {
                                        var dist = neighbor.Get_Distance();
                                        courier_list[j].Add_Distance_to_Courier(dist);
                                        courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                                        courier_list[j].Add_Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                                        courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                        _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                        assigned = false;
                                    }
                                }
                                
                            }
                        }
                    }
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
            deleted_courier_count = list_of_remove.Count;
            Complete_Final_Assignments();
            list_of_remove = new List<Courier>();
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
                courier_list.RemoveAll(x => x.Return_Courier_Id() == list_of_remove[i].Return_Courier_Id());
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
                                if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == neighborhood.Get_To() && courier_list[j].Return_Total_Demand() < _courier_max_cap - _mahalle_list[i].Return_Mahalle_Demand())
                                {
                                    neighborhood_assigned = true;
                                    var dist = neighborhood.Get_Distance();
                                    courier_list[j].Add_Distance_to_Courier(dist);
                                    courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                                    courier_list[j].Add_Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                                    courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                    _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                    go_in = false;
                                }
                            }
                        }
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

            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() > 0)
                {
                    var filtered_distance_list = _all_distances.Where(x => x.Get_From() == _mahalle_list[i].Return_Mahalle_Id());
                    var sorted_distance_list = filtered_distance_list.OrderBy(x => x.Get_Distance()).ToList();
                    var neighbor = sorted_distance_list[1];
                    var go_in = true;
                    var assigned = true;
                    for (int j = 0;  j < courier_list.Count & assigned ;  j++)
                    {
                        for (int k = 0; k < courier_list[j].Return_Assigned_Mahalle().Count & assigned; k++)
                        {   
                            for (int d = 1; d < sorted_distance_list.Count & go_in; d++)
                            {
                                var demand = _mahalle_list.Find(x => x.Return_Mahalle_Id() == sorted_distance_list[d].Get_To()).Return_Mahalle_Demand();
                                if (demand == 0)
                                {
                                    neighbor = sorted_distance_list[d];
                                    go_in = false;
                                }
                            }
                            
                            if (courier_list[j].Return_Assigned_Mahalle()[k].Return_Mahalle_Id() == neighbor.Get_To())
                            {
                                var dist = neighbor.Get_Distance();
                                courier_list[j].Add_Distance_to_Courier(dist);
                                courier_list[j].Add_Mahalle_To_Courier(_mahalle_list[i]);
                                courier_list[j].Add_Demand_From_Mahalle(_mahalle_list[i].Return_Mahalle_Demand());
                                courier_list[j].Set_Total_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                _mahalle_list[i].Set_Remaning_Demand(_mahalle_list[i].Return_Mahalle_Demand());
                                assigned = false;
                            }
                        }
                    }
                }
            }
        }

        private void Courier_Assignments()
        {
            var xdock_city = _xDock.Get_City();
            var xdock_district = _xDock.Get_District();
            var xdock_id = _xDock.Get_Id();
            var xdock_lat = _xDock.Get_Latitude();
            var xdock_long = _xDock.Get_Longitude();
            var count = 0;
            for (int i = 0; i < courier_list.Count; i++)
            {
                count += 1;
                for (int j = 0; j < courier_list[i].Return_Assigned_Mahalle().Count; j++)
                {
                    var courier_id = "Courier "+ count;
                    var courier_mahalle_demand = courier_list[i].Return_Demand_At_Mahalle()[j];
                    var courier_mahalle_name = courier_list[i].Return_Assigned_Mahalle()[j].Return_Mahalle_Id();
                    var courier_district = courier_list[i].Return_Assigned_Mahalle()[j].Return_Mahalle_District();
                    var courier_mahalle_longitude = courier_list[i].Return_Assigned_Mahalle()[j].Return_Longitude();
                    var courier_mahalle_lattitude = courier_list[i].Return_Assigned_Mahalle()[j].Return_Lattitude();
                    var courier_distance = courier_list[i].Return_Distance_List()[j];
                    var overload = "";
                    if (courier_list[i].Return_Total_Demand() > _courier_max_cap)
                    {
                        overload = "Overload";
                    }
                    var list= $"{xdock_city },{xdock_district},{xdock_id},{xdock_lat},{xdock_long},{courier_id},{courier_mahalle_name},{courier_district},{courier_mahalle_lattitude},{courier_mahalle_longitude},{courier_mahalle_demand},{courier_distance},{overload}";
                    _assigned_courier_list.Add(list);
                }
            }
            for (int i = 0; i < _mahalle_list.Count; i++)
            {
                if (_mahalle_list[i].Return_Mahalle_Demand() > 0)
                {
                    var courier_id = "NA";
                    var courier_mahalle_demand = _mahalle_list[i].Return_Mahalle_Demand();
                    var courier_mahalle_name = _mahalle_list[i].Return_Mahalle_Id();
                    var courier_district = _mahalle_list[i].Return_Mahalle_District();
                    var courier_mahalle_longitude = _mahalle_list[i].Return_Longitude();
                    var courier_mahalle_lattitude = _mahalle_list[i].Return_Lattitude();
                    var courier_distance = "NA";
                    var overload = "ATANAMADI";
                    var list = $"{xdock_city },{xdock_district},{xdock_id},{xdock_lat},{xdock_long},{courier_id},{courier_mahalle_name},{courier_district},{courier_mahalle_lattitude},{courier_mahalle_longitude},{courier_mahalle_demand},{courier_distance},{overload}";
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
            Create_Starting_xDock();
            Create_Distance_Matrix();
            Assign_Big_Mahalle_Partially();
            Assign_Big_Mahalle_Completely();
            Assign_Remaining_Mahalle();
            Complete_Final_Assignments();
            New_Termination_Phase();
            //Termination_Phase();
            Courier_Assignments();
        }

        private void Create_Starting_xDock()
        {
            starting_xdock = new Mahalle(_xDock.Get_Id(),_xDock.Get_District(), _xDock.Get_Longitude(), _xDock.Get_Latitude(), 0);
            var neighborhood_found = _mahalle_list.Find(x => x.Return_Mahalle_Id() == starting_xdock.Return_Mahalle_Id());
            if (Object.Equals(neighborhood_found, null))
            {
                for (int j = 0; j < _mahalle_list.Count; j++)
                {
                    var distance = Calculate_Distances(starting_xdock.Return_Longitude(), starting_xdock.Return_Lattitude(), _mahalle_list[j].Return_Longitude(), _mahalle_list[j].Return_Lattitude());
                    var Distance = new Distance_Class(starting_xdock.Return_Mahalle_Id(), _mahalle_list[j].Return_Mahalle_Id(), distance);
                    _all_distances.Add(Distance);
                }
            }
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


