using System;
using System.Collections.Generic;
using System.Text;

public class Seller
{
    private String _city;

    private String _region;

    private String _Id;

    private Double _latitude;

    private Double _longitude;

    private Double _demand;

    private Double _dist_thres;

    private String _type;
    public Seller(String city, String id, String region, Double longitude, Double latitude, Double dist_thres, Double demand, String type)
    {
        _city = city;
        _Id = id;
        _region = region;
        _longitude = longitude;
        _latitude = latitude;
        _dist_thres = dist_thres;
        _demand = demand;
        _type = type;

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
    public String Get_City()
    {
        return _city;
    }
    public String Get_County()
    {
        return _Id;
    }
    public String Get_Region()
    {
        return _region;
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
 
