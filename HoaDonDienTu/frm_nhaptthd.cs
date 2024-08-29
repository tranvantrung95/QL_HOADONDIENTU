using DevExpress.Utils.Extensions;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraLayout.Converter;
using DevExpress.XtraPrinting;
using HoaDonDienTu.Class;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HoaDonDienTu
{
    public partial class frm_nhaptthd : DevExpress.XtraEditors.XtraForm
    {
        public DataGridView dataGridView = new DataGridView();
        public Form frm_new = new Form();
        public string ma = "", ten = "";
        public string ma_kh_tam = "", ma_thb_tam = "",ma_kh_upd="",ma_thb_up="", chung_tu_cu="",ngay_thc_cu=""; 
        public decimal so_luong=0, don_gia=0, ps_no1 = 0,vat_tong=0;
        Dictionary<Tuple<int, string>, object> cellTags = new Dictionary<Tuple<int, string>, object>(); //Tạo tag cho ô lưới
        public frm_nhaptthd()
        {
            InitializeComponent();
            Class.Functions.Connect();


        }

        private void frm_nhaptthd_Load(object sender, EventArgs e)
        {
            // Gọi hàm để cấu hình GridControl và GridView
            SetupGridColumnsAndRows(gridView1);
            txt_ngay_thc.EditValue = DateTime.Now;

            // Thiết lập màu văn bản của TextEdit
            txt_makh.Properties.Appearance.ForeColor = Color.Blue;
            txt_tenkh.Properties.Appearance.ForeColor = Color.Blue;
            txt_httt.Properties.Appearance.ForeColor = Color.Blue;

            gridView1.RowCellStyle += gridView_RowCellStyle; //Tạo màu chữ cho cột 

            gridView1.CustomDrawColumnHeader += gridView1_CustomDrawColumnHeader; //Tạo màu header cột

            //gridView1.ShownEditor += new EventHandler(gridView1_ShownEditor);


        }

        //Sự kiện thay đổi ký tự sẽ thực hiện
        private void txt_makh_Validating(object sender, CancelEventArgs e)
        {
            //string ma_kh = "";
            //ma_kh = txt_makh.Text;
            //if (!string.IsNullOrWhiteSpace(ma_kh))
            //{
            //    string sql = "select ma,ten,dia_chi,ma_gtgt,ma_kh from _khang where ma like N'%" + ma_kh + "%'";
            //    GetValue_khang("Danh mục khách hàng", sql, txt_makh, txt_tenkh, txt_diachi, txt_msthue);
            //}

        }

        //Sự kiện enter ký tự sẽ thực hiện
        private void txt_makh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ma_kh = "";
                ma_kh = txt_makh.Text;
                string sql = "select ma,ten,dia_chi,ma_gtgt,ma_kh from _khang where ma like N'%" + ma_kh + "%'";
                GetValue_khang("Danh mục khách hàng", sql, txt_makh, txt_tenkh, txt_diachi, txt_msthue, txt_khthue);
            }
            if (e.KeyCode == Keys.F5)
            {
                string ma_kh = "";
                ma_kh = txt_makh.Text;
                string sql = "select ma,ten,dia_chi,ma_gtgt,ma_kh from _khang where ma like N'%" + ma_kh + "%'";
                GetValue_khang("Danh mục khách hàng", sql, txt_makh, txt_tenkh, txt_diachi, txt_msthue,txt_khthue);
            }
        }

        private void txt_tenkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ten_kh = "";
                ten_kh = txt_tenkh.Text;
                string sql = "select ma,ten,dia_chi,ma_gtgt,ma_kh from _khang where ma like N'%" + ten_kh + "%'";
                GetValue_khang("Danh mục khách hàng", sql, txt_makh, txt_tenkh, txt_diachi, txt_msthue, txt_khthue);
            }
            if (e.KeyCode == Keys.F5)
            {
                string ten_kh = "";
                ten_kh = txt_tenkh.Text;
                string sql = "select ma,ten,dia_chi,ma_gtgt,ma_kh from _khang where ma like N'%" + ten_kh + "%'";
                GetValue_khang("Danh mục khách hàng", sql, txt_makh, txt_tenkh, txt_diachi, txt_msthue, txt_khthue);
            }
        }

        private void txt_tenkh_Leave(object sender, EventArgs e)
        {
            //string ten_kh = "";
            //ten_kh = txt_tenkh.Text;
            //if (!string.IsNullOrWhiteSpace(ten_kh))
            //{
            //    string sql = "select * from _khang where ma like N'%" + ten_kh + "%'";
            //    GetValue("Danh mục khách hàng", sql, txt_makh, txt_tenkh);
            //}
        }

        public void GetValue(string tenform, string sql, DevExpress.XtraEditors.TextEdit ma, DevExpress.XtraEditors.TextEdit ten)
        {
            Form1 f = new Form1();
            f.Text = tenform;

            f.sql = sql;
            f.cot_mangam = "ma_kh";

            f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
            {
                ma.Text = f.ma;
                ten.Text = f.ten;
            };

            f.Show();
        }

        public void GetValue_khang(string tenform, string sql, DevExpress.XtraEditors.TextEdit ma, DevExpress.XtraEditors.TextEdit ten, DevExpress.XtraEditors.TextEdit dia_chi, DevExpress.XtraEditors.TextEdit ma_gtgt, DevExpress.XtraEditors.TextEdit khthue)
        {
            Form1 f = new Form1();
            f.Text = tenform;

            f.sql = sql;
            f.cot_mangam = "ma_kh";
            f.cot_ma = "ma";
            f.cot_ten = "ten";

            f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
            {
                ma.Text = f.ma;
                ten.Text = f.ten;
                dia_chi.Text = f.dia_chi;
                ma_gtgt.Text = f.ma_gtgt;
                khthue.Text = f.ten;
                if (f.mangam > 0)
                {
                    ma_kh_tam = f.mangam.ToString();
                    ma.Tag = ma_kh_tam;
                    ten.Tag = ma_kh_tam;
                }
                //MessageBox.Show(f.dia_chi + " - "+ f.ma_gtgt);
            };
            f.Show();
        }

        private void txt_httt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Form1 f = new Form1();
                f.Text = "Danh mục hình thức thanh toán";

                string httt = "";
                httt = txt_httt.Text;

                f.sql = "SELECT ten, ViettelId,f_identity FROM _HTTT WHERE f_identity<>3 and ViettelId like '%" + httt + "%'";

                f.cot_mangam = "f_identity";
                f.cot_ma = "ViettelId";
                f.cot_ten = "ten";

                f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                {
                    txt_httt.Text = f.httt;
                    //MessageBox.Show(f.dia_chi + " - "+ f.ma_gtgt);
                };
                f.Show();
            }

            if (e.KeyCode == Keys.F5)
            {
                
                Form1 f = new Form1();
                f.Text = "Danh mục hình thức thanh toán";

                string httt = "";
                httt = txt_httt.Text;

                f.sql = "SELECT ten, ViettelId,f_identity FROM _HTTT WHERE f_identity<>3";

                f.cot_mangam = "f_identity";
                f.cot_ma = "ViettelId";
                f.cot_ten = "ten";

                f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                {
                    txt_httt.Text = f.httt;
                    //MessageBox.Show(f.dia_chi + " - "+ f.ma_gtgt);
                };
                f.Show();
            }
        }

        private void SetupGridColumnsAndRows(GridView gridView)
        {
            // Tạo cột cho GridView
            gridView.Columns.Clear(); // Xóa tất cả các cột trước khi thêm mới
            gridView.Columns.AddVisible("ma_thb", "Mã Vlsphh");
            gridView.Columns.AddVisible("ten", "Tên Vlsphh");
            gridView.Columns.AddVisible("gch_detail", "Diễn giải chi tiết");
            gridView.Columns.AddVisible("so_luong", "Số lượng");
            gridView.Columns.AddVisible("don_gia", "Đơn giá");
            gridView.Columns.AddVisible("ps_no1", "Thành tiền");
            gridView.Columns.AddVisible("pt_ck", "% CK");
            gridView.Columns.AddVisible("tien_ck", "Tiền CK Thẳng");
            gridView.Columns.AddVisible("vat", "Thuế VAT");

            // Thêm cột ẩn vào GridView
            DevExpress.XtraGrid.Columns.GridColumn ma_ngam = new DevExpress.XtraGrid.Columns.GridColumn();
            ma_ngam.FieldName = "ma_ngam";
            ma_ngam.Visible = false; // Làm cho cột này không hiển thị
            gridView.Columns.Add(ma_ngam);

            gridView = gridControl1.MainView as GridView;

     
            //Định dạng số
            RepositoryItemTextEdit numericEdit = new RepositoryItemTextEdit();
            numericEdit.Mask.EditMask = "n0";
            numericEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            numericEdit.Mask.UseMaskAsDisplayFormat = true;
            numericEdit.CustomDisplayText += NumericEdit_CustomDisplayText;

            void NumericEdit_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
            {
                if (e.Value != null && e.Value != DBNull.Value && int.TryParse(e.Value.ToString(), out int value))
                {
                    e.DisplayText = value.ToString("#,##0").Replace(",", " ");
                }
            }



            // Tạo cột
            GridColumn col_ma_thb = gridView.Columns["ma_thb"];
            GridColumn col_ten = gridView.Columns["ten"];
            GridColumn col_so_luong = gridView.Columns["so_luong"];
            GridColumn col_don_gia = gridView.Columns["don_gia"];
            GridColumn col_ps_no1 = gridView.Columns["ps_no1"];
            GridColumn col_pt_ck = gridView.Columns["pt_ck"];
            GridColumn col_tien_ck = gridView.Columns["tien_ck"];
            GridColumn col_vat = gridView.Columns["vat"];



            //Gán cột định dạng số
            col_so_luong.ColumnEdit = numericEdit;
            col_don_gia.ColumnEdit = numericEdit;
            col_ps_no1.ColumnEdit = numericEdit;
            col_pt_ck.ColumnEdit = numericEdit;
            col_tien_ck.ColumnEdit = numericEdit;
            col_vat.ColumnEdit = numericEdit;


            //Cho phép ô pt_ck chỉ nhập từ 0-100
            void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
            {
                if (gridView.FocusedColumn.FieldName == "pt_ck")
                {
                    int value=0;
                    if (e.Value != DBNull.Value && e.Value != null)
                    {
                        if (int.TryParse(e.Value.ToString(), out value))
                        {
                            if (value < 0 || value > 100)
                            {
                                e.Valid = false;
                                e.ErrorText = "Phần trăm chiết khấu phải nằm trong khoảng từ 0 đến 100.";
                            }
                        }
                    }
                }

                ////Kiểm tra ps_no1 dưới lưới có trống không, có thì báo
                //if (gridView.FocusedColumn.FieldName == "ps_no1")
                //{
                //    // Kiểm tra xem giá trị có null hoặc trống không
                //    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                //    {
                //        e.Valid = false;
                //        e.ErrorText = "Tiền không được trống";
                //    }
                //    else
                //    {
                //        int value;
                //        // Sau đó kiểm tra xem giá trị có thể chuyển đổi thành số không
                //        if (!int.TryParse(e.Value.ToString(), out value))
                //        {
                //            e.Valid = false;
                //            e.ErrorText = "Giá trị phải là một số nguyên";
                //        }
                //    }
                //}
            }

            // Đăng ký sự kiện ValidatingEditor
            gridView1.ValidatingEditor += gridView1_ValidatingEditor;



            //Đặt tên cột chính giữa
            foreach (GridColumn column in gridView.Columns)
            {
                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }


            // Đặt chế độ thêm mới ở dòng cuối cùng
            gridView.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;

            // Tạo DataTable tạm để bind vào GridView
            DataTable tempDataTable = new DataTable();
            foreach (GridColumn column in gridView.Columns)
            {
                tempDataTable.Columns.Add(column.FieldName, column.ColumnType);
            }

            // Thêm 100 dòng trống vào DataTable
            for (int i = 0; i < 100; i++)
            {
                DataRow newRow = tempDataTable.NewRow();
                tempDataTable.Rows.Add(newRow);
            }

            // Bind DataTable vào GridView
            gridView.GridControl.DataSource = tempDataTable;
        }

        //Hàm thêm số thứ tự trong gridview
        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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


        //Xử lý khi chọn lại thuế suất thì tính lại
        private void txt_thuesuat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị thueSuat từ txt_thuesuat, giả sử txt_thuesuat là một ComboBox hoặc một loại DropDownList
            string thueSuatText = (txt_thuesuat.SelectedItem.ToString()!=null) ? txt_thuesuat.SelectedItem.ToString(): null;
            decimal thueSuat = 0;

            // Chuyển đổi thueSuatText sang số thực (decimal)
            switch (thueSuatText.Trim())
            {
                case "X":
                    thueSuat = 0;
                    break;
                case "0":
                    thueSuat = 0;
                    break;
                case "1":
                    thueSuat = 1;
                    break;
                case "5":
                    thueSuat = 5;
                    break;
                case "8":
                    thueSuat = 8;
                    break;
                case "10":
                    thueSuat = 10;
                    break;
                default:
                    // Xử lý nếu giá trị nhập không khớp với các trường hợp trên
                    break;
            }

            decimal soLuong, donGia;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                object cellValue_so_luong = gridView1.GetRowCellValue(i, "so_luong");
                object cellValue_don_gia = gridView1.GetRowCellValue(i, "don_gia");

                // Kiểm tra và chuyển đổi so_luong
                soLuong = (cellValue_so_luong != null && cellValue_so_luong != DBNull.Value) ?
                            Convert.ToDecimal(cellValue_so_luong) : 0;

                // Kiểm tra và chuyển đổi don_gia
                donGia = (cellValue_don_gia != null && cellValue_don_gia != DBNull.Value) ?
                            Convert.ToDecimal(cellValue_don_gia) : 0;

                // Chỉ tính và cập nhật vat nếu cả so_luong và don_gia đều có giá trị hợp lệ
                if (soLuong > 0 && donGia > 0)
                {
                    decimal vat = Math.Round((soLuong * donGia) * (thueSuat / 100), 0); // Tính toán giá trị mới
                    gridView1.SetRowCellValue(i, "vat", vat);
                }
                else
                {
                    gridView1.SetRowCellValue(i, "vat", DBNull.Value); // Đặt giá trị rỗng hoặc null
                }
            }

            // Cập nhật lại tổng giá trị cho Label
            UpdateTotalLabel();
        }


        //Cập nhật lại lable
        private void UpdateTotalLabel()
        {
            decimal tong_ps_no1 = 0;
            decimal tong_vat = 0;
            decimal tong_tien_ck = 0;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                object psNo1Value = gridView1.GetRowCellValue(i, "ps_no1");
                object vatValue = gridView1.GetRowCellValue(i, "vat");
                object tienckValue = gridView1.GetRowCellValue(i, "tien_ck");

                if (psNo1Value != null && psNo1Value != DBNull.Value)
                {
                    tong_ps_no1 += Convert.ToDecimal(psNo1Value);
                }

                if (vatValue != null && vatValue != DBNull.Value)
                {
                    tong_vat += Convert.ToDecimal(vatValue);
                }

                if (tienckValue != null && tienckValue != DBNull.Value)
                {
                    tong_tien_ck += Convert.ToDecimal(tienckValue);
                }
            }

            decimal tien_sau_thue = tong_ps_no1 + tong_vat - tong_tien_ck;
            vat_tong = tong_vat;

            // Tạo một NumberFormatInfo mới với cài đặt mong muốn
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = "."; // Dấu chấm cho phân cách hàng nghìn
            nfi.NumberDecimalSeparator = ","; // Dấu phẩy cho phân cách thập phân (nếu cần)
            nfi.NumberDecimalDigits = 0; // Không hiển thị phần thập phân

            lb_hientong.Text = $"Tổng PS No1: {tong_ps_no1.ToString("N",nfi)}, Tổng VAT: {tong_vat.ToString("N", nfi)}, Tổng CK: {tong_tien_ck.ToString("N", nfi)}, Tổng Cộng: {tien_sau_thue.ToString("N", nfi)}";
            lb_hientong.Font = new Font(lb_hientong.Font, FontStyle.Bold); // Đặt chữ in đậm
            lb_hientong.ForeColor = Color.Red; // Đặt màu chữ là đỏ
            lb_hientong.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near; //Hiện từ bên phải

        }

        //Xử lý STT
        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());


            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            ResetValue();
            txt_ngay_thc.EditValue = DateTime.Today;
            txt_phi_tm.EditValue = DateTime.Today;
        }

        private void ResetValue()
        {
            txt_makh.Text = "";
            txt_tenkh.Text = "";
            txt_ngay_thc.EditValue = DateTime.Now;
            txt_diachi.Text = "";
            txt_thuesuat.SelectedText = string.Empty;
            txt_ongba.Text = "";
            txt_ghichu.Text = "";
            txt_khthue.Text = "";
            txt_msthue.Text = "";
            txt_mau_hd.Text = "";
            txt_serihd.Text = "";
            txt_sohd.Text = "";

            //Tính và điền số chứng từ
            string sql = @"DECLARE @MaxChungTu NVARCHAR(100);
DECLARE @CurrentYear INT = YEAR(GETDATE());
DECLARE @CurrentMonth INT = MONTH(GETDATE());
DECLARE @NewNumberPart INT;
DECLARE @DefaultValue NVARCHAR(100) = 'HD' + RIGHT('0'+CAST(@CurrentMonth AS NVARCHAR),2) + '/' + CAST(@CurrentYear AS NVARCHAR) + '-0001';  -- Giá trị mặc định nếu không có bản ghi nào

-- Lấy giá trị lớn nhất của chung_tu cho năm hiện tại
SELECT @MaxChungTu = MAX(chung_tu)
FROM _thu_tien
WHERE YEAR(ngay_thc) = @CurrentYear;

-- Kiểm tra và xử lý giá trị
IF CHARINDEX('-', @MaxChungTu) > 0
BEGIN
    DECLARE @LastNumberPart NVARCHAR(100);
    SET @LastNumberPart = RIGHT(@MaxChungTu, CHARINDEX('-', REVERSE(@MaxChungTu)) - 1);
    IF LEN(@LastNumberPart) > 0 AND ISNUMERIC(@LastNumberPart) = 1
    BEGIN
        SET @NewNumberPart = CAST(@LastNumberPart AS INT) + 1;
        
        -- Tạo giá trị chung_tu mới với số thứ tự mới
        SET @MaxChungTu = 'HD' + RIGHT('0'+CAST(@CurrentMonth AS NVARCHAR),2) + '/' + CAST(@CurrentYear AS NVARCHAR) + '-' + RIGHT('0000' + CAST(@NewNumberPart AS NVARCHAR), 4);
    END
    ELSE
    BEGIN
        SET @MaxChungTu = @DefaultValue;  -- Fallback to default value in case of unexpected format
    END
END
ELSE
BEGIN
    SET @MaxChungTu = @DefaultValue;  -- Use default value if '-' not found
END

-- @MaxChungTu giờ chứa giá trị bạn cần
SELECT @MaxChungTu AS chung_tu
";

            DataTable dt_chungtu = Class.Functions.GetDataToTable(sql);
            string chung_tu = "";

            if(dt_chungtu.Rows.Count>0)
            {
                chung_tu = dt_chungtu.Rows[0]["chung_tu"].ToString();
            }
            txt_chung_tu.Text = chung_tu;

            txt_mau_hd.Text = "1/018";
            txt_serihd.Text = "C24TMO";
            txt_dv_do.Text = "VND";
            txt_ty_gia.Text = "1";
            txt_ma_tk0.Text = "1311";
            txt_ma_tk1.Text = "51111";

            // Xóa dữ liệu trong GridView
            if (gridView1.DataSource != null)
            {
                gridControl1.DataSource = null;
                SetupGridColumnsAndRows(gridView1);//Load lại grdiview
                
            }
        }

        private void btn_bo_qua_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cap0");
            dt.Columns.Add("ma");
            dt.Columns.Add("ten");
            dt.Columns.Add("image");

            // Thêm kiểm tra nếu btn_timkiem thực sự là một nút và nó được nhấn (nếu cần)
            if (btn_timkiem != null)
            {
                dt.Rows.Add(0, "01", "&1. Tại ngày " + " " + txt_ngay_thc.Text, "cham.bmp");
                dt.Rows.Add(0, "02", "&2. Đến ngày " + " " + txt_ngay_thc.Text, "cham.bmp");
                dt.Rows.Add(0, "03", "&3. Từ ngày... đến ngày...", "cham.bmp");
                dt.Rows.Add(0, "04", "&4. Tất cả các ngày", "cham.bmp");
            }

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Font = new Font("Arial", 10);

            foreach (DataRow row in dt.Rows)
            {
                string ma = row["ma"].ToString();
                string ten = row["ten"].ToString();
                string imagePath = row["image"].ToString(); // Lấy đường dẫn hình ảnh từ DataTable

                // Tạo một ToolStripMenuItem mới
                ToolStripMenuItem menuItem = new ToolStripMenuItem(ten);
                menuItem.Tag = ma; // Lưu trữ mã trong thuộc tính Tag để dễ dàng truy cập sau này
                menuItem.Name = ma;

                //// Đặt hình ảnh cho mục menu (nếu có)
                //if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                //{
                //    menuItem.Image = Image.FromFile(imagePath);
                //}

                // Thêm mục menu vào ContextMenuStrip
                menu.Items.Add(menuItem);
            }
            // Thêm sự kiện khi mục menu được nhấp vào
            menu.ItemClicked += new ToolStripItemClickedEventHandler(menu_ItemClicked);

            // Hiển thị menu tại vị trí chuột hiện tại so với btn_timkiem
            menu.Show(btn_timkiem, btn_timkiem.PointToClient(MousePosition));
            

        }

        public string ngay1 = "", ngay2 = "", tablename = "_thu_tien";


        private void menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Đóng menu trước khi xử lý sự kiện
            ((ContextMenuStrip)sender).Hide();

            ToolStripMenuItem clickedItem = e.ClickedItem as ToolStripMenuItem;
            DateTime dt = DateTime.ParseExact(txt_ngay_thc.Text.ToString(), "dd/MM/yyyy", null);
            string ab = dt.ToString("MM/dd/yyyy");

            // Kiểm tra xem mục menu có hợp lệ không

            switch (e.ClickedItem.Name)
            {
                case "01":
                    {
                        searchfilldata("select * from " + tablename + "  with (NOLOCK) where ngay_thc = '" + ab + "' order by ngay_thc,chung_tu,f_identity");

                        break;
                    }
                case "02":
                    {
                        searchfilldata("select * from " + tablename + "  with (NOLOCK) where ngay_thc <= '" + ab + "' order by ngay_thc,chung_tu,f_identity");
                        break;
                    }
                case "03":
                    {
                        frm_searchTime frm = new frm_searchTime();
                        frm.Text = "Chọn thời gian";
                        string tu_ngay = "", den_ngay = "";

                            frm.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                            {
                                tu_ngay = frm.tu_ngay.ToString();
                                den_ngay = frm.den_ngay.ToString();

                                if (!string.IsNullOrWhiteSpace(tu_ngay) && !string.IsNullOrWhiteSpace(den_ngay))
                                {
                                    searchfilldata("select * from " + tablename + "  with (NOLOCK) where ngay_thc >= '" + tu_ngay + "' and ngay_thc <='" + den_ngay + "' order by ngay_thc,chung_tu,f_identity");
                                    
                                }
                                else
                                {
                                    XtraMessageBox.Show("Không tìm thấy dữ liệu");
                                }
                            };

                            frm.Show();   
                        break;
                    }
                case "04":
                    {
                        searchfilldata("select * from " + tablename + " order by ngay_thc,chung_tu,f_identity");
                        break;
                    }
            }
        }

        private void searchfilldata(string sql)
        {
            frm_searchdata f = new frm_searchdata();
            f.Text = "Dữ liệu tìm thấy";

            string httt = "";
            httt = txt_httt.Text;

            //f.sql = "select * from " + tablename + "  with (NOLOCK) where ngay_thc = '" + ngay_thc0.Trim() + "' order by ngay_thc,chung_tu,f_identity";
            f.sql = sql;

            f.cot_mangam = "f_identity";
            DataTable dt_kiemtra = Class.Functions.GetDataToTable(f.sql);
            if (dt_kiemtra.Rows.Count == 0)
            {
                XtraMessageBox.Show("Không tìm thấy dữ liệu", "Thông bá", MessageBoxButtons.OK); return;
            }
            else
            {
                f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                {
                    DateTime ngay_thcDate, phi_tmDate;
                    if (DateTime.TryParse(f.ngay_thc, out ngay_thcDate))
                    {
                        // Nếu chuyển đổi thành công, sử dụng ngày đã chuyển đổi.
                        txt_ngay_thc.EditValue = ngay_thcDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        // Nếu chuyển đổi không thành công, sử dụng ngày hiện tại.
                        //txt_ngay_thc.EditValue = DateTime.Now.ToString("MM/dd/yyyy");
                        txt_ngay_thc.EditValue = DBNull.Value;
                    }

                    if (DateTime.TryParse(f.ngay_thc, out phi_tmDate))
                    {
                        // Nếu chuyển đổi thành công, sử dụng ngày đã chuyển đổi.
                        txt_phi_tm.EditValue = phi_tmDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        // Nếu chuyển đổi không thành công, sử dụng ngày hiện tại.
                        txt_phi_tm.EditValue = DBNull.Value;
                    }


                    txt_thuesuat.Text = f.thuesuat;
                    txt_mau_hd.Text = f.mau_hd;
                    txt_serihd.Text = f.serihd;
                    txt_sohd.Text = f.sohd;
                    txt_chung_tu.Text = f.chung_tu;
                    txt_ongba.Text = f.ongba;
                    txt_diachi.Text = f.dcthue;
                    txt_dv_do.Text = f.dv_do;
                    txt_ty_gia.Text = f.ty_gia;
                    txt_ma_tk0.Text = f.ma_tk0;
                    txt_ma_tk1.Text = f.ma_tk1;
                    txt_httt.Text = f.httt;
                    txt_khthue.Text = f.khthue;
                    txt_msthue.Text = f.msthue;

                    //Xử lý điền vào ô ma và tên khách hàng
                    string ma_kh = f.ma_kh.Trim();
                    ma_kh_upd = f.ma_kh.Trim(); //Gán vào để dùng khi nhấn nút lưu update
                    ma_thb_up = f.ma_thb.Trim();
                    chung_tu_cu = f.chung_tu;
                    ngay_thc_cu = f.ngay_thc;
                    txt_makh.Tag = f.ma_kh;
                    txt_tenkh.Tag = f.ma_kh;
                    if (!string.IsNullOrWhiteSpace(ma_kh))
                    {
                        DataTable dt_khang = Class.Functions.GetDataToTable("select ma,ten from _khang where ma_kh=" + ma_kh + "");
                        if (dt_khang.Rows.Count > 0)
                        {
                            txt_makh.Text = dt_khang.Rows[0]["ma"].ToString();
                            txt_tenkh.Text = dt_khang.Rows[0]["ten"].ToString();
                        }
                    }

                    //Điền thông tin xuống lưới
                    if (!string.IsNullOrWhiteSpace(f.chung_tu) && !string.IsNullOrWhiteSpace(f.ngay_thc))
                    {
                        DataTable dt2 = Class.Functions.GetDataToTable("Select *, _THIETBI.MA as ma_thietbi,_THIETBI.ten as ten_thietbi from _thu_tien INNER JOIN _THIETBI ON _thu_tien.MA_THB = _thietbi.MA_THB where chung_tu='" + f.chung_tu + "' and ngay_thc='" + Convert.ToDateTime(f.ngay_thc).ToString("MM/dd/yyyy") + "' AND MA_TK0 LIKE '131%' AND MA_TK1 LIKE '511%'");

                        if (dt2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                // Đảm bảo rằng gridView1 có ít nhất đủ số dòng để cập nhật
                                if (i < gridView1.RowCount)
                                {
                                    // Cập nhật giá trị của cột 'so_luong' tại dòng thứ i của GridView
                                    gridView1.SetRowCellValue(i, "ma_thb", dt2.Rows[i]["ma_thietbi"]);
                                    gridView1.SetRowCellValue(i, "ten", dt2.Rows[i]["ten_thietbi"]);
                                    gridView1.SetRowCellValue(i, "gch_detail", dt2.Rows[i]["gch_detail"]);
                                    gridView1.SetRowCellValue(i, "so_luong", dt2.Rows[i]["so_luong"]);
                                    gridView1.SetRowCellValue(i, "don_gia", dt2.Rows[i]["don_gia"]);
                                    gridView1.SetRowCellValue(i, "ps_no1", dt2.Rows[i]["ps_no1"]);
                                    gridView1.SetRowCellValue(i, "pt_ck", dt2.Rows[i]["pt_ck"]);
                                    gridView1.SetRowCellValue(i, "tien_ck", dt2.Rows[i]["tien_ck"]);
                                    gridView1.SetRowCellValue(i, "ma_ngam", dt2.Rows[i]["ma_thb"]);

                                }
                            }
                        }
                    }
                    else
                    {
                        ResetValue();
                    }

                };
                f.Show();
            }
        }

        private void searchfilldata(string tablename, string ngay_thc0,string ngay_thc1)
        {
            frm_searchdata f = new frm_searchdata();
            f.Text = "Dữ liệu tìm thấy";

            string httt = "";
            httt = txt_httt.Text;

            f.sql = "select * from " + tablename + "  with (NOLOCK) where ngay_thc >= '" + ngay_thc0 + "' and ngay_thc <='"+ ngay_thc1 +"' order by ngay_thc,chung_tu,f_identity";

            f.cot_mangam = "f_identity";
            DataTable dt_kiemtra = Class.Functions.GetDataToTable(f.sql);
            if (dt_kiemtra.Rows.Count == 0)
            {
                XtraMessageBox.Show("Không tìm thấy dữ liệu", "Thông bá", MessageBoxButtons.OK); return;
            }
            else
            {
                f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                {
                    DateTime ngay_thcDate, phi_tmDate;
                    if (DateTime.TryParse(f.ngay_thc, out ngay_thcDate))
                    {
                        // Nếu chuyển đổi thành công, sử dụng ngày đã chuyển đổi.
                        txt_ngay_thc.EditValue = ngay_thcDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        // Nếu chuyển đổi không thành công, sử dụng ngày hiện tại.
                        //txt_ngay_thc.EditValue = DateTime.Now.ToString("MM/dd/yyyy");
                        txt_ngay_thc.EditValue = DBNull.Value;
                    }

                    if (DateTime.TryParse(f.ngay_thc, out phi_tmDate))
                    {
                        // Nếu chuyển đổi thành công, sử dụng ngày đã chuyển đổi.
                        txt_phi_tm.EditValue = phi_tmDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        // Nếu chuyển đổi không thành công, sử dụng ngày hiện tại.
                        txt_phi_tm.EditValue = DBNull.Value;
                    }


                    txt_thuesuat.Text = f.thuesuat;
                    txt_mau_hd.Text = f.mau_hd;
                    txt_serihd.Text = f.serihd;
                    txt_sohd.Text = f.sohd;
                    txt_chung_tu.Text = f.chung_tu;
                    txt_ongba.Text = f.ongba;
                    txt_diachi.Text = f.dcthue;
                    txt_dv_do.Text = f.dv_do;
                    txt_ty_gia.Text = f.ty_gia;
                    txt_ma_tk0.Text = f.ma_tk0;
                    txt_ma_tk1.Text = f.ma_tk1;
                    txt_httt.Text = f.httt;
                    txt_khthue.Text = f.khthue;
                    txt_msthue.Text = f.msthue;

                    //Xử lý điền vào ô ma và tên khách hàng
                    string ma_kh = f.ma_kh.Trim();
                    ma_kh_upd = f.ma_kh.Trim(); //Gán vào để dùng khi nhấn nút lưu update
                    ma_thb_up = f.ma_thb.Trim();
                    chung_tu_cu = f.chung_tu;
                    ngay_thc_cu = f.ngay_thc;
                    txt_makh.Tag = f.ma_kh;
                    txt_tenkh.Tag = f.ma_kh;
                    if (!string.IsNullOrWhiteSpace(ma_kh))
                    {
                        DataTable dt_khang = Class.Functions.GetDataToTable("select ma,ten from _khang where ma_kh=" + ma_kh + "");
                        if (dt_khang.Rows.Count > 0)
                        {
                            txt_makh.Text = dt_khang.Rows[0]["ma"].ToString();
                            txt_tenkh.Text = dt_khang.Rows[0]["ten"].ToString();
                        }
                    }

                    //Điền thông tin xuống lưới
                    if (!string.IsNullOrWhiteSpace(f.chung_tu) && !string.IsNullOrWhiteSpace(f.ngay_thc))
                    {
                        DataTable dt2 = Class.Functions.GetDataToTable("Select *, _THIETBI.MA as ma_thietbi,_THIETBI.ten as ten_thietbi from _thu_tien INNER JOIN _THIETBI ON _thu_tien.MA_THB = _thietbi.MA_THB where chung_tu='" + f.chung_tu + "' and ngay_thc='" + Convert.ToDateTime(f.ngay_thc).ToString("MM/dd/yyyy") + "' AND MA_TK0 LIKE '131%' AND MA_TK1 LIKE '511%'");

                        if (dt2.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                // Đảm bảo rằng gridView1 có ít nhất đủ số dòng để cập nhật
                                if (i < gridView1.RowCount)
                                {
                                    // Cập nhật giá trị của cột 'so_luong' tại dòng thứ i của GridView
                                    gridView1.SetRowCellValue(i, "ma_thb", dt2.Rows[i]["ma_thietbi"]);
                                    gridView1.SetRowCellValue(i, "ten", dt2.Rows[i]["ten_thietbi"]);
                                    gridView1.SetRowCellValue(i, "gch_detail", dt2.Rows[i]["gch_detail"]);
                                    gridView1.SetRowCellValue(i, "so_luong", dt2.Rows[i]["so_luong"]);
                                    gridView1.SetRowCellValue(i, "don_gia", dt2.Rows[i]["don_gia"]);
                                    gridView1.SetRowCellValue(i, "ps_no1", dt2.Rows[i]["ps_no1"]);
                                    gridView1.SetRowCellValue(i, "pt_ck", dt2.Rows[i]["pt_ck"]);
                                    gridView1.SetRowCellValue(i, "tien_ck", dt2.Rows[i]["tien_ck"]);
                                    gridView1.SetRowCellValue(i, "ma_ngam", dt2.Rows[i]["ma_thb"]);

                                }
                            }
                        }
                    }
                    else
                    {
                        ResetValue();
                    }

                };
                f.Show();
            }
        }
        private void btn_luu_Click(object sender, EventArgs e)
        {
            ValidateTextEdit(txt_thuesuat,"Không được để trống");

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                var value = gridView1.GetRowCellValue(i, "ps_no1");

                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    // Thiết lập ErrorText
                    gridView1.SetColumnError(gridView1.Columns["ps_no1"], "Tiền không được trống", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                    gridView1.FocusedRowHandle = i; // Focus vào hàng có lỗi
                    gridView1.FocusedColumn = gridView1.Columns["ps_no1"]; // Focus vào cột có lỗi
                    gridView1.ShowEditor(); // Hiển thị editor để người dùng có thể sửa
                    break; // Thoát vòng lặp, bạn có thể loại bỏ break nếu muốn hiển thị lỗi ở tất cả các hàng
                }    
            }

            //Lấy giá trị và gán vào biến
            string ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, httt, ghi_chu, khthue, msthue, ma_thb = "0";
            double so_luong=0,don_gia=0,ps_no1=0,pt_ck = 0,tien_ck = 0,vat = 0;

            DateTime ngaythc,phitm;
            DateTime.TryParse(txt_ngay_thc.Text,out ngaythc);
            DateTime.TryParse(txt_phi_tm.Text,out phitm);
            ngay_thc = ngaythc.ToString("MM/dd/yyyy");
            phi_tm = phitm.ToString("MM/dd/yyyy");

            chung_tu = txt_chung_tu.Text;
            mau_hd = txt_mau_hd.Text;
            serihd = txt_serihd.Text;
            sohd = txt_sohd.Text;
            ktdb = txt_thuesuat.Text;
            ongba = txt_ongba.Text;
            dcthue = txt_diachi.Text;
            dv_do = txt_dv_do.Text;
            ty_gia = txt_ty_gia.Text;
            ma_tk0 = txt_ma_tk0.Text;
            ma_tk1 = txt_ma_tk1.Text;
            httt = txt_httt.Text;
            ghi_chu = txt_ghichu.Text;
            khthue = txt_khthue.Text;
            msthue = txt_msthue.Text;
            ma_kh = ma_kh_tam.ToString();
            


            string detailSql = "";
            List<string> sqlCommands = new List<string>(); // Danh sách để chứa các câu lệnh SQL cho thông tin chi tiết
            List<string> sqlCommands_upd = new List<string>(); // Danh sách để chứa các câu lệnh SQL cho thông tin chi tiết

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                // Kiểm tra xem dòng có dữ liệu không (ví dụ, kiểm tra cột 'MaVlSphh')
                if (gridView1.GetRowCellValue(i, "ps_no1") != null && gridView1.GetRowCellValue(i, "ps_no1").ToString().Trim() != "")
                {
                    // Lấy giá trị từ mỗi cột có dữ liệu trong lưới
                    object maNgamValue = gridView1.GetRowCellValue(i, "ma_ngam");
                    int mathb = maNgamValue != null && int.TryParse(maNgamValue.ToString(), out int ma) ? ma : 0;
                    ma_thb = mathb.ToString();

                    //ma_thb = gridView1.GetRowCellValue(i, "ma_ngam").ToString();
                    double.TryParse(gridView1.GetRowCellValue(i, "so_luong").ToString(), out so_luong);
                    double.TryParse(gridView1.GetRowCellValue(i, "don_gia").ToString(), out don_gia);
                    double.TryParse(gridView1.GetRowCellValue(i, "ps_no1").ToString(), out ps_no1);
                    double.TryParse(gridView1.GetRowCellValue(i, "pt_ck").ToString(), out pt_ck);
                    double.TryParse(gridView1.GetRowCellValue(i, "tien_ck").ToString(), out tien_ck);
                    double.TryParse(gridView1.GetRowCellValue(i, "vat").ToString(), out vat);
                    //so_luong = gridView1.GetRowCellValue(i, "so_luong").ToString();
                    
                    // don_gia = gridView1.GetRowCellValue(i, "don_gia").ToString();
                    // ps_no1 = gridView1.GetRowCellValue(i, "ps_no1").ToString();

                    //object pc_tkValue = gridView1.GetRowCellValue(i, "pt_ck");
                    //int ptck = maNgamValue != null && int.TryParse(pc_tkValue.ToString(), out int ptck_out) ? ptck_out : 0;
                    //pt_ck = ptck.ToString();

                    //object tien_ckValue = gridView1.GetRowCellValue(i, "tien_ck");
                    //int tienck = maNgamValue != null && int.TryParse(tien_ckValue.ToString(), out int tienck_out) ? tienck_out : 0;
                    //tien_ck = tienck.ToString();

                    //object vatValue = gridView1.GetRowCellValue(i, "vat");
                    //int vat1 = maNgamValue != null && int.TryParse(vatValue.ToString(), out int vatValue_out) ? vatValue_out : 0;
                    //vat = vat1.ToString();

                    // Tạo câu lệnh SQL cho mỗi dòng thông tin chi tiết có dữ liệu
                    detailSql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,so_luong,don_gia,ps_no1,pt_ck,tien_ck,vat) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh}', N'{dv_do}', '{ty_gia}', '{ma_tk0}', '{ma_tk1}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{so_luong},{don_gia},{ps_no1},{pt_ck},{tien_ck},{vat})";
                    sqlCommands.Add(detailSql);

                    //Tạo dòng lệnh sql cho chiết khấu
                    if(tien_ck>0)
                    {
                        string cksql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,ps_no1,pt_ck,tien_ck) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh}', N'{dv_do}', '{ty_gia}', '5213', '{ma_tk0}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{tien_ck},{pt_ck},{tien_ck})";
                        sqlCommands.Add(cksql);
                    }    
                    //Class.Functions.editcode(detailSql);
                }
            }
            
            //Tạo bút toán thuế
                detailSql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,ps_no1,vat) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh}', N'{dv_do}', '{ty_gia}', '3311', '{ma_tk0}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{vat_tong},{vat_tong})";
                sqlCommands.Add(detailSql);

            //Kiểm tra xem chứng từ đã tồn tại chưa
            string sql_kt = "IF EXISTS(SELECT 1 FROM _thu_tien WHERE chung_tu = '"+chung_tu+"')\r\nBEGIN\r\n    -- Chạy code khi chung_tu tồn tại\r\n    SELECT '1' AS Result;\r\nEND\r\nELSE\r\nBEGIN\r\n    -- Chạy code khi chung_tu không tồn tại\r\n    SELECT '0' AS Result;\r\nEND";
            object kq = Class.Functions.SQLEXEC(sql_kt, true);
            if(kq.ToString()=="0")
            {
                try
                {
                    if (Convert.ToDecimal(ps_no1.ToString())==0)
                    {
                        XtraMessageBox.Show("Có lỗi trong dữ liệu, vui lòng kiểm tra lại.","Thông báo",MessageBoxButtons.OK);
                        return;
                    }
                    if (XtraMessageBox.Show("Bạn có muốn lưu không", "Thông báo", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                    foreach (string sqlCommand in sqlCommands)
                    {
                        Class.Functions.RunSQL(sqlCommand);
                    }
                    XtraMessageBox.Show("Lưu thành công","Thông báo",MessageBoxButtons.OK);
                    ResetValue();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi lưu, vui lòng kiểm tra lại");
                }
            }
            else
            {
                if (XtraMessageBox.Show("Chứng từ đã tồn tại bạn có muốn cập nhật không?", "Thông báo", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                if (!string.IsNullOrWhiteSpace(ma_kh_upd))
                {
                    string sql_delete = "delete from _thu_tien where chung_tu=N'" + chung_tu_cu + "' and ngay_thc='" + Convert.ToDateTime(ngay_thc_cu).ToString("MM/dd/yyyy") + "'";
                    try
                    {
                        Class.Functions.SQLEXEC(sql_delete, true);
                        Class.Functions.editcode(sql_delete);
                    }
                    catch
                    {
                        XtraMessageBox.Show("Lỗi xóa!"); return;
                    }

                    //MessageBox.Show(txt_makh.Tag.ToString()+" - "+txt_tenkh.Tag.ToString());

                    string sqlktra = "IF EXISTS(SELECT 1 from _thu_tien where chung_tu=N'" + chung_tu + "' and ngay_thc='" + ngay_thc + "' and ma_kh= " + ma_kh_upd + ") BEGIN\r\n    -- Chạy code khi chung_tu tồn tại\r\n    SELECT '1' AS Result;\r\nEND\r\nELSE\r\nBEGIN\r\n    -- Chạy code khi chung_tu không tồn tại\r\n    SELECT '0' AS Result;\r\nEND";
                    object ktra = Class.Functions.SQLEXEC(sqlktra,true);
                    if (ktra.ToString()=="0")
                    {
                        string ma_kh_tag = txt_makh.Tag.ToString();
                        string ten_kh_tag = txt_tenkh.Tag.ToString();
                        //MessageBox.Show(txt_makh.Tag.ToString()+" - "+txt_tenkh.Tag.ToString());
                        for (int i = 0; i < gridView1.RowCount; i++)
                        {
                            // Kiểm tra xem dòng có dữ liệu không (ví dụ, kiểm tra cột 'MaVlSphh')
                            if (gridView1.GetRowCellValue(i, "ps_no1") != null && gridView1.GetRowCellValue(i, "ps_no1").ToString().Trim() != "")
                            {
                                // Lấy giá trị từ mỗi cột có dữ liệu trong lưới
                                object maNgamValue = gridView1.GetRowCellValue(i, "ma_ngam");
                                int mathb = maNgamValue != null && int.TryParse(maNgamValue.ToString(), out int ma) ? ma : 0;
                                ma_thb = mathb.ToString();

                                double soluong=0, dongia=0,psno1=0;
                                //so_luong = gridView1.GetRowCellValue(i, "so_luong");
                                double.TryParse(gridView1.GetRowCellValue(i, "so_luong").ToString(),out soluong);
                                string kq_so_luong = Math.Floor(soluong).ToString("N0");
                                //MessageBox.Show(kq_so_luong+" - "+soluong);


                                //don_gia = gridView1.GetRowCellValue(i, "don_gia").ToString();
                                double.TryParse(gridView1.GetRowCellValue(i, "don_gia").ToString().ToString(), out dongia);
                                string kq_don_gia = Math.Floor(dongia).ToString("N0");

                                //ps_no1 = gridView1.GetRowCellValue(i, "ps_no1").ToString();
                                double.TryParse(gridView1.GetRowCellValue(i, "ps_no1").ToString(), out psno1);
                                string kq_ps_no1 = Math.Floor(psno1).ToString("N0");



                                object pc_tkValue = gridView1.GetRowCellValue(i, "pt_ck");
                                int ptck = maNgamValue != null && int.TryParse(pc_tkValue.ToString(), out int ptck_out) ? ptck_out : 0;
                                //pt_ck = ptck.ToString();
                                double.TryParse(ptck.ToString(), out pt_ck);

                                object tien_ckValue = gridView1.GetRowCellValue(i, "tien_ck");
                                int tienck = maNgamValue != null && int.TryParse(tien_ckValue.ToString(), out int tienck_out) ? tienck_out : 0;
                                //tien_ck = tienck.ToString();
                                double.TryParse(tienck.ToString(), out tien_ck);

                                object vatValue = gridView1.GetRowCellValue(i, "vat");
                                int vat1 = maNgamValue != null && int.TryParse(vatValue.ToString(), out int vatValue_out) ? vatValue_out : 0;
                                //vat = vat1.ToString();
                                double.TryParse(vat1.ToString(), out vat);

                                // Tạo câu lệnh SQL cho mỗi dòng thông tin chi tiết có dữ liệu
                                detailSql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,so_luong,don_gia,ps_no1,pt_ck,tien_ck,vat) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh_tag}', N'{dv_do}', '{ty_gia}', '{ma_tk0}', '{ma_tk1}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{so_luong},{don_gia},{ps_no1},{pt_ck},{tien_ck},{vat})";
                                sqlCommands_upd.Add(detailSql);

                                //Tạo dòng lệnh sql cho chiết khấu
                                if (tienck > 0)
                                {
                                    string cksql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,ps_no1,pt_ck,tien_ck) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh_tag}', N'{dv_do}', '{ty_gia}', '5213', '{ma_tk0}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{tien_ck},{pt_ck},{tien_ck})";
                                    sqlCommands_upd.Add(cksql);
                                }
                                //Class.Functions.editcode(detailSql);
                            }
                        }

                        //Tạo bút toán thuế
                        detailSql = $"INSERT INTO _thu_tien (ngay_thc, ktdb, chung_tu, mau_hd, serihd, sohd, phi_tm, ongba, dcthue, ma_kh, dv_do, ty_gia, ma_tk0, ma_tk1, tt3, ghi_chu, khthue, msthue,ma_thb,ps_no1,vat) VALUES ('{ngay_thc}','{ktdb}', N'{chung_tu}', N'{mau_hd}', N'{serihd}', '{sohd}', '{phi_tm}',N'{ongba}', N'{dcthue}', '{ma_kh_tag}', N'{dv_do}', '{ty_gia}', '3311', '{ma_tk0}', N'{httt}', N'{ghi_chu}',N'{khthue}', N'{msthue}','{ma_thb}',{vat_tong},{vat_tong})";
                        sqlCommands_upd.Add(detailSql);

                        //Kiểm tra xem chứng từ đã tồn tại chưa
                        string sql_kt1 = "IF EXISTS(SELECT 1 FROM _thu_tien WHERE chung_tu = '" + chung_tu + "')\r\nBEGIN\r\n    -- Chạy code khi chung_tu tồn tại\r\n    SELECT '1' AS Result;\r\nEND\r\nELSE\r\nBEGIN\r\n    -- Chạy code khi chung_tu không tồn tại\r\n    SELECT '0' AS Result;\r\nEND";
                        Class.Functions.editcode(sql_kt1);
                        object kq1 = Class.Functions.SQLEXEC(sql_kt1, true);
                        if (kq1.ToString() == "0")
                        {
                            try
                            {
                                if (Convert.ToDecimal(ps_no1.ToString()) == 0)
                                {
                                    XtraMessageBox.Show("Có lỗi trong dữ liệu, vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK);
                                    return;
                                }
                                if (XtraMessageBox.Show("Bạn có muốn lưu không", "Thông báo", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                                foreach (string sqlCommand in sqlCommands_upd)
                                {
                                    Class.Functions.RunSQL(sqlCommand);
                                    Class.Functions.editcode(sqlCommand);
                                }
                                XtraMessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK);
                                ResetValue();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi lưu, vui lòng kiểm tra lại");
                            }
                        }
                        else
                        {
                            MessageBox.Show("đến đây rồi");
                        }    
                    }
                    else
                    {
                        MessageBox.Show("Lỗi");
                    }
                }
            }    

        }

        //Xử lý khi textbox để trống sẽ hiện thông báo
        private void ValidateTextEdit(DevExpress.XtraEditors.TextEdit textEdit, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(textEdit.Text))
            {
                textEdit.ErrorText = errorMessage; // Hiển thị thông báo lỗi
            }
            else
            {
                textEdit.ErrorText = ""; // Xóa thông báo lỗi nếu không có lỗi
            }
        }


        //Xử lý khi lăn chuột sẽ để griview thành dạng không sửa
        private void gridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((sender as GridView).IsEditing)
            {
                (sender as GridView).CloseEditor();
                (sender as GridView).UpdateCurrentRow();
            }
        }


        //Xử lý khi thay đổi giá trị
        bool isUpdatingCellValue = false;
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gridView1.ActiveEditor is DevExpress.XtraEditors.TextEdit)
            {
                DevExpress.XtraEditors.TextEdit activeEditor = gridView1.ActiveEditor as DevExpress.XtraEditors.TextEdit;
                if (activeEditor != null)
                {
                    if (isUpdatingCellValue) //Tạo flag để cho không hiển thị liên tục
                        return;
                    if (gridView1.FocusedColumn.FieldName == "ma_thb")
                    {
                        string value_ma_thb = activeEditor.Text;
                        string sql = "select ma, ten, ma_thb from _thietbi where ma like N'%" + value_ma_thb + "%'";
                        DataTable dt = Class.Functions.GetDataToTable(sql);

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy dữ liệu.");
                            isUpdatingCellValue = true;
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ma_thb", DBNull.Value);
                            isUpdatingCellValue = false;
                        }
                        else
                        {
                            Form1 f = new Form1();
                            f.Text = "Danh mục thiết bị";

                            f.sql = sql;
                            f.cot_mangam = "ma_thb";
                            f.cot_ma = "ma";
                            f.cot_ten = "ten";

                            f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ma_thb", f.ma);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ten", f.ten);
                                if (f.mangam > 0)
                                {
                                    ma_thb_tam = f.mangam.ToString();
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ma_ngam", ma_thb_tam);
                                }
                            };
                            f.Show();
                        }
                    }
                    if (gridView1.FocusedColumn.FieldName == "ten")
                    {
                        string value_ten_thb = activeEditor.Text;
                        string sql = "select ma, ten, ma_thb from _thietbi where ten like N'%" + value_ten_thb + "%'";
                        // Giả định rằng bạn có phương thức để lấy dữ liệu từ cơ sở dữ liệu
                        DataTable dt = Class.Functions.GetDataToTable(sql);

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy dữ liệu.");
                            isUpdatingCellValue = true;
                            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ten", DBNull.Value);
                            isUpdatingCellValue = false;
                        }
                        else
                        {
                            Form1 f = new Form1();
                            f.Text = "Danh mục thiết bị";

                            f.sql = sql;
                            f.cot_mangam = "ma_thb";
                            f.cot_ma = "ma";
                            f.cot_ten = "ten";

                            f.FormClosed += delegate (object sender2, FormClosedEventArgs e2)
                            {
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ma_thb", f.ma);
                                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ten", f.ten);
                                if (f.mangam > 0)
                                {
                                    ma_thb_tam = f.mangam.ToString();
                                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "ma_ngam",ma_thb_tam);
                                }
                            };
                            f.Show();
                        }
                    }
                }
            }

            //Xử lý tính cột tiền
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            // Kiểm tra xem ô thay đổi có phải là so_luong hoặc don_gia không
            if (e.Column.FieldName == "so_luong" || e.Column.FieldName == "don_gia")
            {
                // Lấy giá trị so_luong và don_gia từ dòng hiện tại
                decimal so_luong = 0;
                decimal don_gia = 0;
                int thuesuat = 0;
                
                decimal vat = 0;
                decimal tong_ps_no1 = 0;
                decimal tong_vat = 0;

                if (view.GetRowCellValue(e.RowHandle, "so_luong") != null)
                {
                    decimal.TryParse(view.GetRowCellValue(e.RowHandle, "so_luong").ToString(), out so_luong);
                }

                if (view.GetRowCellValue(e.RowHandle, "don_gia") != null)
                {
                    decimal.TryParse(view.GetRowCellValue(e.RowHandle, "don_gia").ToString(), out don_gia);
                }
                
                // Tính toán giá trị mới cho ps_no1
                decimal ps_no1 = so_luong * don_gia;

    

                if (!string.IsNullOrWhiteSpace(txt_thuesuat.Text))
                {
                    switch (txt_thuesuat.Text.Trim())
                    {
                        case "X":
                            thuesuat = 0;
                            break;
                        case "0":
                            thuesuat = 0;
                            break;
                        case "1":
                            thuesuat = 1;
                            break;
                        case "5":
                            thuesuat = 5;
                            break;
                        case "8":
                            thuesuat = 8;
                            break;
                        case "10":
                            thuesuat = 10;
                            break;
                        default:
                            // Xử lý nếu giá trị nhập không khớp với các trường hợp trên
                            break;
                    }
                }

                

                // Cập nhật giá trị ps_no1 trong GridView
                view.SetRowCellValue(e.RowHandle, "ps_no1", ps_no1);
                view.SetRowCellValue(e.RowHandle, "vat", Math.Round(((ps_no1 * thuesuat) / 100),0));

                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    object psNo1Value = gridView1.GetRowCellValue(i, "ps_no1");
                    object vatValue = gridView1.GetRowCellValue(i, "vat");

                    if (psNo1Value != null && psNo1Value != DBNull.Value)
                    {
                        tong_ps_no1 += Convert.ToDecimal(psNo1Value);
                    }

                    if (vatValue != null && vatValue != DBNull.Value)
                    {
                        tong_vat += Convert.ToDecimal(vatValue);
                    }
                }

                decimal tien_sau_thue = tong_ps_no1 + tong_vat;


                UpdateTotalLabel(); //Cập nhật lable hiển thị tổng tiền


            }

            if (isUpdatingCellValue)
                return;

            if (e.Column.FieldName == "pt_ck" || e.Column.FieldName == "tien_ck")
            {
                isUpdatingCellValue = true; // Set the flag to indicate programmatic change

                int pt_ck = 0;
                decimal ps_no1 = 0, tien_ck = 0;

                // Lấy giá trị của ps_no1
                if (view.GetRowCellValue(e.RowHandle, "ps_no1") != DBNull.Value)
                {
                    decimal.TryParse(view.GetRowCellValue(e.RowHandle, "ps_no1").ToString(), out ps_no1);
                }

                // Tính toán tien_ck dựa trên pt_ck hoặc cho phép người dùng nhập tien_ck
                if (e.Column.FieldName == "pt_ck")
                {
                    if (view.GetRowCellValue(e.RowHandle, "pt_ck") != DBNull.Value)
                    {
                        if (int.TryParse(view.GetRowCellValue(e.RowHandle, "pt_ck").ToString(), out pt_ck) && pt_ck >= 0 && pt_ck <= 100)
                        {
                            tien_ck = Math.Round((ps_no1 * pt_ck) / 100, 0);
                            view.SetRowCellValue(e.RowHandle, "tien_ck", tien_ck);
                        }
                    }
                }
                else if (e.Column.FieldName == "tien_ck")
                {
                    if (view.GetRowCellValue(e.RowHandle, "tien_ck") != DBNull.Value)
                    {
                        if (decimal.TryParse(view.GetRowCellValue(e.RowHandle, "tien_ck").ToString(), out tien_ck))
                        {
                            // Nếu tien_ck được nhập bởi người dùng, không cần tính toán lại dựa trên pt_ck
                        }
                    }
                }

                isUpdatingCellValue = false; // Reset the flag
                UpdateTotalLabel();
            }
        }

        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            // Kiểm tra xem ô hiện tại có thuộc cột "ma" hoặc "ten" không
            if (e.Column.FieldName == "ma_thb" || e.Column.FieldName == "ten")
            {
                // Thay đổi màu văn bản
                e.Appearance.ForeColor = Color.Blue; // Đặt màu bạn muốn

                // Bạn cũng có thể thay đổi màu nền nếu muốn
                // e.Appearance.BackColor = Color.LightGray;
            }
        }

        //Hàm xử lý màu header cột
        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                // Kiểm tra nếu cột là "Ma" hoặc "Ten" và thiết lập màu sắc
                if (e.Column.FieldName == "ma_thb" || e.Column.FieldName == "ten")
                {
                    //// Thiết lập màu nền cho tiêu đề cột
                    //e.Info.Appearance.BackColor = Color.Blue; // Màu nền
                    //e.Info.Appearance.ForeColor = Color.Black; // Màu chữ

                    //// Vẽ tiêu đề cột
                    //e.Painter.DrawObject(e.Info);

                    //// Ngăn GridView thực hiện vẽ mặc định
                    //e.Handled = true;

                    e.Column.AppearanceHeader.BackColor = Color.MediumSlateBlue;
                    e.Info.AllowColoring = true;

                }
            }
        }


    }
}