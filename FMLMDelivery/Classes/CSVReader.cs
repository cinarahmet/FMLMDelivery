using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using FMLMDelivery;

public class CSVReader
{
    private readonly List<xDocks> _xDocks = new List<xDocks>();

    private readonly List<Hub> _hubs = new List<Hub>();

    private readonly List<Hub> _hubs_2 = new List<Hub>();

    private readonly string _xDocks_file;

    private readonly string _hub_file;

	public CSVReader(string xDocks_file,string hub_file)
	{
        _xDocks_file = xDocks_file;
        _hub_file = hub_file;
	}

    public void Read()
    {
        Read_Hub();
        Read_xDock();
    }
    public void Read_new_Location()
    {
        Read_Hub_2();
    }

    private void Read_xDock()
    {
        using (var sr = File.OpenText(_xDocks_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(',');
                var xDock_City = line[0];
                var xDock_Id = line[1];
                var xDock_region = line[2];
                var xDock_long = Convert.ToDouble(line[3], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_lat = Convert.ToDouble(line[4], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_dist_threshold = Convert.ToDouble(line[5], System.Globalization.CultureInfo.InvariantCulture);
                var xDock_Demand = Convert.ToDouble(line[6]);

                var xDock = new xDocks(xDock_City, xDock_Id, xDock_region, xDock_long, xDock_lat, xDock_dist_threshold, xDock_Demand);
                _xDocks.Add(xDock);
            }
        }
    }


    private void Read_Hub_2()
    {
        using (var sr = File.OpenText(_hub_file))
        {
            String s = sr.ReadLine();
            var line = s.Split(',');
            var hub_Id = line[0];
            var hub_long = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
            var hub_lat = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
            var hub_cost = Convert.ToDouble(line[3]);

            var hub = new Hub(hub_Id, hub_long, hub_lat, hub_cost);
            _hubs_2.Add(hub);
            while ((s = sr.ReadLine()) != null)
            {
                line = s.Split(',');
                hub_Id = line[0];
                hub_long = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                hub_lat = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                hub_cost = Convert.ToDouble(line[3]);

                hub = new Hub(hub_Id, hub_long, hub_lat, hub_cost);
                _hubs_2.Add(hub);

            }
        }
    }

    private void Read_Hub()
    {
        using (var sr = File.OpenText(_hub_file))
        {
            String s = sr.ReadLine();
            while ((s = sr.ReadLine()) != null)
            {
                var line = s.Split(',');
                var hub_Id = line[0];
                var hub_long = Convert.ToDouble(line[1], System.Globalization.CultureInfo.InvariantCulture);
                var hub_lat = Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture);
                var hub_cost = Convert.ToDouble(line[3]);

                var hub = new Hub(hub_Id, hub_long, hub_lat,hub_cost);
                _hubs.Add(hub);

            }
        }
    }

    public List<xDocks> Get_XDocks()
    {
        return _xDocks;
    }

    public List<Hub> Get_Hubs()
    {
        return _hubs;
    }

    public List<Hub> Get_New_Hubs()
    {
        return _hubs_2;
    }
    

}
