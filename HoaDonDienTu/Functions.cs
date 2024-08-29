using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using System.IO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using DataLayer;
using System.Runtime.Serialization.Formatters.Binary;

namespace HoaDonDienTu.Class
{
    internal class Functions
    {
        public static SqlConnection Con;  //Khai báo đối tượng kết nối
        private static dynamic mdiParentForm;
        private static string servername = "", username = "", pass = "", database = "";

        public static void Connect()
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
            //connect cp = (connect)bf.Deserialize(fs);

            ////Dectypt noi dung
            //string servername = Encryptor.Decrypt(cp.servername, "adshghd98@#$", true);
            //string username = Encryptor.Decrypt(cp.username, "adshghd98@#$", true);
            //string pass = Encryptor.Decrypt(cp.passwd, "adshghd98@#$", true);
            //string database = Encryptor.Decrypt(cp.database, "adshghd98@#$", true);

            using (FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                connect cp = (connect)bf.Deserialize(fs);
                // Giải mã nội dung
                 servername = Encryptor.Decrypt(cp.servername, "adshghd98@#$", true);
                 username = Encryptor.Decrypt(cp.username, "adshghd98@#$", true);
                 pass = Encryptor.Decrypt(cp.passwd, "adshghd98@#$", true);
                 database = Encryptor.Decrypt(cp.database, "adshghd98@#$", true);
            }

            Con = new SqlConnection();   //Khởi tạo đối tượng
            //Con.ConnectionString = @"Data Source=27.72.31.203,9225;Initial Catalog=EFF_EFFE_200;Persist Security Info=True;User ID=xuanhoa;PassWord=kythuat@EFFECT2019$$$;Encrypt=False";
            Con.ConnectionString = @"Data Source="+ servername +";Initial Catalog="+ database +";Persist Security Info=True;User ID="+ username +";PassWord="+ pass +";Encrypt=False";
            Con.Open();                  //Mở kết nối
            //Kiểm tra kết nối
            if (Con.State == ConnectionState.Open)
            {
                //MessageBox.Show("Kết nối thành công");
            }
            else MessageBox.Show("Không thể kết nối với dữ liệu");

        }
        public static void Disconnect()
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();   	//Đóng kết nối
                Con.Dispose(); 	//Giải phóng tài nguyên
                Con = null;
            }
        }

        //Hiển thị bảng
        public static void Brow(DataTable dt)
        {
            DevExpress.XtraGrid.GridControl gridControl;
            DevExpress.XtraGrid.Views.Grid.GridView view;
            gridControl = new DevExpress.XtraGrid.GridControl();
            gridControl.DataSource = dt;
            view = new DevExpress.XtraGrid.Views.Grid.GridView(gridControl);
            gridControl.MainView = view;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowFooter = true;
            view.OptionsView.ShowAutoFilterRow = true;
            view.OptionsFind.AlwaysVisible = true;

            // view.OptionsBehavior.ReadOnly = true;
            view.OptionsBehavior.Editable = true;
            gridControl.Dock = DockStyle.Fill;

            Form f = new Form();
            f.Controls.Add(gridControl);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
        }

        //Hiển thị câu lệnh sql
        public static void editcode(string sql)
        {
            RichTextBox richtxt = new RichTextBox();
            richtxt.Text = sql;
            richtxt.Dock = DockStyle.Fill;

            richtxt.Font = new Font("Arial", 12, FontStyle.Regular);
            //richtxt.Width = 1500; // Set the width to 300 pixels
            //richtxt.Height = 900; // Set the height to 200 pixels
            //richtxt.WordWrap = true; // Disable word wrapping

            Form f = new Form();
            f.Controls.Add(richtxt);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
        }

        //Lấy dữ liệu vào bảng
        public static DataTable GetDataToTable(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp SqlCommand
            dap.SelectCommand = new SqlCommand();
            dap.SelectCommand.Connection = Functions.Con; //Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; //Lệnh SQL
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }

