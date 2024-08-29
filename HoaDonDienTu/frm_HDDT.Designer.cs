namespace HoaDonDienTu
{
    partial class frm_HDDT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_HDDT));
            this.stepProgressBar1 = new DevExpress.XtraEditors.StepProgressBar();
            this.spBar_item_khoitao = new DevExpress.XtraEditors.StepProgressBarItem();
            this.spBar_item_choky = new DevExpress.XtraEditors.StepProgressBarItem();
            this.spBar_item_phathanh = new DevExpress.XtraEditors.StepProgressBarItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_view_cky = new System.Windows.Forms.TextBox();
            this.txt_search_tu_ngay = new DevExpress.XtraEditors.DateEdit();
            this.txt_search_den_ngay = new DevExpress.XtraEditors.DateEdit();
            this.lb_tu_ngay = new DevExpress.XtraEditors.LabelControl();
            this.lb_den_ngay = new DevExpress.XtraEditors.LabelControl();
            this.btn_timkiem = new DevExpress.XtraEditors.SimpleButton();
            this.lb_view_cky = new System.Windows.Forms.Label();
            this.lb_tnhap = new System.Windows.Forms.Label();
            this.txt_view_tnhap = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.stepProgressBar1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_tu_ngay.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_tu_ngay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_den_ngay.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_den_ngay.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // stepProgressBar1
            // 
            this.stepProgressBar1.DistanceBetweenContentBlockElements = 5;
            this.stepProgressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stepProgressBar1.IndentBetweenItems = 27;
            this.stepProgressBar1.IndicatorToContentBlockDistance = 5;
            this.stepProgressBar1.Items.Add(this.spBar_item_khoitao);
            this.stepProgressBar1.Items.Add(this.spBar_item_choky);
            this.stepProgressBar1.Items.Add(this.spBar_item_phathanh);
            this.stepProgressBar1.Location = new System.Drawing.Point(0, 0);
            this.stepProgressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stepProgressBar1.Name = "stepProgressBar1";
            this.stepProgressBar1.Padding = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.stepProgressBar1.Size = new System.Drawing.Size(1186, 129);
            this.stepProgressBar1.TabIndex = 0;
            this.stepProgressBar1.SelectedItemChanged += new DevExpress.XtraEditors.StepProgressBarSelectedItemChangedEventHandler(this.stepProgressBar1_SelectedItemChanged);
            this.stepProgressBar1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.stepProgressBar1_MouseClick);
            // 
            // spBar_item_khoitao
            // 
            this.spBar_item_khoitao.ContentBlock2.Caption = "Hóa đơn vừa tạo";
            this.spBar_item_khoitao.Name = "spBar_item_khoitao";
            this.spBar_item_khoitao.Options.Indicator.ActiveStateImageOptions.Image = global::HoaDonDienTu.Properties.Resources.apply_32x32;
            // 
            // spBar_item_choky
            // 
            this.spBar_item_choky.ContentBlock2.Caption = "Hóa đơn đã phát hành";
            this.spBar_item_choky.Name = "spBar_item_choky";
            this.spBar_item_choky.Options.Indicator.ActiveStateImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("spBar_item_choky.Options.Indicator.ActiveStateImageOptions.Image")));
            // 
            // spBar_item_phathanh
            // 
            this.spBar_item_phathanh.ContentBlock2.Caption = "Hóa đơn đã hủy bỏ";
            this.spBar_item_phathanh.Name = "spBar_item_phathanh";
            this.spBar_item_phathanh.Options.Indicator.ActiveStateImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("spBar_item_phathanh.Options.Indicator.ActiveStateImageOptions.Image")));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 129);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1186, 247);
            this.panel1.TabIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1186, 247);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 284;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gridView1_MouseWheel);
            this.gridView1.RowCountChanged += new System.EventHandler(this.gridView1_RowCountChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControl2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 376);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1186, 98);
            this.panel2.TabIndex = 2;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(1186, 98);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.DetailHeight = 284;
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView2.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gridView1_MouseWheel);
            this.gridView2.RowCountChanged += new System.EventHandler(this.gridView1_RowCountChanged);
            // 
            // txt_view_cky
            // 
            this.txt_view_cky.BackColor = System.Drawing.Color.MediumPurple;
            this.txt_view_cky.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_view_cky.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_view_cky.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.txt_view_cky.Location = new System.Drawing.Point(14, 10);
            this.txt_view_cky.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_view_cky.Multiline = true;
            this.txt_view_cky.Name = "txt_view_cky";
            this.txt_view_cky.ReadOnly = true;
            this.txt_view_cky.Size = new System.Drawing.Size(65, 42);
            this.txt_view_cky.TabIndex = 3;
            this.txt_view_cky.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_view_cky.TextChanged += new System.EventHandler(this.txt_view_TextChanged);
            // 
            // txt_search_tu_ngay
            // 
            this.txt_search_tu_ngay.EditValue = null;
            this.txt_search_tu_ngay.Location = new System.Drawing.Point(611, 82);
            this.txt_search_tu_ngay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_search_tu_ngay.Name = "txt_search_tu_ngay";
            this.txt_search_tu_ngay.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_search_tu_ngay.Properties.Appearance.Options.UseFont = true;
            this.txt_search_tu_ngay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txt_search_tu_ngay.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txt_search_tu_ngay.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.txt_search_tu_ngay.Size = new System.Drawing.Size(167, 24);
            this.txt_search_tu_ngay.TabIndex = 4;
            // 
            // txt_search_den_ngay
            // 
            this.txt_search_den_ngay.EditValue = null;
            this.txt_search_den_ngay.Location = new System.Drawing.Point(892, 82);
            this.txt_search_den_ngay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_search_den_ngay.Name = "txt_search_den_ngay";
            this.txt_search_den_ngay.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_search_den_ngay.Properties.Appearance.Options.UseFont = true;
            this.txt_search_den_ngay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txt_search_den_ngay.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txt_search_den_ngay.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.txt_search_den_ngay.Size = new System.Drawing.Size(168, 24);
            this.txt_search_den_ngay.TabIndex = 5;
            // 
            // lb_tu_ngay
            // 
            this.lb_tu_ngay.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_tu_ngay.Appearance.Options.UseFont = true;
            this.lb_tu_ngay.Location = new System.Drawing.Point(532, 84);
            this.lb_tu_ngay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_tu_ngay.Name = "lb_tu_ngay";
            this.lb_tu_ngay.Size = new System.Drawing.Size(59, 18);
            this.lb_tu_ngay.TabIndex = 6;
            this.lb_tu_ngay.Text = "Từ ngày";
            // 
            // lb_den_ngay
            // 
            this.lb_den_ngay.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_den_ngay.Appearance.Options.UseFont = true;
            this.lb_den_ngay.Location = new System.Drawing.Point(809, 85);
            this.lb_den_ngay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lb_den_ngay.Name = "lb_den_ngay";
            this.lb_den_ngay.Size = new System.Drawing.Size(66, 17);
            this.lb_den_ngay.TabIndex = 7;
            this.lb_den_ngay.Text = "Đến ngày";
            // 
            // btn_timkiem
            // 
            this.btn_timkiem.ImageOptions.SvgImage = global::HoaDonDienTu.Properties.Resources.actions_zoom1;
            this.btn_timkiem.Location = new System.Drawing.Point(1103, 77);
            this.btn_timkiem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_timkiem.Name = "btn_timkiem";
            this.btn_timkiem.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btn_timkiem.Size = new System.Drawing.Size(95, 32);
            this.btn_timkiem.TabIndex = 8;
            this.btn_timkiem.Text = "Tìm kiếm";
            this.btn_timkiem.Click += new System.EventHandler(this.btn_timkiem_Click);
            // 
            // lb_view_cky
            // 
            this.lb_view_cky.AutoSize = true;
            this.lb_view_cky.Location = new System.Drawing.Point(4, 62);
            this.lb_view_cky.Name = "lb_view_cky";
            this.lb_view_cky.Size = new System.Drawing.Size(85, 13);
            this.lb_view_cky.TabIndex = 9;
            this.lb_view_cky.Text = "Chưa phát hành";
            // 
            // lb_tnhap
            // 
            this.lb_tnhap.AutoSize = true;
            this.lb_tnhap.Location = new System.Drawing.Point(106, 62);
            this.lb_tnhap.Name = "lb_tnhap";
            this.lb_tnhap.Size = new System.Drawing.Size(67, 13);
            this.lb_tnhap.TabIndex = 11;
            this.lb_tnhap.Text = "Đã tạo nháp";
            // 
            // txt_view_tnhap
            // 
            this.txt_view_tnhap.BackColor = System.Drawing.Color.MediumPurple;
            this.txt_view_tnhap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_view_tnhap.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_view_tnhap.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.txt_view_tnhap.Location = new System.Drawing.Point(106, 10);
            this.txt_view_tnhap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_view_tnhap.Multiline = true;
            this.txt_view_tnhap.Name = "txt_view_tnhap";
            this.txt_view_tnhap.ReadOnly = true;
            this.txt_view_tnhap.Size = new System.Drawing.Size(65, 42);
            this.txt_view_tnhap.TabIndex = 10;
            this.txt_view_tnhap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frm_HDDT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 474);
            this.Controls.Add(this.txt_view_tnhap);
            this.Controls.Add(this.lb_tnhap);
            this.Controls.Add(this.txt_view_cky);
            this.Controls.Add(this.btn_timkiem);
            this.Controls.Add(this.lb_view_cky);
            this.Controls.Add(this.lb_den_ngay);
            this.Controls.Add(this.lb_tu_ngay);
            this.Controls.Add(this.txt_search_den_ngay);
            this.Controls.Add(this.txt_search_tu_ngay);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stepProgressBar1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frm_HDDT";
            this.Text = "frm_HDDT1";
            this.Load += new System.EventHandler(this.frm_HDDT1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stepProgressBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_tu_ngay.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_tu_ngay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_den_ngay.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_search_den_ngay.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.StepProgressBar stepProgressBar1;
        private DevExpress.XtraEditors.StepProgressBarItem spBar_item_khoitao;
        private DevExpress.XtraEditors.StepProgressBarItem spBar_item_choky;
        private DevExpress.XtraEditors.StepProgressBarItem spBar_item_phathanh;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.TextBox txt_view_cky;
        private DevExpress.XtraEditors.DateEdit txt_search_tu_ngay;
        private DevExpress.XtraEditors.DateEdit txt_search_den_ngay;
        private DevExpress.XtraEditors.LabelControl lb_tu_ngay;
        private DevExpress.XtraEditors.LabelControl lb_den_ngay;
        private DevExpress.XtraEditors.SimpleButton btn_timkiem;
        private System.Windows.Forms.Label lb_view_cky;
        private System.Windows.Forms.Label lb_tnhap;
        private System.Windows.Forms.TextBox txt_view_tnhap;
    }
}