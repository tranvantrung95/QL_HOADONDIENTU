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
    public partial class frm_CAUHINHAPI : DevExpress.XtraEditors.XtraForm
    {
        private int apiId;
        DataTable dt_cauhinh;
        string cauhinh = "";
        string tenbang = "_cauhinhvt";
        public frm_CAUHINHAPI(int apiId)
        {
            InitializeComponent();
            this.apiId = apiId;
        }

        private void frm_CAUHINHAPI_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            loadview();
        }

        private void loadview()
        {
            string sql;
            sql = "SELECT * FROM "+ tenbang +" where apiid = "+apiId+" order by stt";
            dt_cauhinh = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_cauhinh;

            string[] columnsToShow = new string[] { "stt","viettel","batdau","effect","ketthuc","cauhinh","ghi_chu","from_alias","key_fields","kieu","dieukien","apiid" };
            string[] displayNames = new string[] { "stt", "viettel", "batdau","effect", "ketthuc", "cauhinh", "ghi_chu", "from_alias", "key_fields", "kieu", "dieukien", "apiid" };

            CustomizeGridView(gridView1, columnsToShow, displayNames);

            // Chọn cột cần enable hoặc disable
            GridColumn gridColumn = gridView1.Columns["cauhinh"]; // Thay "TenCot" bằng tên thực của cột
            GridColumn gridColumn_api = gridView1.Columns["apiid"]; // Thay "TenCot" bằng tên thực của cột
            
            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            gridColumn.OptionsColumn.AllowEdit = false;
            gridColumn.Width = 15;
            gridColumn_api.OptionsColumn.AllowEdit = false;
            gridColumn_api.Width=15;


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
            gridView1.Columns["viettel"].ColumnEdit = repositoryItemButton;

            repositoryItemButton.ButtonClick += repositoryItemButton_ButtonClick;


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

        //Hàm thay đổi độ rộng của cột theo giá trị nội dung
        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column != null)
            {
                // Lấy tên của cột
                string columnName = e.Column.FieldName;

                // Tính toán giá trị lớn nhất của cột
                int maxWidth = CalculateMaxColumnWidth(view, columnName);

                // Đặt độ rộng của cột là giá trị lớn nhất
                e.Info.Caption = e.Info.Caption.PadRight(maxWidth);
            }
        }

        private int CalculateMaxColumnWidth(GridView view, string columnName)
        {
            int maxWidth = 0;

            // Duyệt qua từng dòng để tìm giá trị lớn nhất
            for (int i = 0; i < view.RowCount; i++)
            {
                object cellValue = view.GetRowCellValue(i, columnName);

                // Kiểm tra xem giá trị có phải là số hay không
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int value))
                {
                    // Nếu là số, cập nhật giá trị lớn nhất
                    maxWidth = Math.Max(maxWidth, value.ToString().Length);
                }
            }

            // Cộng thêm một số giả định để tránh việc cột quá chật
            maxWidth += 75;

            return maxWidth;
        }

        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value_cauhinh = gridView1.GetRowCellValue(e.FocusedRowHandle, "cauhinh");
            if (value_cauhinh != null)
            {
                cauhinh = value_cauhinh.ToString().Trim(); //Lấy giá trị apiid của dòng được chọn
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
            object primaryKeyValue = gridView1.GetRowCellValue(e.RowHandle, "cauhinh");
            //MessageBox.Show(columnName + " - "+primaryKeyValue + " - "+cellValue);

            if (MessageBox.Show("Bạn có muốn cập nhật thông tin không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "";
                sql = "UPDATE "+ tenbang +" SET " + columnName + " = N'" + cellValue + "' WHERE cauhinh = " + primaryKeyValue + "";
                Class.Functions.RunSqlDel(sql);
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["cauhinh"]; // Thay "TenCot" bằng tên thực của cột
            GridColumn gridColumn_api = view.Columns["apiid"]; // Thay "TenCot" bằng tên thực của cột
           

            // Kiểm tra xem cột hiện tại có phải là cột cần bôi màu hay không
            if (e.Column == gridColumn || e.Column == gridColumn_api)
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

                            sql = "DECLARE @cauhinh INT,@stt int;\r\nSET @cauhinh = (SELECT ISNULL(MAX(cauhinh), 0) + 1 FROM "+ tenbang +" where apiid="+ apiId +");\r\nset @stt = (SELECT ISNULL(MAX(stt), 0) + 1 FROM "+ tenbang +" where apiid= "+ apiId +");";
                            sql += "INSERT INTO "+ tenbang +" (stt,viettel,batdau,effect,ketthuc,cauhinh,ghi_chu,from_alias,key_fields,kieu,dieukien,apiid,datetime) VALUES(@stt,'','','','',@cauhinh,'','','','','','"+ apiId + "',getdate())";
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
                                sql = "DELETE "+ tenbang +" WHERE cauhinh=N'" + cauhinh + "'";
                                //Class.Functions.editcode(sql); return;
                                Class.Functions.RunSqlDel(sql);
                                loadview();

                            }
                        }
                    }
                }
            }
        }
    }
}