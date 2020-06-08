namespace Core_Form
{
    partial class Network_Design_Form_Core

    {/// <summary>
     /// Required designer variable.
     /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Network_Design_Form_Core));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.run_option_group_box = new System.Windows.Forms.GroupBox();
            this.Run_CitybyCity = new System.Windows.Forms.RadioButton();
            this.Partial_Run = new System.Windows.Forms.RadioButton();
            this.Full_Run = new System.Windows.Forms.RadioButton();
            this.send_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.demand_label = new System.Windows.Forms.Label();
            this.pot_xDock_label = new System.Windows.Forms.Label();
            this.seller_label = new System.Windows.Forms.Label();
            this.parameter_label = new System.Windows.Forms.Label();
            this.presolved_xDock_label = new System.Windows.Forms.Label();
            this.month_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.input_files_parameters = new System.Windows.Forms.GroupBox();
            this.Hub_Cov_Box = new System.Windows.Forms.TextBox();
            this.Presolved_box = new System.Windows.Forms.TextBox();
            this.Parameter_Box = new System.Windows.Forms.TextBox();
            this.Seller_Box = new System.Windows.Forms.TextBox();
            this.Pot_xDock_Box = new System.Windows.Forms.TextBox();
            this.Demand_box = new System.Windows.Forms.TextBox();
            this.Outbut_loc = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.Month_Selected = new System.Windows.Forms.ComboBox();
            this.run_option_group_box.SuspendLayout();
            this.input_files_parameters.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // run_option_group_box
            // 
            this.run_option_group_box.BackColor = System.Drawing.Color.White;
            this.run_option_group_box.Controls.Add(this.Run_CitybyCity);
            this.run_option_group_box.Controls.Add(this.Partial_Run);
            this.run_option_group_box.Controls.Add(this.Full_Run);
            this.run_option_group_box.Location = new System.Drawing.Point(692, 82);
            this.run_option_group_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.run_option_group_box.Name = "run_option_group_box";
            this.run_option_group_box.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.run_option_group_box.Size = new System.Drawing.Size(574, 224);
            this.run_option_group_box.TabIndex = 1;
            this.run_option_group_box.TabStop = false;
            this.run_option_group_box.Text = "Model Tipi ";
            // 
            // Run_CitybyCity
            // 
            this.Run_CitybyCity.AutoSize = true;
            this.Run_CitybyCity.Location = new System.Drawing.Point(24, 58);
            this.Run_CitybyCity.Name = "Run_CitybyCity";
            this.Run_CitybyCity.Size = new System.Drawing.Size(190, 24);
            this.Run_CitybyCity.TabIndex = 2;
            this.Run_CitybyCity.TabStop = true;
            this.Run_CitybyCity.Text = "İl Bazlı xDock Ataması";
            this.toolTip1.SetToolTip(this.Run_CitybyCity, "Kullanıcı belirli illerin çalışmasını yapmak istiyorsa bu modeli seçmelidir." +
                "\r\n Bu modelin sonucunda çalıştırılan illerde belirlenen parametreler kullanılarak xDock" +
                "\r\nalokasyonu ile birlikte mahalle atamaları gerçekleşmektedir.");
            this.Run_CitybyCity.UseVisualStyleBackColor = true;
            this.Run_CitybyCity.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Partial_Run
            // 
            this.Partial_Run.AutoSize = true;
            this.Partial_Run.Location = new System.Drawing.Point(24, 174);
            this.Partial_Run.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Partial_Run.Name = "Partial_Run";
            this.Partial_Run.Size = new System.Drawing.Size(244, 24);
            this.Partial_Run.TabIndex = 1;
            this.Partial_Run.TabStop = true;
            this.Partial_Run.Text = "Xdock-Tedarikçi-Hub Ataması";
            this.toolTip3.SetToolTip(this.Partial_Run, "Bu modelde önceden alokasyonu yapılmış olan xDocklar\r\n'Kısmi Çalıştırma Dosyası' kullanılarak sisteme verilmelidir."+
                "\r\nModel xDock bilgilerini kullanarak küçük tedarikçi atamalarını,\r\n Hub alokasyonu ve büyük tedarikçi atamalarını gerçekleştirir.");
            this.Partial_Run.UseVisualStyleBackColor = true;
            this.Partial_Run.CheckedChanged += new System.EventHandler(this.no_button_CheckedChanged);
            // 
            // Full_Run
            // 
            this.Full_Run.AutoSize = true;
            this.Full_Run.Location = new System.Drawing.Point(24, 114);
            this.Full_Run.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Full_Run.Name = "Full_Run";
            this.Full_Run.Size = new System.Drawing.Size(348, 24);
            this.Full_Run.TabIndex = 0;
            this.Full_Run.TabStop = true;
            this.Full_Run.Text = "Talep Noktası-xDock-Tedarikçi-Hub Ataması ";
            this.toolTip2.SetToolTip(this.Full_Run, "Bu modelde verilen iller kapsamında xDock alokasyonu, Hub \r\naloksayonu, Büyük ve " +
        "küçük tedarikçi atamalarını gerçekleşir.\r\n");
            this.Full_Run.UseVisualStyleBackColor = true;
            this.Full_Run.CheckedChanged += new System.EventHandler(this.yes_button_CheckedChanged);
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(599, 630);
            this.send_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(147, 61);
            this.send_button.TabIndex = 2;
            this.send_button.Text = "Çalıştır";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Kaydedilecek Lokasyon:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // demand_label
            // 
            this.demand_label.AutoSize = true;
            this.demand_label.Location = new System.Drawing.Point(9, 60);
            this.demand_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.demand_label.Name = "demand_label";
            this.demand_label.Size = new System.Drawing.Size(169, 20);
            this.demand_label.TabIndex = 0;
            this.demand_label.Text = "Talep Noktası Dosyası:";
            // 
            // pot_xDock_label
            // 
            this.pot_xDock_label.AutoSize = true;
            this.pot_xDock_label.Location = new System.Drawing.Point(9, 118);
            this.pot_xDock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pot_xDock_label.Name = "pot_xDock_label";
            this.pot_xDock_label.Size = new System.Drawing.Size(193, 20);
            this.pot_xDock_label.TabIndex = 1;
            this.pot_xDock_label.Text = "Potensiyel xDock Dosyası:";
            // 
            // seller_label
            // 
            this.seller_label.AutoSize = true;
            this.seller_label.Location = new System.Drawing.Point(9, 178);
            this.seller_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.seller_label.Name = "seller_label";
            this.seller_label.Size = new System.Drawing.Size(136, 20);
            this.seller_label.TabIndex = 2;
            this.seller_label.Text = "Tedarikçi Dosyası:";
            // 
            // parameter_label
            // 
            this.parameter_label.AutoSize = true;
            this.parameter_label.Location = new System.Drawing.Point(9, 238);
            this.parameter_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.parameter_label.Name = "parameter_label";
            this.parameter_label.Size = new System.Drawing.Size(147, 20);
            this.parameter_label.TabIndex = 3;
            this.parameter_label.Text = "Parametre Dosyası:";
            // 
            // presolved_xDock_label
            // 
            this.presolved_xDock_label.AutoSize = true;
            this.presolved_xDock_label.Location = new System.Drawing.Point(9, 306);
            this.presolved_xDock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.presolved_xDock_label.Name = "presolved_xDock_label";
            this.presolved_xDock_label.Size = new System.Drawing.Size(170, 20);
            this.presolved_xDock_label.TabIndex = 4;
            this.presolved_xDock_label.Text = "Kısmi Çalıştırma Dosyası:";
            // 
            // month_label
            // 
            this.month_label.AutoSize = true;
            this.month_label.Location = new System.Drawing.Point(9, 369);
            this.month_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.month_label.Name = "month_label";
            this.month_label.Size = new System.Drawing.Size(113, 20);
            this.month_label.TabIndex = 9;
            this.month_label.Text = "Çalışılacak Ay:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 434);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Xdock-Hub Talep Kapsamı(0.00 - 1.00)";
            // 
            // input_files_parameters
            // 
            this.input_files_parameters.Controls.Add(this.Month_Selected);
            this.input_files_parameters.Controls.Add(this.Hub_Cov_Box);
            this.input_files_parameters.Controls.Add(this.Presolved_box);
            this.input_files_parameters.Controls.Add(this.Parameter_Box);
            this.input_files_parameters.Controls.Add(this.Seller_Box);
            this.input_files_parameters.Controls.Add(this.Pot_xDock_Box);
            this.input_files_parameters.Controls.Add(this.Demand_box);
            this.input_files_parameters.Controls.Add(this.label3);
            this.input_files_parameters.Controls.Add(this.month_label);
            this.input_files_parameters.Controls.Add(this.presolved_xDock_label);
            this.input_files_parameters.Controls.Add(this.parameter_label);
            this.input_files_parameters.Controls.Add(this.seller_label);
            this.input_files_parameters.Controls.Add(this.pot_xDock_label);
            this.input_files_parameters.Controls.Add(this.demand_label);
            this.input_files_parameters.Location = new System.Drawing.Point(13, 82);
            this.input_files_parameters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.input_files_parameters.Name = "input_files_parameters";
            this.input_files_parameters.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.input_files_parameters.Size = new System.Drawing.Size(650, 499);
            this.input_files_parameters.TabIndex = 0;
            this.input_files_parameters.TabStop = false;
            this.input_files_parameters.Text = "Dökümanlar ve Parametreler";
            // 
            // Hub_Cov_Box
            // 
            this.Hub_Cov_Box.Location = new System.Drawing.Point(355, 428);
            this.Hub_Cov_Box.Name = "Hub_Cov_Box";
            this.Hub_Cov_Box.Size = new System.Drawing.Size(55, 26);
            this.Hub_Cov_Box.TabIndex = 24;
            // 
            // Presolved_box
            // 
            this.Presolved_box.Location = new System.Drawing.Point(214, 300);
            this.Presolved_box.Name = "Presolved_box";
            this.Presolved_box.Size = new System.Drawing.Size(335, 26);
            this.Presolved_box.TabIndex = 22;
            this.Presolved_box.Click += new System.EventHandler(this.Presolved_box_Click);
            // 
            // Parameter_Box
            // 
            this.Parameter_Box.Location = new System.Drawing.Point(214, 240);
            this.Parameter_Box.Name = "Parameter_Box";
            this.Parameter_Box.Size = new System.Drawing.Size(335, 26);
            this.Parameter_Box.TabIndex = 21;
            this.Parameter_Box.Click += new System.EventHandler(this.Parameter_box_Click);
            // 
            // Seller_Box
            // 
            this.Seller_Box.Location = new System.Drawing.Point(214, 178);
            this.Seller_Box.Name = "Seller_Box";
            this.Seller_Box.Size = new System.Drawing.Size(335, 26);
            this.Seller_Box.TabIndex = 20;
            this.Seller_Box.Click += new System.EventHandler(this.Seller_box_Click);
            // 
            // Pot_xDock_Box
            // 
            this.Pot_xDock_Box.Location = new System.Drawing.Point(214, 115);
            this.Pot_xDock_Box.Name = "Pot_xDock_Box";
            this.Pot_xDock_Box.Size = new System.Drawing.Size(335, 26);
            this.Pot_xDock_Box.TabIndex = 19;
            this.Pot_xDock_Box.Click += new System.EventHandler(this.Pot_xdock_box_Click);
            // 
            // Demand_box
            // 
            this.Demand_box.Location = new System.Drawing.Point(214, 60);
            this.Demand_box.Name = "Demand_box";
            this.Demand_box.Size = new System.Drawing.Size(335, 26);
            this.Demand_box.TabIndex = 18;
            this.Demand_box.Click += new System.EventHandler(this.Demand_box_Click);
            // 
            // Outbut_loc
            // 
            this.Outbut_loc.Location = new System.Drawing.Point(24, 145);
            this.Outbut_loc.Name = "Outbut_loc";
            this.Outbut_loc.Size = new System.Drawing.Size(435, 26);
            this.Outbut_loc.TabIndex = 25;
            this.Outbut_loc.Click += new System.EventHandler(this.Outbut_loc_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Outbut_loc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(692, 371);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 209);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Çıktı Dosyaları";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipTitle = "Açıklama";
            this.toolTip1.UseFading = false;
            // 
            // toolTip2
            // 
            this.toolTip2.AutoPopDelay = 20000;
            this.toolTip2.InitialDelay = 500;
            this.toolTip2.ReshowDelay = 100;
            this.toolTip2.ToolTipTitle = "Açıklama";
            this.toolTip2.UseFading = false;
            // 
            // toolTip3
            // 
            this.toolTip3.AutoPopDelay = 20000;
            this.toolTip3.InitialDelay = 500;
            this.toolTip3.ReshowDelay = 100;
            this.toolTip3.ToolTipTitle = "Açıklama";
            this.toolTip3.UseFading = false;
            // 
            // Month_Selected
            // 
            this.Month_Selected.FormattingEnabled = true;
            this.Month_Selected.Items.AddRange(new object[] {
            "Ay",
            "Ocak",
            "Şubat",
            "Mart",
            "Nisan",
            "Mayıs",
            "Haziran",
            "Temmuz",
            "Ağustos",
            "Eylül",
            "Ekim",
            "Kasım",
            "Aralık"});
            this.Month_Selected.Location = new System.Drawing.Point(322, 361);
            this.Month_Selected.Name = "Month_Selected";
            this.Month_Selected.Size = new System.Drawing.Size(119, 28);
            this.Month_Selected.TabIndex = 25;
            this.Month_Selected.Text = "Ay Seçiniz";
            
            // 
            // Network_Design_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1281, 743);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.run_option_group_box);
            this.Controls.Add(this.input_files_parameters);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Network_Design_Form";
            this.Text = "Network Design Decision Making Tool";
            this.Load += new System.EventHandler(this.Network_Design_Form_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Close_The_Form);
            this.run_option_group_box.ResumeLayout(false);
            this.run_option_group_box.PerformLayout();
            this.input_files_parameters.ResumeLayout(false);
            this.input_files_parameters.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox run_option_group_box;
        private System.Windows.Forms.RadioButton Partial_Run;
        private System.Windows.Forms.RadioButton Full_Run;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label demand_label;
        private System.Windows.Forms.Label pot_xDock_label;
        private System.Windows.Forms.Label seller_label;
        private System.Windows.Forms.Label parameter_label;
        private System.Windows.Forms.Label presolved_xDock_label;
        private System.Windows.Forms.Label month_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox input_files_parameters;
        private System.Windows.Forms.TextBox Hub_Cov_Box;
        private System.Windows.Forms.TextBox Presolved_box;
        private System.Windows.Forms.TextBox Parameter_Box;
        private System.Windows.Forms.TextBox Seller_Box;
        private System.Windows.Forms.TextBox Pot_xDock_Box;
        private System.Windows.Forms.TextBox Demand_box;
        private System.Windows.Forms.TextBox Outbut_loc;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Run_CitybyCity;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ComboBox Month_Selected;
    }
}

