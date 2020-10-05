namespace ViewPort.Views
{
    partial class XYLocationFilter
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.X_TB = new System.Windows.Forms.TextBox();
            this.Y_TB = new System.Windows.Forms.TextBox();
            this.YFilter_TB = new System.Windows.Forms.TextBox();
            this.Xfilter_TB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "기존 X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "기존 Y";
            // 
            // X_TB
            // 
            this.X_TB.Location = new System.Drawing.Point(61, 56);
            this.X_TB.Name = "X_TB";
            this.X_TB.Size = new System.Drawing.Size(66, 21);
            this.X_TB.TabIndex = 1;
            // 
            // Y_TB
            // 
            this.Y_TB.Location = new System.Drawing.Point(210, 56);
            this.Y_TB.Name = "Y_TB";
            this.Y_TB.Size = new System.Drawing.Size(66, 21);
            this.Y_TB.TabIndex = 2;
            // 
            // YFilter_TB
            // 
            this.YFilter_TB.Location = new System.Drawing.Point(211, 90);
            this.YFilter_TB.Name = "YFilter_TB";
            this.YFilter_TB.Size = new System.Drawing.Size(66, 21);
            this.YFilter_TB.TabIndex = 6;
            // 
            // Xfilter_TB
            // 
            this.Xfilter_TB.Location = new System.Drawing.Point(62, 90);
            this.Xfilter_TB.Name = "Xfilter_TB";
            this.Xfilter_TB.Size = new System.Drawing.Size(66, 21);
            this.Xfilter_TB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y 범위";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "X 범위";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(62, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Filter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // XYLocationFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 200);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.YFilter_TB);
            this.Controls.Add(this.Xfilter_TB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Y_TB);
            this.Controls.Add(this.X_TB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "XYLocationFilter";
            this.Text = "XYLocationFilter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XYLocationFilter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox X_TB;
        private System.Windows.Forms.TextBox Y_TB;
        private System.Windows.Forms.TextBox YFilter_TB;
        private System.Windows.Forms.TextBox Xfilter_TB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}