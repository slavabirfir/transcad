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
    public partial class BaseTableManager : GUIBase.BaseForm
    {
        private IBaseTablePresentation baseTablePresentation = null;

        public BaseTableManager()
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
            lstBoxTableName.DataSource = baseTablePresentation.GetBaseTableNames(); 
        }

        private void lstBoxTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            bndSource.DataSource = baseTablePresentation.GetBaseTableEntities(lstBoxTableName.SelectedValue.ToString());  
        }
    }
}
