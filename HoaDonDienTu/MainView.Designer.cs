namespace HoaDonDienTu
{
    partial class MainView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.icon_login = new DevExpress.XtraBars.BarButtonItem();
            this.btn_nhaplieu = new DevExpress.XtraBars.BarButtonItem();
            this.btn_phathanhhd = new DevExpress.XtraBars.BarButtonItem();
            this.btn_khachhang = new DevExpress.XtraBars.BarButtonItem();
            this.btn_thietbi = new DevExpress.XtraBars.BarButtonItem();
            this.btn_api = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.Thôn = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.btn_connectDB = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btn_baocao = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.mvvmContext1 = new DevExpress.Utils.MVVM.MVVMContext(this.components);
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.CaptionBarItemLinks.Add(this.icon_login);
            this.ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 37, 35, 37);
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.icon_login,
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btn_nhaplieu,
            this.btn_phathanhhd,
            this.btn_khachhang,
            this.btn_thietbi,
            this.btn_api,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.Thôn,
            this.barButtonItem4,
            this.btn_connectDB,
            this.barEditItem1,
            this.btn_baocao});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonControl1.MaxItemId = 16;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsMenuMinWidth = 385;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.ribbonControl1.Size = new System.Drawing.Size(1384, 193);
            // 
            // icon_login
            // 
            this.icon_login.Caption = "barButtonItem1";
            this.icon_login.Id = 7;
            this.icon_login.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.bouser_16x16;
            this.icon_login.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.bouser_32x32;
            this.icon_login.Name = "icon_login";
            this.icon_login.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // btn_nhaplieu
            // 
            this.btn_nhaplieu.Caption = "Nhập liệu hóa đơn";
            this.btn_nhaplieu.Id = 1;
            this.btn_nhaplieu.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_nhaplieu.ImageOptions.Image")));
            this.btn_nhaplieu.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btn_nhaplieu.ImageOptions.LargeImage")));
            this.btn_nhaplieu.Name = "btn_nhaplieu";
            this.btn_nhaplieu.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_nhaplieu_ItemClick);
            // 
            // btn_phathanhhd
            // 
            this.btn_phathanhhd.Caption = "Phát hành hóa đơn";
            this.btn_phathanhhd.Id = 3;
            this.btn_phathanhhd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_phathanhhd.ImageOptions.Image")));
            this.btn_phathanhhd.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btn_phathanhhd.ImageOptions.LargeImage")));
            this.btn_phathanhhd.Name = "btn_phathanhhd";
            this.btn_phathanhhd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_phathanhhd_ItemClick);
            // 
            // btn_khachhang
            // 
            this.btn_khachhang.Caption = "Khách hàng";
            this.btn_khachhang.Id = 4;
            this.btn_khachhang.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_khachhang.ImageOptions.Image")));
            this.btn_khachhang.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btn_khachhang.ImageOptions.LargeImage")));
            this.btn_khachhang.Name = "btn_khachhang";
            this.btn_khachhang.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_khachhang_ItemClick);
            // 
            // btn_thietbi
            // 
            this.btn_thietbi.Caption = "VLSPHH";
            this.btn_thietbi.Id = 5;
            this.btn_thietbi.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_thietbi.ImageOptions.Image")));
            this.btn_thietbi.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btn_thietbi.ImageOptions.LargeImage")));
            this.btn_thietbi.Name = "btn_thietbi";
            this.btn_thietbi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_thietbi_ItemClick);
            // 
            // btn_api
            // 
            this.btn_api.Caption = "API";
            this.btn_api.Id = 6;
            this.btn_api.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_api.ImageOptions.Image")));
            this.btn_api.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btn_api.ImageOptions.LargeImage")));
            this.btn_api.Name = "btn_api";
            this.btn_api.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_api_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Users";
            this.barButtonItem1.Id = 8;
            this.barButtonItem1.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.bouser_16x162;
            this.barButtonItem1.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.bouser_32x322;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick_1);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Backup DL";
            this.barButtonItem2.Id = 9;
            this.barButtonItem2.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.database_16x16;
            this.barButtonItem2.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.database_32x32;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Hỗ trợ từ xa";
            this.barButtonItem3.Id = 10;
            this.barButtonItem3.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.bocontact_16x16;
            this.barButtonItem3.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.bocontact_32x32;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // Thôn
            // 
            this.Thôn.Caption = "Thông tin phần mềm";
            this.Thôn.Id = 11;
            this.Thôn.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.information_16x16;
            this.Thôn.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.information_32x32;
            this.Thôn.Name = "Thôn";
            this.Thôn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Thôn_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Phân quyền";
            this.barButtonItem4.Id = 12;
            this.barButtonItem4.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.usergroup_16x16;
            this.barButtonItem4.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.usergroup_32x32;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // btn_connectDB
            // 
            this.btn_connectDB.Caption = "Kết nối Database";
            this.btn_connectDB.Id = 13;
            this.btn_connectDB.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.managedatasource_16x16;
            this.btn_connectDB.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.managedatasource_32x32;
            this.btn_connectDB.Name = "btn_connectDB";
            this.btn_connectDB.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_connectDB_ItemClick);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "Báo cáo";
            this.barEditItem1.Edit = this.repositoryItemComboBox1;
            this.barEditItem1.Id = 14;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // btn_baocao
            // 
            this.btn_baocao.Caption = "Báo cáo";
            this.btn_baocao.Id = 15;
            this.btn_baocao.ImageOptions.Image = global::HoaDonDienTu.Properties.Resources.runchartdesigner_16x16;
            this.btn_baocao.ImageOptions.LargeImage = global::HoaDonDienTu.Properties.Resources.runchartdesigner_32x32;
            this.btn_baocao.Name = "btn_baocao";
            this.btn_baocao.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_baocao_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup1,
            this.ribbonPageGroup5,
            this.ribbonPageGroup4});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btn_nhaplieu);
            this.ribbonPageGroup2.ItemLinks.Add(this.btn_phathanhhd);
            this.ribbonPageGroup2.ItemLinks.Add(this.btn_baocao);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Chức năng";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup3.ImageOptions.Image")));
            this.ribbonPageGroup3.ItemLinks.Add(this.btn_khachhang);
            this.ribbonPageGroup3.ItemLinks.Add(this.btn_thietbi);
            this.ribbonPageGroup3.ItemLinks.Add(this.btn_api);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Danh mục";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.btn_connectDB);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Backup DL";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "Quản lý người dùng";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup4.ItemLinks.Add(this.Thôn);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Trợ giúp";
            // 
            // mvvmContext1
            // 
            this.mvvmContext1.ContainerControl = this;
            this.mvvmContext1.ViewModelType = typeof(HoaDonDienTu.MainViewModel);
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 549);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainView";
            this.Ribbon = this.ribbonControl1;
            this.Text = "MainView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainView_FormClosing);
            this.Load += new System.EventHandler(this.MainView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.Utils.MVVM.MVVMContext mvvmContext1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem btn_nhaplieu;
        private DevExpress.XtraBars.BarButtonItem btn_phathanhhd;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btn_khachhang;
        private DevExpress.XtraBars.BarButtonItem btn_thietbi;
        private DevExpress.XtraBars.BarButtonItem btn_api;
        private DevExpress.XtraBars.BarButtonItem icon_login;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem Thôn;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.BarButtonItem btn_connectDB;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem btn_baocao;
    }
}

