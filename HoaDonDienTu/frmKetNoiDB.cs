using DataLayer;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HoaDonDienTu
{
    public partial class frmKetNoiDB : DevExpress.XtraEditors.XtraForm
    {
        public frmKetNoiDB()
        {
            InitializeComponent();
        }
        SqlConnection GetConn(string server, string username, string pass, string database)
        {
            return new SqlConnection("Data source=" + server + "; Initial catalog=" + database + "; User ID=" + username + "; Password=" + pass + ";");
        }
        private void frmKetNoiDB_Load(object sender, EventArgs e)
        {

        }

        private void btn_ketnoi_Click(object sender, EventArgs e)
        {
            SqlConnection con = GetConn(txt_server.Text, txt_username.Text, txt_pass.Text, cbb_database.Text);
            try
            {
                con.Open();
                MessageBox.Show("Kết nối thành công");
            }
            catch (Exception)
            {
                MessageBox.Show("Kết nối thất bại");
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbb_database_MouseClick_1(object sender, MouseEventArgs e)
        {
            cbb_database.Items.Clear();
            string conn = "Data source=" + txt_server.Text + "; User ID=" + txt_username.Text + "; Password=" + txt_pass.Text + ";";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            string sql = "SELECT NAME FROM SYS.DATABASES";
            SqlCommand cmd = new SqlCommand(sql, con);
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cbb_database.Items.Add(dr[0].ToString());
            }
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            string svEncrypt = Encryptor.Encrypt(txt_server.Text, "adshghd98@#$", true);
            string usEncrypt = Encryptor.Encrypt(txt_username.Text, "adshghd98@#$", true);
            string psEncrypt = Encryptor.Encrypt(txt_pass.Text, "adshghd98@#$", true);
            string dbEncrypt = Encryptor.Encrypt(cbb_database.Text, "adshghd98@#$", true);
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "Chọn nơi lưu trữ";
            sf.Filter = "Text Files (*.dba)|*.dba|AllFiles(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                connect cn = new connect(svEncrypt, usEncrypt, psEncrypt, dbEncrypt);
                cn.ConnectData(sf.FileName);
                MessageBox.Show("Lưu file thành công");
            }

        }

        private void btn_doc_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();

            op.Title = "Chọn nơi lưu trữ";
            op.Filter = "Text Files (*.dba)|*.dba|AllFiles(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(op.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                connect con = (connect)bf.Deserialize(fs);
                string srv = Encryptor.Decrypt(con.Servername, "adshghd98@#$", true);
                string us = Encryptor.Decrypt(con.Username, "adshghd98@#$", true);
                string ps = Encryptor.Decrypt(con.passwd, "adshghd98@#$", true);
                string db = Encryptor.Decrypt(con.Database, "adshghd98@#$", true);
                txt_server.Text = srv;
                txt_username.Text = us;
                txt_pass.Text = ps;
                cbb_database.Text = db;
            }
        }
    }
}