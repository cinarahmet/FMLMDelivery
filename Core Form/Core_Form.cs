﻿using FMLMDelivery.Classes;
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
        private string direct_initial = "";
        private double hub_demand_coverage = 0.95;
        private Boolean only_cities = new Boolean();
        private Dictionary<String, int> month_dict = new Dictionary<string, int>();
        public Network_Design_Form_Core()
        {
            InitializeComponent();
            Demand_box.Enabled = false;
            Pot_xDock_Box.Enabled = false;
            Seller_Box.Enabled = false;
            Parameter_Box.Enabled = false;
            Presolved_box.Enabled = false;
            Outbut_loc.Enabled = false;
            Month_Selected.Enabled = false;
            Hub_Cov_Box.Enabled = false;
            send_button.Enabled = false;
            Run_CitybyCity.Checked = true;
            _threshold.Enabled = false;
            Min_Cap.Enabled = false;
            Km_başı_paket.Enabled = false;
            Mahalle_xDock_Ataması.Enabled = false;
            Create_Dictionary_Month();
        }
        private void output_box_Enter(object sender, EventArgs e)
        {

        }
        
        

        private void Mahalle_xDock_Ataması_TextChanged(object sender, EventArgs e)
        {

        }

        
        

        private void _threshold_TextChanged(object sender, EventArgs e)
        {

        }

        private void Min_Cap_TextChanged(object sender, EventArgs e)
        {

        }

        private void Km_başı_paket_TextChanged(object sender, EventArgs e)
        {

        }
        private void yes_button_CheckedChanged(object sender, EventArgs e)
        {
            if (Full_Run.Checked)
            {
                Presolved_box.Enabled = false;
                partial_solution = false;
                Demand_box.Enabled = true;
                Pot_xDock_Box.Enabled = true;
                Partial_Run.Enabled = true;
                Seller_Box.Enabled = true;
                Parameter_Box.Enabled = true;
                Presolved_box.Enabled = false;
                Outbut_loc.Enabled = true;
                Month_Selected.Enabled = true;
                Hub_Cov_Box.Enabled = true;
                send_button.Enabled = true;
                _threshold.Enabled = true;
                Min_Cap.Enabled = true;
                Km_başı_paket.Enabled = true;
                Mahalle_xDock_Ataması.Enabled = false;
                only_cities = false;
            }
           
        }
        private void no_button_CheckedChanged(object sender, EventArgs e)
        {
            if(Partial_Run.Checked)
            {
                Demand_box.Enabled = true;
                Pot_xDock_Box.Enabled = true;
                Partial_Run.Enabled = true;
                Seller_Box.Enabled = true;
                Parameter_Box.Enabled = true;
                Presolved_box.Enabled = true;
                Outbut_loc.Enabled = true;
                Month_Selected.Enabled = true;
                Hub_Cov_Box.Enabled = true;
                send_button.Enabled = true;
                Presolved_box.Enabled = true;
                Mahalle_xDock_Ataması.Enabled = true;
                _threshold.Enabled = true;
                Min_Cap.Enabled = true;
                Km_başı_paket.Enabled = true;
                partial_solution = true;
                only_cities = false;
                
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (Run_CitybyCity.Checked)
            {
                Demand_box.Enabled = true;
                Pot_xDock_Box.Enabled = true;
                Partial_Run.Enabled = true;
                Seller_Box.Enabled = true;
                Parameter_Box.Enabled = true;
                Presolved_box.Enabled = false;
                Outbut_loc.Enabled = true;
                Month_Selected.Enabled = true;
                Hub_Cov_Box.Enabled = false;
                send_button.Enabled = true;
                _threshold.Enabled = true;
                Min_Cap.Enabled = true;
                Km_başı_paket.Enabled = true;
                Mahalle_xDock_Ataması.Enabled = false;
                partial_solution = false;
                only_cities = true;
            }
        }
        private void Courier_Checked(object sender, EventArgs e)
        {
            if (Courier_Run.Checked)
            {
                Demand_box.Enabled = false;
                Pot_xDock_Box.Enabled = false;
                Partial_Run.Enabled = true;
                Seller_Box.Enabled = false;
                Parameter_Box.Enabled = false;
                Presolved_box.Enabled = false;
                Outbut_loc.Enabled = true;
                Month_Selected.Enabled = false;
                Hub_Cov_Box.Enabled = false;
                send_button.Enabled = true;
                _threshold.Enabled = true;
                Min_Cap.Enabled = true;
                Km_başı_paket.Enabled = true;

            }

        }
        private void Network_Design_Form_Load(object sender, EventArgs e)
        {
           
        }
        private void Demand_box_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            direct_initial = openFileDialog1.FileName;
            Demand_box.Text = direct_initial;
        }
        
        private void Pot_xdock_box_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = direct_initial;
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            var direct = openFileDialog1.FileName;
            Pot_xDock_Box.Text = direct;

        }

        private void Seller_box_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = direct_initial;
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            var direct = openFileDialog1.FileName;
            Seller_Box.Text = direct;
        }
        
        private void Parameter_box_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = direct_initial;
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            var direct = openFileDialog1.FileName;
            Parameter_Box.Text = direct;
        }
        private void Mahalle_xDock_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            direct_initial = openFileDialog1.FileName;
            Mahalle_xDock_Ataması.Text = direct_initial;
        }
        private void Create_Dictionary_Month()
        {
            var dict = new Dictionary<String, int>();
            dict.Add("Ocak", 1);
            dict.Add("Şubat", 2);
            dict.Add("Mart", 3);
            dict.Add("Nisan", 4);
            dict.Add("Mayıs", 5);
            dict.Add("Haziran", 6);
            dict.Add("Temmuz", 7);
            dict.Add("Ağustos", 8);
            dict.Add("Eylül", 9);
            dict.Add("Ekim", 10);
            dict.Add("Kasım", 11);
            dict.Add("Aralık", 12);

            month_dict = dict;
        }
        private void Presolved_box_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = direct_initial;
            openFileDialog1.ShowDialog();
            openFileDialog1.RestoreDirectory = true;
            var direct = openFileDialog1.FileName;
            Presolved_box.Text = direct;

        }
        private void Close_The_Form(object sender, FormClosingEventArgs e)
        {

        }

        private void Outbut_loc_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();         
            
            var direct = folderBrowserDialog1.SelectedPath;
            Outbut_loc.Text = direct;
        }
        
        private async void send_button_Click(object sender, EventArgs e)
        {
            demand_file = Demand_box.Text ;
            pot_xDock_file = Pot_xDock_Box.Text;
            seller_file = Seller_Box.Text ;
            parameter_file = Parameter_Box.Text;
            presolved_xDock_file = Presolved_box.Text ;
            var month = month_dict[Month_Selected.SelectedItem.ToString()];
            if (Hub_Cov_Box.Text != "") hub_demand_coverage = Convert.ToDouble(Hub_Cov_Box.Text);
            directory = Outbut_loc.Text;
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
                var runner = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution,directory, hub_demand_coverage,only_cities);
                (xDocks, hubs) = await Task.Run(() => runner.Run());
                //Console.ReadKey();
            }
            else
            {
                var partial_reader = new CSVReader("", presolved_xDock_file, "", "", month);
                partial_reader.Read_Partial_Solution_Xdocks();
                partial_xDocks = partial_reader.Get_Partial_Solution_Xdocks();
                //partial_xDocks = partial_reader.Get_();
                var runner_partial = new Runner(demand_point, potential_xDocks, partial_xDocks, agency, prior_small_sellers, regular_small_sellers, prior_big_sellers, regular_big_sellers, parameter_list, partial_solution, discrete_solution,directory, hub_demand_coverage,only_cities);
                (xDocks, hubs) = await Task.Run(() => runner_partial.Run());
                //Console.ReadKey();
            }

            var path = directory + "\\";
            var dirname = new DirectoryInfo(path).Name;
            MessageBoxButtons buttons= MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            MessageBox.Show("Çalıştırma Bitti! Sonuçları "+"'"+dirname+"'"+" dosyasında bulabilirsiniz.","Bilgi", buttons,icon);
            
        }
    }
}
