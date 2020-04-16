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
            this.input_files_parameters = new System.Windows.Forms.GroupBox();
            this.Month_box = new System.Windows.Forms.TextBox();
            this.month_label = new System.Windows.Forms.Label();
            this.presolved_box = new System.Windows.Forms.TextBox();
            this.parameter_box = new System.Windows.Forms.TextBox();
            this.seller_box = new System.Windows.Forms.TextBox();
            this.pot_xDock_box = new System.Windows.Forms.TextBox();
            this.demand_box = new System.Windows.Forms.TextBox();
            this.presolved_xDock_label = new System.Windows.Forms.Label();
            this.parameter_label = new System.Windows.Forms.Label();
            this.seller_label = new System.Windows.Forms.Label();
            this.pot_xDock_label = new System.Windows.Forms.Label();
            this.demand_label = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.run_option_group_box = new System.Windows.Forms.GroupBox();
            this.no_button = new System.Windows.Forms.RadioButton();
            this.yes_button = new System.Windows.Forms.RadioButton();
            this.send_button = new System.Windows.Forms.Button();
            this.output_box = new System.Windows.Forms.GroupBox();
            this.username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Directory_Name_Submit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.input_files_parameters.SuspendLayout();
            this.run_option_group_box.SuspendLayout();
            this.output_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // input_files_parameters
            // 
            this.input_files_parameters.Controls.Add(this.Month_box);
            this.input_files_parameters.Controls.Add(this.month_label);
            this.input_files_parameters.Controls.Add(this.presolved_box);
            this.input_files_parameters.Controls.Add(this.parameter_box);
            this.input_files_parameters.Controls.Add(this.seller_box);
            this.input_files_parameters.Controls.Add(this.pot_xDock_box);
            this.input_files_parameters.Controls.Add(this.demand_box);
            this.input_files_parameters.Controls.Add(this.presolved_xDock_label);
            this.input_files_parameters.Controls.Add(this.parameter_label);
            this.input_files_parameters.Controls.Add(this.seller_label);
            this.input_files_parameters.Controls.Add(this.pot_xDock_label);
            this.input_files_parameters.Controls.Add(this.demand_label);
            this.input_files_parameters.Location = new System.Drawing.Point(13, 210);
            this.input_files_parameters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.input_files_parameters.Name = "input_files_parameters";
            this.input_files_parameters.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.input_files_parameters.Size = new System.Drawing.Size(428, 443);
            this.input_files_parameters.TabIndex = 0;
            this.input_files_parameters.TabStop = false;
            this.input_files_parameters.Text = "Dökümanlar ve Parametreler";
            // 
            // Month_box
            // 
            this.Month_box.Location = new System.Drawing.Point(252, 358);
            this.Month_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Month_box.Name = "Month_box";
            this.Month_box.Size = new System.Drawing.Size(148, 26);
            this.Month_box.TabIndex = 10;
            // 
            // month_label
            // 
            this.month_label.AutoSize = true;
            this.month_label.Location = new System.Drawing.Point(9, 369);
            this.month_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.month_label.Name = "month_label";
            this.month_label.Size = new System.Drawing.Size(226, 20);
            this.month_label.TabIndex = 9;
            this.month_label.Text = "Çalışılan Ay (1: Ocak, 12:Aralık)";
            // 
            // presolved_box
            // 
            this.presolved_box.Location = new System.Drawing.Point(252, 302);
            this.presolved_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.presolved_box.Name = "presolved_box";
            this.presolved_box.Size = new System.Drawing.Size(148, 26);
            this.presolved_box.TabIndex = 8;
            // 
            // parameter_box
            // 
            this.parameter_box.Location = new System.Drawing.Point(252, 234);
            this.parameter_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.parameter_box.Name = "parameter_box";
            this.parameter_box.Size = new System.Drawing.Size(148, 26);
            this.parameter_box.TabIndex = 7;
            // 
            // seller_box
            // 
            this.seller_box.Location = new System.Drawing.Point(252, 174);
            this.seller_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.seller_box.Name = "seller_box";
            this.seller_box.Size = new System.Drawing.Size(148, 26);
            this.seller_box.TabIndex = 6;
            // 
            // pot_xDock_box
            // 
            this.pot_xDock_box.Location = new System.Drawing.Point(252, 114);
            this.pot_xDock_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pot_xDock_box.Name = "pot_xDock_box";
            this.pot_xDock_box.Size = new System.Drawing.Size(148, 26);
            this.pot_xDock_box.TabIndex = 5;
            // 
            // demand_box
            // 
            this.demand_box.Location = new System.Drawing.Point(252, 55);
            this.demand_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.demand_box.Name = "demand_box";
            this.demand_box.Size = new System.Drawing.Size(148, 26);
            this.demand_box.TabIndex = 1;
            // 
            // presolved_xDock_label
            // 
            this.presolved_xDock_label.AutoSize = true;
            this.presolved_xDock_label.Location = new System.Drawing.Point(9, 306);
            this.presolved_xDock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.presolved_xDock_label.Name = "presolved_xDock_label";
            this.presolved_xDock_label.Size = new System.Drawing.Size(170, 20);
            this.presolved_xDock_label.TabIndex = 4;
            this.presolved_xDock_label.Text = "Açılmış xDock Dosyası:";
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
            // run_option_group_box
            // 
            this.run_option_group_box.Controls.Add(this.no_button);
            this.run_option_group_box.Controls.Add(this.yes_button);
            this.run_option_group_box.Location = new System.Drawing.Point(534, 210);
            this.run_option_group_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.run_option_group_box.Name = "run_option_group_box";
            this.run_option_group_box.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.run_option_group_box.Size = new System.Drawing.Size(643, 123);
            this.run_option_group_box.TabIndex = 1;
            this.run_option_group_box.TabStop = false;
            this.run_option_group_box.Text = "Model Tipi Seçiniz";
            // 
            // no_button
            // 
            this.no_button.AutoSize = true;
            this.no_button.Location = new System.Drawing.Point(391, 55);
            this.no_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.no_button.Name = "no_button";
            this.no_button.Size = new System.Drawing.Size(244, 24);
            this.no_button.TabIndex = 1;
            this.no_button.TabStop = true;
            this.no_button.Text = "Xdock-Tedarikçi-Hub Ataması";
            this.no_button.UseVisualStyleBackColor = true;
            // 
            // yes_button
            // 
            this.yes_button.AutoSize = true;
            this.yes_button.Location = new System.Drawing.Point(8, 55);
            this.yes_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.yes_button.Name = "yes_button";
            this.yes_button.Size = new System.Drawing.Size(348, 24);
            this.yes_button.TabIndex = 0;
            this.yes_button.TabStop = true;
            this.yes_button.Text = "Talep Noktası-xDock-Tedarikçi-Hub Ataması ";
            this.yes_button.UseVisualStyleBackColor = true;
            this.yes_button.CheckedChanged += new System.EventHandler(this.yes_button_CheckedChanged);
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(942, 608);
            this.send_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(124, 45);
            this.send_button.TabIndex = 2;
            this.send_button.Text = "Çalıştır";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // output_box
            // 
            this.output_box.Controls.Add(this.Directory_Name_Submit);
            this.output_box.Controls.Add(this.username);
            this.output_box.Controls.Add(this.label1);
            this.output_box.Location = new System.Drawing.Point(47, 14);
            this.output_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.output_box.Name = "output_box";
            this.output_box.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.output_box.Size = new System.Drawing.Size(962, 154);
            this.output_box.TabIndex = 3;
            this.output_box.TabStop = false;
            this.output_box.Text = "Hedef Dizin";
            this.output_box.Enter += new System.EventHandler(this.output_box_Enter);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(14, 83);
            this.username.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(318, 26);
            this.username.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lütfen Trendyol Kullanıcı Adınızı Giriniz";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(534, 448);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(532, 28);
            this.comboBox1.TabIndex = 4;
            // 
            // Directory_Name_Submit
            // 
            this.Directory_Name_Submit.Location = new System.Drawing.Point(655, 61);
            this.Directory_Name_Submit.Name = "Directory_Name_Submit";
            this.Directory_Name_Submit.Size = new System.Drawing.Size(142, 38);
            this.Directory_Name_Submit.TabIndex = 2;
            this.Directory_Name_Submit.Text = "Giriş";
            this.Directory_Name_Submit.UseVisualStyleBackColor = true;
            this.Directory_Name_Submit.Click += new System.EventHandler(this.Directory_Name_Submit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(530, 408);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Kaydedilecek Lokasyon:";
            // 
            // Network_Design_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 755);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.output_box);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.run_option_group_box);
            this.Controls.Add(this.input_files_parameters);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Network_Design_Form";
            this.Text = "Network Design Decision Making Tool";
            this.Load += new System.EventHandler(this.Network_Design_Form_Load);
            this.input_files_parameters.ResumeLayout(false);
            this.input_files_parameters.PerformLayout();
            this.run_option_group_box.ResumeLayout(false);
            this.run_option_group_box.PerformLayout();
            this.output_box.ResumeLayout(false);
            this.output_box.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox input_files_parameters;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox Month_box;
        private System.Windows.Forms.Label month_label;
        private System.Windows.Forms.TextBox presolved_box;
        private System.Windows.Forms.TextBox parameter_box;
        private System.Windows.Forms.TextBox seller_box;
        private System.Windows.Forms.TextBox pot_xDock_box;
        private System.Windows.Forms.TextBox demand_box;
        private System.Windows.Forms.Label presolved_xDock_label;
        private System.Windows.Forms.Label parameter_label;
        private System.Windows.Forms.Label seller_label;
        private System.Windows.Forms.Label pot_xDock_label;
        private System.Windows.Forms.Label demand_label;
        private System.Windows.Forms.GroupBox run_option_group_box;
        private System.Windows.Forms.RadioButton no_button;
        private System.Windows.Forms.RadioButton yes_button;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.GroupBox output_box;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Directory_Name_Submit;
        private System.Windows.Forms.Label label2;
    }
}

