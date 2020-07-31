namespace ViewPort
{
    partial class FormViewPort
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormViewPort));
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.Height_TB = new System.Windows.Forms.TextBox();
            this.Width_TB = new System.Windows.Forms.TextBox();
            this.Rows_TB = new System.Windows.Forms.TextBox();
            this.E_Page_TB = new System.Windows.Forms.TextBox();
            this.Cols_TB = new System.Windows.Forms.TextBox();
            this.S_Page_TB = new System.Windows.Forms.TextBox();
            this.Equipment_DF_CLB = new System.Windows.Forms.CheckedListBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.zipLoadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer3.Panel1.Controls.Add(this.Height_TB);
            this.splitContainer3.Panel1.Controls.Add(this.Width_TB);
            this.splitContainer3.Panel1.Controls.Add(this.Rows_TB);
            this.splitContainer3.Panel1.Controls.Add(this.E_Page_TB);
            this.splitContainer3.Panel1.Controls.Add(this.Cols_TB);
            this.splitContainer3.Panel1.Controls.Add(this.S_Page_TB);
            this.splitContainer3.Panel1.Controls.Add(this.Equipment_DF_CLB);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer3.Size = new System.Drawing.Size(145, 215);
            this.splitContainer3.SplitterDistance = 186;
            this.splitContainer3.TabIndex = 0;
            // 
            // Height_TB
            // 
            this.Height_TB.Location = new System.Drawing.Point(165, 107);
            this.Height_TB.Name = "Height_TB";
            this.Height_TB.Size = new System.Drawing.Size(60, 21);
            this.Height_TB.TabIndex = 3;
            this.Height_TB.Text = "175";
            // 
            // Width_TB
            // 
            this.Width_TB.Location = new System.Drawing.Point(89, 107);
            this.Width_TB.Name = "Width_TB";
            this.Width_TB.Size = new System.Drawing.Size(60, 21);
            this.Width_TB.TabIndex = 2;
            this.Width_TB.Text = "175";
            // 
            // Rows_TB
            // 
            this.Rows_TB.Location = new System.Drawing.Point(165, 57);
            this.Rows_TB.Name = "Rows_TB";
            this.Rows_TB.Size = new System.Drawing.Size(60, 21);
            this.Rows_TB.TabIndex = 1;
            this.Rows_TB.Text = "5";
            // 
            // E_Page_TB
            // 
            this.E_Page_TB.Location = new System.Drawing.Point(165, 157);
            this.E_Page_TB.Name = "E_Page_TB";
            this.E_Page_TB.Size = new System.Drawing.Size(60, 21);
            this.E_Page_TB.TabIndex = 1;
            // 
            // Cols_TB
            // 
            this.Cols_TB.Location = new System.Drawing.Point(89, 57);
            this.Cols_TB.Name = "Cols_TB";
            this.Cols_TB.Size = new System.Drawing.Size(60, 21);
            this.Cols_TB.TabIndex = 1;
            this.Cols_TB.Text = "8";
            // 
            // S_Page_TB
            // 
            this.S_Page_TB.Location = new System.Drawing.Point(89, 157);
            this.S_Page_TB.Name = "S_Page_TB";
            this.S_Page_TB.Size = new System.Drawing.Size(60, 21);
            this.S_Page_TB.TabIndex = 1;
            // 
            // Equipment_DF_CLB
            // 
            this.Equipment_DF_CLB.BackColor = System.Drawing.Color.DarkGray;
            this.Equipment_DF_CLB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Equipment_DF_CLB.FormattingEnabled = true;
            this.Equipment_DF_CLB.Location = new System.Drawing.Point(0, 102);
            this.Equipment_DF_CLB.Name = "Equipment_DF_CLB";
            this.Equipment_DF_CLB.Size = new System.Drawing.Size(145, 84);
            this.Equipment_DF_CLB.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer4.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer4.Size = new System.Drawing.Size(145, 207);
            this.splitContainer4.SplitterDistance = 124;
            this.splitContainer4.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.DarkGray;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(145, 124);
            this.dataGridView1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.LightGray;
            this.splitContainer1.Size = new System.Drawing.Size(800, 426);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(145, 426);
            this.splitContainer2.SplitterDistance = 215;
            this.splitContainer2.TabIndex = 0;
            // 
            // zipLoadFileToolStripMenuItem
            // 
            this.zipLoadFileToolStripMenuItem.Name = "zipLoadFileToolStripMenuItem";
            this.zipLoadFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zipLoadFileToolStripMenuItem.Text = "Zip Load File";
            this.zipLoadFileToolStripMenuItem.Click += new System.EventHandler(this.zipLoadFileToolStripMenuItem_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkGray;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zipLoadFileToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // FormViewPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormViewPort";
            this.Text = "Carlo ViewPort";
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer3;
        public System.Windows.Forms.TextBox Height_TB;
        public System.Windows.Forms.TextBox Width_TB;
        public System.Windows.Forms.TextBox Rows_TB;
        public System.Windows.Forms.TextBox E_Page_TB;
        public System.Windows.Forms.TextBox Cols_TB;
        public System.Windows.Forms.TextBox S_Page_TB;
        private System.Windows.Forms.CheckedListBox Equipment_DF_CLB;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem zipLoadFileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
    }
}

