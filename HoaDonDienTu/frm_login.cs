using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DevExpress.CodeParser;

namespace HoaDonDienTu
{
    public partial class frm_login : DevExpress.XtraEditors.XtraForm
    {
        private object ma_user = 0;
        public frm_login()
        {
            Class.Functions.Connect();
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private List<string> SearchData(string searchText)
        {
            List<string> searchResults = new List<string>();
            string query = "SELECT ten FROM _USERS WHERE ten LIKE N'" +searchText + "%'"; // Điều chỉnh câu truy vấn theo cơ sở dữ liệu của bạn.
            //MessageBox.Show(query);

            DataTable dt = Class.Functions.GetDataToTable(query);

            // Lặp qua DataTable và thêm từng tên vào searchResults.
            foreach (DataRow row in dt.Rows)
            {
                searchResults.Add(row["ten"].ToString());
            }
            return searchResults;
        }

        private void cbb_username_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbb_username.Text))
            {
                // Thực hiện truy vấn tìm kiếm
                var results = SearchData(cbb_username.Text);

                // Cập nhật ComboBoxEdit với kết quả tìm kiếm
                if (results.Any())
                {
                    // Ngăn việc gọi sự kiện TextChanged khi cập nhật danh sách
                    cbb_username.TextChanged -= cbb_username_TextChanged;

                    // Xóa các mục hiện có để tránh trùng lặp
                    cbb_username.Properties.Items.Clear();

                    // Thêm các mục tìm kiếm vào ComboBoxEdit
                    foreach (var result in results)
                    {
                        cbb_username.Properties.Items.Add(result);
                    }

                    // Mở dropdown list để hiển thị kết quả
                    cbb_username.ShowPopup();

                    // Kết nối lại sự kiện TextChanged sau khi cập nhật
                    cbb_username.TextChanged += cbb_username_TextChanged;
                }
                else
                {
                    // Nếu không có kết quả, đóng popup
                    cbb_username.ClosePopup();
                }
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            // Hiển thị splash screen
            SplashScreenManager.ShowDefaultWaitForm("Vui lòng đợi", "Đang đăng nhập...");

            // Lấy thông tin người dùng nhập vào
            string username = cbb_username.Text;
            string password = txt_pass.Text; // Giả sử bạn đã mã hóa mật khẩu

            // Kiểm tra thông tin đăng nhập
            if (IsValidLogin(username, password))
            {
                // Tạo instance của UserPass và gán giá trị
                UserPass userPass = new UserPass
                {
                    username = username,
                    password = password,
                    ma_user = ma_user.ToString()
                };

                // Đường dẫn đến file lưu tên đăng nhập
                string filePath = Path.Combine(Application.StartupPath, "usertruoc.txt");
                string filePath_ma_user = Path.Combine(Application.StartupPath, "ma_user.txt");
                // Lưu tên đăng nhập vào file
                File.WriteAllText(filePath, username);
                File.WriteAllText(filePath_ma_user, ma_user.ToString());

                // Nếu bạn đang thực hiện xử lý không đồng bộ, thay thế Sleep bằng xử lý thực tế
                Task.Run(() =>
                {
                    // Thực hiện xử lý đăng nhập thực tế ở đây
                    // System.Threading.Thread.Sleep(3000);
                    // Bỏ Sleep và thay thế bằng logic kiểm tra đăng nhập thực tế của bạn
                })
                .ContinueWith(t =>
                {
                    // Mở form chính sau khi đăng nhập thành công
                    this.Invoke(new Action(() =>
                    {
                        MainView form_main = new MainView(userPass); // Truyền userPass vào constructor
                        this.Hide();
                        form_main.ShowDialog();
                        // Đóng splash screen khi việc xử lý đã hoàn tất
                        SplashScreenManager.CloseForm(false);
                        this.Show();

                    }));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                // Đóng splash screen khi đăng nhập không thành công
                SplashScreenManager.CloseForm(false);

                // Hiển thị thông báo lỗi
                XtraMessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsValidLogin(string username, string password)
        {
            // Thực hiện truy vấn đến cơ sở dữ liệu để kiểm tra thông tin đăng nhập
            string query = "SELECT COUNT(1) FROM _USERS WHERE ten=@username AND dbo.Encrypt(pwd)=@password";

                using (SqlCommand cmd = new SqlCommand(query, Class.Functions.Con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // Nên sử dụng mã hóa mật khẩu

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    ma_user = Class.Functions.SQLEXEC("SELECT ma_user FROM _USERS WHERE ten=N'"+username+"' AND dbo.Encrypt(pwd)='"+ password +"'", true);
                    return result > 0;
                }
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            // Đối với một TextEdit từ DevExpress
            cbb_username.Properties.NullValuePrompt = "Nhập tên người dùng";
            cbb_username.Properties.NullValuePromptShowForEmptyValue = true;

            txt_pass.Properties.NullValuePrompt = "Nhập mật khẩu người dùng";
            txt_pass.Properties.NullValuePromptShowForEmptyValue = true;

            // Đường dẫn đến file lưu tên đăng nhập
            string filePath = Path.Combine(Application.StartupPath, "usertruoc.txt");

            // Kiểm tra xem file có tồn tại không
            if (File.Exists(filePath))
            {
                // Đọc tên đăng nhập từ file
                string lastUsername = File.ReadAllText(filePath);

                // Kiểm tra nếu ComboBoxEdit đã có DataSource, bạn cần đảm bảo rằng giá trị này tồn tại trong DataSource đó
                if (cbb_username.Properties.Items.Contains(lastUsername))
                {
                    // Đặt tên đăng nhập vào ComboBoxEdit
                    cbb_username.EditValue = lastUsername;
                }
                else
                {
                    // Nếu không tồn tại trong DataSource, bạn có thể thêm vào
                    cbb_username.Properties.Items.Add(lastUsername);
                    cbb_username.EditValue = lastUsername;
                }
            }
        }

        private void imageSlider1_Click(object sender, EventArgs e)
        {

        }

        private void txt_pass_Enter(object sender, EventArgs e)
        {

        }

        private void txt_pass_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra nếu phím nhấn là Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Thực hiện hành động của nút đăng nhập
                btn_login.PerformClick();
            }
        }
    }
}