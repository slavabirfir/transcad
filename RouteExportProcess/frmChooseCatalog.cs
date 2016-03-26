using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using BLEntities.Entities;
using BLManager;

namespace RouteExportProcess
{
    public partial class frmChooseCatalog : GUIBase.BaseForm
    {
        public int SelectedCatalog { get; set; }
        private ChooseCatalogBLManager chooseCatalogBLManager; 

        public frmChooseCatalog()
        {
            InitializeComponent();
        }

        public frmChooseCatalog(List<CatalogInfo> catalogList)
        {
            InitializeComponent();
            chooseCatalogBLManager = new ChooseCatalogBLManager(catalogList);
            bndSource.DataSource = chooseCatalogBLManager.GetList();
            rdNewCatalog_CheckedChanged(null, null); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdNewCatalog_CheckedChanged(object sender, EventArgs e)
        {
            grpCatalogList.Enabled = !rdNewCatalog.Checked;
            SetDefaultListBoxValue();
        }

        private void SetDefaultListBoxValue()
        {
            bndSource.DataSource = chooseCatalogBLManager.GetList();
        }

        private void rdExistCatalog_CheckedChanged(object sender, EventArgs e)
        {
            grpCatalogList.Enabled = rdExistCatalog.Checked;
            SetDefaultListBoxValue();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (rdExistCatalog.Checked)
            {
                if (bndSource.DataSource != null && bndSource.Position>-1)
                    SelectedCatalog = (bndSource.Current as CatalogPresenter).Catalog;
                else
                    SelectedCatalog = 0;
            }
            else
            {
                SelectedCatalog = 0;
            }
        }

        private void dtgData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!rdExistCatalog.Checked )
            {
                dtgData.ClearSelection();
            }
        }
    }
}
