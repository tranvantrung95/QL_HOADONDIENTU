using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Svg;
using DevExpress.XtraWaitForm;
using DevExpress.XtraTabbedMdi;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using HoaDonDienTu.Class;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DevExpress.XtraGrid.Views.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection.Emit;
using ContentAlignment = System.Drawing.ContentAlignment;
using DevExpress.XtraPrinting.Export.Pdf;
using System.IO;
using DevExpress.Emf;
using System.Diagnostics;
using DevExpress.Data.Filtering;
using DevExpress.XtraBars;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Menu;
using static DevExpress.Utils.Svg.CommonSvgImages;

namespace HoaDonDienTu
{
    public partial class frm_HDDT : DevExpress.XtraEditors.XtraForm
    {
        string ma_gtgt = "0100109106-503", sericert, bangcapnhat="_thu_tien";
        double tgianhd;
        DateTime dt_hd1 = new DateTime(1970, 1, 1);
        DateTime dt_hd = new DateTime(1970, 1, 1);

        private string selectedFilterOperator;
        private string selectedFieldName;

        // Định nghĩa enum cho trạng thái để mã dễ đọc hơn
        enum TrangThai
        {
            None = 0,
            TaoNhaps = 1,
            XoaNhaps = 2,
            PhatHanhs = 3,
            HuyHoaDons = 4,
            XemHoaDons = 5
        }

        private TrangThai trangthai = TrangThai.None;
        private struct StatusInfo
        {
            public string Text;
            public Brush CircleBrush;
        }

        private readonly Brush newStatusBrush = Brushes.Red;       // Màu cho trạng thái "-Mới"
        private readonly Brush draftStatusBrush = Brushes.Green;   // Màu cho trạng thái "-Đã tạo nháp"

        public frm_HDDT()
        {
            InitializeComponent();

            //Cấu hình lại định dạng số thập phân
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture = customCulture;

        }


        private void frm_HDDT1_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            stepProgressBar1.SelectedItemIndex = 0; //Set khi hiển thị luôn ở item 0
            if (stepProgressBar1.SelectedItemIndex == 0)
            {
                loadview();
            }

            DateTime now = DateTime.Now;

            // Ngày đầu tiên của tháng
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            // Ngày đầu tiên của tháng tiếp theo
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            // Ngày cuối cùng của tháng là ngày trước ngày đầu tiên của tháng tiếp theo
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            txt_search_tu_ngay.EditValue = firstDayOfMonth;
            txt_search_den_ngay.EditValue = lastDayOfMonth;


        }

        private void loadview(string tungay = null, string denngay = null)
        {
            string sqlcmd = "select *,C.ma as ma , C.TEN as ten,case when ps_no1<0 or ma_tk0 like '521%' then 3 else 1 end as selection,Case when ps_no1>=0 then 'true' else 'false' end as isIncrease,case when _thu_tien.so_luong<>0 then ps_no1/_thu_tien.so_luong else don_gia end as don_giavnd,case when _thu_tien.so_luong<>0 then tien_usd/_thu_tien.so_luong else don_gia end as don_giausd,case when ktdb='' or ktdb='X' then '-1' else ktdb end as ktdb";
            string sqlcmd1 = @"
		select ty_gia,dvcs,loaidl,check_hddt,dv_do,tt3,ngay_thc,ma_scr,chung_tu,left(mau_hd,1) as mau6,mau_hd,sohd,
		serihd,case when datepart(day,getdate())=datepart(day,phi_tm) then (case when datepart(hh,getdate())>7 then dateadd(hh,-7,dateadd(mi,datepart(mi,getdate()),Dateadd(hh,datepart(hh,getdate()),phi_tm))) else phi_tm end) else dateadd(hh,15,phi_tm) end as phi_tm,ongba,ma_kh,dcthue,khthue,ghi_chu
		,msthue,ktdb,keyhd,f_identity,tthaihd,muserk1,date1,makh,tenkh,
		muserk2,date2,ps_no1,vat,pt_ck,tien_ck,ps_no1-tien_ck as tien_sau_ck,ps_no1+vat-tien_ck as tien_sau_thue,tien_usd+vat-tien_ck as tien_sau_thue_u,trangthai,ltrim(rtrim(serihd))+ltrim(rtrim(sohd)) as serihd_sohd,
		substring(ctu_ref,1,case when charindex('#',ctu_ref)>0 then charindex('#',ctu_ref)-1 else 0 end) as ctu_ref, 
	substring(ctu_ref,case when charindex('#',ctu_ref)>0 then charindex('#',ctu_ref)+1 else 0 end,14) as ctu_ref_date
	,Case when ps_no1>=0 then 'true' else 'false' end as isIncrease
		from (
		select _thu_tien.dvcs,max(ty_gia) as ty_gia,loaidl,max(C.ma) as makh, Max(C.TEN) as tenkh,0 as check_hddt,max(case when ma_tk1 like '511%' or ma_tk1 like '3387%' or ma_tk1 like '711%' then ghi_chu else '' end) as ghi_chu,max(dv_do) as dv_do,tt3,ngay_thc,ma_scr,chung_tu,mau_hd,sohd,serihd,phi_tm,ongba,_thu_tien.ma_kh,dcthue,khthue,msthue,max(case when ktdb='' or ktdb='X' then '-1' else ktdb end) as ktdb,max(keyhd) as keyhd,max(_thu_tien.f_identity) as f_identity,max(tthaihd) as tthaihd,max(muserk1) as muserk1,max(date1) as date1,max(muserk2) as muserk2,max(date2) as date2,
		sum(case when ma_tk1 like '511%' or ma_tk1 like '3387%' or ma_tk1 like '711%' then ps_no1 else 0 end) as ps_no1,
		sum(case when ma_tk1 like '511%' or ma_tk1 like '3387%' or ma_tk1 like '711%' then tien_usd else 0 end) as tien_usd,
		sum(case when ma_tk1 like '3331%' then ps_no1 else 0 end) as vat,
		sum(case when ma_tk1 like '3331%' then tien_usd else 0 end) as vat_u,
		max(pt_ck) as pt_ck,
		sum(case when ma_tk0 like '5118%' then ps_no1 else 0 end) as tien_ck,
		sum(case when ma_tk0 like '5118%' then tien_usd else 0 end) as tien_ck_u,
case tthaihd when 'ck1phhd' then N'Chờ phát hành' when N'ck1tthd' then N'Chờ thay thế' when 'ck1dchd' then N'Chờ điều chỉnh' 
when 'ck1hhd' then N'Chờ hủy' when 'ck2phhd' then N'Đã phát hành' when 'ck2tthd' then N'Phát hành thay thế' when 'ck2dchd' then N'Phát hành điều chỉnh' else N'Chưa có' end as trangthai,max(ctu_ref) as ctu_ref";
            string groupby = " group by _thu_tien.dvcs,loaidl,tthaihd,mau_hd,sohd,serihd,ongba,_thu_tien.ma_kh,dcthue,phi_tm,khthue,msthue,ngay_thc,ma_scr,chung_tu,tt3";
            string from = " from _thu_tien ";
            string where = "";
            string tthaihd = "";

            if (stepProgressBar1.SelectedItemIndex == 0)
            {
                tthaihd = "(_thu_tien.tthaihd= '' or _thu_tien.tthaihd like 'ck1%')";

            }
            else
               if (stepProgressBar1.SelectedItemIndex == 1)
            {
                tthaihd = "(_thu_tien.tthaihd like 'ck2phhd%')";
            }
            else
                if (stepProgressBar1.SelectedItemIndex == 2)
            {
                tthaihd = "(_thu_tien.tthaihd like 'ck2hhd%' and loaidl=12)";
            }

            if ((tungay != null && denngay != null))
            {
                where = " where _thu_tien.mau_hd<>'' and _thu_tien.serihd <>'' and "+ tthaihd +" and ngay_thc>='" + tungay + "' and ngay_thc<='" + denngay + "' ";
            }
            else
            {
                where = " where _thu_tien.mau_hd<>'' and _thu_tien.serihd <>'' and "+ tthaihd +"";
            }

            string innerjoin = "cross apply\r\n(select top(1) * from _khang B where _thu_tien.MA_KH = B.MA_KH ) C";
            string innerjoin_ct = "cross apply\r\n(select top(1) * from _THIETBI B where _thu_tien.MA_THB = B.MA_THB ) C";

            string sqlcmdgroup = sqlcmd1 + from + innerjoin + where + " and (ma_tk1 like '511%' or ma_tk1 like '3387%' or ma_tk1 like '711%' or ma_tk1 like '3331%' or ma_tk0 like '521%') " + groupby + ") A order by ngay_thc,chung_tu,sohd desc,phi_tm desc";
            string sqlcmd_ct = sqlcmd + from + innerjoin_ct + where + " and (ma_tk1 like '511%' or ma_tk1 like '3387%' or ma_tk1 like '711%' or ma_tk0 like '521%') order by ngay_thc,chung_tu,phi_tm,sohd,_thu_tien.f_identity";

            //Class.Functions.editcode(sqlcmdgroup+"\n"+ sqlcmd_ct);

            DataTable dt;
            dt = Class.Functions.GetDataToTable(sqlcmdgroup);

            DataTable dt_ct;
            dt_ct = Class.Functions.GetDataToTable(sqlcmd_ct);
            gridControl1.DataSource = dt;
            gridControl2.DataSource = dt_ct;

            gridView1.OptionsView.ColumnAutoWidth = false; //Tắt chế độ tự động giãn cột
            gridView1.HorzScrollVisibility = ScrollVisibility.Auto; //Tạo thanh cuộn ngang
            gridView1.BestFitColumns(); //Cho phép tự giãn cột

            // Chọn dòng đầu tiên của gridView1 để lọc giá trị cho dòng
            if (gridView1.RowCount > 0)
            {
                gridView1.FocusedRowHandle = 0;
                gridView1.SelectRow(0);

                DataView dtv = gridView1.DataSource as DataView;

                int donght = 0;
                if (gridView2.DataSource != null)
                {
                    DataView dtv2 = gridView2.DataSource as DataView;
                    string filter = "chung_tu='" + dtv[donght]["chung_tu"].ToString().Trim() + "' and ma_scr=" + dtv[donght]["ma_scr"].ToString().Trim()
        + " and ngay_thc='" + dtv[donght]["ngay_thc"].ToString().Trim() + "' and ma_kh= " + dtv[donght]["ma_kh"].ToString();
                    dtv2.RowFilter = filter;
                }
            }




            string[] columnsToShow_grp = new string[] { "check_hddt", "chung_tu", "mau_hd", "serihd", "sohd", "phi_tm", "ongba", "makh", "tenkh", "ma_kh", "msthue", "khthue", "dcthue", "vat", "ktdb", "tt3", "ps_no1", "pt_ck", "tien_ck", "trangthai", "keyhd", "muserk1", "date1", "muserk2", "date2", "tthaihd", "dv_do" };
            string[] columnsToShow_ct = new string[] { "gch_detail", "ma", "ten", "so_luong", "tt3", "don_gia", "ps_no1", "selection", "ma_thb" };

            // Mảng mới chứa thông tin về tên hiển thị và thứ tự hiển thị
            string[] displayNames_grp = new string[] { "Chọn", "Chứng từ", "Mẫu HĐ", "Seri HĐ", "Số HĐ", "Ngày HĐ", "Ông bà", "Mã KH", "Tên KH", "ID KH", "MS Thuế", "KH Thuế", "DC Thuế", "VAT", "KTDB", "TT3", "VNĐ", "PT CK", "Tiền CK", "Trạng thái", "Key HĐ", "Người đẩy", "Ngày đẩy", "Người ký", "Ngày ký", "TThaiHD", "dv_do" };
            string[] displayNames_ct = new string[] { "Diễn giải chi tiết", "Mã Vlsphh", "Tên Vlsphh", "Số lượng", "ĐVT", "Đơn giá", "VNĐ", "selection", "ma_thb" };

            // Usage for gridView1
            CustomizeGridView(gridView1, columnsToShow_grp, displayNames_grp);

            // Usage for gridView2
            CustomizeGridView(gridView2, columnsToShow_ct, displayNames_ct);

            gridView1.CustomDrawCell += gridView1_CustomDrawCell; //tại cột trạng thái cho hiển thị thông tin mới..
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);//Bôi màu cho dòng có trạng thái ck1tn

            int count = 0;
            int count_tn = 0;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                object trangthai = gridView1.GetRowCellValue(i, "tthaihd");
                if (trangthai == null || trangthai.ToString() != "ck1tn")
                {
                    // Tăng biến đếm
                    count++;
                }
                else
                    if (trangthai.ToString() == "ck1tn")
                {
                    count_tn++;
                }
            }

