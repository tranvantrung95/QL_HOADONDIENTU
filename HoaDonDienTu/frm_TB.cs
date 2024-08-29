using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //Sử dụng thư viện để làm việc SQL server
using HoaDonDienTu.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo; //Sử dụng class Functions.cs
using System.Text.RegularExpressions;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.Mvvm.Native;
using DevExpress.XtraGrid;
using static DevExpress.Utils.Svg.CommonSvgImages;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraEditors.Mask.Design;

namespace HoaDonDienTu
{
    public partial class frm_TB : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_thietbi;
        string ma_thb = "";
        public frm_TB()
        {
            InitializeComponent();
        }

        private void frm_TB_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            loadview();
        }


        private void loadview()
        {
            string sql;
            sql = "SELECT ma_thb,ma,ten,dv_do,so_luong,gia_ban,gia_nhap FROM _thietbi WHERE ma<>''";
            dt_thietbi = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_thietbi;

            // Chọn cột cần enable hoặc disable
            GridColumn gridColumn = gridView1.Columns["ma_thb"]; // Thay "TenCot" bằng tên thực của cột

            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            gridColumn.OptionsColumn.AllowEdit = false;
            gridColumn.Width = 15;

            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

            List<String> columnHeaders = new List<String>();
            columnHeaders.Add("ID");
            columnHeaders.Add("Mã TB");
            columnHeaders.Add("Tên TB");
            columnHeaders.Add("ĐVT");
            columnHeaders.Add("Số lượng");
            columnHeaders.Add("Giá bán");
            columnHeaders.Add("Giá nhập");

            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                gridView1.Columns[i].Caption = columnHeaders[i]; // Use index i to access both grid columns and header names
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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (btn_them.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_matb.Focus();
                return;
            }
            if (dt_thietbi.Rows.Count == 0) //Nếu không có dữ liệu
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (gridView1 == null && e == null && e.FocusedRowHandle < 0)
            {
                return;
            }
            else
            {
                string ma = "", ten = "", dv_do = "", so_luong = "", gia_ban = "", gia_nhap = "";
                object value_ma = gridView1.GetRowCellValue(e.FocusedRowHandle, "ma");
                object value_ten = gridView1.GetRowCellValue(e.FocusedRowHandle, "ten");
                object value_dv_do = gridView1.GetRowCellValue(e.FocusedRowHandle, "dv_do");
                object value_so_luong = gridView1.GetRowCellValue(e.FocusedRowHandle, "so_luong");
                object value_gia_ban = gridView1.GetRowCellValue(e.FocusedRowHandle, "gia_ban");
                object value_gia_nhap = gridView1.GetRowCellValue(e.FocusedRowHandle, "gia_nhap");
                object value_ma_thb = gridView1.GetRowCellValue(e.FocusedRowHandle, "ma_thb");

                if (value_ma != null)
                {
                    ma = value_ma.ToString();
                }
                if (value_ten != null)
                {
                    ten = value_ten.ToString();
                }
                if (value_dv_do != null)
                {
                    dv_do = value_dv_do.ToString();
                }
                if (value_so_luong != null)
                {
                    so_luong = value_so_luong.ToString();
                }
                if (value_gia_ban != null)
                {
                    gia_ban = value_gia_ban.ToString();
                }
                if (value_gia_nhap != null)
                {
                    gia_nhap = value_gia_nhap.ToString();
                }
                if (value_ma_thb != null)
                {
                    ma_thb = value_ma_thb.ToString().Trim();
                }

                txt_matb.Text = ma;
                txt_tentb.Text = ten;
                txt_so_luong.Text = so_luong;
                txt_dv_do.Text = dv_do;
                txt_gia_nhap.Text = gia_nhap;
                txt_gia_ban.Text = gia_ban;
            }
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

        private void btn_them_Click(object sender, EventArgs e)
        {
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
            btn_boqua.Enabled = true;
            btn_luu.Enabled = true;
            btn_them.Enabled = false;
            ResetValue(); //Xoá trắng các textbox
            txt_matb.Enabled = true; //cho phép nhập mới
            txt_matb.Focus();
        }

        private void ResetValue()
        {
            txt_matb.Text = "";
            txt_tentb.Text = "";
            txt_so_luong.Text = "";
            txt_dv_do.Text = "";
            txt_gia_nhap.Text = "";
            txt_gia_ban.Text = "";
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            string sql; //Lưu lệnh sql
            if (txt_matb.Text.Trim().Length == 0) //Nếu chưa nhập mã thiết bị
            {
                MessageBox.Show("Bạn phải nhập mã thiết bị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_matb.Focus();
                return;
            }
            if (txt_tentb.Text.Trim().Length == 0) //Nếu chưa nhập tên thiết bị
            {
                MessageBox.Show("Bạn phải nhập tên thiết bị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tentb.Focus();
                return;
            }
            sql = "Select ma From _thietbi where ma=N'" + txt_matb.Text.Trim() + "'";
            if (Class.Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã thiết bị này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_matb.Focus();
                return;
            }

            if (MessageBox.Show("Bạn có muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DECLARE @ma_thb INT;\r\nSET @ma_thb = (SELECT ISNULL(MAX(ma_thb), 0) + 1 FROM _thietbi);\n";
                sql += "INSERT INTO _thietbi (ma_thb,ma,ten,dv_do,so_luong,gia_ban,gia_nhap) VALUES(@ma_thb,N'" +
                    txt_matb.Text + "'," +
                    "N'" + txt_tentb.Text + "'," +
                    "N'" + txt_dv_do.Text + "'," +
                    "'" + txt_so_luong.Text + "'," +
                    "'" + txt_gia_ban.Text + "'," +
                    "'" + txt_gia_nhap.Text + "')";
                //Class.Functions.editcode(sql); return;

                Class.Functions.RunSQL(sql); //Thực hiện câu lệnh sql
                loadview(); //Nạp lại DataGridView
                ResetValue();
                btn_xoa.Enabled = true;
                btn_them.Enabled = true;
                btn_sua.Enabled = true;
                btn_boqua.Enabled = false;
                btn_luu.Enabled = false;
                txt_matb.Enabled = false;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            object columnName = e.Column.FieldName;
            object cellValue = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            object primaryKeyValue = gridView1.GetRowCellValue(e.RowHandle, "ma_thb");
            //MessageBox.Show(columnName + " - "+primaryKeyValue + " - "+cellValue);

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "";
                sql = "UPDATE _thietbi SET " + columnName + " = '" + cellValue + "' WHERE ma_thb = " + primaryKeyValue + "";
                Class.Functions.RunSqlDel(sql);
            }
        }

        private void txt_so_luong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem phím được nhấn có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Từ chối ký tự nếu không phải là số
            }
        }

        private void txt_so_luong_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra độ dài của chuỗi và cắt ngắn nếu nó vượt quá 12 số
            if (txt_so_luong.Text.Length > 12)
            {
                txt_so_luong.Text = txt_so_luong.Text.Substring(0, 12);
            }
        }

        // Tạo một hàm kiểm tra email hợp lệ bằng biểu thức chính quy
        public static bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        private void txt_email_EditValueChanged(object sender, EventArgs e)
        {
            //// Lấy giá trị của textedit email
            //string email = txt_email.Text;

            //// Kiểm tra xem email có hợp lệ hay không
            //bool isValid = IsEmail(email);

            //// Nếu email không hợp lệ, hiển thị một thông báo lỗi
            //if (!isValid)
            //{
            //    // Tạo một đối tượng XtraMessageBoxArgs để tùy chỉnh thông báo lỗi
            //    XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            //    args.AutoCloseOptions.Delay = 3000; // Đóng thông báo sau 3 giây
            //    args.Caption = "Lỗi"; // Tiêu đề của thông báo
            //    args.Text = "Email không đúng định dạng. Vui lòng nhập lại."; // Nội dung của thông báo
            //    args.Buttons = new DialogResult[] { DialogResult.OK }; // Nút nhấn của thông báo
            //    args.Icon = System.Drawing.SystemIcons.Error; // Biểu tượng của thông báo
            //    args.DefaultButtonIndex = 0; // Chỉ số của nút mặc định

            //    // Hiển thị thông báo lỗi
            //    XtraMessageBox.Show(args);
            //}
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (dt_thietbi.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_matb.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_tentb.Text.Trim().Length == 0) //nếu chưa nhập tên thiết bị
            {
                MessageBox.Show("Bạn chưa nhập tên thiết bị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn cập nhật không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "UPDATE _thietbi SET ma= N'" +
                    txt_matb.Text + "', ten =" +
                    "N'" + txt_tentb.Text + "',so_luong =" +
                    "'" + txt_so_luong.Text + "',dv_do=" +
                    "N'" + txt_dv_do.Text + "',gia_ban=" +
                    "'" + txt_gia_ban.Text + "',gia_nhap=" +
                    "'" + txt_gia_nhap.Text + "' where ma_thb=" + ma_thb + "";
                //Class.Functions.editcode(sql); return;
                Class.Functions.RunSQL(sql);
                loadview();
                ResetValue();
                btn_boqua.Enabled = false;
            }
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (dt_thietbi.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_matb.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE _thietbi WHERE ma_thb=N'" + ma_thb + "'";
                Class.Functions.RunSqlDel(sql);
                loadview();
                ResetValue();
            }
        }

        private void btn_boqua_Click(object sender, EventArgs e)
        {
            ResetValue();
            btn_boqua.Enabled = false;
            btn_them.Enabled = true;
            btn_xoa.Enabled = true;
            btn_sua.Enabled = true;
            btn_luu.Enabled = false;
            txt_matb.Enabled = false;
        }

        private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["ma_thb"]; // Thay "TenCot" bằng tên thực của cột

            // Kiểm tra xem cột hiện tại có phải là cột cần bôi màu hay không
            if (e.Column == gridColumn)
            {
                // Thay đổi màu sắc tùy thuộc vào điều kiện nào đó
                if (gridColumn.OptionsColumn.AllowEdit == false)
                {
                    e.Appearance.BackColor = Color.DarkKhaki; // Màu nền là đỏ
                    e.Appearance.ForeColor = Color.White; // Màu chữ là trắng
                }
                else
                {
                    // Đặt màu về mặc định nếu không thỏa mãn điều kiện
                    e.Appearance.BackColor = Color.Empty;
                    e.Appearance.ForeColor = Color.Empty;
                }
            }
        }
    }
}