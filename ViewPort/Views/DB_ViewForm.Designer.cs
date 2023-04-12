namespace ViewPort.Views
{
    partial class DB_ViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DB_ViewForm));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.Add = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.RegBT = new DevExpress.XtraBars.BarButtonItem();
            this.DataSerach_BT = new DevExpress.XtraBars.BarButtonItem();
            this.ZipLoad_BT = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.Menu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.LogMenu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.LotMenu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.UserMenu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.DeletePathMenU = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.Zip = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement5 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.KPI = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement2 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.LotGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.accordionControlElement4 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.E_DateTime = new System.Windows.Forms.DateTimePicker();
            this.S_DateTime = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LotGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.Add,
            this.barButtonItem1,
            this.RegBT,
            this.DataSerach_BT,
            this.ZipLoad_BT});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 7;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            this.ribbon.Size = new System.Drawing.Size(1147, 90);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // Add
            // 
            this.Add.Caption = "추가";
            this.Add.Hint = "DB 데이터 추가";
            this.Add.Id = 1;
            this.Add.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Add.ImageOptions.Image")));
            this.Add.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("Add.ImageOptions.LargeImage")));
            this.Add.Name = "Add";
            this.Add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.button5_Click);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Save";
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.barButtonItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick_1);
            // 
            // RegBT
            // 
            this.RegBT.Caption = "가입승인요청목록";
            this.RegBT.Hint = "권한부여기능";
            this.RegBT.Id = 4;
            this.RegBT.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("RegBT.ImageOptions.Image")));
            this.RegBT.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("RegBT.ImageOptions.LargeImage")));
            this.RegBT.Name = "RegBT";
            this.RegBT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RegBT_ItemClick);
            // 
            // DataSerach_BT
            // 
            this.DataSerach_BT.Caption = "데이터조회";
            this.DataSerach_BT.Id = 5;
            this.DataSerach_BT.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("DataSerach_BT.ImageOptions.Image")));
            this.DataSerach_BT.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("DataSerach_BT.ImageOptions.LargeImage")));
            this.DataSerach_BT.Name = "DataSerach_BT";
            this.DataSerach_BT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DataSerach_BT_ItemClick);
            // 
            // ZipLoad_BT
            // 
            this.ZipLoad_BT.Caption = "ZipLoad";
            this.ZipLoad_BT.Id = 6;
            this.ZipLoad_BT.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ZipLoad_BT.ImageOptions.Image")));
            this.ZipLoad_BT.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ZipLoad_BT.ImageOptions.LargeImage")));
            this.ZipLoad_BT.Name = "ZipLoad_BT";
            this.ZipLoad_BT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ZipLoad_BT_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Edit";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.Add);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.RegBT);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.DataSerach_BT);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "ribbonPageGroup3";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.ZipLoad_BT);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 759);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1147, 24);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 90);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.accordionControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1147, 669);
            this.splitContainer1.SplitterDistance = 167;
            this.splitContainer1.TabIndex = 2;
            // 
            // accordionControl1
            // 
            this.accordionControl1.AllowItemSelection = true;
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.Menu,
            this.KPI});
            this.accordionControl1.Location = new System.Drawing.Point(0, 0);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;
            this.accordionControl1.Size = new System.Drawing.Size(167, 669);
            this.accordionControl1.TabIndex = 0;
            this.accordionControl1.Text = "accordionControl1";
            this.accordionControl1.SelectedElementChanged += new DevExpress.XtraBars.Navigation.SelectedElementChangedEventHandler(this.accordionControl1_SelectedElementChanged);
            // 
            // Menu
            // 
            this.Menu.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.LogMenu,
            this.LotMenu,
            this.UserMenu,
            this.DeletePathMenU,
            this.Zip});
            this.Menu.Expanded = true;
            this.Menu.Name = "Menu";
            this.Menu.Text = "Grid Menu";
            // 
            // LogMenu
            // 
            this.LogMenu.Name = "LogMenu";
            this.LogMenu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.LogMenu.Text = "LOG";
            // 
            // LotMenu
            // 
            this.LotMenu.Name = "LotMenu";
            this.LotMenu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.LotMenu.Text = "LOT";
            // 
            // UserMenu
            // 
            this.UserMenu.Name = "UserMenu";
            this.UserMenu.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.UserMenu.Text = "User";
            // 
            // DeletePathMenU
            // 
            this.DeletePathMenU.Name = "DeletePathMenU";
            this.DeletePathMenU.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.DeletePathMenU.Text = "DeletePath";
            // 
            // Zip
            // 
            this.Zip.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement5});
            this.Zip.Expanded = true;
            this.Zip.Name = "Zip";
            this.Zip.Text = "Zip";
            this.Zip.Click += new System.EventHandler(this.Zip_Click);
            // 
            // accordionControlElement5
            // 
            this.accordionControlElement5.Name = "accordionControlElement5";
            this.accordionControlElement5.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement5.Text = "Coments";
            // 
            // KPI
            // 
            this.KPI.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement2,
            this.accordionControlElement3});
            this.KPI.Expanded = true;
            this.KPI.Name = "KPI";
            this.KPI.Text = "Chart";
            this.KPI.Click += new System.EventHandler(this.KPI_Click);
            // 
            // accordionControlElement2
            // 
            this.accordionControlElement2.Name = "accordionControlElement2";
            this.accordionControlElement2.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement2.Text = "C_Lot";
            this.accordionControlElement2.Click += new System.EventHandler(this.accordionControlElement2_Click);
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.Name = "accordionControlElement3";
            this.accordionControlElement3.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement3.Text = "UserLog";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.LotGrid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(976, 669);
            this.splitContainer2.SplitterDistance = 686;
            this.splitContainer2.TabIndex = 0;
            // 
            // LotGrid
            // 
            this.LotGrid.AllowRestoreSelectionAndFocusedRow = DevExpress.Utils.DefaultBoolean.True;
            this.LotGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LotGrid.Location = new System.Drawing.Point(0, 0);
            this.LotGrid.MainView = this.gridView3;
            this.LotGrid.MenuManager = this.ribbon;
            this.LotGrid.Name = "LotGrid";
            this.LotGrid.Size = new System.Drawing.Size(686, 669);
            this.LotGrid.TabIndex = 0;
            this.LotGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            this.LotGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LotGrid_KeyDown);
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.LotGrid;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsSelection.MultiSelect = true;
            this.gridView3.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView3_RowClick);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.simpleButton1);
            this.splitContainer3.Size = new System.Drawing.Size(286, 669);
            this.splitContainer3.SplitterDistance = 639;
            this.splitContainer3.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(286, 639);
            this.propertyGrid1.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleButton1.Location = new System.Drawing.Point(0, 0);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(286, 26);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "저장";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // accordionControlElement4
            // 
            this.accordionControlElement4.Name = "accordionControlElement4";
            this.accordionControlElement4.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement4.Text = "Coments";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(1060, 61);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(87, 23);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "조회";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(832, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "~";
            // 
            // E_DateTime
            // 
            this.E_DateTime.Location = new System.Drawing.Point(854, 62);
            this.E_DateTime.Name = "E_DateTime";
            this.E_DateTime.Size = new System.Drawing.Size(200, 22);
            this.E_DateTime.TabIndex = 6;
            this.E_DateTime.Value = new System.DateTime(2022, 12, 16, 14, 4, 37, 0);
            // 
            // S_DateTime
            // 
            this.S_DateTime.Location = new System.Drawing.Point(626, 62);
            this.S_DateTime.Name = "S_DateTime";
            this.S_DateTime.Size = new System.Drawing.Size(200, 22);
            this.S_DateTime.TabIndex = 5;
            this.S_DateTime.Value = new System.DateTime(2022, 12, 15, 0, 0, 0, 0);
            // 
            // DB_ViewForm
            // 
            this.AllowFormGlass = DevExpress.Utils.DefaultBoolean.True;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 783);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.S_DateTime);
            this.Controls.Add(this.E_DateTime);
            this.Controls.Add(this.ribbon);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("DB_ViewForm.IconOptions.LargeImage")));
            this.Name = "DB_ViewForm";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "DB_ViewForm";
            this.TopMost = true;
            this.MouseHover += new System.EventHandler(this.DB_ViewForm_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LotGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement Menu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement LogMenu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement LotMenu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement UserMenu;
        private DevExpress.XtraBars.Navigation.AccordionControlElement Zip;
        private DevExpress.XtraBars.Navigation.AccordionControlElement DeletePathMenU;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevExpress.XtraGrid.GridControl LotGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private DevExpress.XtraBars.BarButtonItem Add;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem RegBT;
        private DevExpress.XtraBars.Navigation.AccordionControlElement KPI;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement2;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
        private DevExpress.XtraBars.BarButtonItem DataSerach_BT;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem ZipLoad_BT;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement5;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement4;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker E_DateTime;
        private System.Windows.Forms.DateTimePicker S_DateTime;
    }
}