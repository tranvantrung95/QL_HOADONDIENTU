using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting.Preview;
using DevExpress.CodeParser;
using Microsoft.SqlServer.Management.Smo;
using DevExpress.XtraBars.Docking2010.Views.NativeMdi;
using DevExpress.XtraReports.UI;
using System.Drawing.Printing;
using System.Security.Cryptography;
using DevExpress.XtraReports.Native;
using DevExpress.XtraRichEdit.Layout;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.Diagram.Core.Themes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using DevExpress.Text.Fonts;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraSplashScreen;
using System.Diagnostics;
using DevExpress.DataAccess.Native.Data;
using DataTable = System.Data.DataTable;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using System.Globalization;
using DevExpress.LookAndFeel;


namespace HoaDonDienTu
{
    public partial class frm_baocao : DevExpress.XtraEditors.XtraForm
    {
        public frm_baocao()
        {
            InitializeComponent();
        }
        public string SqlQuery { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

        string font = "Times New Roman";
        private void frm_baocao_Load(object sender, EventArgs e)
        {
            //drong_grid(gridView1);
            gridView1.BestFitColumns();
            //gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
            GridViewFormat.CustomizeGridView(gridView1);
            
        }

        public void drong_grid(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            Font font1 = new Font(""+ font +"", 10); // Font dùng để vẽ
            using (Graphics graphics = Graphics.FromHwnd(gridView.GridControl.Handle)) // Sử dụng using để tự động giải phóng tài nguyên
            {
                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView.Columns)
                {
                    float maxWidth = graphics.MeasureString(col.GetCaption(), font1).Width; // Bắt đầu từ độ rộng của tiêu đề cột

                    for (int i = 0; i < gridView.DataRowCount; i++) // Duyệt qua mỗi hàng
                    {
                        object cellValue = gridView.GetRowCellValue(i, col);
                        if (cellValue != null)
                        {
                            string cellValueString = cellValue.ToString();
                            SizeF size = graphics.MeasureString(cellValueString, font1);
                            maxWidth = Math.Max(maxWidth, size.Width); // Chọn giá trị lớn nhất giữa maxWidth và kích thước mới
                        }
                    }

                    int padding = 20; // Khoảng padding
                    col.Width = (int)maxWidth + padding; // Áp dụng độ rộng cột mới
                }
            }
        }

