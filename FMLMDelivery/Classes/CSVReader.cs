using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using FMLMDelivery;

public class CSVReader
{
    private readonly List<DemandPoint> _county = new List<DemandPoint>();

    private readonly List<xDocks> _xDocks = new List<xDocks>();

    private readonly List<xDocks> _agency = new List<xDocks>();

    private readonly List<Seller> _prior_small_seller = new List<Seller>();

    private List<Seller> _regular_small_seller = new List<Seller>();

    private readonly List<Seller> _prior_big_seller = new List<Seller>();

    private List<Seller> _regular_big_seller = new List<Seller>();

    private readonly List<Seller> _total_seller = new List<Seller>();

    private readonly string _county_file;

    private readonly string _xDocks_file;

    private readonly string _seller_file;

    private Dictionary<String, Double> region_county_threshold = new Dictionary<string, double>();

    private Dictionary<String, Double> region_xDock_threshold = new Dictionary<string, double>();

    private Int32 _month;


    public CSVReader(string county_file, string xDock_file, string Seller_file, Int32 month)
    {
        _county_file = county_file;
        _xDocks_file = xDock_file;
        _month = month + 7;
        _seller_file = Seller_file;
    }

    private void Create_County_Region_Threshold()
    {
        var s_1 = "Akdeniz";
        var value = 30;
        var s_2 = "Marmara";
        var value_2 = 30;
        var s_3 = "İçAnadolu";
        var value_3 = 30;
        var s_4 = "Ege";
        var value_4 = 30;
        var s_5 = "Güneydoğu";
        var value_5 = 30;
        var s_6 = "Karadeniz";
        var value_6 = 30;
        var s_7 = "Güneydoğu Anadolu";
        var value_7 = 30;
        region_county_threshold.TryAdd(s_1, value);
        region_county_threshold.TryAdd(s_2, value_2);
        region_county_threshold.TryAdd(s_3, value_3);
        region_county_threshold.TryAdd(s_4, value_4);
        region_county_threshold.TryAdd(s_5, value_5);
        region_county_threshold.TryAdd(s_6, value_6);
        region_county_threshold.TryAdd(s_7, value_7);
    }

    private void Create_xDock_Region_Threshold()
    {
        var s_1 = "Akdeniz";
        var value = 320;
        var s_2 = "Marmara";
        var value_2 = 300;
        var s_3 = "İçAnadolu";
        var value_3 = 300;
        var s_4 = "Ege";
        var value_4 = 300;
        var s_5 = "Güneydoğu";
        var value_5 = 320;
        var s_6 = "Karadeniz";
        var value_6 = 300;
        var s_7 = "Güneydoğu Anadolu";
        var value_7 = 300;
        region_xDock_threshold.TryAdd(s_1, value);
        region_xDock_threshold.TryAdd(s_2, value_2);
        region_xDock_threshold.TryAdd(s_3, value_3);
        region_xDock_threshold.TryAdd(s_4, value_4);
        region_xDock_threshold.TryAdd(s_5, value_5);
        region_xDock_threshold.TryAdd(s_6, value_6);
        region_xDock_threshold.TryAdd(s_7, value_7);
    }

    public void Read()
    {
        Create_County_Region_Threshold();
        Create_xDock_Region_Threshold();
        Read_XDock();
        Read_Demand_Point();
        Read_Sellers();
    }

