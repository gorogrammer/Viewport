﻿namespace ViewPort.Views
{
    partial class Change_Code
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
            this.Code_Change_TB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Code_Change_TB
            // 
            this.Code_Change_TB.Location = new System.Drawing.Point(37, 37);
            this.Code_Change_TB.Name = "Code_Change_TB";
            this.Code_Change_TB.Size = new System.Drawing.Size(100, 21);
            this.Code_Change_TB.TabIndex = 0;
            this.Code_Change_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Code_Change_TB_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(198, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "코드 변경";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "코드 변경은 1,과 200번대로 변경할 수 없습니다.";
            // 
            // Change_Code
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 99);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Code_Change_TB);
            this.Name = "Change_Code";
            this.Text = "Change_Code";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Code_Change_TB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}