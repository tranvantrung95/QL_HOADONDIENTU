using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HoaDonDienTu
{
    public partial class MainView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MainView()
        {
            InitializeComponent();
            if (!mvvmContext1.IsDesignMode)
                InitializeBindings();
        }

        void InitializeBindings()
        {
            var fluent = mvvmContext1.OfType<MainViewModel>();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_nhaptthd frm_Nhaptthd = new frm_nhaptthd();

            // Tạo XtraTabPage mới
            DevExpress.XtraTab.XtraTabPage tabPage = new DevExpress.XtraTab.XtraTabPage();
            tabPage.Text = "Tên Tab"; // Đặt tên cho tab

            // Tạo và thêm MyXtraForm vào XtraTabPage

            tabPage.Controls.Add(frm_nhaptthd);
            XtraTabControl tabControl = new XtraTabControl();

            // Thêm XtraTabPage vào XtraTabControl
            tabControl.TabPages.Add(tabPage);

            // Hiển thị tab mới
            tabControl.SelectedTabPage = tabPage;
            myForm.Show();
        }
    }
}
