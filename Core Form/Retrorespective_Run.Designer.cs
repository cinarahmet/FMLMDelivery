namespace Core_Form
{
    partial class Retrorespective_Run
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Json_File_Location = new System.Windows.Forms.TextBox();
            this.Stable_Run = new System.Windows.Forms.RadioButton();
            this.Different_Run = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Month_Selected = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Hub_Cov_Box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._threshold = new System.Windows.Forms.TextBox();
            this.Min_Cap_Courier = new System.Windows.Forms.TextBox();
            this.Max_Cap_Courier = new System.Windows.Forms.TextBox();
            this.Km_Başı_Paket = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Output_Loc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Json_File_Location
            // 
            this.Json_File_Location.Location = new System.Drawing.Point(164, 53);
            this.Json_File_Location.Name = "Json_File_Location";
            this.Json_File_Location.Size = new System.Drawing.Size(218, 27);
            this.Json_File_Location.TabIndex = 0;
            this.Json_File_Location.Click += new System.EventHandler(this.Stable_Clik);
            // 
            // Stable_Run
            // 
            this.Stable_Run.AutoSize = true;
            this.Stable_Run.Location = new System.Drawing.Point(6, 46);
            this.Stable_Run.Name = "Stable_Run";
            this.Stable_Run.Size = new System.Drawing.Size(135, 24);
            this.Stable_Run.TabIndex = 2;
            this.Stable_Run.TabStop = true;
            this.Stable_Run.Text = "Sabit Parametre";
            this.Stable_Run.UseVisualStyleBackColor = true;
            this.Stable_Run.Click += new System.EventHandler(this.Stable_Parameter);
            // 
            // Different_Run
            // 
            this.Different_Run.AutoSize = true;
            this.Different_Run.Location = new System.Drawing.Point(6, 98);
            this.Different_Run.Name = "Different_Run";
            this.Different_Run.Size = new System.Drawing.Size(162, 24);
            this.Different_Run.TabIndex = 3;
            this.Different_Run.TabStop = true;
            this.Different_Run.Text = "Değişken Parametre";
            this.Different_Run.UseVisualStyleBackColor = true;
            this.Different_Run.Click += new System.EventHandler(this.Different_Parameter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Different_Run);
            this.groupBox1.Controls.Add(this.Stable_Run);
            this.groupBox1.Location = new System.Drawing.Point(26, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 153);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Çalıştırma Tipi";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Girdi Dosyası";
            // 
            // Month_Selected
            // 
            this.Month_Selected.FormattingEnabled = true;
            this.Month_Selected.Items.AddRange(new object[] {
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
            this.Month_Selected.Location = new System.Drawing.Point(274, 21);
            this.Month_Selected.Name = "Month_Selected";
            this.Month_Selected.Size = new System.Drawing.Size(119, 28);
            this.Month_Selected.TabIndex = 7;
            this.Month_Selected.Text = "Ay Seçiniz";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Çalışılacak Ay";
            // 
            // Hub_Cov_Box
            // 
            this.Hub_Cov_Box.Location = new System.Drawing.Point(274, 90);
            this.Hub_Cov_Box.Name = "Hub_Cov_Box";
            this.Hub_Cov_Box.Size = new System.Drawing.Size(64, 27);
            this.Hub_Cov_Box.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "xDock-Hub Talep Kapsamı(0.00 -1.00)";
            // 
            // _threshold
            // 
            this._threshold.Location = new System.Drawing.Point(274, 169);
            this._threshold.Name = "_threshold";
            this._threshold.Size = new System.Drawing.Size(125, 27);
            this._threshold.TabIndex = 11;
            // 
            // Min_Cap_Courier
            // 
            this.Min_Cap_Courier.Location = new System.Drawing.Point(274, 226);
            this.Min_Cap_Courier.Name = "Min_Cap_Courier";
            this.Min_Cap_Courier.Size = new System.Drawing.Size(125, 27);
            this.Min_Cap_Courier.TabIndex = 12;
            // 
            // Max_Cap_Courier
            // 
            this.Max_Cap_Courier.Location = new System.Drawing.Point(274, 286);
            this.Max_Cap_Courier.Name = "Max_Cap_Courier";
            this.Max_Cap_Courier.Size = new System.Drawing.Size(125, 27);
            this.Max_Cap_Courier.TabIndex = 13;
            // 
            // Km_Başı_Paket
            // 
            this.Km_Başı_Paket.Location = new System.Drawing.Point(274, 339);
            this.Km_Başı_Paket.Name = "Km_Başı_Paket";
            this.Km_Başı_Paket.Size = new System.Drawing.Size(125, 27);
            this.Km_Başı_Paket.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "İstenen Kurye Verimliliği :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Min Kurye Verimliliği:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "Max Kurye Verimliliği:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 346);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "Km Başı Paket:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Km_Başı_Paket);
            this.groupBox2.Controls.Add(this.Max_Cap_Courier);
            this.groupBox2.Controls.Add(this.Min_Cap_Courier);
            this.groupBox2.Controls.Add(this._threshold);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.Hub_Cov_Box);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Month_Selected);
            this.groupBox2.Location = new System.Drawing.Point(477, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 436);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parametreler";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.Json_File_Location);
            this.groupBox3.Location = new System.Drawing.Point(8, 40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 284);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Girdi ve Çalıştırma Tipi";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(421, 492);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 21;
            this.button1.Text = "Çalıştır";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Run);
            // 
            // Output_Loc
            // 
            this.Output_Loc.Location = new System.Drawing.Point(164, 55);
            this.Output_Loc.Name = "Output_Loc";
            this.Output_Loc.Size = new System.Drawing.Size(218, 27);
            this.Output_Loc.TabIndex = 22;
            this.Output_Loc.Click += new System.EventHandler(this.Output_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "Çıktı Dosyaları";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.Output_Loc);
            this.groupBox4.Location = new System.Drawing.Point(8, 370);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(425, 105);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Çıktı Dosya Lokasyonu";
            // 
            // Retrorespective_Run
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 533);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "Retrorespective_Run";
            this.Text = "Eskiye Dönük Çalıştırma";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Json_File_Location;
        private System.Windows.Forms.RadioButton Stable_Run;
        private System.Windows.Forms.RadioButton Different_Run;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Month_Selected;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Hub_Cov_Box;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _threshold;
        private System.Windows.Forms.TextBox Min_Cap_Courier;
        private System.Windows.Forms.TextBox Max_Cap_Courier;
        private System.Windows.Forms.TextBox Km_Başı_Paket;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Output_Loc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
