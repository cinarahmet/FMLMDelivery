using System;
using System.Collections.Generic;
using System.Text;

public class Seller
{
    private String _name;

    private String _Id;

    private String _city;

    private String _district;

    private Double _priority;

    private Double _latitude;

    private Double _longitude;

    private Double _demand;

    private Double _dist_thres;

    private String _type;
    public Seller(String name, String id, String city, String district, Double priority, Double longitude, Double latitude, Double demand, Double dist_thres, String type)
    {
        _name = name;
        _city = city;
        _Id = id;
        _priority = priority;
        _district = district;
        _longitude = longitude;
        _latitude = latitude;
        _dist_thres = dist_thres;
        _demand = demand;
        _type = type;

    }

    public String Get_Name()
    {
        return _name;
    }

    public String Get_District()
    {
        return _district;
    }

    public Double Get_Longitude()
    {
        return _longitude;
    }

    public Double Get_Priority()
    {
        return _priority;
    }

    public Double Get_Latitude()
    {
        return _latitude;
    }
    public Double Get_Demand()
    {
        return _demand;
    }
    public String Get_City()
    {
        return _city;
    }
    public String Get_Id()
    {
        return _Id;
    }
    public String Get_Region()
    {
        return _district;
    }
    public Double Get_Distance_Threshold()
    {
        return _dist_thres;
    }
    public String Get_Type()
    {
        return _type;
    }

}
 
