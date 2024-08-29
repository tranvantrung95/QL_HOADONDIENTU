using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace HoaDonDienTu.Class
{
    internal class Gridview
    {
        private GridView _gridView;

        public Gridview(GridView gridView)
        {
            _gridView = gridView;
            InitializeGridView();
        }

        private void InitializeGridView()
        {
            //SetupColumns();
            SetupRowIndicator();
            SetupRowCountChangedEvent();
           // SetupFocusedRowChangedEvent();
            SetupMouseWheelEvent();
           // SetupCellValueChangedEvent();
            SetupCustomDrawCellEvent();
        }

        //Hàm thêm số thứ tự trong gridview
        public static bool indicatorIcon = true;
        private void SetupRowIndicator()
        {
            _gridView.CustomDrawRowIndicator += gridView_CustomDrawRowIndicator;
        }

        public static void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void SetupRowCountChangedEvent()
        {
            _gridView.RowCountChanged += gridView_RowCountChanged;
        }

        public static void gridView_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());


            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }

        private void SetupMouseWheelEvent()
        {
            _gridView.MouseWheel += gridView_MouseWheel;
        }

        public static void gridView_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((sender as GridView).IsEditing)
            {
                (sender as GridView).CloseEditor();
                (sender as GridView).UpdateCurrentRow();
            }
        }

        private void SetupCustomDrawCellEvent()
        {
            _gridView.CustomDrawCell += gridView_CustomDrawCell;
        }

        private void gridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            // Chọn cột cần bôi màu
            GridColumn gridColumn = view.Columns["ma_kh"]; // Thay "TenCot" bằng tên thực của cột

            // Kiểm tra xem cột hiện tại có phải là cột cần bôi màu hay không
            if (e.Column == gridColumn)
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

    }
}
