using FMLMDelivery.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core_Form
{
    public partial class Network_Design_Form_Core : Form
    {
        private string demand_file = "";
        private string pot_xDock_file = "";
        private string seller_file = "";
        private string presolved_xDock_file = "";
        private string parameter_file = "";
        private String directory = "";
        private Boolean partial_solution = new Boolean();

        public Network_Design_Form_Core()
        {
            InitializeComponent();
            yes_button.Checked = true;
        }
        private void output_box_Enter(object sender, EventArgs e)
        {

        }
        private void yes_button_CheckedChanged(object sender, EventArgs e)
        {
            if (yes_button.Checked)
            {
                presolved_box.Enabled = false;
                partial_solution = false;
            }
            else
            {
                presolved_box.Enabled = true;
                partial_solution = true;
            }
        }
        private void send_button_Click(object sender, EventArgs e)
        {
            demand_file = demand_box.Text + ".csv";
            pot_xDock_file = pot_xDock_box.Text + ".csv";
            seller_file = seller_box.Text + ".csv";
            parameter_file = parameter_box.Text + ".csv";
            presolved_xDock_file = presolved_box.Text + ".csv";
            var month = Convert.ToInt32(Month_box.Text);

            directory = uotput_box.Text + ".csv";
            //Application.Run(new Form1());
            var demand_point = new List<DemandPoint>();
            var potential_xDocks = new List<xDocks>();
            var xDocks = new List<xDocks>();
            var agency = new List<xDocks>();
            var hubs = new List<Hub>();
            var potential_hubs = new List<Hub>();
            var partial_xDocks = new List<xDocks>();
            var parameters = new List<Parameters>();

            //This variable decides which solution method will be run. If true; every city individually assigned, else regions are assigned as a whole
            var discrete_solution = true;

            //Provide the month index (1-January, 12-December)
            //var reader = new CSVReader("Demand_Points.csv", "2020_Potential_xDocks_Case3.csv", "2020_FM_Ekim.csv", "Parameters.csv", month);
            var reader = new CSVReader(demand_file, pot_xDock_file, seller_file, parameter_file, month);

            reader.Read();
            demand_point = reader.Get_County();
            potential_xDocks = reader.Get_XDocks();
            agency = reader.Get_Agency();
            var prior_small_sellers = reader.Get_Prior_Small_Sellers();
            var regular_small_sellers = reader.Get_Regular_Small_Sellers();
            var prior_big_sellers = reader.Get_Prior_Big_Sellers();
            var regular_big_sellers = reader.Get_Regular_Big_Sellers();
            var parameter_list = reader.Get_Parameter_List();
            if (!partial_solution)
            {
                var runner = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution);
                (xDocks, hubs) = runner.Run();
                Console.ReadKey();
            }
            else
            {
                var partial_reader = new CSVReader("", "Output/Temporary_xDocks.csv", "", "", month);
                partial_reader.Read_Partial_Solution_Xdocks();
                partial_xDocks = partial_reader.Get_Partial_Solution_Xdocks();
                //partial_xDocks = partial_reader.Get_();
                var runner_partial = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution);
                (xDocks, hubs) = runner_partial.Run();
                Console.ReadKey();

            }


        }
    }
}