            txt_view_cky.Text = count.ToString();
            txt_view_tnhap.Text = count_tn.ToString();

            // Gán RepositoryItemButtonEdit cho cột chức năng
            GridColumn functionColumn = gridView1.Columns.Add();
            functionColumn.Caption = "Chức năng";
            functionColumn.FieldName = "function";
            functionColumn.Visible = true;

            if (stepProgressBar1.SelectedItemIndex == 0)
            {
                //// Tạo RepositoryItemButtonEdit để thêm nút chức năng cho dòng
                RepositoryItemButtonEdit repositoryItemButton = new RepositoryItemButtonEdit();
                repositoryItemButton.TextEditStyle = TextEditStyles.HideTextEditor;

                repositoryItemButton.Buttons[0].Caption = "Đẩy hoá đơn";
                repositoryItemButton.Buttons[0].ToolTip = "Đẩy hoá đơn";
                repositoryItemButton.Buttons[0].Kind = ButtonPredefines.Glyph;
                repositoryItemButton.Buttons[0].Appearance.BackColor = Color.Red;

                // Sử dụng hàm ResizeImage để thay đổi kích thước hình ảnh và gán nó vào nút
                Image originalImage = Properties.Resources.previous_32x32;
                Image resizedImage = ResizeImage(originalImage, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
                repositoryItemButton.Buttons[0].ImageOptions.Image = resizedImage;

                //Tạo nút Xóa nháp
                Image originalImage1 = Properties.Resources.convert_32x32;
                Image resizedImage1 = ResizeImage(originalImage1, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
                repositoryItemButton.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)
                {
                    Caption = "Button2",
                    ToolTip = "Xóa nháp",
                    Image = resizedImage1
                });

                //Tạo nút ký phát hành
                Image phathanh = Properties.Resources.finishmerge_32x32;
                Image ky_phathanh = ResizeImage(phathanh, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
                repositoryItemButton.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)
                {
                    Caption = "Button3",
                    ToolTip = "Phát hành",
                    Image = ky_phathanh
                });


