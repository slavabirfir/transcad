using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IBLManager;
using BLManager;

namespace RouteExportProcess
{
    public partial class frmBaseTableManager : GUIBase.BaseForm
    {
        private IBaseTablePresentation baseTablePresentation = null;

        public frmBaseTableManager()
        {
            baseTablePresentation = new BaseTablePresentation();
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        protected override void InitComponents()
        {
            lstBoxTableName.DataSource = baseTablePresentation.GetBaseTableNames;
            lstBoxTableName.ValueMember = "EnglishName";
            lstBoxTableName.DisplayMember = "HebrewName";
        }

        private void lstBoxTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            bndSource.DataSource = baseTablePresentation.GetBaseTableEntities(lstBoxTableName.SelectedValue.ToString());  
        }
    }
}
