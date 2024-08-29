using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HoaDonDienTu
{
    public partial class frm_USERS : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_users;
        string ma_user = "";
        string giatridoc = "";
        Cls_EnCrypting ecr = new Cls_EnCrypting();
        private EFF_EFFE_200Entities _entities;
        public frm_USERS()
        {
            InitializeComponent();
        }

        private void frm_USERS_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            loadview();
        }

        private void loadview()
        {
            // Xác định đường dẫn của file. Ví dụ, sử dụng đường dẫn nơi ứng dụng được khởi động
            string filePath = Path.Combine(Application.StartupPath, "ma_user.txt");

            try
            {
                // Đọc toàn bộ nội dung của file
                string fileContent = File.ReadAllText(filePath);

                // Nếu bạn muốn hiển thị nội dung file, ví dụ:
                Console.WriteLine(fileContent);

                // Nếu bạn muốn sử dụng giá trị này trong chương trình, chỉ cần gán nó cho một biến
                // Ví dụ, đây là biến chứa nội dung của file
                giatridoc = fileContent;

                // Tiếp tục sử dụng myVariable cho các mục đích khác trong ứng dụng của bạn
            }
            catch (Exception ex)
            {
                // Trường hợp xảy ra lỗi khi đọc file (ví dụ: file không tồn tại)
                Console.WriteLine("Có lỗi xảy ra khi đọc file: " + ex.Message);
            }

            string sql;
            if (!string.IsNullOrWhiteSpace(giatridoc) && giatridoc.ToString().Trim() == "1")
            {
                sql = "SELECT ma, ten, ghi_chu, pwd, ma_user,datetime FROM _USERS where ma_user<>0";
            }
            else
            {
                sql = "SELECT ma, ten, ghi_chu, pwd, ma_user,datetime FROM _USERS where ma_user<>0 and ma_user="+ giatridoc.Trim() +"";
            }    

            dt_users = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_users;

            //gridView1.BestFitColumns(); //Tự động căn độ rộng cột

            // Chọn cột cần enable hoặc disable
            GridColumn gridColumn = gridView1.Columns["ma_user"]; // Thay "TenCot" bằng tên thực của cột
            GridColumn gridColumn_date = gridView1.Columns["datetime"]; // Thay "TenCot" bằng tên thực của cột

            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            gridColumn.OptionsColumn.AllowEdit = false;
            gridColumn.Width = 20;
            gridColumn_date.OptionsColumn.AllowEdit = false;
            gridColumn_date.Width = 30;

           

            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

            // căn tiêu đề cột ở giữa cho tất cả các cột
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
            {
                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            //// Tạo RepositoryItemButtonEdit để thêm nút chức năng thêm dòng cho dòng
            RepositoryItemButtonEdit repositoryItemButton = new RepositoryItemButtonEdit();
            repositoryItemButton.Buttons.Clear();

            EditorButton button_them = new EditorButton(ButtonPredefines.Plus);
            EditorButton button_xoa = new EditorButton(ButtonPredefines.Delete);

            button_them.Caption = "Thêm dòng";
            //button_them.Kind = ButtonPredefines.Plus;
            button_them.Appearance.BackColor = Color.Green;
            button_xoa.Tag = "AddRow"; // Thêm tag để phân biệt giữa các nút

            button_xoa.Caption = "Xóa dòng";
            //button_xoa.Kind = ButtonPredefines.Delete;
            button_xoa.Appearance.BackColor = Color.Red;
            button_xoa.Tag = "DeleteRow"; // Thêm tag để phân biệt giữa các nút

            repositoryItemButton.Buttons.Add(button_them);
            repositoryItemButton.Buttons.Add(button_xoa);

            // Gắn RepositoryItemButtonEdit vào cột "apiName"
            gridView1.Columns["ma"].ColumnEdit = repositoryItemButton;
    
            int ma_user = Convert.ToInt32(giatridoc.Trim());

            DataTable dt_in = Class.Functions.GetDataToTable("SELECT * FROM tblUserFuntions AS a INNER JOIN tblFunctions AS b ON a.Menu = b.Menu WHERE b.Application='PQND' AND a.ma_user="+ ma_user +"");

            foreach (DataRow row in dt_in.Rows) // Sử dụng DataRow thay vì var
            {
                // Giả sử AllowAddNew là một cột trong DataTable và kiểu dữ liệu là bit hoặc boolean
                if ((bool)row["AllowAddNew"]) // Ép kiểu giá trị cột "AllowAddNew" về bool
                {
                    repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick;
                    break; // Thêm break nếu bạn chỉ muốn gán sự kiện một lần khi điều kiện đầu tiên được thỏa mãn
                }
            }


            //repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick;

            RepositoryItemButtonEdit repositoryItemButton1 = new RepositoryItemButtonEdit();
            repositoryItemButton1.Buttons.Clear();

            //Tạo nút Xóa nháp
            Image originalImage1 = Properties.Resources.show_32x32;
            Image resizedImage1 = ResizeImage(originalImage1, 24, 24); // Thay đổi 16, 16 theo kích thước bạn muốn
            repositoryItemButton1.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)
            {
                Caption = "Button1",
                ToolTip = "Xem mật khẩu",
                Image = resizedImage1
            });

            gridView1.Columns["pwd"].ColumnEdit = repositoryItemButton1;
            repositoryItemButton1.ButtonClick += repositoryItemButton1_ButtonClick;

            string[] columnsToShow = new string[] { "ma", "ten", "pwd", "ghi_chu", "ma_user", "datetime" };
            string[] displayNames = new string[] { "Mã", "Tên", "Mật khẩu", "Ghi chú", "MA_USER", "datetime" };

            CustomizeGridView(gridView1, columnsToShow, displayNames);

        }

        //Hàm xử lý hiển thị cột lên gridview và sắp xếp cột
        private void CustomizeGridView(GridView gridView, string[] columnsToShow, string[] displayNames)
        {
            List<GridColumn> visibleColumns = new List<GridColumn>();

            for (int i = 0; i < columnsToShow.Length; i++)
            {
                string columnName = columnsToShow[i];
                string displayName = displayNames.Length > i ? displayNames[i] : columnName; // Use default name if display name is not provided

                // Find the corresponding column in the GridView
                // GridColumn column = gridView.Columns.ColumnByFieldName(columnName);

                GridColumn column = gridView.Columns
           .Cast<GridColumn>()
           .FirstOrDefault(col => string.Equals(col.FieldName, columnName, StringComparison.OrdinalIgnoreCase));


                // If the column is found, add it to the list and set the display name
                if (column != null)
                {
                    visibleColumns.Add(column);
                    column.Caption = displayName;
                }
            }

            gridView.Columns.Clear();

            foreach (GridColumn column in visibleColumns)
            {
                column.VisibleIndex = gridView.Columns.Count;
                column.Visible = true;
                gridView.Columns.Add(column);

            }
        }

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
            object value_ma_user = gridView1.GetRowCellValue(e.FocusedRowHandle, "ma_user");
            if (value_ma_user != null)
            {
                ma_user = value_ma_user.ToString().Trim(); //Lấy giá trị ma_user của dòng được chọn
            }
        }

        private void gridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((sender as GridView).IsEditing)
            {
                (sender as GridView).CloseEditor();
                (sender as GridView).UpdateCurrentRow();
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            object columnName = e.Column.FieldName;
            object cellValue = gridView1.GetRowCellValue(e.RowHandle, e.Column);
            object primaryKeyValue = gridView1.GetRowCellValue(e.RowHandle, "ma_user");
            int index = e.RowHandle;
            //MessageBox.Show(columnName + " - "+primaryKeyValue + " - "+cellValue);

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "";
                if (columnName.ToString() == "pwd")
                {
                    sql = "UPDATE _users SET " + columnName + " = N'" + ecr.EnCrypt(cellValue.ToString()) + "' WHERE ma_user = " + primaryKeyValue + "";

                }
                else
                {
                    sql = "UPDATE _users SET " + columnName + " = N'" + cellValue + "' WHERE ma_user = " + primaryKeyValue + "";
                }
                Class.Functions.RunSqlDel(sql);
            }
            loadview();
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["ma_user"]; // Thay "TenCot" bằng tên thực của cột
            GridColumn gridColumn_date = view.Columns["datetime"]; // Thay "TenCot" bằng tên thực của cột

            // Kiểm tra xem cột hiện tại có phải là cột cần bôi màu hay không
            if (e.Column == gridColumn || e.Column == gridColumn_date)
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

        private void repositoryItemButton_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            string sql;
            if (sender is ButtonEdit buttonEdit)
            {
                // Lấy GridControl chứa GridView
                GridControl gridControl = buttonEdit.Parent as GridControl;

                if (gridControl != null)
                {
                    // Lấy GridView từ GridControl
                    GridView gridView = gridControl.MainView as GridView;
                    if (e.Button.Kind == ButtonPredefines.Plus)
                    {

                        if (XtraMessageBox.Show("Bạn có muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            gridView1.FocusedRowHandle = gridView1.DataRowCount - 1;
                            gridView1.ShowEditor();

                            sql = "DECLARE @ma_user INT;SET @ma_user = (SELECT ISNULL(MAX(ma_user), 0) + 1 FROM _users);";
                            sql += "INSERT INTO _users (ma, ten, GHI_CHU, PWD, MA_USER,datetime) VALUES('','','','',@ma_user,getdate())";
                           // Class.Functions.editcode(sql);
                            Class.Functions.RunSQL(sql); //Thực hiện câu lệnh sql
                            loadview(); //Nạp lại DataGridView
                        }
                    }
                    if (e.Button.Kind == ButtonPredefines.Delete)
                    {
                        if (gridView1.FocusedRowHandle >= 0)
                        {
                            if (XtraMessageBox.Show("Bạn có muốn xóa dòng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                sql = "DELETE _users WHERE ma_user=N'" + ma_user + "'";
                                //Class.Functions.editcode(sql); return;
                                Class.Functions.RunSqlDel(sql);
                                loadview();

                            }
                        }
                    }
                }
            }
        }

        private void repositoryItemButton1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (sender is ButtonEdit buttonEdit)
            {
                // Lấy GridControl chứa GridView
                GridControl gridControl = buttonEdit.Parent as GridControl;

                if (gridControl != null)
                {
                    // Lấy GridView từ GridControl
                    GridView gridView = gridControl.MainView as GridView;

                    if (gridView != null)
                    {
                        int rowHandle = gridView.FocusedRowHandle;

                        if (rowHandle >= 0)
                        {
                            if (e.Button == buttonEdit.Properties.Buttons[0])
                            {
                                //DialogResult dialogResult = XtraMessageBox.Show("Bạn có muốn xem không?", "Xác nhận", MessageBoxButtons.YesNo);
                                //if (dialogResult == DialogResult.Yes)
                                //{
                                    object ma_users = gridView.GetRowCellValue(rowHandle, "ma_user");
                                    object str_pwd = Class.Functions.SQLEXEC("SELECT dbo.Encrypt(pwd) FROM _USERS where ma_user="+ ma_users.ToString().Trim() +"",true);
                                    XtraMessageBox.Show(str_pwd.ToString());
                                //}
                                //else if (dialogResult == DialogResult.No)
                                //{
                                //    return;
                                //}
                            }
                        }
                    }
                }
            }
        }

        //Kiểm tra xem form đã tồn tại chưa
        private bool Exitform(Form form)
        {
            foreach (var child in MdiChildren)
            {
                if (child.Name == form.Name)
                {
                    child.Activate();
                    return true;
                }
            }
            return false;
        }

        // Hàm thay đổi kích thước hình ảnh
        public static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}