namespace Core_Form
{
    partial class Network_Design_Form_Core
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            this.label1 = new System.Windows.Forms.Label();
            this.uotput_box = new System.Windows.Forms.TextBox();
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
            this.input_files_parameters.Location = new System.Drawing.Point(33, 64);
            this.input_files_parameters.Name = "input_files_parameters";
            this.input_files_parameters.Size = new System.Drawing.Size(285, 274);
            this.input_files_parameters.TabIndex = 0;
            this.input_files_parameters.TabStop = false;
            this.input_files_parameters.Text = "Dökümanlar ve Parametreler";
            // 
            // Month_box
            // 
            this.Month_box.Location = new System.Drawing.Point(168, 233);
            this.Month_box.Name = "Month_box";
            this.Month_box.Size = new System.Drawing.Size(100, 20);
            this.Month_box.TabIndex = 10;
            // 
            // month_label
            // 
            this.month_label.AutoSize = true;
            this.month_label.Location = new System.Drawing.Point(6, 240);
            this.month_label.Name = "month_label";
            this.month_label.Size = new System.Drawing.Size(154, 13);
            this.month_label.TabIndex = 9;
            this.month_label.Text = "Çalışılan Ay (1: Ocak, 12:Aralık)";
            // 
            // presolved_box
            // 
            this.presolved_box.Location = new System.Drawing.Point(168, 196);
            this.presolved_box.Name = "presolved_box";
            this.presolved_box.Size = new System.Drawing.Size(100, 20);
            this.presolved_box.TabIndex = 8;
            // 
            // parameter_box
            // 
            this.parameter_box.Location = new System.Drawing.Point(168, 152);
            this.parameter_box.Name = "parameter_box";
            this.parameter_box.Size = new System.Drawing.Size(100, 20);
            this.parameter_box.TabIndex = 7;
            // 
            // seller_box
            // 
            this.seller_box.Location = new System.Drawing.Point(168, 113);
            this.seller_box.Name = "seller_box";
            this.seller_box.Size = new System.Drawing.Size(100, 20);
            this.seller_box.TabIndex = 6;
            // 
            // pot_xDock_box
            // 
            this.pot_xDock_box.Location = new System.Drawing.Point(168, 74);
            this.pot_xDock_box.Name = "pot_xDock_box";
            this.pot_xDock_box.Size = new System.Drawing.Size(100, 20);
            this.pot_xDock_box.TabIndex = 5;
            // 
            // demand_box
            // 
            this.demand_box.Location = new System.Drawing.Point(168, 36);
            this.demand_box.Name = "demand_box";
            this.demand_box.Size = new System.Drawing.Size(100, 20);
            this.demand_box.TabIndex = 1;
            // 
            // presolved_xDock_label
            // 
            this.presolved_xDock_label.AutoSize = true;
            this.presolved_xDock_label.Location = new System.Drawing.Point(6, 199);
            this.presolved_xDock_label.Name = "presolved_xDock_label";
            this.presolved_xDock_label.Size = new System.Drawing.Size(116, 13);
            this.presolved_xDock_label.TabIndex = 4;
            this.presolved_xDock_label.Text = "Açılmış xDock Dosyası:";
            // 
            // parameter_label
            // 
            this.parameter_label.AutoSize = true;
            this.parameter_label.Location = new System.Drawing.Point(6, 155);
            this.parameter_label.Name = "parameter_label";
            this.parameter_label.Size = new System.Drawing.Size(98, 13);
            this.parameter_label.TabIndex = 3;
            this.parameter_label.Text = "Parametre Dosyası:";
            // 
            // seller_label
            // 
            this.seller_label.AutoSize = true;
            this.seller_label.Location = new System.Drawing.Point(6, 116);
            this.seller_label.Name = "seller_label";
            this.seller_label.Size = new System.Drawing.Size(94, 13);
            this.seller_label.TabIndex = 2;
            this.seller_label.Text = "Tedarikçi Dosyası:";
            // 
            // pot_xDock_label
            // 
            this.pot_xDock_label.AutoSize = true;
            this.pot_xDock_label.Location = new System.Drawing.Point(6, 77);
            this.pot_xDock_label.Name = "pot_xDock_label";
            this.pot_xDock_label.Size = new System.Drawing.Size(132, 13);
            this.pot_xDock_label.TabIndex = 1;
            this.pot_xDock_label.Text = "Potensiyel xDock Dosyası:";
            // 
            // demand_label
            // 
            this.demand_label.AutoSize = true;
            this.demand_label.Location = new System.Drawing.Point(6, 39);
            this.demand_label.Name = "demand_label";
            this.demand_label.Size = new System.Drawing.Size(116, 13);
            this.demand_label.TabIndex = 0;
            this.demand_label.Text = "Talep Noktası Dosyası:";
            // 
            // run_option_group_box
            // 
            this.run_option_group_box.Controls.Add(this.no_button);
            this.run_option_group_box.Controls.Add(this.yes_button);
            this.run_option_group_box.Location = new System.Drawing.Point(443, 73);
            this.run_option_group_box.Name = "run_option_group_box";
            this.run_option_group_box.Size = new System.Drawing.Size(285, 71);
            this.run_option_group_box.TabIndex = 1;
            this.run_option_group_box.TabStop = false;
            this.run_option_group_box.Text = "Talep Noktası - xDock Ataması gerçekleştirilsin mi ?";
            // 
            // no_button
            // 
            this.no_button.AutoSize = true;
            this.no_button.Location = new System.Drawing.Point(157, 36);
            this.no_button.Name = "no_button";
            this.no_button.Size = new System.Drawing.Size(49, 17);
            this.no_button.TabIndex = 1;
            this.no_button.TabStop = true;
            this.no_button.Text = "Hayır";
            this.no_button.UseVisualStyleBackColor = true;
            // 
            // yes_button
            // 
            this.yes_button.AutoSize = true;
            this.yes_button.Location = new System.Drawing.Point(51, 36);
            this.yes_button.Name = "yes_button";
            this.yes_button.Size = new System.Drawing.Size(47, 17);
            this.yes_button.TabIndex = 0;
            this.yes_button.TabStop = true;
            this.yes_button.Text = "Evet";
            this.yes_button.UseVisualStyleBackColor = true;
            this.yes_button.CheckedChanged += new System.EventHandler(this.yes_button_CheckedChanged);
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(534, 332);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(75, 23);
            this.send_button.TabIndex = 2;
            this.send_button.Text = "OK";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // output_box
            // 
            this.output_box.Controls.Add(this.uotput_box);
            this.output_box.Controls.Add(this.label1);
            this.output_box.Location = new System.Drawing.Point(443, 198);
            this.output_box.Name = "output_box";
            this.output_box.Size = new System.Drawing.Size(285, 100);
            this.output_box.TabIndex = 3;
            this.output_box.TabStop = false;
            this.output_box.Text = "Hedef Dizin";
            this.output_box.Enter += new System.EventHandler(this.output_box_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Çıktıların yazdırılacağı hedef klasörü belirtiniz:";
            // 
            // uotput_box
            // 
            this.uotput_box.Location = new System.Drawing.Point(9, 54);
            this.uotput_box.Name = "uotput_box";
            this.uotput_box.Size = new System.Drawing.Size(213, 20);
            this.uotput_box.TabIndex = 1;
            // 
            // Network_Design_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.output_box);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.run_option_group_box);
            this.Controls.Add(this.input_files_parameters);
            this.Name = "Network_Design_Form";
            this.Text = "Network Design Decision Making Tool";
            this.input_files_parameters.ResumeLayout(false);
            this.input_files_parameters.PerformLayout();
            this.run_option_group_box.ResumeLayout(false);
            this.run_option_group_box.PerformLayout();
            this.output_box.ResumeLayout(false);
            this.output_box.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TextBox uotput_box;
        private System.Windows.Forms.Label label1;
    }
}

