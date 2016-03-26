using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLEntities.Entities;
using GUIBase;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public sealed partial class FrmClusterStateSelect : BaseForm
    {
        #region props and vars
        private readonly string _clusterName;
        private readonly List<ClusterState> _clusterStateList;
        public ClusterState SelectedClusterState { get; set; }
        #endregion
        /// <summary>
        /// default constractor
        /// </summary>
        public FrmClusterStateSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ctor
        /// </summary>
        public FrmClusterStateSelect(List<ClusterState> clusterStateList, string clusterName)
        {
            InitializeComponent();
            _clusterStateList = clusterStateList;
            _clusterName = clusterName;
            
            BindData();
            lstviewcategories.Columns.Add(new ColumnHeader
            {
                Name = "IdMainMahoz",
                Text = ResourceGUI.IdMahozMain,
                Width = 95

            });
            lstviewcategories.Columns.Add(new ColumnHeader
            {
                Name = "NameMainMahoz",
                Text = ResourceGUI.MahozMainName,
                Width = 134
            });

            lstviewcategories.Columns.Add(new ColumnHeader
            {
                Name = "IdSubZone",
                Text =ResourceGUI.IdMahozSub,
                Width = 90
            });
            lstviewcategories.Columns.Add(new ColumnHeader
            {
                Name = "NameSubZone",
                Text =ResourceGUI.MahozSubName,
                Width = 134
            });

            Text = ResourceGUI.SelectMahozForStateSelect;
        }
        /// <summary>
        /// BtnApproveClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnApproveClick(object sender, EventArgs e)
        {
            var indexes = lstviewcategories.SelectedIndices;
            if (indexes != null && indexes.Count == 1)
                SelectedClusterState = _clusterStateList[indexes[0]];
            else if (indexes != null && indexes.Count == 0)
            {
                GuiHelper.ShowInfoMessage(ResourceGUI.SelectMahozMainAndSub);
                BindData();
                return;
            }
            //foreach (int index in indexes)
            //{
            //    MessageBox.Show(index.ToString());//price += Double.Parse(//    this.lstviewcategories.Items[index].SubItems[1].Text);
            //}
            Close();
        }
        /// <summary>
        /// BindData
        /// </summary>
        private void BindData()
        {
           
            lstviewcategories.Items.Clear();
            lstviewcategories.View = View.Details;
            lstviewcategories.GridLines = true;
            lstviewcategories.FullRowSelect = true;
            var isSelected = true;
            SelectedClusterState = _clusterStateList[0];
            foreach (var item in _clusterStateList)
            {
                var lvi = new ListViewItem(item.ToListViewItem()) { ImageIndex = 0, Selected = isSelected };
                isSelected = false;
                lstviewcategories.Items.Add(lvi);
            }


        }
        /// <summary>
        /// BtnCancelClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
