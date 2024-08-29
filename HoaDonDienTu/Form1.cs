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

namespace HoaDonDienTu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string sql = "";
        public string ma = "";
        public string ten = "";
        public int mangam = 0;
        public string cot_mangam = "";
        public string cot_ma = "";
        public string cot_ten = "";
        public string dia_chi = "";
        public string ma_gtgt = "";
        public string httt = "";
        public string khthue = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = Class.Functions.GetDataToTable(sql);
            //if (dt.Rows.Count == 0)
            //{
            //    MessageBox.Show("Không tìm thấy dữ liệu");
            //    this.Close();
            //}   
                
            dataGridView2.DataSource = dt;
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                ma = GetCellValue(dataGridView2, e.RowIndex, cot_ma);
                ten = GetCellValue(dataGridView2, e.RowIndex, cot_ten);

                // Kiểm tra và lấy giá trị mangam
                string mangamValue = GetCellValue(dataGridView2, e.RowIndex, cot_mangam);
                if (int.TryParse(mangamValue, out mangam))
                {
                    // Mangam có giá trị hợp lệ
                }
                else
                {
                    // Xử lý khi không thể chuyển đổi thành số nguyên
                }

                // Kiểm tra và lấy giá trị các cột có thể có
                dia_chi = GetCellValue(dataGridView2, e.RowIndex, "dia_chi");
                ma_gtgt = GetCellValue(dataGridView2, e.RowIndex, "ma_gtgt");
                httt = GetCellValue(dataGridView2, e.RowIndex, "ViettelId");

                this.Close();

            }
        }

        private string GetCellValue(DataGridView dataGridView, int rowIndex, string columnName)
        {
            // Kiểm tra xem tên cột có tồn tại hay không
            if (dataGridView.Columns.Contains(columnName))
            {
                // Kiểm tra xem ô có giá trị không rỗng hay không
                object cellValue = dataGridView.Rows[rowIndex].Cells[columnName].Value;
                if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    return cellValue.ToString();
                }
            }
            return ""; // Trả về chuỗi rỗng nếu không tìm thấy giá trị
        }

        //private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.KeyCode == Keys.Enter)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            ma = GetCellValue(dataGridView2, e.RowIndex, cot_ma);
        //            ten = GetCellValue(dataGridView2, e.RowIndex, cot_ten);

        //            // Kiểm tra và lấy giá trị mangam
        //            string mangamValue = GetCellValue(dataGridView2, e.RowIndex, cot_mangam);
        //            if (int.TryParse(mangamValue, out mangam))
        //            {
        //                // Mangam có giá trị hợp lệ
        //            }
        //            else
        //            {
        //                // Xử lý khi không thể chuyển đổi thành số nguyên
        //            }

        //            // Kiểm tra và lấy giá trị các cột có thể có
        //            dia_chi = GetCellValue(dataGridView2, e.RowIndex, "dia_chi");
        //            ma_gtgt = GetCellValue(dataGridView2, e.RowIndex, "ma_gtgt");
        //            httt = GetCellValue(dataGridView2, e.RowIndex, "ViettelId");

        //            this.Close();

        //        }
        //    }    
        //}

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var dataGridView = sender as DataGridView;
                if (dataGridView != null && dataGridView.CurrentRow != null)
                {
                    int rowIndex = dataGridView.CurrentRow.Index;

                    ma = GetCellValue(dataGridView, rowIndex, cot_ma);
                    ten = GetCellValue(dataGridView, rowIndex, cot_ten);

                    // Kiểm tra và lấy giá trị mangam
                    string mangamValue = GetCellValue(dataGridView, rowIndex, cot_mangam);
                    if (int.TryParse(mangamValue, out mangam))
                    {
                        // Mangam có giá trị hợp lệ
                    }
                    else
                    {
                        // Xử lý khi không thể chuyển đổi thành số nguyên
                    }

                    // Kiểm tra và lấy giá trị các cột khác
                    dia_chi = GetCellValue(dataGridView, rowIndex, "dia_chi");
                    ma_gtgt = GetCellValue(dataGridView, rowIndex, "ma_gtgt");
                    httt = GetCellValue(dataGridView, rowIndex, "ViettelId");

                    this.Close();
                }
            }
        }
    }
}
