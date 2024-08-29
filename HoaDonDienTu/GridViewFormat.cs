using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace HoaDonDienTu
{
    public class GridViewFormat
    {
        public static void CustomizeGridView(GridView gridView)
        {

            // Thêm event handler cho CustomDrawRowIndicator
            gridView.CustomDrawRowIndicator += gridView_CustomDrawRowIndicator;
        }

        //Hàm thêm số thứ tự trong gridview
        private static void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                string sText = (e.RowHandle + 1).ToString();
                Graphics gr = e.Info.Graphics;
                gr.PageUnit = GraphicsUnit.Pixel;
                SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 20;
                if (view.IndicatorWidth < nNewSize)
                {
                    view.IndicatorWidth = nNewSize;
                }

                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                e.Info.DisplayText = sText;
            }
            if (!view.OptionsView.ShowIndicator || e.RowHandle != GridControl.InvalidRowHandle)
                e.Info.ImageIndex = -1;

            if (e.RowHandle == GridControl.InvalidRowHandle)
            {
                Graphics gr = e.Info.Graphics;
                gr.PageUnit = GraphicsUnit.Pixel;
                SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 20;
                if (view.IndicatorWidth < nNewSize)
                {
                    view.IndicatorWidth = nNewSize;
                }

                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                e.Info.DisplayText = "STT";
            }
        }

    }
}
