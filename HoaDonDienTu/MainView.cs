using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HoaDonDienTu
{
    public partial class MainView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private UserPass userPass;
        private EFF_EFFE_200Entities _entities;
        public MainView(UserPass userPass)
        {
            InitializeComponent();
            if (!mvvmContext1.IsDesignMode)
                InitializeBindings();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.userPass = userPass;
            icon_login.Caption = "Đăng nhập bởi: " + userPass.username.ToString()+" - "+ userPass.ma_user.ToString(); //Lấy thông tin đăng nhập
            icon_login.Tag = userPass.ma_user.ToString();


        }

        void InitializeBindings()
        {
            var fluent = mvvmContext1.OfType<MainViewModel>();
        }


        //Kiểm tra xem form đã tồn tại chưa
        private bool Exitform(Form form)
        {
            foreach(var child in MdiChildren)
            {
                if(child.Name == form.Name)
                {
                    child.Activate();
                    return true;
                }    
            }
            return false;
        }

        private void btn_nhaplieu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm_Nhaptthd = new frm_nhaptthd();
            frm_Nhaptthd.Text = "Nhập thông tin hóa đơn";
            if (Exitform(frm_Nhaptthd)) return;
            frm_Nhaptthd.MdiParent = this;
            frm_Nhaptthd.Show();
        }

        private void btn_khachhang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_KH();
            frm.Text = "Nhập thông khách hàng";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btn_thietbi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_TB();
            frm.Text = "Nhập thông VLSPHH";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btn_api_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_API();
            frm.Text = "Nhập thông API";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btn_phathanhhd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_HDDT();
            frm.Text = "Quản lý hóa đơn điện tử";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            icon_login.Caption = "Đăng nhập bởi: " + userPass.username.ToString();
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = XtraMessageBox.Show("Bạn có chắc chắn muốn thoát phần mềm không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    // Hủy việc đóng form nếu người dùng chọn "No"
                    e.Cancel = true;
                }
            }
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            _entities = new EFF_EFFE_200Entities();
            LoadRights();
            Save();
        }

        private void Save()
        {
            foreach (RibbonPage page in ribbonControl1.Pages)
            {

                var pageF = new tblFunctions();
                pageF.Menu = page.Name;
                pageF.Application = "PQND";
                pageF.Description = page.Text;
                pageF.ParentMenu = null;
                _entities.tblFunctions.AddOrUpdate(pageF);

                foreach (RibbonPageGroup pageGroup in page.Groups)
                {
                    var pageGroupF = new tblFunctions();
                    pageGroupF.Menu = pageGroup.Name;
                    pageGroupF.Application = "PQND";
                    pageGroupF.Description = pageGroup.Text;
                    pageGroupF.ParentMenu = page.Name;
                    _entities.tblFunctions.AddOrUpdate(pageGroupF);
                    foreach (BarItemLink barItemLink in pageGroup.ItemLinks)
                    {
                        var barItemLinkF = new tblFunctions();
                        barItemLinkF.Menu = barItemLink.Item.Name;
                        barItemLinkF.Application = "PQND";
                        barItemLinkF.Description = barItemLink.Caption;
                        barItemLinkF.ParentMenu = pageGroup.Name;
                        _entities.tblFunctions.AddOrUpdate(barItemLinkF);
                    }
                }
                _entities.SaveChanges();
            }
        }

        private void LoadRights()
        {
            int ma_user = Convert.ToInt32(userPass.ma_user);
            var dt = (from a in _entities.tblUserFuntions
                      join b in _entities.tblFunctions on a.Menu equals b.Menu
                      where b.Application == "PQND" && a.ma_user == ma_user
                      select a).ToList();

            foreach (RibbonPage page in ribbonControl1.Pages)
            {
                foreach (var items in dt)
                {
                    if (items.Menu == page.Name && items.Disable == true)
                    {
                        page.Visible = false;
                    }
                }

                foreach (RibbonPageGroup pageGroup in page.Groups)
                {
                    foreach (var items in dt)
                    {
                        if (items.Menu == pageGroup.Name && items.Disable == true)
                        {
                            pageGroup.Enabled = false;
                        }
                    }

                    foreach (BarButtonItemLink barItemLink in pageGroup.ItemLinks)
                    {
                        foreach (var items in dt)
                        {
                            if (items.Menu == barItemLink.Item.Name && items.Disable == true)
                            {
                                barItemLink.Item.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_USERS();
            frm.Text = "Quản lý người dùng";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_backupDB();
            frm.Text = "Backup dữ liệu";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new frm_UserQuyen();
            frm.Text = "Backup dữ liệu";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btn_connectDB_ItemClick(object sender, ItemClickEventArgs e)
        {
            var frm = new frmKetNoiDB();
            frm.Text = "Kết nối Database";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btn_baocao_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Tạo một ContextMenuStrip mới
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            // Thêm các mục vào menu
            ToolStripMenuItem item1 = new ToolStripMenuItem("1.Bảng kê hóa đơn hàng hóa dịch vụ mua vào");
            item1.Click += Item1_Click; // Thêm sự kiện click cho item
            item1.Image = Properties.Resources.bouser_16x16;
            contextMenu.Items.Add(item1);

            ToolStripMenuItem item2 = new ToolStripMenuItem("2.Item 2");
            item2.Click += Item2_Click; // Thêm sự kiện click cho item
            contextMenu.Items.Add(item2);

            // Hiển thị ContextMenuStrip tại vị trí chuột hiện tại
            // Hoặc bạn có thể chỉ định một vị trí cố định nếu muốn
            Point screenPoint = Control.MousePosition;
            contextMenu.Show(screenPoint);
        }

        private void Item1_Click(object sender, EventArgs e)
        {
            // Tạo một instance mới của form
            Form dateRangeForm = new Form
            {
                Text = "Chọn Khoảng Thời Gian",
                // Tăng kích thước chiều rộng của form để phù hợp với nội dung
                Size = new System.Drawing.Size(400, 150), // Thay đổi giá trị này để phù hợp
                StartPosition = FormStartPosition.CenterScreen
            };

            // Cài đặt cho Label "Từ ngày"
            Label lblFromDate = new Label
            {
                Text = "Từ ngày:",
                Location = new System.Drawing.Point(10, 12), // Điều chỉnh vị trí nếu cần
                Width = 80, // Tăng chiều rộng nếu cần
                TextAlign = ContentAlignment.MiddleLeft
            };
            dateRangeForm.Controls.Add(lblFromDate);

            // Cài đặt cho DateTimePicker "Từ ngày"
            DateTimePicker dtpFromDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new System.Drawing.Point(100, 10), // Điều chỉnh vị trí nếu cần
                Width = 200 // Tăng chiều rộng nếu cần
            };
            dateRangeForm.Controls.Add(dtpFromDate);

            // Cài đặt cho Label "Đến ngày"
            Label lblToDate = new Label
            {
                Text = "Đến ngày:",
                Location = new System.Drawing.Point(10, 42), // Điều chỉnh vị trí nếu cần
                Width = 80, // Tăng chiều rộng nếu cần
                TextAlign = ContentAlignment.MiddleLeft
            };
            dateRangeForm.Controls.Add(lblToDate);

            // Cài đặt cho DateTimePicker "Đến ngày"
            DateTimePicker dtpToDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new System.Drawing.Point(100, 40), // Điều chỉnh vị trí nếu cần
                Width = 200 // Tăng chiều rộng nếu cần
            };
            dateRangeForm.Controls.Add(dtpToDate);

            // Tính ngày đầu tiên của tháng hiện tại
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Tính ngày hiện tại
            DateTime currentDate = DateTime.Now;

            // Áp dụng giá trị cho DateTimePicker "Từ ngày"
            dtpFromDate.Value = firstDayOfMonth;

            // Áp dụng giá trị cho DateTimePicker "Đến ngày"
            dtpToDate.Value = currentDate;


            // Tạo nút "Tìm kiếm"
            Button btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new System.Drawing.Point(100, 70),
                Width = 200
            };
            btnSearch.Click += (sender1, e1) =>
            {
                DateTime fromDate = dtpFromDate.Value;
                DateTime toDate = dtpToDate.Value;

                DateTime fDate_sql = fromDate.AddDays(-1);
                DateTime tDate_sql = toDate.AddDays(1);

                string sql = @"SELECT  _tk.mau_hd as mau_hd,_tk.serihd as serihd,_tk.sohd as sohd,CONVERT(VARCHAR, _tk.phi_tm, 103) as phi_tm,MAX(case when _tk.khthue<>'' then _tk.khthue else _khang.ten end) AS tr5,MAX(case when _tk.msthue<>'' then _tk.msthue else _khang.ma_gtgt end) AS tr6,MAX(_tk.nhom_vat)  as nhom_vat,Sum( case when left(_tk.ma_tk0,4)<>'3331' then (_tk.ps_co1-_tk.ps_no1) end) as dso,Sum( case when left(_tk.ma_tk0,4)='3331' and left(ma_tk1,4)<>'1331' then (_tk.ps_co1) end) as tienthue,MAX(_tk.ktdb)  as ktdb,MAX(_tk.ghi_chu)  as ghi_chu,SUM(_tk.ps_no1*0)  as sx,SUM(_tk.ps_no1*0)  as stt,MAX('')  as dgiai,MAX(_khang.ten) AS tr15,Max(case when _tk.loaidl=0 then 0 else 3 end) as cap,Max(case when _tk.loaidl=0 then 0 else 3 end) as in_dam

                                FROM _tk
                                INNER JOIN _khang ON _tk.MA_KH = _Khang.Ma_kh AND _tk.DVCS = _Khang.DVCS
                                INNER JOIN _tkhoan ON _tk.MA_TK0 = _TKHOAN.MA
                                WHERE _tk.dvcs = 1 and _tk.loaidl IN(0,11,12) 
                                and(_tk.ktdb<>'' and _tk.ma_tk1 not like '911%')
                                and((Select _tkhoan.ma) like N'511%' or(Select _tkhoan.ma) like N'711%' or(Select _tkhoan.ma) like N'33311%' or(Select _tkhoan.ma) like N'5211%') and(_tk.ma_tk0 < 'N') 
AND _tk.NGAY_THC < N'"+ tDate_sql.ToString("MM/dd/yyyy") +@"' AND _tk.NGAY_THC > N'"+ fDate_sql.ToString("MM/dd/yyyy") +@"' 
AND(_tk.ma_tk0 like '511%' OR _tk.ma_tk0 like '711%' OR _tk.ma_tk0 like '33311%' OR _tk.ma_tk0 like '5211%')
                                GROUP BY _tk.mau_hd,_tk.serihd,_tk.sohd,phi_tm
                                ORDER BY _tk.phi_tm ASC";



                var frm = new frm_baocao();
                frm.Text = "Bảng kê hóa đơn hàng hóa dịch vụ bán ra   " +"["+ fromDate.ToString("dd/MM/yyyy") +"] ["+ toDate.ToString("dd/MM/yyyy") +"]";
                frm.DisplayData(sql);
                frm.fromDate = fromDate;
                frm.toDate = toDate;
                frm.SqlQuery = sql;
                if (Exitform(frm)) return;
                frm.MdiParent = this;
                frm.Show();
                dateRangeForm.Close();


            };
            dateRangeForm.Controls.Add(btnSearch);

            // Hiển thị DateRangeForm
            dateRangeForm.Show();
        }


        private void Item2_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện click cho Item 2
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Đường dẫn tới UltraViewer.exe. Thay đổi nếu cần thiết.
                string ultraViewerPath = @"C:\Program Files (x86)\UltraViewer\UltraViewer_Desktop.exe";

                // Kiểm tra xem file UltraViewer.exe có tồn tại không
                if (System.IO.File.Exists(ultraViewerPath))
                {
                    Process.Start(ultraViewerPath);
                }
                else
                {
                    MessageBox.Show("UltraViewer không được tìm thấy tại vị trí xác định.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi mở UltraViewer: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Thôn_ItemClick(object sender, ItemClickEventArgs e)
        {
            var frm = new frm_ttpm();
            frm.Text = "Thông tin phần mềm";
            if (Exitform(frm)) return;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
