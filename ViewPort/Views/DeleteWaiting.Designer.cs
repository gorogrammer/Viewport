namespace ViewPort.Views
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
            this.label1 = new System.Windows.Forms.Label();
            this.Del_img_list = new System.Windows.Forms.TextBox();
            this.Delete_Img_In_ZIp = new System.Windows.Forms.Button();
            this.Delete_wait_img_bt = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.S_Page_TB = new System.Windows.Forms.TextBox();
            this.E_Page_TB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(20, 60);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
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
            this.splitContainer1.Size = new System.Drawing.Size(998, 511);
            this.splitContainer1.SplitterDistance = 31;
            this.splitContainer1.TabIndex = 0;
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
            this.Delete_wait_img_bt.Location = new System.Drawing.Point(562, 4);
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
            this.S_Page_TB.ReadOnly = true;
            this.S_Page_TB.Size = new System.Drawing.Size(38, 21);
            this.S_Page_TB.TabIndex = 9;
            // 
            // E_Page_TB
            // 
            this.E_Page_TB.Location = new System.Drawing.Point(158, 5);
            this.E_Page_TB.Name = "E_Page_TB";
            this.E_Page_TB.ReadOnly = true;
            this.E_Page_TB.Size = new System.Drawing.Size(38, 21);
            this.E_Page_TB.TabIndex = 11;
            // 
            // DeleteWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1038, 591);
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
    }
}