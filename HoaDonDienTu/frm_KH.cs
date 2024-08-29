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

namespace HoaDonDienTu
{
    public partial class frm_KH : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_khang;
        string ma_kh="";
        public frm_KH()
        {
            InitializeComponent();
        }

        private void frm_KH1_Load(object sender, EventArgs e)
        {

                Class.Functions.Connect();
                loadview();

        }

        private void loadview()
        {
            string sql;
            sql = "SELECT ma_kh, ma,ten,DIA_CHI,MA_GTGT,PHONE,TenNganHang,TKHOAN_NH,email FROM _KHANG WHERE ma<>'' ";
            dt_khang = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_khang;

            // Chọn cột cần enable hoặc disable
            GridColumn gridColumn = gridView1.Columns["ma_kh"]; // Thay "TenCot" bằng tên thực của cột

            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            gridColumn.OptionsColumn.AllowEdit = false;
            gridColumn.Width = 15;

            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

            List<String> columnHeaders = new List<String>();
            columnHeaders.Add("ID");
            columnHeaders.Add("Mã KH");
            columnHeaders.Add("Tên KH");
            columnHeaders.Add("Địa chỉ");
            columnHeaders.Add("Mã số thuế");
            columnHeaders.Add("Phone");
            columnHeaders.Add("Tên ngân hàng");
            columnHeaders.Add("Số TK");
            columnHeaders.Add("Email");

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
                    txt_makh.Focus();
                    return;
                }
                if (dt_khang.Rows.Count == 0) //Nếu không có dữ liệu
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
                    string ma = "", ten = "", dia_chi = "", phone = "", ma_gtgt = "", tennh = "", tknh = "", email = "";
                    object value_ma = gridView1.GetRowCellValue(e.FocusedRowHandle, "ma");
                    object value_ten = gridView1.GetRowCellValue(e.FocusedRowHandle, "ten");
                    object value_dia_chi = gridView1.GetRowCellValue(e.FocusedRowHandle, "DIA_CHI");
                    object value_phone = gridView1.GetRowCellValue(e.FocusedRowHandle, "PHONE");
                    object value_ma_gtgt = gridView1.GetRowCellValue(e.FocusedRowHandle, "MA_GTGT");
                    object value_tennh = gridView1.GetRowCellValue(e.FocusedRowHandle, "TenNganHang");
                    object value_tknh = gridView1.GetRowCellValue(e.FocusedRowHandle, "TKHOAN_NH");
                    object value_email = gridView1.GetRowCellValue(e.FocusedRowHandle, "email");
                    object value_ma_kh = gridView1.GetRowCellValue(e.FocusedRowHandle, "ma_kh");
                    if (value_ma != null)
                    {
                        ma = value_ma.ToString();
                    }
                    if (value_ten != null)
                    {
                        ten = value_ten.ToString();
                    }
                    if (value_dia_chi != null)
                    {
                        dia_chi = value_dia_chi.ToString();
                    }
                    if (value_phone != null)
                    {
                        phone = value_phone.ToString();
                    }
                    if (value_ma_gtgt != null)
                    {
                        ma_gtgt = value_ma_gtgt.ToString();
                    }
                    if (value_tennh != null)
                    {
                        tennh = value_tennh.ToString();
                    }
                    if (value_tknh != null)
                    {
                        tknh = value_tknh.ToString();
                    }
                    if (value_email != null)
                    {
                        email = value_email.ToString();
                    }
                    if (value_ma_kh != null)
                    {
                        ma_kh = value_ma_kh.ToString().Trim();
                    }