    private void Read_Demand_Point()
    {
        using (var sr = File.OpenText(_county_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(',');
                var county_City = line[0];
                var county_district = line[1];
                var county_ID = line[2];
                var county_Region = line[3];
                var type_value = Convert.ToBoolean(Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture));
                if (!type_value)
                {
                    var county_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                    var county_lat = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                    var county_dis_thres = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                    if (county_dis_thres == 0.0)
                    {
                        county_dis_thres = region_county_threshold[county_Region];
                    }
                    var county_Demand = Convert.ToDouble(line[_month]);
                    if (county_Demand != 0.0)
                    {
                        county_Demand = Convert.ToDouble(line[_month]) / Math.Ceiling(Convert.ToDouble(line[_month]) / 4000);
                    }
                    if (county_Demand > 10.0)
                    {
                        var county = new DemandPoint(county_City, county_district, county_ID, county_Region, county_long, county_lat, county_dis_thres, county_Demand);
                        _county.Add(county);
                    }

                    if (Math.Ceiling(Convert.ToDouble(line[_month]) / 4000) > 1)
                    {
                        for (int i = 2; i <= Math.Ceiling(Convert.ToDouble(line[_month]) / 4000); i++)
                        {
                            var county_City_ = line[0];
                            var county_district_ = line[1];
                            var county_ID_ = line[2] + " " + i;
                            var county_Region_ = line[3];
                            var county_long_ = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                            var county_lat_ = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                            var county_dis_thres_ = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                            var county_Demand_ = Convert.ToDouble(line[_month]) / Math.Ceiling(Convert.ToDouble(line[_month]) / 4000);
                            if (county_Demand_ > 10.0)
                            {
                                var county_ = new DemandPoint(county_City_, county_district_, county_ID_, county_Region_, county_long_, county_lat_, county_dis_thres_, county_Demand_);
                                _county.Add(county_);
                            }
                            
                        }
                    }
                }

            }
        }
    }




    private void Read_XDock()
    {
        using (var sr = File.OpenText(_xDocks_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(',');
                var xDock_City = line[0];
                var xDock_District = line[1];
                var xDock_Id = line[2];
                var xDock_region = line[3];
                var type_value = Convert.ToBoolean(Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture));
                var xDock_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_lat = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_dist_threshold = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                if (xDock_dist_threshold == 0.0)
                {
                    xDock_dist_threshold = region_xDock_threshold[xDock_region];
                }
                var Already_Opened = Convert.ToDouble(line[8]);
                var xDock_Already_Opened = false;
                if (Already_Opened == 1.0)
                {
                    xDock_Already_Opened = true;
                }

                var xDock_Capacity = Convert.ToDouble(line[_month + 1]);
                if (xDock_Capacity != 0.0)
                {
                    xDock_Capacity = Convert.ToDouble(line[_month + 1]) / Math.Ceiling(Convert.ToDouble(line[_month + 1]) / 4000);
                }
                if (xDock_Capacity > 10.0)
                {
                    var xDock = new xDocks(xDock_City, xDock_District, xDock_Id, xDock_region, xDock_long, xDock_lat, xDock_dist_threshold, xDock_Capacity, xDock_Already_Opened);
                    if (type_value)
                    {
                        _agency.Add(xDock);
                    }
                    else
                    {
                        _xDocks.Add(xDock);
                    }
                }
                

                if (Math.Ceiling(Convert.ToDouble(line[_month + 1]) / 4000) > 1)
                {
                    for (int i = 2; i <= Math.Ceiling(Convert.ToDouble(line[_month + 1]) / 4000); i++)
                    {
                        var xDock_city_ = line[0];
                        var xDock_district_ = line[1];
                        var xDock_Id_ = line[2] + " " + i;
                        var xDock_region_ = line[3];
                        var type_value_ = Convert.ToBoolean(Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture));
                        var xDock_long_ = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                        var xDock_lat_ = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                        var xDock_dist_threshold_ = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                        if (xDock_dist_threshold_ == 0.0)
                        {
                            xDock_dist_threshold_ = region_xDock_threshold[xDock_region];
                        }
                        var Already_Opened_ = Convert.ToDouble(line[8]);
                        var xDock_Already_Opened_ = false;
                        if (Already_Opened_ == 1.0)
                        {
                            xDock_Already_Opened_ = true;
                        }
                        var xDock_Capacity_ = Convert.ToDouble(line[_month + 1]) / Math.Ceiling(Convert.ToDouble(line[_month + 1]) / 4000);
                        if (xDock_Capacity > 10.0)
                        {
                            var xDock_ = new xDocks(xDock_city_, xDock_district_, xDock_Id_, xDock_region_, xDock_long_, xDock_lat_, xDock_dist_threshold_, xDock_Capacity_, xDock_Already_Opened_);
                            if (type_value_)
                            {
                                _agency.Add(xDock_);
                            }
                            else
                            {
                                _xDocks.Add(xDock_);
                            }
                        }
                        
                    }
                }


            }
        }
    }
    public void Read_Sellers()
    {
        using ( var sr = File.OpenText(_seller_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(",");
                var seller_name = line[0];
                var seller_id = line[1];
                var seller_city = line[2];
                var seller_district = line[3];
                var seller_priority = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                var seller_long = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                var seller_lat = Convert.ToDouble(line[6], System.Globalization.CultureInfo.InvariantCulture);
                var seller_dist = Convert.ToDouble(line[7], System.Globalization.CultureInfo.InvariantCulture);
                var seller_demand = Convert.ToDouble(line[8], System.Globalization.CultureInfo.InvariantCulture);
                var seller_size = line[9];
                if (seller_demand <= 1000)
                {
                    var small_seller = new Seller(seller_name,seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_dist, seller_demand, seller_size);
                    if (seller_priority == 1)
                    {
                        _prior_small_seller.Add(small_seller);
                    }
                    else
                    {
                        _regular_small_seller.Add(small_seller);
                    }
                }
                else
                {
                    var big_seller = new Seller(seller_name, seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_dist, seller_demand, seller_size);
                    if (seller_priority==1)
                    {
                        _prior_big_seller.Add(big_seller);
                    }
                    _regular_big_seller.Add(big_seller);
                }
                var total_seller = new Seller(seller_name, seller_id, seller_city, seller_district, seller_priority, seller_long, seller_lat, seller_dist, seller_demand, seller_size);
                _total_seller.Add(total_seller);
            }
        }


    }

    public List<xDocks> Get_XDocks()
    {
        return _xDocks;
    }

    public List<DemandPoint> Get_County()
    {
        return _county;
    }

    public List<xDocks> Get_Agency()
    {
        return _agency;
    }
    public List<Seller> Get_Prior_Small_Sellers()
    {
        return _prior_small_seller;
    }
    public List<Seller> Get_Prior_Big_Sellers()
    {
        return _prior_big_seller;
    }
    public List<Seller> Get_Regular_Small_Sellers()
    {
        return _regular_small_seller;
    }
    public List<Seller> Get_Regular_Big_Sellers()
    {
        return _regular_big_seller;
    }

    public List<Seller> Get_Total_Sellers()
    {
        return _total_seller;
    }
}
