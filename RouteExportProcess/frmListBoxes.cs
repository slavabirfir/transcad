using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLManager;
using BLEntities.Entities;

namespace RouteExportProcess
{
    public partial class frmListBoxes : GUIBase.BaseForm
    {
        /// <summary>
        /// List Box BL Manager
        /// </summary>
        private ListBoxBLManager listBoxBLManager = null; 
        /// <summary>
        /// Selected Delimeted String
        /// </summary>
        public string  SelectedDelimetedString { get; set; }
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="selectedList"></param>
        /// <param name="title"></param>
        public frmListBoxes(List<BaseTableEntity> allList,List<BaseTableEntity> selectedList, string    title)
        {
            InitializeComponent();
            listBoxBLManager = new ListBoxBLManager(allList, selectedList);
            bindingSourceAll.DataSource = listBoxBLManager.AllData;
            bindingSourceSelected.DataSource = listBoxBLManager.SelectedEntities;
            this.Text = title; 
        }
        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (bindingSourceAll.Current != null)
            {
                listBoxBLManager.AddToSelected(bindingSourceAll.Current as BaseTableEntity);
                bindingSourceSelected.ResetBindings(false);
                //bindingSourceAll.ResetBindings(false);
                //bindingSourceAll.ResumeBinding();
                //lstAll.Refresh();
            }
        }
        /// <summary>
        /// btn Right Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Click(object sender, EventArgs e)
        {
            if (bindingSourceSelected.Current != null)
            {
                listBoxBLManager.RemoveFromSelected(bindingSourceSelected.Current as BaseTableEntity);
                bindingSourceSelected.ResetBindings(false);
                //lstSelected.Refresh();
                //bindingSourceSelected.ResumeBinding();
            }
        }
        /// <summary>
        /// btnSave Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            SelectedDelimetedString = listBoxBLManager.BuildSelectedDelimetedString();
            this.Close(); 
        }
    }
}
