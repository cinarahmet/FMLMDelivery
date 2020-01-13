using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using FMLMDelivery;

public class CSVReader2
{
    private readonly List<County> _county = new List<County>();

    private readonly List<xDocks> _xDocks = new List<xDocks>();

    private readonly string _county_file;

    private readonly string _xDocks_file;

    public CSVReader2(string county_file, string cDock_file)
    {
        _county_file = county_file;
        _xDocks_file = cDock_file;
    }

    public void Read()
    {
        Read_XDock();
        Read_County();
    }

    private void Read_County()
    {
        using (var sr = File.OpenText(_county_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(',');
                var county_ID = line[0];
                var county_long = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                var county_lat = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                var county_Demand = Convert.ToDouble(line[3])/ Math.Ceiling(Convert.ToDouble(line[3]) / 4000);

                var county = new County(county_ID, county_long, county_lat, county_Demand);
                _county.Add(county);

                if (Math.Ceiling(Convert.ToDouble(line[3])/4000) > 1)
                {
                    for (int i = 2; i <= Math.Ceiling(Convert.ToDouble(line[3]) / 4000); i++)
                    {
                        var county_ID_ = line[0] + " " + i;
                        var county_long_ = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                        var county_lat_ = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                        var county_Demand_ = Convert.ToDouble(line[3]) / Math.Ceiling(Convert.ToDouble(line[3]) / 4000);

                        var county_ = new County(county_ID_, county_long_, county_lat_, county_Demand_);
                        _county.Add(county_);
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
                var xDock_Id = line[0];
                var xDock_long = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_lat = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_Capacity = Convert.ToDouble(line[3]) / Math.Ceiling(Convert.ToDouble(line[3]) / 4000);

                var xDock = new xDocks(xDock_Id, xDock_long, xDock_lat, xDock_Capacity);
                _xDocks.Add(xDock);

                if (Math.Ceiling(Convert.ToDouble(line[3]) / 4000) > 1)
                {
                    for (int i = 2; i <= Math.Ceiling(Convert.ToDouble(line[3]) / 4000); i++)
                    {
                        var xDock_Id_ = line[0] + " " + i ;
                        var xDock_long_ = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                        var xDock_lat_ = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                        var xDock_Capacity_ = Convert.ToDouble(line[3]) / Math.Ceiling(Convert.ToDouble(line[3]) / 4000);

                        var xDock_ = new xDocks(xDock_Id_, xDock_long_, xDock_lat_, xDock_Capacity_);
                        _xDocks.Add(xDock_);
                    }
                }


            }
        }
    }

    public List<xDocks> Get_XDocks()
    {
        return _xDocks;
    }

    public List<County> Get_County()
    {
        return _county;
    }

}
