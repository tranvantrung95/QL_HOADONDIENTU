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
    public partial class frm_searchTime : DevExpress.XtraEditors.XtraForm
    {
        public frm_searchTime()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public string tu_ngay, den_ngay;

        private void txt_tu_ngay_EditValueChanged(object sender, EventArgs e)
        {
            DateTime.TryParse(txt_tu_ngay.EditValue.ToString(), out DateTime tungay);
            tu_ngay = tungay.ToString("MM/dd/yyyy");
            //MessageBox.Show(tu_ngay.ToString());
        }

        private void txt_den_ngay_EditValueChanged(object sender, EventArgs e)
        {
            DateTime.TryParse(txt_den_ngay.EditValue.ToString(), out DateTime denngay);
            den_ngay = denngay.ToString("MM/dd/yyyy");
            //MessageBox.Show(den_ngay.ToString());
        }

        private void frm_searchTime_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;

            // Get the first day of the current month
            DateTime tungay = new DateTime(today.Year, today.Month, 1);

            // Get the last day of the current month
            DateTime denngay = tungay.AddMonths(1).AddDays(-1);

            txt_tu_ngay.EditValue = tungay;
            txt_den_ngay.EditValue= denngay;

            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}