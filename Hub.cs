using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

public class Hub
{
    private readonly String _id;

    private readonly Double _longitude;

    private readonly Double _latitude;


    public Hub(String id, Double longitude, Double latitude)
    {
        _id = id;
        _longitude = longitude;
        _latitude = latitude;

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


}
