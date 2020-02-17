using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

public class xDocks
{
    private readonly String _city;

    private readonly String _id;

    private readonly String _region;

    private readonly Double _longitude;

    private readonly Double _latitude;

    private readonly Double _distance_threshold;

    private readonly Double _demand;

    private readonly Boolean _already_opened;

    private readonly Double _cumulative_demand;

    private readonly String _adress;

    public xDocks(String city,String id,String region, Double longitude, Double latitude, Double distance_threshold,Double demand,Boolean already_opened, String adress)
    {
        _city = city;
        _id = id;
        _region = region;
        _longitude = longitude;
        _latitude = latitude;
        _distance_threshold = distance_threshold;
        _demand = demand;
        _already_opened = already_opened;
        // _cumulative_demand = cumulative_demand;
        _adress = adress;

    }

    public Boolean If_Already_Opened()
    {
        return _already_opened;
    }

    public Double Get_Distance_Threshold()
    {
        return _distance_threshold;
    }

    public string Get_City()
    {
        return _city;
    }

    public string Get_Region()
    {
        return _region;
    }

    public string Get_Id()
    {
        return _id;
    }

    public Double Get_Longitude()
    {
        return _longitude;
    }

    public Double Get_Latitude()
    {
        return _latitude;
    }

    public Double Get_Demand()
    {
        return _demand;
    }
    public Double Get_Cumulative_Demand()
    {
        return _cumulative_demand;
    }
    public String Get_Adress()
    {
        return _adress;
    }
}

    