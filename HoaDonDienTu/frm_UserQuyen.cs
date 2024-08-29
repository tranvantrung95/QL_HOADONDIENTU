using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using HoaDonDienTu.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Migrations;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DevExpress.XtraEditors.Filtering;

namespace HoaDonDienTu
{
    
    public partial class frm_UserQuyen : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_users;
        private EFF_EFFE_200Entities _entities;
        string ma_user = "";
        Cls_EnCrypting ecr = new Cls_EnCrypting();
        public frm_UserQuyen()
        {
            InitializeComponent();
        }

        private void frm_UserQuyen_Load_1(object sender, EventArgs e)
        {
            Class.Functions.Connect();
            _entities = new EFF_EFFE_200Entities();
            loadview();
            
        }

        private void loadview()
        {
            string sql;
            sql = "SELECT ma_user, ten FROM _users WHERE ma_user<>''";
            dt_users = Class.Functions.GetDataToTable(sql);
            gridControl1.DataSource = dt_users;

            // Chọn cột cần enable hoặc disable
            //GridColumn gridColumn = gridView1.Columns["ma_thb"]; // Thay "TenCot" bằng tên thực của cột

            //// Enable cột
            //gridColumn.OptionsColumn.AllowEdit = true;

            // Hoặc disable cột
            //gridColumn.OptionsColumn.AllowEdit = false;
            //gridColumn.Width = 15;

            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
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

        public class UserQuyen
        {
            public string Menu { get; set;}
            public bool? AllowAddNew { get; set;}
            public bool? AllowDelete { get; set;}
            public bool? Disable { get; set;}
            public bool? AllowEdit { get; set;}
            public string ParentMenu { get; set; }
            public string Description { get; set; }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int ma_user = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ma_user"]));
            var dt = from a in _entities.tblFunctions.Where(x => x.Application == "PQND")
                     join b in _entities.tblUserFuntions.Where(x => x.ma_user == ma_user )
                     on a.Menu equals b.Menu
                     into ab
                     from bc in ab.DefaultIfEmpty()
                     select new UserQuyen
                     {
                         Menu = a.Menu,
                         Description = a.Description,
                         AllowAddNew = bc.AllowAddNew == null ? false : bc.AllowAddNew,
                         AllowDelete = bc.AllowDelete == null ? false : bc.AllowDelete,
                         Disable = bc.Disable == null ? false : bc.Disable,
                         AllowEdit = bc.AllowEdit == null ? false : bc.AllowEdit,
                         ParentMenu = a.ParentMenu,
                     };
            treeList.DataSource = dt.ToList();
            treeList.KeyFieldName = "Menu"; // The unique identifier for each node
            treeList.ParentFieldName = "ParentMenu";
            treeList.ExpandAll();
        }
        #region TREELIST

        private void treeList_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.HasChildren)
            {
                e.Appearance.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
                e.Appearance.Options.UseFont = true;
            }
        }

        private void treeList_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            e.Node.SetValue(e.Column, e.Value);
            SetCheckedChildNodes(e.Node, e.Column, (bool)e.Value);
            SetCheckedParentNodes(e.Node, e.Column, (bool)e.Value);
        }

        #endregion

        #region SET CHECK CHILD AND PARENT

        private void SetCheckedChildNodes(TreeListNode node, TreeListColumn col, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i][col] = check;
                SetCheckedChildNodes(node.Nodes[i], col, check); // Recursively check all children
            }
        }

        private void SetCheckedParentNodes(TreeListNode node, TreeListColumn col, bool check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    var state = (bool)node.ParentNode.Nodes[i][col];
                    if (!state)
                    {
                        b = true; // If any of the siblings are unchecked, set b to true
                        break;
                    }
                }
                bool bb = !b && check; // The check value for the parent should be true if none of the siblings are unchecked
                node.ParentNode[col] = bb;
                SetCheckedParentNodes(node.ParentNode, col, bb); // Recursively check parent
            }
        }

        #endregion

        private void btn_lammoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_UserQuyen_Load_1(sender, e);
        }

        private void btn_luu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int ma_user = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ma_user"]));
            string sql_delete = @" DELETE FROM tblUserFuntions WHERE ma_user=" + ma_user;
            Class.Functions.RunSQL(sql_delete);
            var nodes = treeList.GetNodeList();
            for (int i = 0; i < nodes.Count; i++)
            {
                //var menu = (string)nodes[i].GetValue(colMenu);
                //var existing = _entities.tblUserFuntions
                //            .FirstOrDefault(f => f.ma_user == ma_user && f.Menu == menu);
                //if (existing == null)
                //{
                //    var dtl = new tblUserFuntions();
                //    dtl.ma_user = ma_user;
                //    dtl.f_idenity = ma_user;
                //    dtl.Menu = (string)nodes[i].GetValue(colMenu);
                //    dtl.SetTime = DateTime.Now;
                //    dtl.AllowAddNew = (bool)nodes[i].GetValue(colAdd);
                //    dtl.AllowEdit = (bool)nodes[i].GetValue(colEdit);
                //    dtl.AllowDelete = (bool)nodes[i].GetValue(colDelete);
                //    dtl.Disable = (bool)nodes[i].GetValue(colDisable);

                //    // Add or update the details in your entity collection
                //    _entities.tblUserFuntions.AddOrUpdate(dtl);
                //}
                //else
                //{
                //    // Update properties of existing
                //}
                string sql = @"
                INSERT INTO tblUserFuntions
                (
	                ma_user,
	                Menu,
	                SetTime,
	                AllowAddNew,
	                AllowEdit,
	                AllowDelete,
	                [Disable]
	                -- f_idenity -- this column value is auto-generated
                )
                VALUES
                (
	                " + ma_user + @",
	                N'" + (string)nodes[i].GetValue(colMenu) + @"',
	                '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"', " +
                                    ((bool)nodes[i].GetValue(colAdd) ? "1" : "0") + @", " +
                                    ((bool)nodes[i].GetValue(colEdit) ? "1" : "0") + @", " +
                                    ((bool)nodes[i].GetValue(colDelete) ? "1" : "0") + @", " +
                                    ((bool)nodes[i].GetValue(colDisable) ? "1" : "0") + @"
                )";
                Class.Functions.RunSQL(sql);

            }

            //_entities.SaveChanges();

            // Show a success message to the user
            XtraMessageBox.Show("Thao tác thành công", "Thông báo [Message]", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}