public static void BackupDatabase(string destinationPath)
    {
        // Câu lệnh T-SQL để backup cơ sở dữ liệu
        string backupSQL = $@"
        BACKUP DATABASE EFF_EFFE_200 
        TO DISK = N'{destinationPath}'
        WITH FORMAT;";
            Functions.editcode(backupSQL);
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = backupSQL; //Gán lệnh SQL
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Backup dữ liệu thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
                cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
    }



    //Hàm kiểm tra khoá trùng
    public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, Con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        //Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }

        public static object SQLEXEC(string str, bool ret)
        {
            try
            {
                SqlCommand sqlCommand;
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = Con;
                sqlCommand.CommandText = str;
                sqlCommand.CommandTimeout = 0;
                object result = ((!ret) ? ((object)sqlCommand.ExecuteNonQuery()) : sqlCommand.ExecuteScalar());
                sqlCommand.Dispose();
                sqlCommand = null;
                return result;
            }
            catch (Exception ex)
            {
                WriteTextLog(ex.Message + ": commonfunctions SQLEXEC");
                return 0;
            }
        }

        public static bool UpdateAField(string tableName, string fieldName, string value, string where)
        {
            //string text = Cls_VariGlobal.cnnstring;
            //string text2 = "";
            //DataTable dataTable = GetDataToTable("Select * from _danhmuc");
            //if (dataTable != null && dataTable.Columns.Contains("dbName"))
            //{
            //    DataRow[] array = dataTable.Select("tendm='" + tableName + "' or alia = '" + tableName + "'", "alia");
            //    if (array.Length > 0 && !string.IsNullOrEmpty(array[0]["dbName"].ToString().Trim()))
            //    {
            //        string text3 = "";
            //        text3 = SubstringBetween(text, "=", ";", 0);
            //        text = text.Substring(text.IndexOf(";") + 1);
            //        text = text.Substring(text.IndexOf(";") + 1);
            //        text = "SERVER = " + text3 + ";Database = " + array[0]["dbName"].ToString().Trim() + ";" + text;
            //    }
            //}
            string commandText = "\n Update " + tableName + " set " + fieldName + "=@" + fieldName + ",DateTime=GETDATE() where " + where;
            try
            {
                SqlCommand sqlCommand;
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = Con;
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandTimeout = 0;
                if (value != "")
                {
                    sqlCommand.Parameters.Add(fieldName, SqlDbType.NVarChar).Value = value;
                }
                else
                {
                    sqlCommand.Parameters.Add(fieldName, SqlDbType.NVarChar).Value = " ";
                }
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                sqlCommand = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + tableName);
                WriteTextLog(ex.Message + ": commonfunctions UpdateAfield ");
                return false;
            }
            return true;
        }

        public static bool UpdateAField(string tableName, string fieldName, string value, string where, bool noDateTime)
        {
            string commandText = (noDateTime ? ("\n Update " + tableName + " set " + fieldName + "=@" + fieldName + " where " + where) : ("\n Update " + tableName + " set " + fieldName + "=@" + fieldName + ",DateTime=GETDATE() where " + where));
            try
            {
                SqlCommand sqlCommand;
                sqlCommand = new SqlCommand();
                sqlCommand.Connection = Con;
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandTimeout = 0;
                if (value != "")
                {
                    sqlCommand.Parameters.Add(fieldName, SqlDbType.NVarChar).Value = value;
                }
                else
                {
                    sqlCommand.Parameters.Add(fieldName, SqlDbType.NVarChar).Value = " ";
                }
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                sqlCommand = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + tableName);
                WriteTextLog(ex.Message + ": commonfunctions updateafield");
                return false;
            }
            return true;
        }

        public static string SubstringBetween(string str, string str_from, string str_to, int position)
        {
            string result = "";
            int num = str.IndexOf(str_from, position) + str_from.Length;
            int num2 = str.IndexOf(str_to, position);
            if (num2 > num)
            {
                result = str.Substring(num, num2 - num);
            }
            return result;
        }

        //Hàm thực hiện câu lệnh xóa SQL
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Functions.Con;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Dữ liệu đang được dùng, không thể xoá...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }

        public static bool IsNumberType(Type checktype)
        {
            bool flag = false;
            switch (Type.GetTypeCode(checktype))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        public static string bangchuv(double value, string dvtt)
        {
            bool am = false;
            if (value < 0)
            {
                am = true;
                value = Math.Abs(value);
            }

            string unit = "đồng";
            string cent = "cents";
            dvtt = dvtt.ToUpper();
            if (dvtt != "VND")
            {
                unit = dvtt;
            }
            DataTable dtbdv = Class.Functions.GetDataToTable("Select tenvt, tenle from _DV where ma = '" + dvtt + "'");
            if (dtbdv.Rows.Count > 0)
            {
                unit = dtbdv.Rows[0]["tenvt"].ToString();
                cent = dtbdv.Rows[0]["tenle"].ToString();
            }
            int nganti = 0, ti = 0, trieu = 0, ngan = 0, dv = 0, Dec_Frac = 0, AddAnd = 0, i = 0;
            string Temp = "";
            double ModVal = 0;
            if (value > 999999999999999.99)
            {
                return "Số quá lớn!";
            }

            nganti = (int)(value / 1000000000000);
            //MessageBox.Show("ngan ti "+nganti.ToString());
            ModVal = value - ((nganti * 1.0) * 1000000000000);
            //MessageBox.Show("ModVal "+ModVal.ToString());
            ti = (int)(ModVal / 1000000000);
            //MessageBox.Show("ti "+ti.ToString());
            // MessageBox.Show("ti "+((ti*1.0)*1000000000).ToString());
            ModVal = ModVal - ((ti * 1.0) * 1000000000);
            //MessageBox.Show("ModVal "+ModVal.ToString());
            trieu = (int)(ModVal / 1000000);
            //MessageBox.Show("trieu "+trieu.ToString());
            ModVal = ModVal - ((trieu * 1.0) * 1000000);
            //MessageBox.Show("ModVal "+ModVal.ToString());
            ngan = (int)(ModVal / 1000);
            //MessageBox.Show("Ngan "+ngan.ToString());
            dv = (int)(ModVal - ((ngan * 1.0) * 1000));
            //MessageBox.Show("dv "+dv.ToString());
            //Dec_Frac=(int)(((value-(value))*100)+0.5);
            Temp = value.ToString("0.00").Trim();
            i = Temp.IndexOf(".");
            if (i != -1)
            {
                Temp = Temp.Substring(i + 1, Temp.Length - i - 1);
                Dec_Frac = Convert.ToInt32(Temp);
                Temp = "";
            }

            Temp = " ";
            if (nganti > 0)
            {
                Temp = Temp + TriConv(nganti, false) + " ngàn tỉ, ";
            }

            if (ti > 0)
            {
                if (ti < 100 && Temp.Length > 1)
                    Temp = Temp + " không trăm " + TriConv(ti, false) + " tỉ, ";
                else
                    Temp = Temp + TriConv(ti, false) + " tỉ, ";
            }

            if (trieu > 0)
            {
                if (trieu < 100 && Temp.Length > 1)
                    Temp = Temp + " không trăm " + TriConv(trieu, false) + " triệu, ";
                else
                    Temp = Temp + TriConv(trieu, false) + " triệu, ";
            }

            if (ngan > 0)
            {
                if (ngan < 100 && Temp.Length > 1)
                    Temp = Temp + " không trăm " + TriConv(ngan, false) + " ngàn, ";
                else
                    Temp = Temp + TriConv(ngan, false) + " ngàn, ";
            }

            if (dv > 0)
            {
                if (dv < 100 && Temp.Length > 1)
                    Temp = Temp + " không trăm " + TriConv(dv, true);
                else
                    Temp = Temp + TriConv(dv, true);
            }

            if (Dec_Frac > 0)
            {
                //Temp = Temp+" "+unit+" lẻ "
                string duoi = TriConv(Dec_Frac, false) + " " + cent; ;
                if (duoi.Trim().StartsWith("lẻ"))
                    Temp = Temp + " " + unit + duoi;
                else
                    Temp = Temp + " " + unit + " lẻ " + duoi;
            }
            else
            {
                Temp = Temp + " " + unit + " chẵn";
            }

            Temp = Temp.Trim();
            if ((Temp.Substring(0, 2)) == "lẻ")
            {
                Temp = (Temp.Substring(2, Temp.Length - 2));
            }

            if ((Temp.Substring(Temp.Length - 2, 1)) == ", ")
            {
                Temp = (Temp.Substring(0, Temp.Length - 2));
            }

            Temp = Temp.Trim();
            Temp = Temp.Replace(", ", ", ");
            Temp = Temp.Replace(", ", " ");
            Temp = Temp.Replace(" ", " ");
            Temp = Temp.Replace(" đồng chẵn", " đồng chẵn");
            //upper ký tự đầu
            if (am)
            {
                Temp = "âm " + Temp;
            }

            Temp = UppercaseFirst(Temp);
            Temp = Temp + ".";
            Temp = Temp.Replace(" ", " ");
            Temp = Temp.Replace(" ", " ");
            Temp = Temp.Replace(" ", " ");
            Temp = Temp.Replace("mươi năm", "mươi lăm");
            Temp = Temp.Replace("mười năm", "mười lăm");
            Temp = Temp.Replace("lẻ lẻ", "lẻ");
            return Temp;
        }

        public static string TriConv(int value, bool AddAnd)
        {
            string[] words = new string[] { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] words2 = new string[] { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
            string[] words1 = new string[] { "", "mười một", "mười hai", "mười ba", "mười bốn", "mười năm", "mười sáu", "mười bảy", "mười tám", "mười chín", "" };
            string tram = "", chuc = "", donvi = "", temp = "", trip = "", AndVal = "";
            int i = 0;
            if (value == 0)
            {
                temp = "Không";
            }
            else
            {
                trip = value.ToString().Trim();
                i = trip.Length;
                if (i == 1)
                {
                    tram = "0";
                    chuc = "0";
                    donvi = trip;
                }
                if (i == 2)
                {
                    tram = "0";
                    chuc = (trip.Substring(0, 1));
                    donvi = trip.Substring(1, 1);
                }
                if (i == 3)
                {
                    tram = (trip.Substring(0, 1));
                    chuc = (trip.Substring(1, 1));
                    donvi = trip.Substring(2, 1);
                }
                temp = "";
                int itram = 0;
                itram = Convert.ToInt32(tram);
                if (itram > 0)
                {
                    temp = temp + words[itram] + " trăm ";
                }
                if (AddAnd == true && itram > 0)
                {
                    AndVal = " ";
                }
                else
                {
                    AndVal = " ";
                }
                int idonvi = 0, ichuc = 0;
                idonvi = Convert.ToInt32(donvi);
                ichuc = Convert.ToInt32(chuc);
                if (idonvi > 0 && ichuc == 1)
                {
                    temp = temp + AndVal + words1[idonvi];
                }
                if (idonvi > 0 && ichuc > 1)
                {
                    if (idonvi == 1)
                    {
                        words[idonvi] = "mốt";
                    }
                    if (idonvi == 5)
                    {
                        words[idonvi] = "lăm";
                    }
                    temp = temp + AndVal + words2[ichuc] + " " + words[idonvi];
                }
                if (idonvi > 0 && ichuc == 0)
                {
                    temp = temp + AndVal + " lẻ " + words[idonvi];
                }
                if (idonvi == 0 && ichuc == 1)
                {
                    temp = temp + AndVal + " mười ";
                }
                if (idonvi == 0 && ichuc > 1)
                {
                    temp = temp + AndVal + words2[ichuc];
                }
            }
            words[1] = "một";
            words[5] = "năm";
            return temp;
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            bool cach = false;
            foreach (char ch in s)
            {
                if (cach == false && ch == ' ')
                    sb.Append(ch);
                if (ch == ' ')
                    cach = true;
                else
                    cach = false;
                if (cach == false)
                    sb.Append(ch);
            }
            s = sb.ToString();
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static void WriteTextLog(string _log)
        {
            _log = Environment.NewLine + "(" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "): " + _log;
            string text = System.Windows.Forms.Application.StartupPath + "\\log.txt";
            if (!File.Exists(text))
            {
                File.WriteAllText(text, _log, Encoding.Unicode);
            }
            else
            {
                File.AppendAllText(text, _log, Encoding.Unicode);
            }
            FileInfo fileInfo = new FileInfo(text);
            if (fileInfo.Length > 8000000)
            {
                DateTime now = DateTime.Now;
                File.Copy(text, System.Windows.Forms.Application.StartupPath + "\\log" + now.Year.ToString("0000") + now.Month.ToString("00") + now.Day.ToString("00") + now.Hour.ToString("00") + now.Minute.ToString("00") + now.Second.ToString("00") + ".txt");
                File.Delete(text);
            }
        }


        public static bool CheckRowCondition(DataRow dtr, DataTable dataTable, string condition)
        {
            if (string.IsNullOrWhiteSpace(condition) || dataTable == null)
            {
                return false;
            }

            try
            {
                DataTable clonedTable = dataTable.Clone();
                clonedTable.TableName = dataTable.TableName + "_copy";
                clonedTable.ImportRow(dtr);
                //Brow(clonedTable);

                if (condition.ToUpper().Contains("PARENT("))
                {
                    string parentCondition = condition.Substring(condition.IndexOf("PARENT(") + 7);
                    parentCondition = parentCondition.Substring(0, parentCondition.LastIndexOf(")"));
                    MessageBox.Show("1234");

                    if (!clonedTable.DataSet.Tables.Contains(clonedTable.TableName))
                    {
                        clonedTable.DataSet.Tables.Add(clonedTable);

                        // ... (Tương tự như phần code để thêm các mối quan hệ)

                    }
                    else
                    {
                        DataTable existingTable = clonedTable.DataSet.Tables[clonedTable.TableName];
                        existingTable.Rows.Clear();
                        existingTable.Merge(clonedTable);
                        existingTable.AcceptChanges();
                        //Brow(existingTable);
                    }

                    clonedTable = clonedTable.DataSet.Tables[clonedTable.TableName];
                    condition = parentCondition;
                }

                if (clonedTable.Select(condition).Length > 0)
                {
                    MessageBox.Show("abc");
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        //Hàm định dạng số cho cột được khai báo
        public static void formatNumber(GridView gridView, params string[] columnNames)
        {
            // Tạo và cấu hình RepositoryItemTextEdit
            RepositoryItemTextEdit numericEdit = new RepositoryItemTextEdit();
            numericEdit.Mask.EditMask = "n0";  // Sử dụng "n0" cho định dạng số nguyên
            numericEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            numericEdit.Mask.UseMaskAsDisplayFormat = true;
            numericEdit.CustomDisplayText += NumericEdit_CustomDisplayText;

            // Áp dụng định dạng số cho từng cột được chỉ định
            foreach (string columnName in columnNames)
            {
                GridColumn column = gridView.Columns[columnName];
                if (column != null)
                {
                    column.ColumnEdit = numericEdit;
                }
                else
                {
                    // Cột không tồn tại, xử lý tùy ý (ví dụ: ghi log, ném lỗi, ...)
                    Console.WriteLine($"Cột '{columnName}' không tồn tại trong GridView.");
                }
            }
        }

        public static void NumericEdit_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.Value != null && e.Value != DBNull.Value && int.TryParse(e.Value.ToString(), out int value))
            {
                e.DisplayText = value.ToString("#,##0", CultureInfo.InvariantCulture).Replace(",", " ");
            }
        }

        public static bool CheckRowCondition(DataRow row, string condition)
        {
            // Xử lý nhiều điều kiện được phân cách bằng "and"
            var conditions = condition.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var cond in conditions)
            {
                string[] parts;
                string key, op, value;

                // Xác định toán tử và phân chia chuỗi điều kiện
                if (cond.Contains("<>"))
                {
                    parts = cond.Split(new string[] { "<>" }, StringSplitOptions.None);
                    op = "<>";
                }
                else if (cond.Contains(">="))
                {
                    parts = cond.Split(new string[] { ">=" }, StringSplitOptions.None);
                    op = ">=";
                }
                else if (cond.Contains("<="))
                {
                    parts = cond.Split(new string[] { "<=" }, StringSplitOptions.None);
                    op = "<=";
                }
                else if (cond.Contains(">"))
                {
                    parts = cond.Split(new string[] { ">" }, StringSplitOptions.None);
                    op = ">";
                }
                else if (cond.Contains("<"))
                {
                    parts = cond.Split(new string[] { "<" }, StringSplitOptions.None);
                    op = "<";
                }
                else if (cond.Contains("="))
                {
                    parts = cond.Split(new string[] { "=" }, StringSplitOptions.None);
                    op = "=";
                }
                else
                {
                    return false; // Không tìm thấy toán tử hợp lệ
                }

                if (parts.Length != 2)
                    return false; // Điều kiện không hợp lệ

                key = parts[0].Trim();
                value = parts[1].Trim().Trim('\''); // Loại bỏ dấu nháy đơn nếu có

                if (!row.Table.Columns.Contains(key))
                    return false; // Cột không tồn tại

                string actualValue = row[key].ToString().Trim();

                // Áp dụng so sánh dựa trên toán tử
                switch (op)
                {
                    case "=":
                        if (actualValue != value) return false;
                        break;
                    case "<>":
                        if (actualValue == value) return false;
                        break;
                    case ">":
                        if (!(decimal.Parse(actualValue) > decimal.Parse(value))) return false;
                        break;
                    case "<":
                        if (!(decimal.Parse(actualValue) < decimal.Parse(value))) return false;
                        break;
                    case ">=":
                        if (!(decimal.Parse(actualValue) >= decimal.Parse(value))) return false;
                        break;
                    case "<=":
                        if (!(decimal.Parse(actualValue) <= decimal.Parse(value))) return false;
                        break;
                }
            }

            // Nếu tất cả các điều kiện đều khớp
            return true;
        }
    }
}
