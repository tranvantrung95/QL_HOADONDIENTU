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
    public partial class frm_searchdata : Form
    {
        public string sql = "";
        public string ma = "";
        public string ten = "";
        public int mangam = 0;
        public string cot_mangam = "";
        public string cot_ma = "";
        public string cot_ten = "";
        public string ngay_thc = "";
        public string thuesuat = "";
        public string chung_tu = "";
        public string mau_hd = "";
        public string serihd = "";
        public string sohd = "";
        public string phi_tm = "";
        public string ongba = "";
        public string dcthue = "";
        public string ma_kh = "";
        public string dv_do = "";
        public string ty_gia = "";
        public string ma_tk0 = "";
        public string ma_tk1 = "";
        public string httt = "";
        public string khthue = "";
        public string msthue = "";
        public string ma_thb = "";
        public string gch_detail = "";
        public string so_luong = "";
        public string don_gia = "";
        public string ps_no1 = "";
        public string pt_ck = "";
        public string tien_ck = "";
        public string vat = "";
        public frm_searchdata()
        {
            InitializeComponent();
        }

        private void frm_searchdata_Load(object sender, EventArgs e)
        {
            DataTable dt = Class.Functions.GetDataToTable(sql);

            dataGridView1.DataSource = dt;
            dataGridView1.AllowUserToAddRows = false; //bỏ dòng thêm mới

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //ma = GetCellValue(dataGridView1, e.RowIndex, cot_ma);
                //ten = GetCellValue(dataGridView1, e.RowIndex, cot_ten);

                // Kiểm tra và lấy giá trị mangam
                string mangamValue = GetCellValue(dataGridView1, e.RowIndex, cot_mangam);
                if (int.TryParse(mangamValue, out mangam))
                {
                    // Mangam có giá trị hợp lệ
                }
                else
                {
                    // Xử lý khi không thể chuyển đổi thành số nguyên
                }

                // Kiểm tra và lấy giá trị các cột có thể có
                ngay_thc = GetCellValue(dataGridView1, e.RowIndex, "ngay_thc");
                thuesuat = GetCellValue(dataGridView1, e.RowIndex, "ktdb");
                chung_tu = GetCellValue(dataGridView1, e.RowIndex, "chung_tu");
                mau_hd = GetCellValue(dataGridView1, e.RowIndex, "mau_hd");
                serihd = GetCellValue(dataGridView1, e.RowIndex, "serihd");
                sohd = GetCellValue(dataGridView1, e.RowIndex, "sohd");
                phi_tm = GetCellValue(dataGridView1, e.RowIndex, "phi_tm");
                ongba = GetCellValue(dataGridView1, e.RowIndex, "ongba");
                dcthue = GetCellValue(dataGridView1, e.RowIndex, "dcthue");
                ma_kh = GetCellValue(dataGridView1, e.RowIndex, "ma_kh");
                dv_do = GetCellValue(dataGridView1, e.RowIndex, "dv_do");
                ty_gia = GetCellValue(dataGridView1, e.RowIndex, "ty_gia");
                ma_tk0 = GetCellValue(dataGridView1, e.RowIndex, "ma_tk0");
                ma_tk1 = GetCellValue(dataGridView1, e.RowIndex, "ma_tk1");
                httt = GetCellValue(dataGridView1, e.RowIndex, "tt3");
                khthue = GetCellValue(dataGridView1, e.RowIndex, "khthue");
                msthue = GetCellValue(dataGridView1, e.RowIndex, "msthue");
                ma_thb = GetCellValue(dataGridView1, e.RowIndex, "ma_thb");
                gch_detail = GetCellValue(dataGridView1, e.RowIndex, "gch_detail");
                so_luong = GetCellValue(dataGridView1, e.RowIndex, "so_luong");
                don_gia = GetCellValue(dataGridView1, e.RowIndex, "don_gia");
                ps_no1 = GetCellValue(dataGridView1, e.RowIndex, "ps_no1");
                pt_ck = GetCellValue(dataGridView1, e.RowIndex, "pt_ck");
                tien_ck = GetCellValue(dataGridView1, e.RowIndex, "tien_ck");
                vat = GetCellValue(dataGridView1, e.RowIndex, "vat");

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

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var dataGridView = sender as DataGridView;
                if (dataGridView != null && dataGridView.CurrentRow != null)
                {
                    int RowIndex = dataGridView.CurrentRow.Index;

                    // Kiểm tra và lấy giá trị mangam
                    string mangamValue = GetCellValue(dataGridView1, RowIndex, cot_mangam);
                    if (int.TryParse(mangamValue, out mangam))
                    {
                        // Mangam có giá trị hợp lệ
                    }
                    else
                    {
                        // Xử lý khi không thể chuyển đổi thành số nguyên
                    }

                    // Kiểm tra và lấy giá trị các cột có thể có
                    ngay_thc = GetCellValue(dataGridView1, RowIndex, "ngay_thc");
                    thuesuat = GetCellValue(dataGridView1, RowIndex, "ktdb");
                    chung_tu = GetCellValue(dataGridView1, RowIndex, "chung_tu");
                    mau_hd = GetCellValue(dataGridView1, RowIndex, "mau_hd");
                    serihd = GetCellValue(dataGridView1, RowIndex, "serihd");
                    sohd = GetCellValue(dataGridView1, RowIndex, "sohd");
                    phi_tm = GetCellValue(dataGridView1, RowIndex, "phi_tm");
                    ongba = GetCellValue(dataGridView1, RowIndex, "ongba");
                    dcthue = GetCellValue(dataGridView1, RowIndex, "dcthue");
                    ma_kh = GetCellValue(dataGridView1, RowIndex, "ma_kh");
                    dv_do = GetCellValue(dataGridView1, RowIndex, "dv_do");
                    ty_gia = GetCellValue(dataGridView1, RowIndex, "ty_gia");
                    ma_tk0 = GetCellValue(dataGridView1, RowIndex, "ma_tk0");
                    ma_tk1 = GetCellValue(dataGridView1, RowIndex, "ma_tk1");
                    httt = GetCellValue(dataGridView1, RowIndex, "tt3");
                    khthue = GetCellValue(dataGridView1, RowIndex, "khthue");
                    msthue = GetCellValue(dataGridView1, RowIndex, "msthue");
                    ma_thb = GetCellValue(dataGridView1, RowIndex, "ma_thb");
                    gch_detail = GetCellValue(dataGridView1, RowIndex, "gch_detail");
                    so_luong = GetCellValue(dataGridView1, RowIndex, "so_luong");
                    don_gia = GetCellValue(dataGridView1, RowIndex, "don_gia");
                    ps_no1 = GetCellValue(dataGridView1, RowIndex, "ps_no1");
                    pt_ck = GetCellValue(dataGridView1, RowIndex, "pt_ck");
                    tien_ck = GetCellValue(dataGridView1, RowIndex, "tien_ck");
                    vat = GetCellValue(dataGridView1, RowIndex, "vat");

                    this.Close();
                }
            }
        }
    }
}
