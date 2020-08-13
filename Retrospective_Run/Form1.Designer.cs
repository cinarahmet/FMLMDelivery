namespace Retrospective_Run
{
    partial class Retro_Form
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
            this.Stable_Parameter_Location = new System.Windows.Forms.TextBox();
            this.Different_Parameter_Location = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Stable_Parameter_Location
            // 
            this.Stable_Parameter_Location.Location = new System.Drawing.Point(170, 22);
            this.Stable_Parameter_Location.Name = "Stable_Parameter_Location";
            this.Stable_Parameter_Location.Size = new System.Drawing.Size(218, 27);
            this.Stable_Parameter_Location.TabIndex = 0;
            this.Stable_Parameter_Location.Click += new System.EventHandler(this.Stable_Clik);
            // 
            // Different_Parameter_Location
            // 
            this.Different_Parameter_Location.Location = new System.Drawing.Point(170, 94);
            this.Different_Parameter_Location.Name = "Different_Parameter_Location";
            this.Different_Parameter_Location.Size = new System.Drawing.Size(218, 27);
            this.Different_Parameter_Location.TabIndex = 1;
            this.Different_Parameter_Location.Click += new System.EventHandler(this.Different_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(33, 33);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(135, 24);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Sabit Parametre";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.Stable_Parameter);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(33, 93);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(162, 24);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Değişken Parametre";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.Different_Parameter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(26, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 153);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Çalıştırma Tipi";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sabit Parametre";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Değişken Parametre";
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
            this.groupBox2.Location = new System.Drawing.Point(448, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(453, 404);
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
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.Different_Parameter_Location);
            this.groupBox3.Controls.Add(this.Stable_Parameter_Location);
            this.groupBox3.Location = new System.Drawing.Point(8, 40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 436);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Girdi ve Çalıştırma Tipi";
            // 
            // Retro_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 500);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "Retro_Form";
            this.Text = "Eskiye Dönük Çalıştırma";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Stable_Parameter_Location;
        private System.Windows.Forms.TextBox Different_Parameter_Location;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
    }
}

