using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
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
    public partial class frm_API : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_api;
        string apiid = "";
        public frm_API()
        {
            InitializeComponent();
        }

        private void frm_API_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            loadview();
        }

        private void loadview()
        {
            string sql;
            sql = "SELECT * FROM _APIViettel where apiid<>0 ";
            dt_api = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_api;

            // Chọn cột cần enable hoặc disable
            GridColumn gridColumn = gridView1.Columns["apiid"]; // Thay "TenCot" bằng tên thực của cột
            GridColumn gridColumn_date = gridView1.Columns["datetime"]; // Thay "TenCot" bằng tên thực của cột

            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            gridColumn.OptionsColumn.AllowEdit = false;
            gridColumn.Width = 15;
            gridColumn_date.OptionsColumn.AllowEdit = false;
            gridColumn_date.Width = 15;

            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

            //List<String> columnHeaders = new List<String>();
            //columnHeaders.Add("ApiName");
            //columnHeaders.Add("Apiid");
            //columnHeaders.Add("Mô tả");
            //columnHeaders.Add("Datetime");
            //columnHeaders.Add("f_identity");
            //columnHeaders.Add("Contentype");

            //for (int i = 0; i < gridView1.Columns.Count; i++)
            //{
            //    gridView1.Columns[i].Caption = columnHeaders[i]; // Use index i to access both grid columns and header names
            //}

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
            gridView1.Columns["apiName"].ColumnEdit = repositoryItemButton;

            repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick;

            string[] columnsToShow = new string[] { "apiName","mota", "datetime","apiid" };
            string[] displayNames = new string[] { "apiName", "Tên", "Datetime", "apiid" };

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
            object value_apiid = gridView1.GetRowCellValue(e.FocusedRowHandle, "apiid");
            if (value_apiid != null)
            {
                apiid = value_apiid.ToString().Trim(); //Lấy giá trị apiid của dòng được chọn
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
            object primaryKeyValue = gridView1.GetRowCellValue(e.RowHandle, "apiid");
            //MessageBox.Show(columnName + " - "+primaryKeyValue + " - "+cellValue);

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "";
                sql = "UPDATE _APIViettel SET " + columnName + " = N'" + cellValue + "' WHERE apiid = " + primaryKeyValue + "";
                Class.Functions.RunSqlDel(sql);
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["apiid"]; // Thay "TenCot" bằng tên thực của cột
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

                            sql = "DECLARE @apiid INT;\r\nSET @apiid = (SELECT ISNULL(MAX(apiid), 0) + 1 FROM _APIViettel);\n";
                            sql += "INSERT INTO _APIViettel (apiName,apiid,mota,datetime,ContentType) VALUES('',@apiid,'',getdate(),'')";
                            //Class.Functions.editcode(sql); return;
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
                                sql = "DELETE _APIViettel WHERE apiid=N'" + apiid + "'";
                                //Class.Functions.editcode(sql); return;
                                Class.Functions.RunSqlDel(sql);
                                loadview();

                            }
                        }
                    }
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ cột "apiid" của dòng được kích đúp
            object apiId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "apiid");

            // Kiểm tra xem giá trị có tồn tại hay không
            if (apiId != null)
            {
                int apiIdValue = Convert.ToInt32(apiId);

                // Tạo và hiển thị form
                var form = new frm_CAUHINHAPI(apiIdValue);
                form.Text = "Cấu hình API";
                if (Exitform(form)) return;
                form.MdiParent = MainView.ActiveForm;
                form.Show();
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
    }
}