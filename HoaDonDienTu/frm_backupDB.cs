using DataLayer;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HoaDonDienTu
{
    public partial class frm_backupDB : DevExpress.XtraEditors.XtraForm
    {
        
        public frm_backupDB()
        {
            InitializeComponent();
        }

        private void frm_backupDB_Load(object sender, EventArgs e)
        {

        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txt_backup.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btn_lưu_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn backup cơ sở dữ liệu không?", "Xác nhận Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
                connect cp = (connect)bf.Deserialize(fs);

                //Dectypt noi dung

                string database = Encryptor.Decrypt(cp.database, "adshghd98@#$", true);
                string DBName = "";
                if (!string.IsNullOrWhiteSpace(txt_ten.Text))
                {
                    DBName = txt_ten.Text.Trim();
                }

                if (DBName == null || DBName.Length < 2)
                {
                    DBName = database;
                }


                string sql = "";
                sql = "DECLARE	@return_value int,@kq int,@fileOUT nvarchar(2000) EXEC	@return_value = [dbo].[sp_BackupData]";
                sql = sql + "@DBName = N'" + DBName + "',";
                sql = sql + "@pathexe = N'',";
                sql = sql + "@path = N'F:\\Share\\Backupall\\',";
                sql = sql + "@Duoi = N'effs',";
                sql = sql + "@Nen =0,";
                sql = sql + "@maHoa=0,";
                sql = sql + "@kq = @kq OUTPUT,@fileOUT = @fileOUT OUTPUT";
                sql = sql + " SELECT	@kq as N'kq',@fileOUT as N'fileOUT'";
                //Class.Functions.editcode(sql);
                try
                {
                    DataTable dtkq = Class.Functions.GetDataToTable(sql);
                    if (dtkq.Rows.Count > 0)
                    {
                        int kq = 0;
                        string ms = "";
                        kq = Convert.ToInt32(dtkq.Rows[0][0]);
                        ms = dtkq.Rows[0][1].ToString().Trim();
                        if (kq == 0)
                        {
                            // Câu lệnh T-SQL để backup cơ sở dữ liệu
                            string backupSQL = $@"
                                            BACKUP DATABASE " + DBName + @" 
                                            TO DISK = '" + ms + @"'
                                            WITH FORMAT;";
                            //Class.Functions.editcode(backupSQL);
                            try
                            {
                                Class.Functions.RunSQL(sql);
                                XtraMessageBox.Show("Backup thành công!");
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
        }
    }
}