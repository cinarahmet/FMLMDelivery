using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FMLMDelivery;
using FMLMDelivery.Classes;
using FsCheck;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {   private int month = new int();
        private Boolean discrete_solution = false;
        private Boolean partial_solution = false;
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button1.Enabled = false;

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Run_The_Solution(int.Parse(textBox6.Text),textBox2.Text,textBox1.Text,textBox3.Text,textBox4.Text,textBox5.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton1.Checked;
            textBox2.Enabled = radioButton1.Checked;
            textBox3.Enabled = radioButton1.Checked;
            textBox4.Enabled = radioButton1.Checked;
            textBox6.Enabled = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton2.Checked;
            textBox2.Enabled = radioButton2.Checked;
            textBox3.Enabled = radioButton2.Checked;
            textBox4.Enabled = radioButton2.Checked;
            textBox5.Enabled = radioButton2.Checked;
            textBox6.Enabled = radioButton2.Checked;
            partial_solution = radioButton2.Checked;
        }
    

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = radioButton3.Checked;
            discrete_solution = radioButton3.Checked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = radioButton4.Checked;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Run_The_Solution(int month_input, string demand_file, string pot_xdock_file, string fm_file, string parameters_file, string temp_xdocks_file)
        {
            var demand_point = new List<DemandPoint>();
            var potential_xDocks = new List<xDocks>();
            var xDocks = new List<xDocks>();
            var agency = new List<xDocks>();
            var hubs = new List<Hub>();
            var potential_hubs = new List<Hub>();
            var partial_xDocks = new List<xDocks>();
            var parameters = new List<Parameters>();

            //This variable decides which solution methıd will be run. If true; every city individually assigned, else regions are assigned as a whole
            

            //Provide the month index (1-January, 12-December)
            var month = month_input;
            var reader = new CSVReader(demand_file+".csv", pot_xdock_file+".csv", fm_file+".csv", parameters_file+".csv", month);
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
                var runner = new FMLMDelivery.Classes.Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution);
                (xDocks, hubs) = runner.Run();
                Console.ReadKey();
            }
            else
            {
                var partial_reader = new CSVReader("", "Output/"+temp_xdocks_file+".csv", "", "", month);
                partial_reader.Read_Partial_Solution_Xdocks();
                partial_xDocks = partial_reader.Get_Partial_Solution_Xdocks();
                //partial_xDocks = partial_reader.Get_();
                var runner_partial = new FMLMDelivery.Classes.Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution);
                (xDocks, hubs) = runner_partial.Run();
                Console.ReadKey();
            }


        }

    }
}
