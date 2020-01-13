using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

public class County
{
    private readonly String _id;

    private readonly Double _longitude;

    private readonly Double _latitude;

    private readonly Double _demand;

    public County(String id, Double longitude, Double latitude, Double demand)
    {
        _id = id;
        _longitude = longitude;
        _latitude = latitude;
        _demand = demand;
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
}

