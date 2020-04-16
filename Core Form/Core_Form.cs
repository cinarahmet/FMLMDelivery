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
using System.IO;

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
            yes_button.Checked = false;
            demand_combo.Enabled = false;
            pot_xdock_combo.Enabled = false;
            yes_button.Enabled = false;
            no_button.Enabled = false;
            seller_combo.Enabled = false;
            parameter_combo.Enabled = false;
            presolved_combo.Enabled = false;
            comboBox1.Enabled = false;
            Month_box.Enabled = false;
            textBox2.Enabled = false;
            send_button.Enabled = false;
        }
        private void output_box_Enter(object sender, EventArgs e)
        {

        }
        private void yes_button_CheckedChanged(object sender, EventArgs e)
        {
            if (yes_button.Checked)
            {
                presolved_combo.Enabled = false;
                partial_solution = false;
            }
            else
            {
                presolved_combo.Enabled = true;
                partial_solution = true;
            }
        }
        private void Network_Design_Form_Load(object sender, EventArgs e)
        {
          
        }
        private void Directory_Name_Submit_Click(object sender, EventArgs e)
        {
            yes_button.Checked = false;
            demand_combo.Enabled = true;
            pot_xdock_combo.Enabled = true;
            yes_button.Enabled = true;
            no_button.Enabled = true;
            seller_combo.Enabled = true;
            parameter_combo.Enabled = true;
            presolved_combo.Enabled = true;
            comboBox1.Enabled = true;
            Month_box.Enabled = true;
            textBox2.Enabled = true;
            send_button.Enabled = true;
            username.Enabled = false;
            DirectoryInfo obj = new DirectoryInfo("C:\\Users\\" + username.Text + "\\Desktop");
            DirectoryInfo[] folders = obj.GetDirectories();
            comboBox1.DataSource = folders;

            string[] filePaths = Directory.GetFiles(@"C:\\Users\\" + username.Text + "\\Desktop\\Input Files\\", "*.csv");
            foreach (string file in filePaths)
            {
                demand_combo.Items.Add(file);
                pot_xdock_combo.Items.Add(file);
                seller_combo.Items.Add(file);
                presolved_combo.Items.Add(file);
                parameter_combo.Items.Add(file);

            }
        }
        private void send_button_Click(object sender, EventArgs e)
        {
            demand_file = demand_combo.Text ;
            pot_xDock_file = pot_xdock_combo.Text;
            seller_file = seller_combo.Text ;
            parameter_file = parameter_combo.Text;
            presolved_xDock_file = presolved_combo.Text ;
            var month = Convert.ToInt32(Month_box.Text);
            var hub_demand_coverage = Convert.ToDouble(textBox2.Text);
            directory = comboBox1.Text;
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
                var runner = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution,directory, hub_demand_coverage);
                (xDocks, hubs) = runner.Run();
                Console.ReadKey();
            }
            else
            {
                var partial_reader = new CSVReader("", "Temporary_xDocks.csv", "", "", month);
                partial_reader.Read_Partial_Solution_Xdocks();
                partial_xDocks = partial_reader.Get_Partial_Solution_Xdocks();
                //partial_xDocks = partial_reader.Get_();
                var runner_partial = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution,directory, hub_demand_coverage);
                (xDocks, hubs) = runner_partial.Run();
                Console.ReadKey();

            }


        }
    }
}