                functionColumn.ColumnEdit = repositoryItemButton;
                repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick; //Tạo sự kiện nhấn vào
            }
            else
                if (stepProgressBar1.SelectedItemIndex == 1)
            {
                //// Tạo RepositoryItemButtonEdit để thêm nút chức năng cho dòng
                RepositoryItemButtonEdit repositoryItemButton = new RepositoryItemButtonEdit();
                repositoryItemButton.TextEditStyle = TextEditStyles.HideTextEditor;

                repositoryItemButton.Buttons[0].Caption = "Hủy hoá đơn";
                repositoryItemButton.Buttons[0].ToolTip = "Hủy hoá đơn";
                repositoryItemButton.Buttons[0].Kind = ButtonPredefines.Glyph;
                repositoryItemButton.Buttons[0].Appearance.BackColor = Color.Red;

                // Sử dụng hàm ResizeImage để thay đổi kích thước hình ảnh và gán nó vào nút
                Image originalImage = Properties.Resources.cancel_32x32;
                Image resizedImage = ResizeImage(originalImage, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
                repositoryItemButton.Buttons[0].ImageOptions.Image = resizedImage;

                //Tạo nút Xem
                Image originalImage1 = Properties.Resources.viewmergeddata_32x32;
                Image resizedImage1 = ResizeImage(originalImage1, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
                repositoryItemButton.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)
                {
                    Caption = "Button2",
                    ToolTip = "Xem hóa đơn",
                    Image = resizedImage1
                });

                functionColumn.ColumnEdit = repositoryItemButton;
                repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick; //Tạo sự kiện nhấn vào
            }

            stepProgressBar1.SelectedItemChanged += StepProgressBar1_SelectedItemChanged; //Khi thay đổi step thì ẩn textbox và lable

            gridView1.Columns["function"].Fixed = FixedStyle.Right; //giữ cho cột chuc_nang luôn hiển thị ở cuối GridView

            gridView1.ShowingEditor += gridView1_ShowingEditor; //Sự kiện cho phép sửa cột chức năng
            gridView2.ShowingEditor += gridView1_ShowingEditor; //Sự kiện cho phép sửa cột chức năng

            // căn tiêu đề cột ở giữa cho tất cả các cột
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
            {
                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView2.Columns)
            {
                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            Class.Functions.formatNumber(gridView1, "ps_no1", "pt_ck", "tien_ck", "vat");
            Class.Functions.formatNumber(gridView2, "so_luong", "don_gia", "ps_no1");


            // Đăng ký sự kiện chuột phải hiển thị menu lọc
            gridView1.MouseDown += gridView_MouseDown;

            gridView1.CalcRowHeight += (sender, e) => {
                if (e.RowHandle >= 0) // Kiểm tra nếu là dòng dữ liệu
                {
                    e.RowHeight = 30; // Đặt độ cao mong muốn cho dòng
                }
            };


        }

        // Hàm thay đổi kích thước hình ảnh
        public static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void StepProgressBar1_SelectedItemChanged(object sender, EventArgs e)
        {
            switch (stepProgressBar1.SelectedItemIndex)
            {
                case 0:
                    // Hiển thị 2 Label và 2 TextBox
                    lb_view_cky.Visible = true;
                    lb_view_cky.Text = "Chưa phát hành";
                    lb_view_cky.TextAlign = ContentAlignment.MiddleCenter;
                    txt_view_cky.Visible = true;
                    lb_tnhap.Visible = true;
                    txt_view_tnhap.Visible = true;
                    break;
                case 1:
                    // Hiển thị 1 Label và 1 TextBox, ẩn 1 Label và 1 TextBox còn lại
                    lb_view_cky.Visible = true;
                    txt_view_cky.Visible = true;
                    lb_view_cky.Text = "Đã ký";
                    lb_view_cky.TextAlign = ContentAlignment.MiddleCenter;
                    
                    lb_tnhap.Visible = false; // ẩn
                    txt_view_tnhap.Visible = false; // ẩn
                    break;
                case 2:
                    // Hiển thị 1 Label và 1 TextBox, ẩn 1 Label và 1 TextBox còn lại
                    lb_view_cky.Visible = true;
                    txt_view_cky.Visible = true;
                    lb_view_cky.Text = "Đã hủy";
                    lb_view_cky.TextAlign = ContentAlignment.MiddleCenter;

                    lb_tnhap.Visible = false; // ẩn
                    txt_view_tnhap.Visible = false; // ẩn
                    break;
            }
        }


        //Hàm xử lý hiển thị cột lên gridview và sắp xếp cột
        private void CustomizeGridView(GridView gridView, string[] columnsToShow, string[] displayNames)
        {
            List<GridColumn> visibleColumns = new List<GridColumn>();

            for (int i = 0; i < columnsToShow.Length; i++)
            {
                string columnName = columnsToShow[i];
                string displayName = displayNames.Length > i ? displayNames[i] : columnName; // Use default name if display name is not provided

                // Find the corresponding column in the GridView
                // GridColumn column = gridView.Columns.ColumnByFieldName(columnName);

                GridColumn column = gridView.Columns
           .Cast<GridColumn>()
           .FirstOrDefault(col => string.Equals(col.FieldName, columnName, StringComparison.OrdinalIgnoreCase));


                // If the column is found, add it to the list and set the display name
                if (column != null)
                {
                    visibleColumns.Add(column);
                    column.Caption = displayName;
                }
            }

            gridView.Columns.Clear();

            foreach (GridColumn column in visibleColumns)
            {
                column.VisibleIndex = gridView.Columns.Count;
                column.Visible = true;
                gridView.Columns.Add(column);

            }


        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "trangthai")
            {
                int circleSize = 10;
                int padding = 3;
                int circleX = e.Bounds.X + padding;
                int circleY = e.Bounds.Y + (e.Bounds.Height - circleSize) / 2;
                int textX = circleX + circleSize + padding;
                int textY = e.Bounds.Y + (e.Bounds.Height - e.Appearance.Font.Height) / 2;

                // Lấy giá trị của trạng thái
                string statusValue = Convert.ToString(gridView1.GetRowCellValue(e.RowHandle, "tthaihd"));

                // Định nghĩa nội dung và màu sắc dựa trên giá trị của trạng thái
                StatusInfo statusInfo;
                if (string.IsNullOrWhiteSpace(statusValue))
                {
                    statusInfo = new StatusInfo { Text = "-Mới", CircleBrush = newStatusBrush };
                }
                else if (statusValue == "ck1tn")
                {
                    statusInfo = new StatusInfo { Text = "-Đã tạo nháp", CircleBrush = draftStatusBrush };
                }
                else if (statusValue == "ck2phhd")
                {
                    statusInfo = new StatusInfo { Text = "-Đã ký", CircleBrush = draftStatusBrush };
                }
                else if (statusValue == "ck2hhd")
                {
                    statusInfo = new StatusInfo { Text = "-Đã hủy", CircleBrush = draftStatusBrush };
                }
                else
                {
                    // Nếu bạn muốn xử lý các giá trị khác, hãy thêm logic tại đây
                    return; // Để ngăn chặn vẽ mặc định
                }

                // Vẽ dấu tròn và chữ
                e.Graphics.FillEllipse(statusInfo.CircleBrush, circleX, circleY, circleSize, circleSize);
                e.Graphics.DrawString(statusInfo.Text, e.Appearance.Font, Brushes.Black, textX, textY);

                e.Handled = true; // Ngăn chặn vẽ mặc định
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string statusValue = Convert.ToString(view.GetRowCellValue(e.RowHandle, "tthaihd"));
                if (statusValue == "ck1tn")
                {
                    e.Appearance.BackColor = Color.LightBlue;  // Chọn màu tùy ý
                    //e.Appearance.BackColor2 = Color.SeaShell;  // Chọn màu tùy ý
                                                               // Bạn có thể thiết lập thêm các thuộc tính khác cho e.Appearance nếu muốn
                }
            }
        }



        private void repositoryItemButton_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                // Lấy GridControl chứa GridView
                GridControl gridControl = buttonEdit.Parent as GridControl;

                if (gridControl != null)
                {
                    // Lấy GridView từ GridControl
                    GridView gridView = gridControl.MainView as GridView;

                    if (gridView != null)
                    {
                        int rowHandle = gridView.FocusedRowHandle;

                        if (rowHandle >= 0)
                        {
                            // Lấy dữ liệu từ cột "function"
                            object functionValue = gridView.GetRowCellValue(rowHandle, "chung_tu");

                            //// Hiển thị thông báo
                            //XtraMessageBox.Show("Chức năng được chọn cho dòng " + (rowHandle + 1) + ": " + functionValue?.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (e.Button == buttonEdit.Properties.Buttons[0])
                            {
                                if (stepProgressBar1.SelectedItemIndex == 0)
                                {
                                    // Code khi bạn muốn hiển thị hộp thoại và chạy function
                                    DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn thực hiện tạo hóa đơn nháp không?", "Xác nhận", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        trangthai = TrangThai.TaoNhaps;
                                        // Nếu người dùng chọn Yes, thực hiện hàm function_hd
                                        funtion_hd(gridControl);
                                    }
                                    else if (dialogResult == DialogResult.No)
                                    {
                                        return;
                                    }
                                }
                                else
                                    if(stepProgressBar1.SelectedItemIndex == 1)
                                {
                                    //Xử lý sự kiện nhấn nút ở trạng thái hóa đơn đã phát hành để hủy hóa đơn
                                    DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn thực hiện hủy bỏ hóa đơn đã phát hành hay không?", "Xác nhận", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        trangthai = TrangThai.HuyHoaDons;
                                        // Nếu người dùng chọn Yes, thực hiện hàm function_hd
                                        funtion_hd(gridControl);
                                    }
                                    else if (dialogResult == DialogResult.No)
                                    {
                                        return;
                                    }
                                }    
                            }
                            else if (e.Button == buttonEdit.Properties.Buttons[1])
                            {
                                if (stepProgressBar1.SelectedItemIndex == 0)
                                {
                                    // Code khi bạn muốn hiển thị hộp thoại và chạy function
                                    DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn thực hiện xóa hóa đơn nháp không?", "Xác nhận", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        trangthai = TrangThai.XoaNhaps;
                                        // Nếu người dùng chọn Yes, thực hiện hàm function_hd
                                        funtion_hd(gridControl);
                                    }
                                    else if (dialogResult == DialogResult.No)
                                    {
                                        return;
                                    }
                                }
                                else
                                    if (stepProgressBar1.SelectedItemIndex == 1)
                                {
                                    // Code khi bạn muốn hiển thị hộp thoại và chạy function
                                    DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn thực hiện xem hóa đơn đã ký không?", "Xác nhận", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        trangthai = TrangThai.XemHoaDons;
                                        // Nếu người dùng chọn Yes, thực hiện hàm function_hd
                                        funtion_hd(gridControl);
                                    }
                                    else if (dialogResult == DialogResult.No)
                                    {
                                        return;
                                    }
                                }
                            }
                            else if (e.Button == buttonEdit.Properties.Buttons[2])
                            {
                                if (stepProgressBar1.SelectedItemIndex == 0)
                                {
                                    // Code khi bạn muốn hiển thị hộp thoại và chạy function
                                    DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn thực hiện phát hành hóa đơn không?", "Xác nhận", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        trangthai = TrangThai.PhatHanhs;
                                        // Nếu người dùng chọn Yes, thực hiện hàm function_hd
                                        funtion_hd(gridControl);
                                    }
                                    else if (dialogResult == DialogResult.No)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Xử lý cho phép sửa cột
        private void gridView1_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView != null && (gridView.FocusedColumn.FieldName == "function" || gridView.FocusedColumn.FieldName == "check_hddt"))
            {
                e.Cancel = false; // Cho phép chỉnh sửa cho cột "function"
            }
            else
            {
                e.Cancel = true; // Không cho phép chỉnh sửa cho các cột khác
            }
        }

        private void funtion_hd(object sender)
        {
            Cls_CallAPI cc = new Cls_CallAPI();

            string user = "0100109106-503";
            string pass = "2wsxCDE#";
            string url0 = "https://api-vinvoice.viettel.vn/services/einvoiceapplication/api/";

            // Lấy GridControl chứa GridView
            GridControl gridControl = sender as GridControl;

            if (gridControl != null)
            {
                // Lấy GridView từ GridControl
                GridView gridView = gridControl.MainView as GridView;

                Cls_EnCrypting ecr = new Cls_EnCrypting();

                DataView dv = gridView.DataSource as DataView;
                int index = gridView.FocusedRowHandle;
                string keyhddt = dv[index]["keyhd"].ToString();
                if (keyhddt == "0")
                {
                    dv[index]["keyhd"] = dv[index]["f_identity"];
                    keyhddt = dv[index]["keyhd"].ToString();
                    Class.Functions.UpdateAField("_thu_tien", "keyhd", dv[index]["keyhd"].ToString(), "chung_tu='" + dv[index]["chung_tu"].ToString() + "' and ngay_thc='" + Convert.ToDateTime(dv[index]["ngay_thc"]).ToString("MM/dd/yyyy") + "'", true);
                }

                dt_hd1 = dt_hd1.AddMilliseconds(tgianhd);
                string keyhd = Convert.ToDecimal(keyhddt).ToString("000000000000000000") + "-" + ma_gtgt;



                string str_json = generateJson_Viettel("1");

                string urltoken = "https://api-vinvoice.viettel.vn/auth/login";
                string jsonContent = "{\"username\":\"" + user + "\",\"password\":\"" + pass + "\"}";

                //Kiểm tra đang chọn vào mới khởi tạo hay không
                if (stepProgressBar1.SelectedItemIndex == 0 && trangthai == TrangThai.TaoNhaps)
                {
                    string urltaonhap = url0 + "InvoiceAPI/InvoiceWS/createOrUpdateInvoiceDraft/" + ma_gtgt;

                    //Class.Functions.editcode("link: " + urltoken + "\n" + "json: \n" + jsonContent);
                    string kq = cc.POST_Json(urltoken, jsonContent);
                    //Class.Functions.editcode(kq);
                    if (kq.StartsWith("OK:"))
                    {
                        kq = kq.Substring(4);
                        string[] access_token1;
                        string[] access_token2;
                        access_token1 = kq.Trim().Split(',');
                        access_token2 = access_token1[0].Trim().Split(':');
                        //string chuoi = access_token2[2].Trim().Replace("\"","");
                        //Class.Functions.editcode(urltaonhap);
                        //Class.Functions.editcode(access_token2[1].Trim().Replace("\"", ""));
                        //Class.Functions.editcode(str_json);
                        string kq1 = cc.POST_Json_Viettel2(urltaonhap, str_json, access_token2[1]);
                        //Class.Functions.editcode(kq1);
                        if (kq1.Length > 3 && kq1.StartsWith("OK:"))
                        {
                            XtraMessageBox.Show("Đẩy dữ liệu hóa đơn nháp thành công: " + kq1, "Thông báo");
                            Class.Functions.UpdateAField("_thu_tien", "tthaihd", "ck1tn", "chung_tu='" + dv[index]["chung_tu"].ToString() + "' and ngay_thc='" + Convert.ToDateTime(dv[index]["ngay_thc"]).ToString("MM/dd/yyyy") + "' and keyhd=" + keyhddt + "", true);

                            loadview();
                        }
                        else
                            XtraMessageBox.Show("Lỗi: " + kq1);
                    }
                    else
                        XtraMessageBox.Show("Lỗi: " + kq);
                }
                else
                    if (stepProgressBar1.SelectedItemIndex == 0 && trangthai == TrangThai.XoaNhaps)
                {
                    string urlxoanhap = url0 + "InvoiceAPI/InvoiceWS/cancelTransactionInvoiceDraft";
                    string kq = cc.POST_Json(urltoken, jsonContent);
                    //Class.Functions.editcode(kq);
                    if (kq.StartsWith("OK:"))
                    {
                        kq = kq.Substring(4);
                        string[] access_token1;
                        string[] access_token2;
                        access_token1 = kq.Trim().Split(',');
                        access_token2 = access_token1[0].Trim().Split(':');
                        //string chuoi = access_token2[2].Trim().Replace("\"","");
                        //Class.Functions.editcode(urltaonhap);
                        //Class.Functions.editcode(access_token2[1].Trim().Replace("\"", ""));
                        //Class.Functions.editcode(str_json);

                        string keyid = (Convert.ToDecimal(keyhddt)).ToString("000000000000000000") + "-" + ma_gtgt.Trim();
                        str_json = @"{
									""supplierTaxCode"":""" + ma_gtgt + @""",
									""transactionUuid"":""" + keyid + @"""
								 }
								";
                        
                        
                        //Class.Functions.editcode(str_json);
                        string kq1 = cc.POST_Json_Viettel2(urlxoanhap, str_json, access_token2[1]);
                        //Class.Functions.editcode(kq1);

                        kq1 = kq1.Substring(3);
                        xoahd xhd = Newtonsoft.Json.JsonConvert.DeserializeObject<xoahd>(kq1);
                        string ketqua = xhd.description.ToString();
                        if (ketqua.Contains("Hủy hóa đơn thành công"))
                        {
                            XtraMessageBox.Show("Xoá hóa đơn nháp trên Website Viettel thành công!");
                            Class.Functions.UpdateAField("_thu_tien", "tthaihd", "", "chung_tu='" + dv[index]["chung_tu"].ToString() + "' and ngay_thc='" + Convert.ToDateTime(dv[index]["ngay_thc"]).ToString("MM/dd/yyyy") + "' and keyhd=" + keyhddt + "", true);

                            loadview();
                        }
                        else
                            XtraMessageBox.Show("Có lỗi khi xóa hóa đơn: " + ketqua);
                    }
                    else
                        XtraMessageBox.Show("Lỗi: " + kq);
                }
                else
                    if (stepProgressBar1.SelectedItemIndex == 0 && trangthai == TrangThai.PhatHanhs)
                {
                    if (dv[index]["tthaihd"].ToString().Contains("ck1"))
                    {
                        XtraMessageBox.Show("Hóa đơn chưa xóa nháp vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        DataTable dtkhang = Class.Functions.GetDataToTable("select * from _khang");
                        DataRow[] drkhang = new DataRow[] { };
                        drkhang = dtkhang.Select("ma_kh = " + dv[index]["ma_kh"].ToString());
                        string emailhd = drkhang[0]["email"].ToString().Trim();
                        string urltaonhap = url0 + "InvoiceAPI/InvoiceWS/createInvoice/" + ma_gtgt;

                        //Class.Functions.editcode("link: " + urltoken + "\n" + "json: \n" + jsonContent);
                        string kq = cc.POST_Json(urltoken, jsonContent);
                        //Class.Functions.editcode(kq);
                        if (kq.StartsWith("OK:"))
                        {
                            kq = kq.Substring(4);
                            string[] access_token1;
                            string[] access_token2;
                            access_token1 = kq.Trim().Split(',');
                            access_token2 = access_token1[0].Trim().Split(':');
                            //string chuoi = access_token2[2].Trim().Replace("\"","");
                            //Class.Functions.editcode(urltaonhap);
                            //Class.Functions.editcode(access_token2[1].Trim().Replace("\"", ""));
                            Class.Functions.editcode(str_json);
                            string kq1 = cc.POST_Json_Viettel2(urltaonhap, str_json, access_token2[1]);
                            //Class.Functions.editcode(kq1);
                            if (kq1.Length > 3 && kq1.StartsWith("OK:"))
                            {
                                
                                //Class.Functions.UpdateAField("_thu_tien", "tthaihd", "ck1tn", "chung_tu='" + dv[index]["chung_tu"].ToString() + "' and ngay_thc='" + Convert.ToDateTime(dv[index]["ngay_thc"]).ToString("MM/dd/yyyy") + "' and keyhd=" + keyhddt + "", true);


                                kq1 = kq1.Substring(3);
                                kqkyhd kqhd = Newtonsoft.Json.JsonConvert.DeserializeObject<kqkyhd>(kq1);
                                if (!string.IsNullOrWhiteSpace(kqhd.result.transactionID.ToString()))
                                {
                                    string invoiceNo = kqhd.result.invoiceNo.ToString();
                                    //MessageBox.Show(invoiceNo);
                                    string sql_upd = "Update " + bangcapnhat + " set tthaihd='ck2phhd', muserk2=1, date2=getdate(), sohd='" + invoiceNo + "' where keyhd='" + keyhddt.ToString() + "'";
                                    Class.Functions.RunSQL(sql_upd);
                                    //Class.Functions.editcode(sql_upd);

                                    string sqlcheck = "if(exists(Select * from _InvoicesViettel1 where partnerInvoiceID=" + keyhddt.ToString() + ")) select 1 else select 0";
                                    //Class.Functions.editcode(sqlcheck);
                                    object kt = Class.Functions.SQLEXEC(sqlcheck, true);
                                    if (kt.ToString() == "1")
                                    {
                                        string up = "update _InvoicesViettel1 set datetime=getdate(),invoiceNo='" + invoiceNo + "',partnerInvoiceStringID='" + kqhd.result.transactionID.ToString() + "',MTC='" + kqhd.result.reservationCode.ToString() + "', InvoiceGuid='"
                                + keyhd.ToString() + "',messLog=N'Đã phát hành',statushd='0',invoiceForm='"
                                + dv[index]["mau_hd"].ToString() + "',email='" + emailhd + "'phi_tm='" + Convert.ToDateTime(dv[index]["phi_tm"]).ToString("MM/dd/yyyy") + "',ma_kh='" + dv[index]["ma_kh"].ToString() + "',invoiceSerial='" + dv[index]["serihd"].ToString() + "' where partnerInvoiceID=" + keyhddt.ToString() + "";
                                        //Class.Functions.editcode(up);
                                        Class.Functions.RunSQL(up);
                                    }
                                    else
                                    {
                                        string insert = "insert into _InvoicesViettel1(datetime,invoiceNo,InvoiceGuid,messLog,statushd,invoiceForm,invoiceSerial,email,phi_tm,ma_kh,partnerInvoiceID,partnerInvoiceStringID,MTC) values(getdate(),'"

                                + keyhd.ToString() + "','" + invoiceNo + "',N'Đã Phát hành','0','"
                                + dv[index]["mau_hd"].ToString() + "','" + dv[index]["serihd"].ToString() + "','" + emailhd + "','" + Convert.ToDateTime(dv[index]["phi_tm"]).ToString("MM/dd/yyyy") + "','" + dv[index]["ma_kh"].ToString() + "','"
                                + keyhddt.ToString() + "','" + kqhd.result.transactionID.ToString() + "','" + kqhd.result.reservationCode.ToString() + "')";
                                        //Class.Functions.editcode(insert);
                                        Class.Functions.RunSQL(insert);
                                    }
                                    XtraMessageBox.Show("Ký phát hành hóa đơn thành công: " + kq1, "Thông báo");
                                }
                                else 
                                {
                                    XtraMessageBox.Show("Hóa đơn tạo nháp nhưng chưa xóa, vui lòng kiểm tra lại", "Thông báo"); return;
                                }

                                loadview();
                            }
                            else
                                XtraMessageBox.Show("Lỗi: " + kq1);
                        }
                        else
                            XtraMessageBox.Show("Lỗi: " + kq);
                    }
                }
                else
                    if (stepProgressBar1.SelectedItemIndex == 1 && trangthai == TrangThai.HuyHoaDons)
                {
                    string urltaonhap = url0 + "InvoiceAPI/InvoiceWS/cancelTransactionInvoice";
                    string url_kt = url0 + "InvoiceAPI/InvoiceWS/searchInvoiceByTransactionUuid";
                    string str_json_kt = "supplierTaxCode=" + ma_gtgt + "&transactionUuid=" + Convert.ToDecimal(keyhddt).ToString("000000000000000000") + "-" + ma_gtgt + "";
                    str_json = generateJson_Viettel("5");
                    //Class.Functions.editcode("link: " + urltoken + "\n" + "json: \n" + jsonContent);

                    string kq = cc.POST_Json(urltoken, jsonContent);
                    //Class.Functions.editcode(kq);
                    if (kq.StartsWith("OK:"))
                    {
                        kq = kq.Substring(4);
                        string[] access_token1;
                        string[] access_token2;
                        access_token1 = kq.Trim().Split(',');
                        access_token2 = access_token1[0].Trim().Split(':');

                        string kq_kt = cc.POST_Form_Viettel2(url_kt, str_json_kt, access_token2[1]);
                        kq_kt = kq_kt.Substring(3).Replace("[", "").Replace("]", "");
                        //Class.Functions.editcode(kq_kt);
                        tracuuhd tchd = Newtonsoft.Json.JsonConvert.DeserializeObject<tracuuhd>(kq_kt);
                        string invoiceNo = (tchd.result.invoiceNo.ToString()).Substring(6);
                        string invoiceSeri = (tchd.result.invoiceNo.ToString()).Substring(0, 6);
                        tgianhd = Convert.ToDouble(tchd.result.issueDate.ToString());
                        string reservationCode = tchd.result.reservationCode.ToString();
                        dt_hd = new DateTime(1970, 1, 1).AddMilliseconds(tgianhd);
                        string ma_tracuu = tchd.result.reservationCode.ToString();

                        if (!string.IsNullOrWhiteSpace(invoiceNo))
                        {
                            str_json = generateJson_Viettel("5");
                            //str_json = str_json.Replace("/", "%2F");
                            //Class.Functions.editcode(str_json);
                            string kq1 = cc.POST_Form_Viettel2(urltaonhap, str_json, access_token2[1]);
                            //Class.Functions.editcode(kq1);
                                if (kq1.Length > 3 && kq1.StartsWith("OK:"))
                                {
                                    XtraMessageBox.Show("Hủy hóa đơn thành công: " + kq1, "Thông báo");
                                    string sql_upd = "Update " + bangcapnhat + " set tthaihd='ck2hhd', muserk2=1, date2=getdate(), loaidl='12' where keyhd='" + keyhddt.ToString() + "'";
                                    Class.Functions.RunSQL(sql_upd);

                                    loadview();
                                }
                                else
                                    XtraMessageBox.Show("Lỗi: " + kq1);
                        }
                        else
                        {
                            XtraMessageBox.Show("Lỗi: " + kq_kt);
                        }
                    }
                    else
                        XtraMessageBox.Show("Lỗi: " + kq);
                }
                else
                    if (stepProgressBar1.SelectedItemIndex == 1 && trangthai == TrangThai.XemHoaDons)
                {
                    string sohd = dv[index]["sohd"].ToString();
                    string mauhd = dv[index]["mau_hd"].ToString();
                    string urltaonhap = url0 + "InvoiceAPI/InvoiceUtilsWS/getInvoiceRepresentationFile";
                    str_json = "{\r\n    \"supplierTaxCode\": \""+ ma_gtgt +"\",\r\n    \"invoiceNo\": \""+ sohd +"\",\r\n    \"templateCode\": \""+ mauhd +"\",\r\n    \"transactionUuid\": \""+ Convert.ToDecimal(keyhddt).ToString("000000000000000000") + "-" + ma_gtgt +"\",\r\n    \"fileType\": \"PDF\"\r\n}";
                    //Class.Functions.editcode(urltoken+"\n"+ jsonContent);
                    string kq = cc.POST_Json(urltoken, jsonContent);
                    //Class.Functions.editcode(kq);
                    if (kq.StartsWith("OK:"))
                    {
                        kq = kq.Substring(4);
                        string[] access_token1;
                        string[] access_token2;
                        access_token1 = kq.Trim().Split(',');
                        access_token2 = access_token1[0].Trim().Split(':');
                        
                        //Class.Functions.editcode(str_json);
                        string kq1 = cc.POST_Json_Viettel2(urltaonhap, str_json, access_token2[1]);
                        //Class.Functions.editcode(kq1);
                        if (kq1.Length > 3 && kq1.StartsWith("OK:"))
                        {
                            kq1 = kq1.Substring(3);
                            tracuu tchd = Newtonsoft.Json.JsonConvert.DeserializeObject<tracuu>(kq1);
                            string errorCode = tchd.errorCode.ToString();
                            string description = tchd.description?.ToString();
                            string fileToBytes = tchd.fileToBytes.ToString();
                            if(!string.IsNullOrWhiteSpace(fileToBytes))
                            {
                                byte[] bt1 = Convert.FromBase64String(fileToBytes);

                                string tempFileName = sohd.ToString() + ".pdf";
                                string tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

                                File.WriteAllBytes(tempFilePath, bt1);
                                Process.Start(tempFilePath);

                            }
                            else
                            {
                                XtraMessageBox.Show(errorCode);
                            }    
                        }
                        else
                            XtraMessageBox.Show("Lỗi: " + kq1);
                    }
                    else
                        XtraMessageBox.Show("Lỗi: " + kq);
                }
            }

            if (gridControl.DataSource == null)
            {
                XtraMessageBox.Show("Không có dữ liệu để lưu. Vui lòng kiểm tra lại!");
                return;
            }
        }



        //Kiểm tra xem form đã tồn tại chưa
        private bool Exitform(Form form)
        {
            foreach (var child in MdiChildren)
            {
                if (child.Name == form.Name)
                {
                    child.Activate();
                    return true;
                }
            }
            return false;
        }

        //xử lý khi kích chuột vào item thì xử lý item
        private void stepProgressBar1_MouseClick(object sender, MouseEventArgs e)
        {
            StepProgressBar stepProgressBar = (StepProgressBar)sender;
            StepProgressBarHitInfo info = stepProgressBar.CalcHitInfo(e.Location);
            //MessageBox.Show(info.Item.ToString());
            if (info.InItem)
            {
                //get the clicked item
                StepProgressBarItem item = info.Item;

                if (item.Name == "spBar_item_khoitao")
                {
                    stepProgressBar.SelectedItemIndex = 0;
                    loadview();
                }

                if (item.Name == "spBar_item_choky")
                {
                    stepProgressBar.SelectedItemIndex = 1;

                    loadview();
                }

                if (item.Name == "spBar_item_phathanh")
                {
                    stepProgressBar.SelectedItemIndex = 2;
                    loadview();
                }

                //stepProgressBar.Appearances.FirstContentBlockAppearance.CaptionActive.ForeColor = Color.Orange;
                //stepProgressBar.Appearances.FirstContentBlockAppearance.CaptionInactive.ForeColor = Color.Red;
                //stepProgressBar.Appearances.SecondContentBlockAppearance.DescriptionActive.ForeColor = Color.BlueViolet;
                //stepProgressBar.Appearances.SecondContentBlockAppearance.DescriptionInactive.ForeColor = Color.BlueViolet;

                item.Appearance.ContentBlockAppearance.Caption.ForeColor = Color.Blue;
                item.Appearance.ContentBlockAppearance.Description.ForeColor = Color.Blue;

                item.ContentBlock1.Appearance.Caption.ForeColor = Color.Blue;
                item.ContentBlock2.Appearance.Description.ForeColor = Color.Blue;

                item.Appearance.ContentBlockAppearance.CaptionActive.ForeColor = Color.Red;
                item.Appearance.ContentBlockAppearance.CaptionInactive.ForeColor = Color.Red;
                item.Appearance.ContentBlockAppearance.DescriptionActive.ForeColor = Color.Red;
                item.Appearance.ContentBlockAppearance.DescriptionInactive.ForeColor = Color.Red;
                item.Appearance.ContentBlockAppearance.DescriptionInactive.TextOptions.HAlignment = HorzAlignment.Center;




            }
        }

        private void MdiManager_PageAdded(object sender, MdiTabPageEventArgs e)
        {
            XtraMdiTabPage page = e.Page;
            page.Tooltip = "Tooltip for the page " + page.Text;
        }

        private void stepProgressBar1_SelectedItemChanged(object sender, StepProgressBarSelectedItemChangedEventArgs e)
        {
            StepProgressBar bar = sender as StepProgressBar;
            foreach (StepProgressBarItem item in bar.Items)
            {
                item.Options.Indicator.ActiveStateImageOptions.SvgImage = null;
                item.Options.Indicator.Width = 40;
                item.Options.ConnectorOffset = -20;
                item.ContentBlock1.Description = null;
            }

            var currentItem = e.SelectedItems.Last();


            if (currentItem != null)
            {
                currentItem.Options.Indicator.Width = 20;
                currentItem.ContentBlock1.Description = "Step " + (bar.SelectedItemIndex + 1).ToString() + " of 3";

                currentItem.Options.ConnectorOffset = 0;

                if (bar.SelectedItemIndex < 3)
                    bar.Appearances.CommonActiveColor = Color.IndianRed;
                if (bar.SelectedItemIndex >= 3)
                    bar.Appearances.CommonActiveColor = Color.Green;
                //    bar.Appearances.CommonActiveColor = Color.Goldenrod;
                //if (bar.SelectedItemIndex >= 3 && bar.SelectedItemIndex < 5)
                //    bar.Appearances.CommonActiveColor = Color.Goldenrod;
                //if (bar.SelectedItemIndex >= 5)
                //    bar.Appearances.CommonActiveColor = Color.Green;
            }
        }

        //Hàm thêm số thứ tự trong gridview
        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());


            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }

        //Xử lý năn chuột
        private void gridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((sender as GridView).IsEditing)
            {
                (sender as GridView).CloseEditor();
                (sender as GridView).UpdateCurrentRow();
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            DataView dtv = gridView1.DataSource as DataView;

            int donght = e.RowHandle;
            if (gridView2.DataSource != null)
            {
                DataView dtv2 = gridView2.DataSource as DataView;
                string filter = "chung_tu='" + dtv[donght]["chung_tu"].ToString().Trim() + "' and ma_scr=" + dtv[donght]["ma_scr"].ToString().Trim()
    + " and ngay_thc='" + dtv[donght]["ngay_thc"].ToString().Trim() + "' and ma_kh= " + dtv[donght]["ma_kh"].ToString();
                dtv2.RowFilter = filter;
            }
        }

        private void txt_view_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            // Giả sử txt_search_tu_ngay và txt_search_den_ngay là các điều khiển DateEdit hoặc TextEdit
            DateTime? tungay = txt_search_tu_ngay.EditValue as DateTime?; // Sử dụng 'as' để chuyển đổi an toàn
            DateTime? dengay = txt_search_den_ngay.EditValue as DateTime?;

            // Khởi tạo chuỗi, sẽ chứa ngày sau khi chúng được kiểm tra và chuyển đổi
            string tu_ngay = string.Empty;
            string den_ngay = string.Empty;

            // Kiểm tra nếu tungay không phải là null và có giá trị hợp lệ
            if (tungay.HasValue)
            {
                tu_ngay = tungay.Value.ToString("MM/dd/yyyy");
            }

            // Kiểm tra nếu dengay không phải là null và có giá trị hợp lệ
            if (dengay.HasValue)
            {
                den_ngay = dengay.Value.ToString("MM/dd/yyyy");
            }

            // Bây giờ bạn có thể sử dụng tu_ngay và den_ngay trong câu truy vấn hoặc logic ứng dụng của bạn

            loadview(tu_ngay, den_ngay);
        }

        public string generateJson_Viettel(string apiid)
        {
            string json = "";
            StringBuilder sb = new StringBuilder();
            string sele = "", docsorachu = "";
            //sb.Append();
            GridView dtg01; //= myform0.grid01;
            GridView dtgct;//= myform0.grid_ct2;
            if (apiid == "1" || apiid == "5") // Nếu là phát hành hoặc hủy
            {
                dtg01 = gridControl1.MainView as GridView;
                dtgct = gridControl2.MainView as GridView;
            }
            else
            {
                if (apiid == "2" || apiid == "3" || apiid == "4")
                {
                    dtg01 = gridControl1.MainView as GridView;
                    dtgct = gridControl2.MainView as GridView;
                }
                else
                {
                    dtg01 = gridControl1.MainView as GridView;
                    dtgct = gridControl2.MainView as GridView;
                }
            }

            DataView dv = dtg01.DataSource as DataView;
            int donght = dtg01.FocusedRowHandle;
            DataView dv2 = dtgct.DataSource as DataView;
            //MessageBox.Show(dv2.Count.ToString()+"--"+apiid);
            DataRow dtr_hd = dv[donght].Row;
            DataTable dt_cauhinh = Class.Functions.GetDataToTable("select * from _cauhinhvt where viettel<>'' and apiid=" + apiid + " order by stt,f_identity");

            DataTable khang = Class.Functions.GetDataToTable("select * from _khang");
            DataTable vung = Class.Functions.GetDataToTable("select * from _vung");
            DataTable tknhang = Class.Functions.GetDataToTable("select * from _tknhang");
            DataTable thietbi = Class.Functions.GetDataToTable("select * from _thietbi");
            DataTable khpw = Class.Functions.GetDataToTable("select * from _khpw");
            DataTable httt = Class.Functions.GetDataToTable("select * from _httt");
            bool chaylandau = true;
            bool mongoac1 = false;
            DataRow[] dtrs_cauhinh = dt_cauhinh.Select("key_fields<>''", "stt,f_identity");
            for (int i = 0; i < dt_cauhinh.Rows.Count; i++)
            {
                DataRow dtr = dt_cauhinh.Rows[i];
                string field = dtr["effect"].ToString();
                if (!string.IsNullOrEmpty(field) || !string.IsNullOrEmpty(dtr["viettel"].ToString()))
                {
                    string field0 = "";
                    string field1 = "";
                    string[] fields = field.Split(',');
                    if (fields.Length == 1)
                    {
                        field1 = fields[0];
                        if (field1.Contains("."))
                            field0 = field1.Substring(field1.IndexOf(".") + 1);

                        if (!string.IsNullOrEmpty(dtr["dieukien"].ToString().Trim()) && string.IsNullOrEmpty(dtr["key_fields"].ToString().Trim().ToLower()))
                        {
                            //MessageBox.Show(dtr["dieukien"].ToString().Trim() + " - " + dtr["effect"].ToString().Trim());
                            bool kq = Class.Functions.CheckRowCondition(dtr_hd, dtr["dieukien"].ToString().Trim());
                            if (kq == false) continue; // Kiểm tra sai thì bỏ thẻ
                            //Class.Functions.editcode(dtr["dieukien"].ToString().Trim());
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dtr["key_fields"].ToString().Trim().ToLower()))
                        {
                            // Kiểm tra điều kiện
                            if (!string.IsNullOrEmpty(dtr["dieukien"].ToString().Trim()))
                            {
                                bool kq = Class.Functions.CheckRowCondition(dtr_hd, dtr["dieukien"].ToString().Trim());
                                if (kq == false) continue; // Kiểm tra sai thì bỏ thẻ
                            }

                            for (int k = 0; k < fields.Length; k++)
                            {
                                field0 = "";
                                field1 = fields[k];
                                if (field1.StartsWith("_thu_tien."))
                                {
                                    // Kiểm tra giá trị
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (dtr_hd.Table.Columns.Contains(field2))
                                    {
                                        if (!string.IsNullOrEmpty(dtr_hd[field2].ToString().Trim()))
                                        {
                                            field0 = field2;

                                            break;
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    if (field1.StartsWith("_httt"))
                                {
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (httt.Columns.Contains(field2))
                                    {
                                        DataRow[] dtrs;
                                        if (apiid == "1" || apiid == "5")
                                            dtrs = httt.Select("httt='" + dtr_hd["tt3"].ToString() + "'");
                                        else
                                            dtrs = httt.Select("httt='" + dtr_hd["tt3_b"].ToString() + "'");
                                        if (dtrs.Length > 0)
                                            if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    if (field1.StartsWith("_thietbi"))
                                {
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (thietbi.Columns.Contains(field2))
                                    {
                                        DataRow[] dtrs;
                                        if (apiid == "1" || apiid == "5")
                                            dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dtr_hd["ma_thb"].ToString() + " and dvcs=1");
                                        else
                                            dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dtr_hd["ma_thb_b"].ToString() + " and dvcs=1");
                                        if (dtrs.Length > 0)
                                            if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    if (field1.StartsWith("_khang"))
                                {
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (khang.Columns.Contains(field2))
                                    {
                                        DataRow[] dtrs;
                                        if (apiid == "1" || apiid == "5")
                                            dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh"].ToString() + " and dvcs=1");
                                        else
                                            dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh_a"].ToString() + " and dvcs=1");
                                        if (dtrs.Length > 0)
                                            if (!string.IsNullOrEmpty(dtrs[0][field2].ToString().Trim()))
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    if (field1.StartsWith("_khangg"))
                                {
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (khang.Columns.Contains(field2))
                                    {
                                        DataRow[] dtrs;
                                        if (apiid == "1" || apiid == "5")
                                            dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh"].ToString() + " and dvcs=1");
                                        else
                                            dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh_b"].ToString() + " and dvcs=1");
                                        if (dtrs.Length > 0)
                                            if (!string.IsNullOrEmpty(dtrs[0][field2].ToString().Trim()))
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                    if (field1.StartsWith("_vung"))
                                {
                                    string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                    if (vung.Columns.Contains(field2))
                                    {
                                        DataRow[] dtrs;
                                        if (apiid == "1" || apiid == "5")
                                            dtrs = vung.Select("vung=" + dtr_hd["vung"].ToString());
                                        else
                                            dtrs = vung.Select("vung=" + dtr_hd["vung_b"].ToString());
                                        if (dtrs.Length > 0)
                                            if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                continue;
                                    }
                                    else
                                        continue;
                                }
                                else break;
                            }
                        }
                    }

                    //if(field.)
                    //if(field.Contains("."))
                    //field0 = field.Substring(field.IndexOf(".")+1);
                    // Nếu là dữ liệu chi tiết
                    if (!string.IsNullOrEmpty(dtr["key_fields"].ToString().Trim().ToLower()))
                    {
                        if (chaylandau)
                            chaylandau = false;
                        else
                            continue;
                        // sử dụng data chi tiết dv2

                        for (int k = 0; k < dv2.Count; k++)
                        {
                            for (int j = 0; j < dtrs_cauhinh.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(dtrs_cauhinh[j]["dieukien"].ToString().Trim()) && !string.IsNullOrEmpty(dtr["key_fields"].ToString().Trim().ToLower()))
                                {

                                    bool kq = Class.Functions.CheckRowCondition(dv2[k].Row, dtrs_cauhinh[j]["dieukien"].ToString().Trim());
                                    if (kq == false) continue; // Kiểm tra sai thì bỏ thẻ
                                }

                                if (!chaylandau && dtrs_cauhinh[j]["viettel"].ToString() == "{")
                                {
                                    //if()
                                    if (!mongoac1)
                                        mongoac1 = true;
                                    else
                                        sb.Append(",");
                                }

                                if (apiid != "5")
                                    sb.Append("\n");
                                sb.Append(dtrs_cauhinh[j]["viettel"].ToString());
                                sb.Append(dtrs_cauhinh[j]["batdau"].ToString());
                                //sb.Append(dtrs_cauhinh[j]["viettel"].ToString());
                                fields = dtrs_cauhinh[j]["effect"].ToString().Split(',');
                                field1 = fields[0]; field0 = "";
                                if (fields.Length > 0)
                                {
                                    for (int k1 = 0; k1 < fields.Length; k1++)
                                    {
                                        field0 = "";
                                        field1 = fields[k1];
                                        if (field1.StartsWith("_thu_tien."))
                                        {
                                            // Kiểm tra giá trị
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);    
                                            if (field2 == "recno")
                                            {
                                                field0 = field2;
                                                break;
                                            }
                                            else
                                                if (dv2.Table.Columns.Contains(field2))
                                            {
                                                if (!string.IsNullOrEmpty(dv2[k][field2].ToString()))
                                                {
                                                    field0 = field2;
                                                    break;
                                                }
                                                else
                                                    continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            if (field1.StartsWith("_httt."))
                                        {
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                            if (httt.Columns.Contains(field2))
                                            {
                                                DataRow[] dtrs;
                                                //if(apiid=="1" || apiid=="5")
                                                dtrs = httt.Select("httt='" + dv2[k]["tt3"].ToString() + "'");
                                                //else
                                                //	dtrs= thietbi.Select("ma_thb>0 and ma_thb="+dv2[0]["ma_thb_b"].ToString()+" and dvcs="+ Cls_VariGlobal.Dvcs.ToString());
                                                if (dtrs.Length > 0)
                                                    if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                                    {
                                                        field0 = field2;
                                                        break;
                                                    }
                                                    else
                                                        continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            if (field1.StartsWith("_thietbi."))
                                        {
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                            if (thietbi.Columns.Contains(field2))
                                            {
                                                DataRow[] dtrs;
                                                //if(apiid=="1" || apiid=="5")
                                                dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dv2[k]["ma_thb"].ToString() + " and dvcs=1");
                                                //else
                                                //	dtrs= thietbi.Select("ma_thb>0 and ma_thb="+dv2[0]["ma_thb_b"].ToString()+" and dvcs="+ Cls_VariGlobal.Dvcs.ToString());
                                                if (dtrs.Length > 0)
                                                    if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                                    {
                                                        field0 = field2;
                                                        break;
                                                    }
                                                    else
                                                        continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            if (field1.StartsWith("_khang."))
                                        {
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                            if (khang.Columns.Contains(field2))
                                            {
                                                DataRow[] dtrs;
                                                //if(apiid=="1" || apiid=="5")
                                                dtrs = khang.Select("ma_kh=" + dv2[k]["ma_kh"].ToString() + " and dvcs=1");
                                                //else
                                                //	dtrs= khang.Select("ma_kh="+dtr_hd["ma_kh_a"].ToString()+" and dvcs="+ Cls_VariGlobal.Dvcs.ToString());
                                                if (dtrs.Length > 0)
                                                    if (!string.IsNullOrEmpty(dtrs[0][field2].ToString().Trim()))
                                                    {
                                                        field0 = field2;
                                                        break;
                                                    }
                                                    else
                                                        continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            if (field1.StartsWith("_khangg."))
                                        {
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                            if (khang.Columns.Contains(field2))
                                            {
                                                DataRow[] dtrs;
                                                //if(apiid=="1" || apiid=="5")
                                                dtrs = khang.Select("ma_kh=" + dv2[k]["ma_kh"].ToString() + " and dvcs=1");
                                                //else
                                                //	dtrs= khang.Select("ma_kh="+dtr_hd["ma_kh_b"].ToString()+" and dvcs="+ Cls_VariGlobal.Dvcs.ToString());
                                                if (dtrs.Length > 0)
                                                    if (!string.IsNullOrEmpty(dtrs[0][field2].ToString().Trim()))
                                                    {
                                                        field0 = field2;
                                                        break;
                                                    }
                                                    else
                                                        continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            if (field1.StartsWith("_vung."))
                                        {
                                            string field2 = field1.Substring(field1.IndexOf(".") + 1);
                                            if (vung.Columns.Contains(field2))
                                            {
                                                DataRow[] dtrs;
                                                //if(apiid=="1" || apiid=="5")
                                                dtrs = vung.Select("vung=" + dv2[k]["vung"].ToString());
                                                //else
                                                //	dtrs= vung.Select("ma_kh="+dtr_hd["ma_kh_b"].ToString()+" and dvcs="+ Cls_VariGlobal.Dvcs.ToString());
                                                if (dtrs.Length > 0)
                                                    if (!string.IsNullOrEmpty(dtrs[0][field2].ToString()))
                                                    {
                                                        field0 = field2;
                                                        break;
                                                    }
                                                    else
                                                        continue;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                        {
                                            field1 = dtrs_cauhinh[j]["effect"].ToString();
                                            break;
                                        }
                                    }
                                }

                                field = field1;
                                // field0 = "";
                                // if(field.Contains("."))
                                // field0 = field.Substring(field.IndexOf(".")+1);	
                                if (field.StartsWith("_thu_tien."))
                                {
                                    if (field0 == "recno")
                                    {
                                        sb.Append((k + 1).ToString());
                                    }
                                    else
                                        if (field0.StartsWith("ktdb"))
                                    {
                                        int pt = 0;
                                        if (int.TryParse(dv2[k][field0].ToString(), out pt))
                                        {
                                            sb.Append(pt.ToString("#0"));
                                        }
                                        else
                                        {
                                            sb.Append(pt.ToString("-1"));
                                        }
                                    }
                                    else
                                        if (dv2.Table.Columns.Contains(field0))
                                    {
                                        if (dv2.Table.Columns[field0].DataType == typeof(DateTime))
                                        {
                                            if (string.IsNullOrEmpty(dtrs_cauhinh[j]["ghi_chu"].ToString()))
                                            {
                                                DateTime dt0 = new DateTime(1970, 1, 1);
                                                DateTime dt = Convert.ToDateTime(dv2[k][field0]);
                                                double total = (dt - dt0).TotalMilliseconds;
                                                sb.Append(total.ToString("#"));
                                            }
                                            else
                                            {
                                                //DateTime dt0= new DateTime(1970,1,1);
                                                DateTime dt = Convert.ToDateTime(dv2[k][field0]);
                                                //double total= (dt-dt0).TotalMilliseconds;
                                                sb.Append(dt.ToString(dtrs_cauhinh[j]["ghi_chu"].ToString().Trim()));
                                            }
                                        }
                                        else
                                            if (Class.Functions.IsNumberType(dv2.Table.Columns[field0].DataType))
                                        {
                                            decimal sotien = Convert.ToDecimal(dv2[k][field0]);
                                            sb.Append(Math.Abs(sotien).ToString(dtrs_cauhinh[j]["ghi_chu"].ToString()));
                                            //MessageBox.Show(dtrs_cauhinh[j]["ghi_chu"].ToString()+ "_"+field+"_"+ dtrs_cauhinh[j]["f_identity"].ToString());
                                        }
                                        else
                                            sb.Append(dv2[k][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    }
                                }
                                else
                                    if (field.StartsWith("_httt."))
                                {
                                    if (httt.Columns.Contains(field0))
                                    {
                                        // Lấy từ _thietbi
                                        DataRow[] dtrs = httt.Select("httt='" + dv2[k]["tt3"].ToString() + "'");
                                        if (dtrs.Length > 0)
                                        {
                                            sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                                        }
                                    }
                                }
                                else
                                    if (field.StartsWith("_thietbi."))
                                {
                                    if (thietbi.Columns.Contains(field0))
                                    {
                                        // Lấy từ _thietbi
                                        DataRow[] dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dv2[k]["ma_thb"].ToString() + " and dvcs=1");
                                        if (dtrs.Length > 0)
                                        {
                                            sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                                        }
                                    }
                                }
                                else
                                    if (field.StartsWith("_khang."))
                                {
                                    if (khang.Columns.Contains(field0))
                                    {
                                        DataRow[] dtrs = khang.Select("ma_kh=" + dv2[k]["ma_kh"].ToString() + " and dvcs=1");
                                        if (dtrs.Length > 0)
                                            sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    }

                                    // Lấy từ khang
                                }
                                else
                                    if (field.StartsWith("_vung."))
                                {
                                    DataRow[] dtrs = vung.Select("vung=" + dv2[k]["vung"].ToString());
                                    if (dtrs.Length > 0)
                                        sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                                    // Lấy từ khang
                                }
                                //else
                                //    if (field1.StartsWith("@@.cv1"))
                                //{
                                //    //string sotien=  "";
                                //    double sotien = 0.0;
                                //    field1 = field1.Substring(field1.IndexOf("(") + 1);
                                //    string fieldn = field1.Substring(0, field1.IndexOf(")"));
                                //    field1 = fieldn.Substring(fieldn.IndexOf(".") + 1);
                                //    //sotien= Convert.ToDouble(dtr_hd[field1]);
                                //    if (fieldn.StartsWith("_vung"))
                                //    {
                                //        DataRow[] dtrs;
                                //        //if(apiid=="1" || apiid=="5")
                                //        dtrs = vung.Select("vung=" + dv2[k]["vung"].ToString());
                                //        if (dtrs.Length > 0)
                                //        {
                                //            string kq;
                                //            kq = Class.Functions.MdiParentForm.cv1(dtrs[0][field1].ToString());
                                //            sb.Append(kq);
                                //        }
                                //    }
                                //}
                                else
                                    if (field.StartsWith("_khpw."))
                                {
                                    //DataRow[] dtrs = khpw.Select("dvcs=1");
                                    //if (dtrs.Length > 0 && khpw.Columns.Contains(field0))
                                    //    sb.Append(dtrs[0][field0].ToString().Trim());
                                    //// Lấy từ khang
                                    sb.Append(ma_gtgt);
                                }
                                else
                                    sb.Append(field);
                                sb.Append(dtrs_cauhinh[j]["ketthuc"].ToString());
                            }
                        }

                        //	sb.Append(dtr["ketthuc"].ToString());
                    }
                    else
                    {
                        if (apiid != "5")
                            sb.Append("\n");
                        //sb.Append(dtr["viettel"].ToString());
                        sb.Append(dtr["viettel"].ToString());
                        // Sử dụng data tổng
                        sb.Append(dtr["batdau"].ToString().Trim());
                        if (field1 == "getdate()")
                        {
                            if (string.IsNullOrEmpty(dtr["ghi_chu"].ToString()))
                            {
                                DateTime dt0 = new DateTime(1970, 1, 1);
                                DateTime dt = DateTime.Now.Date;
                                double total = (dt - dt0).TotalMilliseconds;
                                sb.Append(total.ToString("#"));
                            }
                            else
                            {
                                DateTime dt = DateTime.Now;
                                sb.Append(dt.ToString(dtr["ghi_chu"].ToString()));
                            }
                        }
                        else
                            if (field1.StartsWith("@cert"))
                        {
                            sb.Append(sericert);
                        }
                        else
                        if (field1.StartsWith("@@.bangchuv"))
                        {
                            //string sotien=  "";
                            double sotien = 0.0;
                            field1 = field1.Substring(field1.IndexOf("(") + 1);
                            field1 = field1.Substring(0, field1.Length - 1);
                            field1 = field1.Substring(field1.IndexOf(".") + 1);
                            sotien = Convert.ToDouble(dtr_hd[field1]);

                            string kq;
                            if (apiid == "1" || apiid == "5")
                            {
                                kq = Class.Functions.bangchuv(sotien, dtr_hd["dv_do"].ToString());
                                //Class.Functions.editcode(sotien.ToString()+" - "+kq);
                            }
                            else
                            {
                                kq = Class.Functions.bangchuv(sotien, dtr_hd["dv_do_B"].ToString());
                                //Class.Functions.editcode(sotien.ToString() + " - " + kq);
                            }
                            sb.Append(kq);
                        }
                        else
                            if (field1.StartsWith("_tknhang."))
                        {
                            DataRow[] dtrs;
                            if (apiid == "1" || apiid == "5")
                                dtrs = tknhang.Select("tknhang=" + dtr_hd["tknhang"].ToString() + "");
                            else
                                dtrs = tknhang.Select("tknhang=" + dtr_hd["tknhang_A"].ToString());
                            if (dtrs.Length > 0)
                                sb.Append(dtrs[0][field0].ToString().Trim());
                        }
                        else
                            if (field1.StartsWith("_thu_tien."))
                        {
                            
                            // Lấy trực tiếp từ dtr_hd
                            if (dtr_hd.Table.Columns.Contains(field0))
                            {
                                if (field0.StartsWith("keyhd"))
                                {
                                    decimal keyhd = Convert.ToDecimal(dtr_hd[field0]);
                                    string keyhds = keyhd.ToString("000000000000000000") + "-" + ma_gtgt;
                                    //int pt=0;
                                    sb.Append(keyhds);
                                }
                                else
                                    if (field0.StartsWith("ktdb"))
                                {
                                    int pt = 0;
                                    if (int.TryParse(dtr_hd[field0].ToString(), out pt))
                                    {
                                        sb.Append(pt.ToString("#0"));
                                    }
                                    else
                                    {
                                        sb.Append(pt.ToString("-1"));
                                    }
                                }
                                else
                                    if (dtr_hd.Table.Columns[field0].DataType == typeof(DateTime))
                                {
                                    if (apiid == "5" && field0 == "phi_tm")
                                    {
                                        DateTime dt0 = new DateTime(1970, 1, 1);
                                        DateTime dt = Convert.ToDateTime(dt_hd);
                                        double total = (dt - dt0).TotalMilliseconds;
                                        tgianhd = (dt - dt0).TotalMilliseconds;
                                        sb.Append(tgianhd.ToString());
                                    }
                                    else
                                    if (string.IsNullOrEmpty(dtr["ghi_chu"].ToString()))
                                    {
                                        DateTime dt0 = new DateTime(1970, 1, 1);
                                        DateTime dt = Convert.ToDateTime(dtr_hd[field0]);
                                        double total = (dt - dt0).TotalMilliseconds;
                                        sb.Append(total.ToString("#"));
                                    }
                                    else
                                    {
                                        //DateTime dt0= new DateTime(1970,1,1);
                                        DateTime dt = Convert.ToDateTime(dtr_hd[field0]);
                                        //double total= (dt-dt0).TotalMilliseconds;
                                        sb.Append(dt.ToString(dtr["ghi_chu"].ToString().Trim()));

                                    }
                                }
                                else
                                {
                                    sb.Append(dtr_hd[field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));

                                }

                                //Cls_Common_Functions.EditCode( field0+"::"+dtr_hd[field0].ToString());
                            }
                        }
                        else
                            if (field1.StartsWith("_httt."))
                        {
                            if (httt.Columns.Contains(field0))
                            {
                                DataRow[] dtrs;
                                if (apiid == "1" || apiid == "5")
                                    dtrs = httt.Select("httt='" + dtr_hd["tt3"].ToString() + "'");
                                else
                                    dtrs = httt.Select("httt='" + dtr_hd["tt3_b"].ToString() + "'");
                                if (dtrs.Length > 0)
                                    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            }
                            // Lấy từ khang
                        }
                        else
                            if (field1.StartsWith("_khang."))
                        {
                            if (khang.Columns.Contains(field0))
                            {
                                DataRow[] dtrs;
                                if (apiid == "1" || apiid == "5")
                                    dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh"].ToString() + " and dvcs=1");
                                else
                                    dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh_A"].ToString() + " and dvcs=1");
                                if (dtrs.Length > 0)
                                    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            }

                            // Lấy từ khang
                        }
                        else
                            if (field1.StartsWith("_khangg."))
                        {
                            if (khang.Columns.Contains(field0))
                            {
                                DataRow[] dtrs;
                                if (apiid == "1" || apiid == "5")
                                    dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh"].ToString() + " and dvcs=1");
                                else
                                    dtrs = khang.Select("ma_kh=" + dtr_hd["ma_kh_B"].ToString() + " and dvcs=1");
                                if (dtrs.Length > 0)
                                    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            }

                            // Lấy từ khang
                        }
                        else
                            if (field1.StartsWith("_vung."))
                        {
                            if (vung.Columns.Contains(field0))
                            {
                                DataRow[] dtrs;
                                if (apiid == "1" || apiid == "5")
                                    dtrs = vung.Select("vung=" + dtr_hd["vung"].ToString());
                                else
                                    dtrs = vung.Select("vung=" + dtr_hd["vung_B"].ToString());
                                if (dtrs.Length > 0)
                                    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            }

                            // Lấy từ vung
                        }
                        else
                            if (field1.StartsWith("_khpw."))
                        {
                            //DataRow[] dtrs = khpw.Select("dvcs=1");
                            //if (dtrs.Length > 0 && khpw.Columns.Contains(field0))
                            //    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            //// Lấy từ khang
                            sb.Append(ma_gtgt);
                        }
                        else
                            if (field1.StartsWith("_thietbi."))
                        {
                            // Lấy từ _thietbi
                            if (thietbi.Columns.Contains(field0))
                            {
                                DataRow[] dtrs;
                                if (apiid == "1" || apiid == "5")
                                    dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dtr_hd["ma_thb"].ToString() + " and dvcs=1");
                                else
                                    dtrs = thietbi.Select("ma_thb>0 and ma_thb=" + dtr_hd["ma_thb_A"].ToString() + " and dvcs=1");
                                if (dtrs.Length > 0 && thietbi.Columns.Contains(field0))
                                    sb.Append(dtrs[0][field0].ToString().Trim().Replace("\\", "\\\\").Replace("\"", "\\\""));
                            }
                        }
                        else
                            sb.Append(field1);
                        sb.Append(dtr["ketthuc"].ToString());

                    }
                }
            }

            json = sb.ToString().Trim();
            //Class.Functions.editcode(json);
            return json;
        }

        //Xử lý menu chuột phải tại gridview
        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point clickPoint = new Point(e.X, e.Y);
                GridHitInfo gridHitInfo = gridView1.CalcHitInfo(clickPoint);

                if (gridHitInfo.InColumnPanel) // Chuột phải trên header của cột
                {
                    ShowFilterMenu(clickPoint, gridHitInfo.Column);
                }
                else if (gridHitInfo.InRowCell) // Chuột phải trên dữ liệu của cột
                {
                    ShowFilterMenu(clickPoint, gridHitInfo.Column);
                }
            }
        }


        private void ShowFilterMenu(Point location, GridColumn column)
        {
            ContextMenu menu = new ContextMenu();

            
            if (column.ColumnType == typeof(string))
            {
                menu.MenuItems.Add(".Chứa", (sender, e) => ShowFilterBox("like", column, "Chứa"));
            }
            else
            {
                menu.MenuItems.Add("=.Bằng", (sender, e) => ShowFilterBox("=", column, "=.Bằng"));
                menu.MenuItems.Add(">.Lớn hơn", (sender, e) => ShowFilterBox(">", column, ">.Lớn hơn"));
                menu.MenuItems.Add("<.Nhỏ hơn", (sender, e) => ShowFilterBox("<", column, "<.Nhỏ hơn"));
                menu.MenuItems.Add(">=.Lớn hơn hoặc bằng", (sender, e) => ShowFilterBox(">=", column, ">=.Lớn hơn hoặc bằng"));
                menu.MenuItems.Add("<=.Nhỏ hơn hoặc bằng", (sender, e) => ShowFilterBox("<=", column, "<=.Nhỏ hơn hoặc bằng"));
                menu.MenuItems.Add("!=.Khác", (sender, e) => ShowFilterBox("!=", column, "!=.Khác"));
            }    

            menu.Show(gridView1.GridControl, location);
        }

        private void ShowFilterBox(string filterOperator, GridColumn column, string menuItemCaption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                //Text = "Nhập giá trị cần lọc",
                Text = $"Filter [{column.FieldName}] {menuItemCaption}",
                StartPosition = FormStartPosition.CenterScreen
            };

            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox() { Left = 50, Top = 20, Width = 400 };
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                string filterValue = textBox.Text;
                ApplyFilter(column, filterOperator, filterValue);
            }
        }

        private void ApplyFilter(GridColumn column, string filterOperator, string filterValue)
        {
            string filterString;
            if (filterOperator == "like")
            {
                filterString = string.Format("[{0}] LIKE '%{1}%'", column.FieldName, filterValue);
            }
            else
            {
                filterString = string.Format("[{0}] {1} '{2}'", column.FieldName, filterOperator, filterValue);
            }
            gridView1.ActiveFilterString = filterString;
        }
        //Kết thúc xử lý menu chuột phải

    }

    class xoahd
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string transactionUuid { get; set; }
        //public string result { get;set; }
    }
    //OK:{"errorCode":null,"description":null,"result":{"supplierTaxCode":"0100109106-503","invoiceNo":"C24TMO12","transactionID":"170712329242683842","reservationCode":"JQJC5U4042WSH4R","codeOfTax":null}}
    class kqkyhd
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public result result { get; set; }
    }

    class result
    {
        public string supplierTaxCode { get; set; }
        public string invoiceNo { get; set;}
        public string transactionID { get; set;}
        public string reservationCode { get; set;}

    }

    class tracuuhd
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string transactionUuid { get; set; }
        public invoices result { get; set; }
    }
    class invoices
    {
        public string supplierTaxCode { get; set; }
        public string invoiceNo { get; set; }
        public string reservationCode { get; set; }
        public string issueDate { get; set; }
        public string status { get; set; }
        public string exchangeStatus { get; set; }
        public string exchangeDes { get; set; }
    }

    class tracuu
    {
        public string errorCode { get; set; }
          public string description { get; set; }
            public string fileToBytes { get; set; }
    }
}