                txt_makh.Text = ma;
                    txt_tenkh.Text = ten;
                    txt_phone.Text = phone;
                    txt_diachi.Text = dia_chi;
                    txt_email.Text = email;
                    txt_tennh.Text = tennh;
                    txt_stk.Text = tknh;
                    txt_mst.Text = ma_gtgt;
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
            txt_makh.Enabled = true; //cho phép nhập mới
            txt_makh.Focus();
        }

        private void ResetValue()
        {
            txt_makh.Text = "";
            txt_tenkh.Text = "";
            txt_phone.Text = "";
            txt_diachi.Text = "";
            txt_email.Text = "";
            txt_tennh.Text = "";
            txt_stk.Text = "";
            txt_mst.Text = "";
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
                string sql; //Lưu lệnh sql
                if (txt_makh.Text.Trim().Length == 0) //Nếu chưa nhập mã khách hàng
                {
                    MessageBox.Show("Bạn phải nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_makh.Focus();
                    return;
                }
                if (txt_tenkh.Text.Trim().Length == 0) //Nếu chưa nhập tên khách hàng
                {
                    MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_tenkh.Focus();
                    return;
                }
                sql = "Select ma From _khang where ma=N'" + txt_makh.Text.Trim() + "'";
                if (Class.Functions.CheckKey(sql))
                {
                    MessageBox.Show("Mã khách hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_makh.Focus();
                    return;
                }

            if (MessageBox.Show("Bạn có muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DECLARE @ma_kh INT;\r\nSET @ma_kh = (SELECT ISNULL(MAX(ma_kh), 0) + 1 FROM _khang);\n";
                sql += "INSERT INTO _khang (ma_kh,ma,ten,PHONE,DIA_CHI,email,TenNganHang,TKHOAN_NH,MA_GTGT) VALUES(@ma_kh,N'" +
                    txt_makh.Text + "'," +
                    "N'" + txt_tenkh.Text + "'," +
                    "'" + txt_phone.Text + "'," +
                    "N'" + txt_diachi.Text + "'," +
                    "'" + txt_email.Text + "'," +
                    "'" + txt_tennh.Text + "'," +
                    "'" + txt_stk.Text + "'," +
                    "'" + txt_mst.Text + "')";
                //Class.Functions.editcode(sql); return;

                Class.Functions.RunSQL(sql); //Thực hiện câu lệnh sql
                loadview(); //Nạp lại DataGridView
                ResetValue();
                btn_xoa.Enabled = true;
                btn_them.Enabled = true;
                btn_sua.Enabled = true;
                btn_boqua.Enabled = false;
                btn_luu.Enabled = false;
                txt_makh.Enabled = false;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            object columnName = e.Column.FieldName;
            object cellValue = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            object primaryKeyValue = gridView1.GetRowCellValue(e.RowHandle, "ma_kh");
            //MessageBox.Show(columnName + " - "+primaryKeyValue + " - "+cellValue);

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "";
                sql = "UPDATE _khang SET " + columnName + " = '" + cellValue + "' WHERE ma_kh = " + primaryKeyValue + "";
                Class.Functions.RunSqlDel(sql);
            }
        }

        private void txt_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem phím được nhấn có phải là số không
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Từ chối ký tự nếu không phải là số
            }
        }

        private void txt_phone_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra độ dài của chuỗi và cắt ngắn nếu nó vượt quá 12 số
            if (txt_phone.Text.Length > 12)
            {
                txt_phone.Text = txt_phone.Text.Substring(0, 12);
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
            if (dt_khang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_makh.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_tenkh.Text.Trim().Length == 0) //nếu chưa nhập tên khách hàng
            {
                MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn cập nhật không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "UPDATE _khang SET ma= N'" +
                    txt_makh.Text + "', ten =" +
                    "N'" + txt_tenkh.Text + "',phone =" +
                    "'" + txt_phone.Text + "',dia_chi=" +
                    "N'" + txt_diachi.Text + "',email=" +
                    "'" + txt_email.Text + "',TenNganHang=" +
                    "'" + txt_tennh.Text + "',TKHOAN_NH=" +
                    "'" + txt_stk.Text + "', ma_gtgt =" +
                    "'" + txt_mst.Text + "' where ma_kh=" + ma_kh + "";
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
            if (dt_khang.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_makh.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE _khang WHERE ma_kh=N'" + ma_kh + "'";
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
            txt_makh.Enabled = false;
        }

        private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["ma_kh"]; // Thay "TenCot" bằng tên thực của cột

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