        public void DisplayData(string sqlCommand)
        {
            try
            {
                DataTable dt = Class.Functions.GetDataToTable(sqlCommand);
                if (dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt;

                    string[] columnsToShow = new string[] { "mau_hd", "serihd", "sohd", "phi_tm", "tr5", "tr6", "nhom_vat", "dso", "tienthue", "ktdb", "ghi_chu" };
                    string[] displayNames = new string[] { "Mẫu HĐ", "Ký hiệu", "Số HĐ", "Ngày HĐ", "Tên người mua hàng", "Mã số thuế người mua", "Mặt hàng", "Doanh số bán chưa có thuế", "Tiền thuế", "Thuế suất", "Ghi chú" };

                    CustomizeGridView(gridView1, columnsToShow, displayNames);

                    // Duyệt qua tất cả các cột trong DataTable
                    foreach (System.Data.DataColumn dataColumn in dt.Columns)
                    {
                        // Lấy cột tương ứng trong GridView
                        var gridColumn = gridView1.Columns[dataColumn.ColumnName];

                        if (gridColumn != null)
                        {
                            // Kiểm tra kiểu dữ liệu của cột
                            if (dataColumn.DataType == typeof(decimal) || dataColumn.DataType == typeof(double) ||
                                dataColumn.DataType == typeof(float) || dataColumn.DataType == typeof(int) ||
                                dataColumn.DataType == typeof(long) || dataColumn.DataType == typeof(short) ||
                                dataColumn.DataType == typeof(byte) || dataColumn.DataType == typeof(uint) ||
                                dataColumn.DataType == typeof(ulong) || dataColumn.DataType == typeof(ushort) ||
                                dataColumn.DataType == typeof(sbyte))
                            {
                                // Cài đặt định dạng hiển thị cho cột kiểu số
                                gridColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                gridColumn.DisplayFormat.FormatString = "n0"; // Định dạng số với dấu phân cách hàng nghìn, không có số thập phân
                            }
                        }
                    }

                    // Bắt sự kiện CustomColumnDisplayText của gridView1
                    gridView1.CustomColumnDisplayText += (sender, e) => {
                        if (e.Column.ColumnType == typeof(decimal) || e.Column.ColumnType == typeof(double) ||
                            e.Column.ColumnType == typeof(float) || e.Column.ColumnType == typeof(int) ||
                            e.Column.ColumnType == typeof(long) || e.Column.ColumnType == typeof(short) ||
                            e.Column.ColumnType == typeof(byte) || e.Column.ColumnType == typeof(uint) ||
                            e.Column.ColumnType == typeof(ulong) || e.Column.ColumnType == typeof(ushort) ||
                            e.Column.ColumnType == typeof(sbyte))
                        {
                            if (e.Value != null)
                            {
                                decimal value;
                                if (decimal.TryParse(e.Value.ToString(), out value))
                                {
                                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                                    ci.NumberFormat.NumberGroupSeparator = " ";
                                    e.DisplayText = value.ToString("N0", ci);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
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

        //Hàm thêm số thứ tự trong gridview
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

        private void gridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((sender as GridView).IsEditing)
            {
                (sender as GridView).CloseEditor();
                (sender as GridView).UpdateCurrentRow();
            }
        }

        private void btn_xem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                string sql = SqlQuery;

                DataTable dataTable = Class.Functions.GetDataToTable(sql); // Hàm để lấy dữ liệu từ câu lệnh SQL

                var columnMappings = new Dictionary<string, string>
                    {
                        { "Mẫu HĐ", "mau_hd" },
                        { "Ký hiệu", "serihd" },
                        { "Số HĐ", "sohd" },
                        {"Ngày HĐ","phi_tm"},
                        {"Tên người mua hàng","tr5" },
                        {"Mã số thuế","tr6" },
                        {"Mặt hàng","nhom_vat" },
                        {"Doanh thu trước thuế","dso" },
                        {"Tiền thuế","tienthue" },
                        {"Thuế suất","ktdb" },
                        {"Ghi chú","ghi_chu" }
                        // Thêm các cặp tiêu đề và tên cột tương ứng khác
                    };


                //tạo báo cáo trên một thread không phải là UI thread
                XtraReport report = CreateReportWithTable(dataTable, columnMappings);

                // Tạo một ReportPrintTool mới và gửi báo cáo vào đó
                DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(report);

                // Hiển thị cửa sổ xem trước báo cáo
                printTool.ShowPreview(UserLookAndFeel.Default);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        public float do_rong_ky_tu(Font f, string kytu)
        {
            Image fakeImage = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(fakeImage);
            SizeF size = graphics.MeasureString(kytu, f);
            return size.Width;
        }

        public XtraReport CreateReportWithTable(DataTable data, Dictionary<string, string> columnMappings)
        {
           
            XtraReport report = new XtraReport
            {
                Landscape = true,
                PaperKind = System.Drawing.Printing.PaperKind.A4,
                Margins = new Margins(70, 50, 50, 50),
            };

            //Đầu báo cáo
            #region
            // Tạo ReportHeaderBand nếu nó chưa được thêm vào report
            ReportHeaderBand reportHeaderBand = new ReportHeaderBand()
            {
                HeightF = 0,
            };
            report.Bands.Add(reportHeaderBand);

            // Tạo XRLabel cho tên công ty
            XRLabel companyNameLabel = new XRLabel();
            companyNameLabel.Text = "CÔNG TY CỔ PHẦN PHẦN MỀM T&T";
            companyNameLabel.Font = new Font(""+ font +"", 12, FontStyle.Bold);
            // Căn lề trái và vị trí bắt đầu từ lề trái của trang
            companyNameLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // Đặt vị trí X tương ứng với lề trái của trang
            companyNameLabel.LocationF = new PointF(0, 0);
            // Chiều rộng phải rộng hơn hoặc bằng phần còn lại của trang sau khi trừ lề
            companyNameLabel.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            companyNameLabel.HeightF = 25f; // Đặt chiều cao tùy chỉnh hoặc dựa trên nội dung

            reportHeaderBand.Controls.Add(companyNameLabel);

            // Tạo XRLabel cho địa chỉ
            XRLabel addressLabel = new XRLabel();
            addressLabel.Text = "P502, tòa nhà Viễn Đông, 36 Hoàng Cầu, Phường Ô Chợ Dừa, Quận Đống Đa, Hà Nội";
            addressLabel.Font = new Font("" + font + "", 10);
            // Căn lề trái và vị trí bắt đầu từ lề trái của trang
            addressLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // Đặt vị trí X tương ứng với lề trái của trang
            addressLabel.LocationF = new PointF(0, 0);
            // Chiều rộng phải rộng hơn hoặc bằng phần còn lại của trang sau khi trừ lề
            addressLabel.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            addressLabel.HeightF = 25f; // Đặt chiều cao tùy chỉnh hoặc dựa trên nội dung
            addressLabel.LocationF = new PointF(addressLabel.LocationF.X, companyNameLabel.BottomF);

            reportHeaderBand.Controls.Add(addressLabel);


            XRLabel titleLabel = new XRLabel();
            titleLabel.Text = "BẢNG KÊ HÓA ĐƠN HÀNG HÓA DỊCH VỤ BÁN RA";
            titleLabel.Font = new Font("" + font + "", 14, FontStyle.Bold);
            // Đặt vị trí X ở giữa trang, trừ đi lề trái và chia đôi
            titleLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter; // Căn giữa chính xác
            titleLabel.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right; // Đảm bảo chiều rộng đủ để chứa tiêu đề
            titleLabel.HeightF = 25f; // Đặt chiều cao phù hợp
            titleLabel.LocationF = new PointF(titleLabel.LocationF.X, addressLabel.BottomF + 20);

            reportHeaderBand.Controls.Add(titleLabel);


            XRLabel timeFrameLabel = new XRLabel();
            timeFrameLabel.Text = "Từ ngày "+ fromDate.ToString("dd/MM/yyyy") +" đến ngày "+ toDate.ToString("dd/MM/yyyy");
            timeFrameLabel.Font = new Font("" + font + "", 10, FontStyle.Bold | FontStyle.Italic);
            // Đặt vị trí X ở giữa trang
            timeFrameLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter; // Căn giữa chính xác
            // Chiều rộng bằng chiều rộng có thể in của trang
            timeFrameLabel.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            // Đặt chiều cao phù hợp, chẳng hạn như 25f, hoặc tính toán dựa trên nội dung
            timeFrameLabel.HeightF = 25f;
            // Điều chỉnh vị trí Y dựa trên BottomF của titleLabel và thêm khoảng cách nếu cần
            timeFrameLabel.LocationF = new PointF(timeFrameLabel.LocationF.X, titleLabel.BottomF+5);

            reportHeaderBand.Controls.Add(timeFrameLabel);


            XRLabel dvtLabel = new XRLabel();
            dvtLabel.Text = "Đơn vị tính: VNĐ";
            dvtLabel.Font = new Font("" + font + "", 10, FontStyle.Italic);
            // Đặt vị trí X ở giữa trang
            dvtLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // Chiều rộng bằng chiều rộng có thể in của trang
            dvtLabel.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            // Đặt chiều cao phù hợp, chẳng hạn như 25f, hoặc tính toán dựa trên nội dung
            dvtLabel.HeightF = 25f;
            // Điều chỉnh vị trí Y dựa trên BottomF của titleLabel và thêm khoảng cách nếu cần
            dvtLabel.LocationF = new PointF(dvtLabel.LocationF.X, timeFrameLabel.BottomF + 10);

            reportHeaderBand.Controls.Add(dvtLabel);

            #endregion
            //Kết thúc đầu báo cáo

            //Chi tiết báo cáo
            #region
            PageHeaderBand pageHeaderBand = new PageHeaderBand()
            {
                HeightF = 0,
            };
            report.Bands.Add(pageHeaderBand);
            

            DetailBand detailBand = new DetailBand
            {
                HeightF = 0,
            };
            report.Bands.Add(detailBand);

            // Tạo TopMarginBand
            TopMarginBand topMarginBand = new TopMarginBand()
            {
                HeightF = report.Margins.Top,
            };

            // Tạo XRLabel cho tiêu đề bảng kê
            XRLabel titleheader = new XRLabel
            {
                Text = "BẢNG KÊ HÓA ĐƠN HÀNG HÓA DỊCH VỤ BÁN RA",
                Font = new Font("" + font + "", 12),
                ForeColor = Color.Red,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                SizeF = new SizeF(report.PageWidth,0), // Chiều cao của XRLabel có thể điều chỉnh theo ý muốn
                LocationF = new PointF(0, 27) // Đặt ở vị trí trên cùng của TopMarginBand
            };

            // Thêm XRLabel vào TopMarginBand
            topMarginBand.Controls.Add(titleheader);

            // Thêm TopMarginBand vào report
            report.Bands.Add(topMarginBand);

            titleheader.BeforePrint += (sender, e) => {
                // Chỉ hiển thị titleheader từ trang thứ hai trở đi
                titleheader.Visible = (report.PrintingSystem.Document.PageCount > 0);
            };



            XRTable headerTable = new XRTable();
            headerTable.BeginInit();
            headerTable.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            headerTable.LocationF = new PointF(0, 0);
            XRTableRow headerRow = new XRTableRow();
            headerTable.Rows.Add(headerRow);

            XRTable detailTable = new XRTable();
            detailTable.BeginInit();
            detailTable.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
            detailTable.LocationF = new PointF(0, 0);
            XRTableRow detailRow = new XRTableRow();
            detailTable.Rows.Add(detailRow);


            Font font1 = new Font("" + font + "", 10);
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                foreach (var mapping in columnMappings)
                {
                    float maxWidth = 0;
                    // Kiểm tra xem liệu giá trị của cột có phải là số hay không
                    // Lấy kiểu dữ liệu của cột từ DataTable
                    Type columnType = data.Columns[mapping.Value].DataType;

                    // Kiểm tra xem cột có phải là kiểu số hay không
                    bool isNumericColumn = columnType == typeof(decimal) || columnType == typeof(int) ||
                                           columnType == typeof(double) || columnType == typeof(float) ||
                                           columnType == typeof(long) || columnType == typeof(short) ||
                                           columnType == typeof(byte) || columnType == typeof(uint) ||
                                           columnType == typeof(ulong) || columnType == typeof(ushort) ||
                                           columnType == typeof(sbyte);
                    

                    //float maxWidth = g.MeasureString(mapping.Key, font).Width;
                     maxWidth = do_rong_ky_tu(font1, mapping.Key);

                    // Kiểm tra xem cột có chứa giá trị nào không
                    bool columnHasValue = data.AsEnumerable().Any(row => !string.IsNullOrEmpty(row[mapping.Value]?.ToString()));
                    if (!columnHasValue) continue;

                    foreach (DataRow row in data.Rows)
                    {
                        string text = row[mapping.Value]?.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            //maxWidth = Math.Max(maxWidth, g.MeasureString(text, font).Width);
                            maxWidth = do_rong_ky_tu(font1, text);
                        }
                    }

                    XRTableCell headerCell = new XRTableCell
                    {
                        Text = mapping.Key,
                        WidthF = maxWidth + 10, // Thêm một chút padding
                        Font = new Font("" + font + "", 10, FontStyle.Bold), // Đặt phông chữ in đậm
                        BackColor = Color.LightSkyBlue
                    };
                    headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    headerRow.Cells.Add(headerCell);
                    headerRow.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter; // Căn phải cho các cột
                    

                    XRTableCell detailCell = new XRTableCell
                    {
                        WidthF = maxWidth + 10, // Đồng nhất với header
                    };

                    if (isNumericColumn)
                    {
                        maxWidth += 20;
                        detailCell.WordWrap = false; // Ngăn không cho nội dung của cell được gói
                        detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight; // Căn phải cho các giá trị số

                        //// Thiết lập CultureInfo để sử dụng khoảng trắng làm dấu phân cách hàng nghìn
                        //System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                        //customCulture.NumberFormat.NumberGroupSeparator = " ";
                        //System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                        // Sau đó sử dụng định dạng số như trên
                        detailCell.TextFormatString = "{0:#,##0}";
                    }
                    else
                    {
                        detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    }    

                    detailCell.DataBindings.Add("Text", null, mapping.Value);
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    detailRow.Cells.Add(detailCell);
                }
            }

            headerTable.EndInit();
            pageHeaderBand.Controls.Add(headerTable);

            detailTable.EndInit();
            detailBand.Controls.Add(detailTable);

            PageFooterBand pageFooterBand = new PageFooterBand
            {
                HeightF = 20f, // Hoặc chiều cao phù hợp nếu bạn muốn thêm nội dung khác vào footer
            };
            report.Bands.Add(pageFooterBand);


            XRPageInfo pageInfo = new XRPageInfo
            {
                Format = "Trang {0} / {1}", // Định dạng số trang, ví dụ: "Trang 1 của 5"
                LocationF = new PointF(report.PageWidth - report.Margins.Right - 180f, 5), // Cần điều chỉnh để phù hợp
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight, // Căn phải số trang
                PageInfo = DevExpress.XtraPrinting.PageInfo.NumberOfTotal, // Hiển thị số trang và tổng số trang
                SizeF = new SizeF(100f, 20f), // Kích thước phù hợp để chứa text
                                              // Đặt font và màu nền nếu cần
                Font = new Font("" + font + "", 10f),
                Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0), // Padding có thể được điều chỉnh
            };
            pageFooterBand.Controls.Add(pageInfo);
            #endregion
            //Kết thúc chi tiết báo cáo

            //Cuối báo cáo
            #region
            // Tạo ReportFooterBand
            ReportFooterBand reportFooterBand = new ReportFooterBand
            {
                HeightF = 150f // Điều chỉnh chiều cao phù hợp với nội dung của bạn
            };
            report.Bands.Add(reportFooterBand);

            float reportContentWidth = report.PageWidth - report.Margins.Left - report.Margins.Right;
            float labelWidth = reportContentWidth / 3;

            // Tạo các XRLabel cho thông tin chữ ký
            XRLabel lblNguoiLapBieu = new XRLabel
            {
                Text = "Người lập biểu",
                WidthF = labelWidth,
                HeightF = 15f, // Điều chỉnh chiều cao nếu cần
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Font = new Font("" + font + "", 10, FontStyle.Bold),
                LocationF = new PointF(0, 70) // Vị trí bắt đầu từ lề trái
            };
            reportFooterBand.Controls.Add(lblNguoiLapBieu);

            // Tạo các XRLabel cho thông tin chữ ký
            XRLabel lblNguoiLapBieuSign = new XRLabel
            {
                Text = "(Ký, họ tên)",
                WidthF = labelWidth,
                HeightF = 15f, // Điều chỉnh chiều cao nếu cần
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Font = new Font(""+ font +"", 10, FontStyle.Italic),
                LocationF = new PointF(Location.X, lblNguoiLapBieu.BottomF) // Vị trí bắt đầu từ lề trái
            };
            reportFooterBand.Controls.Add(lblNguoiLapBieuSign);

            XRLabel lblKeToanTruong = new XRLabel
            {
                Text = "Kế toán trưởng",
                WidthF = labelWidth,
                HeightF = 15f,
                Font = new Font(""+ font +"", 10, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                LocationF = new PointF(labelWidth, 70) // Vị trí bắt đầu từ phần cuối của label trước
            };
            reportFooterBand.Controls.Add(lblKeToanTruong);

            // Tạo các XRLabel cho thông tin chữ ký
            XRLabel lblKeToanTruongSign = new XRLabel
            {
                Text = "(Ký, họ tên)",
                WidthF = labelWidth,
                HeightF = 15f, // Điều chỉnh chiều cao nếu cần
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Font = new Font(""+ font +"", 10, FontStyle.Italic),
                LocationF = new PointF(lblKeToanTruong.Location.X, lblKeToanTruong.BottomF) // Vị trí bắt đầu từ lề trái
            };
            reportFooterBand.Controls.Add(lblKeToanTruongSign);

            XRLabel lblGiamDoc = new XRLabel
            {
                Text = "Giám đốc",
                WidthF = labelWidth,
                HeightF = 15f,
                Font = new Font(""+ font +"", 10, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                LocationF = new PointF(labelWidth * 2, 70) // Vị trí bắt đầu từ phần cuối của label thứ hai
            };
            reportFooterBand.Controls.Add(lblGiamDoc);

            // Tạo các XRLabel cho thông tin chữ ký
            XRLabel lblGiamDocSign = new XRLabel
            {
                Text = "(Ký, họ tên, đóng dấu)",
                WidthF = labelWidth,
                HeightF = 15f, // Điều chỉnh chiều cao nếu cần
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Font = new Font(""+ font +"", 10, FontStyle.Italic),
                LocationF = new PointF(lblGiamDoc.Location.X, lblGiamDoc.BottomF) // Vị trí bắt đầu từ lề trái
            };
            reportFooterBand.Controls.Add(lblGiamDocSign);


            // Tạo label cho ngày tháng (đặt bên phải của footer)
            XRLabel lblNgayThang = new XRLabel
            {
                Text = "Ngày ... tháng ... năm ......",
                WidthF = labelWidth,
                HeightF = 15f,
                Font = new Font(""+ font +"", 10, FontStyle.Italic),
                LocationF = new PointF(lblGiamDoc.Location.X, lblGiamDoc.LocationF.Y - 25f),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            reportFooterBand.Controls.Add(lblNgayThang);
            #endregion
            //Kết thúc cuối báo cáo


            //Gán dữ liệu cho báo cáo
            report.DataSource = data;
            report.DataMember = data.TableName;


            // Hiển thị báo cáo bằng cách sử dụng DocumentViewer
            DocumentViewer documentViewer = new DocumentViewer();
            documentViewer.DocumentSource = report;
            report.CreateDocument();

            //// Tạo một ReportPrintTool mới và gửi báo cáo vào đó
            //DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(report);

            //// Hiển thị cửa sổ xem trước báo cáo
            //printTool.ShowPreview();

            return report;
        }

        private void btn_in_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string sql = SqlQuery;
                DataTable dataTable = Class.Functions.GetDataToTable(sql); // Hàm để lấy dữ liệu từ câu lệnh SQL
                var columnMappings = new Dictionary<string, string>
                        {
                            {"Mẫu HĐ", "mau_hd" },
                            {"Ký hiệu", "serihd" },
                            {"Số HĐ", "sohd" },
                            {"Ngày HĐ","phi_tm"},
                            {"Tên người mua hàng","tr5" },
                            {"Mã số thuế","tr6" },
                            {"Mặt hàng","nhom_vat" },
                            {"Doanh thu trước thuế","dso" },
                            {"Tiền thuế","tienthue" },
                            {"Thuế suất","ktdb" },
                            {"Ghi chú","ghi_chu" }
                            // Thêm các cặp tiêu đề và tên cột tương ứng khác
                        };

                XtraReport report = CreateReportWithTable(dataTable, columnMappings); // Tạo report với DataTable và mappings

                // Tạo ReportPrintTool và truyền report vào
                ReportPrintTool printTool = new ReportPrintTool(report);

                // Hiển thị hộp thoại Print Preview với ribbon (giao diện ribbon nếu có)
                printTool.PrintDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hiển thị hộp thoại in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_lammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowDateRangeForm();
        }

        private string currentSql;

        private void LoadData()
        {
            // Hàm để tải dữ liệu từ câu lệnh SQL và cập nhật dữ liệu trên gridView
            DataTable dataTable = Class.Functions.GetDataToTable(currentSql);
            gridControl1.DataSource = dataTable;
        }

        private void ShowDateRangeForm()
        {
            // Tạo một instance mới của form
            Form dateRangeForm = new Form
            {
                Text = "Chọn Khoảng Thời Gian",
                // Tăng kích thước chiều rộng của form để phù hợp với nội dung
                Size = new System.Drawing.Size(400, 150), // Thay đổi giá trị này để phù hợp
                StartPosition = FormStartPosition.CenterScreen
            };

            // Cài đặt cho Label "Từ ngày"
            System.Windows.Forms.Label lblFromDate = new System.Windows.Forms.Label
            {
                Text = "Từ ngày:",
                Location = new System.Drawing.Point(10, 12), // Điều chỉnh vị trí nếu cần
                Width = 80, // Tăng chiều rộng nếu cần
                TextAlign = ContentAlignment.MiddleLeft
            };
            dateRangeForm.Controls.Add(lblFromDate);

            // Cài đặt cho DateTimePicker "Từ ngày"
            DateTimePicker dtpFromDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new System.Drawing.Point(100, 10), // Điều chỉnh vị trí nếu cần
                Width = 200 // Tăng chiều rộng nếu cần
            };
            dateRangeForm.Controls.Add(dtpFromDate);

            // Cài đặt cho Label "Đến ngày"
            System.Windows.Forms.Label lblToDate = new System.Windows.Forms.Label
            {
                Text = "Đến ngày:",
                Location = new System.Drawing.Point(10, 42), // Điều chỉnh vị trí nếu cần
                Width = 80, // Tăng chiều rộng nếu cần
                TextAlign = ContentAlignment.MiddleLeft
            };
            dateRangeForm.Controls.Add(lblToDate);

            // Cài đặt cho DateTimePicker "Đến ngày"
            DateTimePicker dtpToDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new System.Drawing.Point(100, 40), // Điều chỉnh vị trí nếu cần
                Width = 200 // Tăng chiều rộng nếu cần
            };
            dateRangeForm.Controls.Add(dtpToDate);

            // Tính ngày đầu tiên của tháng hiện tại
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Tính ngày hiện tại
            DateTime currentDate = DateTime.Now;

            // Áp dụng giá trị cho DateTimePicker "Từ ngày"
            dtpFromDate.Value = firstDayOfMonth;

            // Áp dụng giá trị cho DateTimePicker "Đến ngày"
            dtpToDate.Value = currentDate;


            // Tạo nút "Tìm kiếm"
            Button btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new System.Drawing.Point(100, 70),
                Width = 200
            };

            btnSearch.Click += (sender1, e1) =>
            {
                DateTime fromDate = dtpFromDate.Value;
                DateTime toDate = dtpToDate.Value;

                DateTime fDate_sql = fromDate.AddDays(-1);
                DateTime tDate_sql = toDate.AddDays(1);

                // Lưu câu lệnh SQL hiện tại để có thể sử dụng lại khi làm mới
                currentSql = SqlQuery;

                this.Text = "Bảng kê hóa đơn hàng hóa dịch vụ bán ra   " + "[" + fromDate.ToString("dd/MM/yyyy") + "] [" + toDate.ToString("dd/MM/yyyy") + "]";
                this.fromDate = fromDate;
                this.toDate = toDate;
                // Cập nhật dữ liệu trên gridView
                LoadData();
                dateRangeForm.Close();
            };

            dateRangeForm.Controls.Add(btnSearch);

            // Hiển thị DateRangeForm
            dateRangeForm.Show();
        }

    }
}