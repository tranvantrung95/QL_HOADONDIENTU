using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using HoaDonDienTu.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HoaDonDienTu
{
    public partial class frm_BACKUP_DATA : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_users;
        string ma_user = "";
        Cls_EnCrypting ecr = new Cls_EnCrypting();
        public frm_BACKUP_DATA()
        {
            InitializeComponent();
        }

        private void frm_BACKUP_DATA_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBackupFolder.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
                    // Lấy đường dẫn thư mục từ hộp văn bản
                    string folderPath = txtBackupFolder.Text; // txtBackupFolder là tên của hộp văn bản thư mục
                    txtBackupFolder.Text = "F:\\Share\\Backupall";
                    folderPath = "F:\\Share\\Backupall";
                    string fileName = txtFileName.Text; // txtFileName là tên của hộp văn bản tên file

                    //// Kiểm tra đường dẫn thư mục và tên file không được để trống
                    //if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(fileName))
                    //{
                    //    MessageBox.Show("Please specify both the folder path and file name for the backup.");
                    //    return;
                    //}
            string sql = "DECLARE @return_value int, @kq int, @fileOUT nvarchar(2000) EXEC @return_value = [dbo].[sp_BackupData]@DBName = N'EFF_EFFE_200',@pathexe = N'',@path = N'F:\\Share\\Backupall',@Duoi = N'effs',@Nen = 0,@maHoa = 0,@kq = @kq OUTPUT,@fileOUT = @fileOUT OUTPUT SELECT @fileOUT as N'fileOUT'";

            object kq = Functions.SQLEXEC(sql,true);
            if(!string.IsNullOrWhiteSpace(kq.ToString()))
            {
                // Gọi hàm thực hiện backup
                Functions.BackupDatabase(kq.ToString());
            }    
                    //// Xây dựng đường dẫn file backup đầy đủ
                    //string backupFilePath = Path.Combine(folderPath, fileName + ".bak");

                    
        }
    }
}