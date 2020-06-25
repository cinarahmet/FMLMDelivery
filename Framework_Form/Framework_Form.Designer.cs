namespace Framework_Form
{
    partial class Network_Design_Form
    {
        /// <summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Network_Design_Form));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Presolved_box = new System.Windows.Forms.TextBox();
            this.Parameter_Box = new System.Windows.Forms.TextBox();
            this.Seller_Box = new System.Windows.Forms.TextBox();
            this.Pot_xDock_Box = new System.Windows.Forms.TextBox();
            this.Demand_box = new System.Windows.Forms.TextBox();
            this.Mahalle_xDock_Ataması = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Outbut_loc = new System.Windows.Forms.TextBox();
            this.Month_Selected = new System.Windows.Forms.ComboBox();
            this.Hub_Cov_Box = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Run_CitybyCity = new System.Windows.Forms.RadioButton();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.Full_Run = new System.Windows.Forms.RadioButton();
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.Partial_Run = new System.Windows.Forms.RadioButton();
            this.Min_Cap = new System.Windows.Forms.TextBox();
            this._threshold = new System.Windows.Forms.TextBox();
            this.Km_başı_paket = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.run_option_group_box = new System.Windows.Forms.GroupBox();
            this.Courier_Run = new System.Windows.Forms.RadioButton();
            this.Max_kurye_kapasite = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.input_files_parameters.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.run_option_group_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(513, 561);
            this.send_button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(172, 49);
            this.send_button.TabIndex = 2;
            this.send_button.Text = "Çalıştır";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 17);
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
            this.demand_label.Location = new System.Drawing.Point(3, 19);
            this.demand_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.demand_label.Name = "demand_label";
            this.demand_label.Size = new System.Drawing.Size(153, 17);
            this.demand_label.TabIndex = 0;
            this.demand_label.Text = "Talep Noktası Dosyası:";
            // 
            // pot_xDock_label
            // 
            this.pot_xDock_label.AutoSize = true;
            this.pot_xDock_label.Location = new System.Drawing.Point(3, 66);
            this.pot_xDock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pot_xDock_label.Name = "pot_xDock_label";
            this.pot_xDock_label.Size = new System.Drawing.Size(173, 17);
            this.pot_xDock_label.TabIndex = 1;
            this.pot_xDock_label.Text = "Potensiyel xDock Dosyası:";
            // 
            // seller_label
            // 
            this.seller_label.AutoSize = true;
            this.seller_label.Location = new System.Drawing.Point(3, 114);
            this.seller_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.seller_label.Name = "seller_label";
            this.seller_label.Size = new System.Drawing.Size(124, 17);
            this.seller_label.TabIndex = 2;
            this.seller_label.Text = "Tedarikçi Dosyası:";
            // 
            // parameter_label
            // 
            this.parameter_label.AutoSize = true;
            this.parameter_label.Location = new System.Drawing.Point(3, 162);
            this.parameter_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.parameter_label.Name = "parameter_label";
            this.parameter_label.Size = new System.Drawing.Size(132, 17);
            this.parameter_label.TabIndex = 3;
            this.parameter_label.Text = "Parametre Dosyası:";
            // 
            // presolved_xDock_label
            // 
            this.presolved_xDock_label.AutoSize = true;
            this.presolved_xDock_label.Location = new System.Drawing.Point(3, 216);
            this.presolved_xDock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.presolved_xDock_label.Name = "presolved_xDock_label";
            this.presolved_xDock_label.Size = new System.Drawing.Size(151, 17);
            this.presolved_xDock_label.TabIndex = 4;
            this.presolved_xDock_label.Text = "Açılmış xDock Dosyası:";
            // 
            // month_label
            // 
            this.month_label.AutoSize = true;
            this.month_label.Location = new System.Drawing.Point(18, 32);
            this.month_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.month_label.Name = "month_label";
            this.month_label.Size = new System.Drawing.Size(102, 17);
            this.month_label.TabIndex = 9;
            this.month_label.Text = "Çalışılacak Ay :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(263, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Xdock-Hub Talep Kapsamı(0.00 - 1.00) :";
            // 
            // input_files_parameters
            // 
            this.input_files_parameters.Controls.Add(this.groupBox2);
            this.input_files_parameters.Controls.Add(this.groupBox1);
            this.input_files_parameters.Location = new System.Drawing.Point(12, 10);
            this.input_files_parameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_files_parameters.Name = "input_files_parameters";
            this.input_files_parameters.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_files_parameters.Size = new System.Drawing.Size(578, 534);
            this.input_files_parameters.TabIndex = 0;
            this.input_files_parameters.TabStop = false;
            this.input_files_parameters.Text = "Girdi ve Çıktı Dosyaları";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Presolved_box);
            this.groupBox2.Controls.Add(this.Parameter_Box);
            this.groupBox2.Controls.Add(this.Seller_Box);
            this.groupBox2.Controls.Add(this.Pot_xDock_Box);
            this.groupBox2.Controls.Add(this.Demand_box);
            this.groupBox2.Controls.Add(this.presolved_xDock_label);
            this.groupBox2.Controls.Add(this.parameter_label);
            this.groupBox2.Controls.Add(this.seller_label);
            this.groupBox2.Controls.Add(this.Mahalle_xDock_Ataması);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pot_xDock_label);
            this.groupBox2.Controls.Add(this.demand_label);
            this.groupBox2.Location = new System.Drawing.Point(12, 32);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(541, 306);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Girdi Dosyaları";
            // 
            // Presolved_box
            // 
            this.Presolved_box.Location = new System.Drawing.Point(185, 211);
            this.Presolved_box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Presolved_box.Name = "Presolved_box";
            this.Presolved_box.Size = new System.Drawing.Size(298, 22);
            this.Presolved_box.TabIndex = 22;
            this.Presolved_box.Click += new System.EventHandler(this.Presolved_box_Click);
            // 
            // Parameter_Box
            // 
            this.Parameter_Box.Location = new System.Drawing.Point(185, 163);
            this.Parameter_Box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Parameter_Box.Name = "Parameter_Box";
            this.Parameter_Box.Size = new System.Drawing.Size(298, 22);
            this.Parameter_Box.TabIndex = 21;
            this.Parameter_Box.Click += new System.EventHandler(this.Parameter_box_Click);
            // 
            // Seller_Box
            // 
            this.Seller_Box.Location = new System.Drawing.Point(185, 114);
            this.Seller_Box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Seller_Box.Name = "Seller_Box";
            this.Seller_Box.Size = new System.Drawing.Size(298, 22);
            this.Seller_Box.TabIndex = 20;
            this.Seller_Box.Click += new System.EventHandler(this.Seller_box_Click);
            // 
            // Pot_xDock_Box
            // 
            this.Pot_xDock_Box.Location = new System.Drawing.Point(185, 63);
            this.Pot_xDock_Box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Pot_xDock_Box.Name = "Pot_xDock_Box";
            this.Pot_xDock_Box.Size = new System.Drawing.Size(298, 22);
            this.Pot_xDock_Box.TabIndex = 19;
            this.Pot_xDock_Box.Click += new System.EventHandler(this.Pot_xdock_box_Click);
            // 
            // Demand_box
            // 
            this.Demand_box.Location = new System.Drawing.Point(185, 19);
            this.Demand_box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Demand_box.Name = "Demand_box";
            this.Demand_box.Size = new System.Drawing.Size(298, 22);
            this.Demand_box.TabIndex = 18;
            this.Demand_box.Click += new System.EventHandler(this.Demand_box_Click);
            // 
            // Mahalle_xDock_Ataması
            // 
            this.Mahalle_xDock_Ataması.Location = new System.Drawing.Point(185, 257);
            this.Mahalle_xDock_Ataması.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Mahalle_xDock_Ataması.Name = "Mahalle_xDock_Ataması";
            this.Mahalle_xDock_Ataması.Size = new System.Drawing.Size(298, 22);
            this.Mahalle_xDock_Ataması.TabIndex = 26;
            this.Mahalle_xDock_Ataması.Click += new System.EventHandler(this.Mahalle_xDock_Click);
            this.Mahalle_xDock_Ataması.TextChanged += new System.EventHandler(this.Mahalle_xDock_Ataması_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 17);
            this.label1.TabIndex = 27;
            this.label1.Text = "Mahalle xDock Ataması:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Outbut_loc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 364);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(476, 142);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Çıktı Dosyaları";
            // 
            // Outbut_loc
            // 
            this.Outbut_loc.Location = new System.Drawing.Point(13, 78);
            this.Outbut_loc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Outbut_loc.Name = "Outbut_loc";
            this.Outbut_loc.Size = new System.Drawing.Size(387, 22);
            this.Outbut_loc.TabIndex = 25;
            this.Outbut_loc.Click += new System.EventHandler(this.Outbut_loc_Click);
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
            "Eylül ",
            "Ekim ",
            "Kasım ",
            "Aralık"});
            this.Month_Selected.Location = new System.Drawing.Point(303, 30);
            this.Month_Selected.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Month_Selected.Name = "Month_Selected";
            this.Month_Selected.Size = new System.Drawing.Size(106, 24);
            this.Month_Selected.TabIndex = 25;
            this.Month_Selected.Text = "Ay Seçiniz";
            // 
            // Hub_Cov_Box
            // 
            this.Hub_Cov_Box.Location = new System.Drawing.Point(303, 79);
            this.Hub_Cov_Box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Hub_Cov_Box.Name = "Hub_Cov_Box";
            this.Hub_Cov_Box.Size = new System.Drawing.Size(49, 22);
            this.Hub_Cov_Box.TabIndex = 24;
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
            // Run_CitybyCity
            // 
            this.Run_CitybyCity.AutoSize = true;
            this.Run_CitybyCity.Location = new System.Drawing.Point(21, 46);
            this.Run_CitybyCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Run_CitybyCity.Name = "Run_CitybyCity";
            this.Run_CitybyCity.Size = new System.Drawing.Size(165, 21);
            this.Run_CitybyCity.TabIndex = 2;
            this.Run_CitybyCity.Text = "İl Bazlı xDock Ataması";
            this.toolTip1.SetToolTip(this.Run_CitybyCity, resources.GetString("Run_CitybyCity.ToolTip"));
            this.Run_CitybyCity.UseVisualStyleBackColor = true;
            this.Run_CitybyCity.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // toolTip2
            // 
            this.toolTip2.AutoPopDelay = 20000;
            this.toolTip2.InitialDelay = 500;
            this.toolTip2.ReshowDelay = 100;
            this.toolTip2.ToolTipTitle = "Açıklama";
            this.toolTip2.UseFading = false;
            // 
            // Full_Run
            // 
            this.Full_Run.AutoSize = true;
            this.Full_Run.Location = new System.Drawing.Point(21, 91);
            this.Full_Run.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Full_Run.Name = "Full_Run";
            this.Full_Run.Size = new System.Drawing.Size(311, 21);
            this.Full_Run.TabIndex = 0;
            this.Full_Run.Text = "Talep Noktası-xDock-Tedarikçi-Hub Ataması ";
            this.toolTip2.SetToolTip(this.Full_Run, "Bu modelde verilen iller kapsamında xDock alokasyonu, Hub \r\naloksayonu, Büyük ve " +
        "küçük tedarikçi atamalarını gerçekleştirir.\r\n");
            this.Full_Run.UseVisualStyleBackColor = true;
            this.Full_Run.CheckedChanged += new System.EventHandler(this.yes_button_CheckedChanged);
            // 
            // toolTip3
            // 
            this.toolTip3.AutoPopDelay = 20000;
            this.toolTip3.InitialDelay = 500;
            this.toolTip3.ReshowDelay = 100;
            this.toolTip3.ToolTipTitle = "Açıklama";
            this.toolTip3.UseFading = false;
            // 
            // Partial_Run
            // 
            this.Partial_Run.AutoSize = true;
            this.Partial_Run.Location = new System.Drawing.Point(21, 139);
            this.Partial_Run.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Partial_Run.Name = "Partial_Run";
            this.Partial_Run.Size = new System.Drawing.Size(216, 21);
            this.Partial_Run.TabIndex = 1;
            this.Partial_Run.Text = "Xdock-Tedarikçi-Hub Ataması";
            this.toolTip3.SetToolTip(this.Partial_Run, resources.GetString("Partial_Run.ToolTip"));
            this.Partial_Run.UseVisualStyleBackColor = true;
            this.Partial_Run.CheckedChanged += new System.EventHandler(this.no_button_CheckedChanged);
            // 
            // Min_Cap
            // 
            this.Min_Cap.Location = new System.Drawing.Point(303, 159);
            this.Min_Cap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Min_Cap.Name = "Min_Cap";
            this.Min_Cap.Size = new System.Drawing.Size(89, 22);
            this.Min_Cap.TabIndex = 28;
            this.Min_Cap.TextChanged += new System.EventHandler(this.Min_Cap_TextChanged);
            // 
            // _threshold
            // 
            this._threshold.Location = new System.Drawing.Point(303, 119);
            this._threshold.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._threshold.Name = "_threshold";
            this._threshold.Size = new System.Drawing.Size(89, 22);
            this._threshold.TabIndex = 29;
            this._threshold.TextChanged += new System.EventHandler(this._threshold_TextChanged);
            // 
            // Km_başı_paket
            // 
            this.Km_başı_paket.Location = new System.Drawing.Point(303, 239);
            this.Km_başı_paket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Km_başı_paket.Name = "Km_başı_paket";
            this.Km_başı_paket.Size = new System.Drawing.Size(89, 22);
            this.Km_başı_paket.TabIndex = 30;
            this.Km_başı_paket.TextChanged += new System.EventHandler(this.Km_başı_paket_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.Max_kurye_kapasite);
            this.groupBox3.Controls.Add(this.Month_Selected);
            this.groupBox3.Controls.Add(this.Hub_Cov_Box);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.Km_başı_paket);
            this.groupBox3.Controls.Add(this._threshold);
            this.groupBox3.Controls.Add(this.Min_Cap);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.month_label);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(615, 264);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(510, 281);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Parametreler";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 33;
            this.label6.Text = "Km Başı Paket :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 17);
            this.label5.TabIndex = 32;
            this.label5.Text = "İstenen Kurye Verimliliği :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 17);
            this.label4.TabIndex = 31;
            this.label4.Text = "Min Kurye Verimliliği :";
            // 
            // run_option_group_box
            // 
            this.run_option_group_box.BackColor = System.Drawing.Color.White;
            this.run_option_group_box.Controls.Add(this.Courier_Run);
            this.run_option_group_box.Controls.Add(this.Run_CitybyCity);
            this.run_option_group_box.Controls.Add(this.Partial_Run);
            this.run_option_group_box.Controls.Add(this.Full_Run);
            this.run_option_group_box.Location = new System.Drawing.Point(615, 11);
            this.run_option_group_box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.run_option_group_box.Name = "run_option_group_box";
            this.run_option_group_box.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.run_option_group_box.Size = new System.Drawing.Size(510, 224);
            this.run_option_group_box.TabIndex = 1;
            this.run_option_group_box.TabStop = false;
            this.run_option_group_box.Text = "Model Tipi ";
            // 
            // Courier_Run
            // 
            this.Courier_Run.AutoSize = true;
            this.Courier_Run.Location = new System.Drawing.Point(21, 189);
            this.Courier_Run.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Courier_Run.Name = "Courier_Run";
            this.Courier_Run.Size = new System.Drawing.Size(172, 21);
            this.Courier_Run.TabIndex = 3;
            this.Courier_Run.TabStop = true;
            this.Courier_Run.Text = "Sadece Kurye Ataması";
            this.Courier_Run.UseVisualStyleBackColor = true;
            this.Courier_Run.CheckedChanged += new System.EventHandler(this.Courier_Checked);
            // 
            // Max_kurye_kapasite
            // 
            this.Max_kurye_kapasite.Location = new System.Drawing.Point(303, 200);
            this.Max_kurye_kapasite.Name = "Max_kurye_kapasite";
            this.Max_kurye_kapasite.Size = new System.Drawing.Size(89, 22);
            this.Max_kurye_kapasite.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 17);
            this.label7.TabIndex = 35;
            this.label7.Text = "Max Kurye Verimliliği :";
            // 
            // Network_Design_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1139, 624);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.run_option_group_box);
            this.Controls.Add(this.input_files_parameters);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Network_Design_Form";
            this.Text = "Network Design Decision Making Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Close_The_Form);
            this.Load += new System.EventHandler(this.Network_Design_Form_Load);
            this.input_files_parameters.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.run_option_group_box.ResumeLayout(false);
            this.run_option_group_box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ComboBox Month_Selected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Mahalle_xDock_Ataması;
        internal System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Km_başı_paket;
        private System.Windows.Forms.TextBox _threshold;
        private System.Windows.Forms.TextBox Min_Cap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox run_option_group_box;
        private System.Windows.Forms.RadioButton Run_CitybyCity;
        private System.Windows.Forms.RadioButton Partial_Run;
        private System.Windows.Forms.RadioButton Full_Run;
        private System.Windows.Forms.RadioButton Courier_Run;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Max_kurye_kapasite;
    }
}

