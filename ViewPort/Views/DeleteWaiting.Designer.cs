﻿namespace ViewPort.Views
{
    partial class DeleteWaiting
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Select_All_BTN = new System.Windows.Forms.Button();
            this.Equipment_DF_CLB = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.Camera_NO_Filter_TB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Frame_S_TB = new System.Windows.Forms.TextBox();
            this.Print_Image_EQ = new System.Windows.Forms.CheckBox();
            this.Print_Image_Name = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Del_img_list = new System.Windows.Forms.TextBox();
            this.Delete_Img_In_ZIp = new System.Windows.Forms.Button();
            this.Delete_wait_img_bt = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.S_Page_TB = new System.Windows.Forms.TextBox();
            this.E_Page_TB = new System.Windows.Forms.TextBox();
            this.Select_Empty_BTN = new System.Windows.Forms.Button();
            this._filterAct_bt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(20, 60);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._filterAct_bt);
            this.splitContainer1.Panel1.Controls.Add(this.Select_Empty_BTN);
            this.splitContainer1.Panel1.Controls.Add(this.Select_All_BTN);
            this.splitContainer1.Panel1.Controls.Add(this.Equipment_DF_CLB);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.Camera_NO_Filter_TB);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.Frame_S_TB);
            this.splitContainer1.Panel1.Controls.Add(this.Print_Image_EQ);
            this.splitContainer1.Panel1.Controls.Add(this.Print_Image_Name);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.Del_img_list);
            this.splitContainer1.Panel1.Controls.Add(this.Delete_Img_In_ZIp);
            this.splitContainer1.Panel1.Controls.Add(this.Delete_wait_img_bt);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.S_Page_TB);
            this.splitContainer1.Panel1.Controls.Add(this.E_Page_TB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel2_MouseDown);
            this.splitContainer1.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel2_MouseMove);
            this.splitContainer1.Panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel2_MouseUp);
            this.splitContainer1.Size = new System.Drawing.Size(998, 540);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.TabIndex = 0;
            // 
            // Select_All_BTN
            // 
            this.Select_All_BTN.Location = new System.Drawing.Point(801, 5);
            this.Select_All_BTN.Name = "Select_All_BTN";
            this.Select_All_BTN.Size = new System.Drawing.Size(75, 23);
            this.Select_All_BTN.TabIndex = 38;
            this.Select_All_BTN.Text = "전체 선택";
            this.Select_All_BTN.UseVisualStyleBackColor = true;
            this.Select_All_BTN.Click += new System.EventHandler(this.Select_All_BTN_Click);
            // 
            // Equipment_DF_CLB
            // 
            this.Equipment_DF_CLB.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Equipment_DF_CLB.CheckOnClick = true;
            this.Equipment_DF_CLB.FormattingEnabled = true;
            this.Equipment_DF_CLB.Location = new System.Drawing.Point(554, 5);
            this.Equipment_DF_CLB.Name = "Equipment_DF_CLB";
            this.Equipment_DF_CLB.Size = new System.Drawing.Size(241, 68);
            this.Equipment_DF_CLB.TabIndex = 37;
            this.Equipment_DF_CLB.SelectedValueChanged += new System.EventHandler(this.Equipment_DF_CLB_SelectedValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 36;
            this.button1.Text = "전체";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(162, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 12);
            this.label6.TabIndex = 34;
            this.label6.Text = "■ Camera No";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Camera_NO_Filter_TB
            // 
            this.Camera_NO_Filter_TB.Location = new System.Drawing.Point(251, 35);
            this.Camera_NO_Filter_TB.Name = "Camera_NO_Filter_TB";
            this.Camera_NO_Filter_TB.Size = new System.Drawing.Size(54, 21);
            this.Camera_NO_Filter_TB.TabIndex = 35;
            this.Camera_NO_Filter_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Camera_NO_Filter_TB_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "■ Frame No";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frame_S_TB
            // 
            this.Frame_S_TB.Location = new System.Drawing.Point(102, 33);
            this.Frame_S_TB.Name = "Frame_S_TB";
            this.Frame_S_TB.Size = new System.Drawing.Size(38, 21);
            this.Frame_S_TB.TabIndex = 32;
            this.Frame_S_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frame_S_TB_KeyDown);
            // 
            // Print_Image_EQ
            // 
            this.Print_Image_EQ.AutoSize = true;
            this.Print_Image_EQ.Location = new System.Drawing.Point(163, 59);
            this.Print_Image_EQ.Name = "Print_Image_EQ";
            this.Print_Image_EQ.Size = new System.Drawing.Size(80, 16);
            this.Print_Image_EQ.TabIndex = 31;
            this.Print_Image_EQ.Text = "Image EQ";
            this.Print_Image_EQ.UseVisualStyleBackColor = true;
            this.Print_Image_EQ.CheckedChanged += new System.EventHandler(this.Print_Image_EQ_CheckedChanged);
            // 
            // Print_Image_Name
            // 
            this.Print_Image_Name.AutoSize = true;
            this.Print_Image_Name.Location = new System.Drawing.Point(14, 59);
            this.Print_Image_Name.Name = "Print_Image_Name";
            this.Print_Image_Name.Size = new System.Drawing.Size(97, 16);
            this.Print_Image_Name.TabIndex = 24;
            this.Print_Image_Name.Text = "Image Name";
            this.Print_Image_Name.UseVisualStyleBackColor = true;
            this.Print_Image_Name.CheckedChanged += new System.EventHandler(this.Print_Image_Name_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "■ 이미지 수";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Del_img_list
            // 
            this.Del_img_list.Location = new System.Drawing.Point(322, 4);
            this.Del_img_list.Name = "Del_img_list";
            this.Del_img_list.ReadOnly = true;
            this.Del_img_list.Size = new System.Drawing.Size(100, 21);
            this.Del_img_list.TabIndex = 22;
            // 
            // Delete_Img_In_ZIp
            // 
            this.Delete_Img_In_ZIp.Location = new System.Drawing.Point(891, 4);
            this.Delete_Img_In_ZIp.Name = "Delete_Img_In_ZIp";
            this.Delete_Img_In_ZIp.Size = new System.Drawing.Size(94, 23);
            this.Delete_Img_In_ZIp.TabIndex = 21;
            this.Delete_Img_In_ZIp.Text = "Image 삭제";
            this.Delete_Img_In_ZIp.UseVisualStyleBackColor = true;
            this.Delete_Img_In_ZIp.Click += new System.EventHandler(this.Delete_Img_In_ZIp_Click);
            // 
            // Delete_wait_img_bt
            // 
            this.Delete_wait_img_bt.Location = new System.Drawing.Point(442, 4);
            this.Delete_wait_img_bt.Name = "Delete_wait_img_bt";
            this.Delete_wait_img_bt.Size = new System.Drawing.Size(95, 23);
            this.Delete_wait_img_bt.TabIndex = 20;
            this.Delete_wait_img_bt.Text = "삭제대기 제외";
            this.Delete_wait_img_bt.UseVisualStyleBackColor = true;
            this.Delete_wait_img_bt.Click += new System.EventHandler(this.Delete_wait_img_bt_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "■ 페이지 번호";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // S_Page_TB
            // 
            this.S_Page_TB.Location = new System.Drawing.Point(101, 5);
            this.S_Page_TB.Name = "S_Page_TB";
            this.S_Page_TB.Size = new System.Drawing.Size(38, 21);
            this.S_Page_TB.TabIndex = 9;
            this.S_Page_TB.TabStop = false;
            this.S_Page_TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.S_Page_TB_KeyDown);
            // 
            // E_Page_TB
            // 
            this.E_Page_TB.Location = new System.Drawing.Point(158, 5);
            this.E_Page_TB.Name = "E_Page_TB";
            this.E_Page_TB.ReadOnly = true;
            this.E_Page_TB.Size = new System.Drawing.Size(38, 21);
            this.E_Page_TB.TabIndex = 11;
            // 
            // Select_Empty_BTN
            // 
            this.Select_Empty_BTN.Location = new System.Drawing.Point(801, 31);
            this.Select_Empty_BTN.Name = "Select_Empty_BTN";
            this.Select_Empty_BTN.Size = new System.Drawing.Size(75, 23);
            this.Select_Empty_BTN.TabIndex = 39;
            this.Select_Empty_BTN.Text = "전체 해제";
            this.Select_Empty_BTN.UseVisualStyleBackColor = true;
            this.Select_Empty_BTN.Click += new System.EventHandler(this.Select_Empty_BTN_Click);
            // 
            // _filterAct_bt
            // 
            this._filterAct_bt.Location = new System.Drawing.Point(801, 59);
            this._filterAct_bt.Name = "_filterAct_bt";
            this._filterAct_bt.Size = new System.Drawing.Size(75, 23);
            this._filterAct_bt.TabIndex = 40;
            this._filterAct_bt.Text = "필터 적용";
            this._filterAct_bt.UseVisualStyleBackColor = true;
            this._filterAct_bt.Click += new System.EventHandler(this._filterAct_bt_Click);
            // 
            // DeleteWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1038, 620);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DeleteWaiting";
            this.Text = "삭제대기 이미지";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeleteWaiting_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DeleteWaiting_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox S_Page_TB;
        public System.Windows.Forms.TextBox E_Page_TB;
        private System.Windows.Forms.Button Delete_wait_img_bt;
        private System.Windows.Forms.Button Delete_Img_In_ZIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Del_img_list;
        public System.Windows.Forms.CheckBox Print_Image_Name;
        public System.Windows.Forms.CheckBox Print_Image_EQ;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox Frame_S_TB;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox Camera_NO_Filter_TB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox Equipment_DF_CLB;
        private System.Windows.Forms.Button Select_All_BTN;
        private System.Windows.Forms.Button _filterAct_bt;
        private System.Windows.Forms.Button Select_Empty_BTN;
    }
}