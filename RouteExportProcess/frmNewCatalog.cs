using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;

namespace RouteExportProcess
{
    public partial class frmNewCatalog : GUIBase.BaseForm
    {

        private BLCreateNewCatalog createNewCatalogBL = new BLCreateNewCatalog(); 
        public CatalogInfo GetNewCatalog()
        {
            return createNewCatalogBL.CatalogInfoNew;  
        }
        /// <summary>
        /// Constractor
        /// </summary>
        public frmNewCatalog()
        {
            InitializeComponent();
            InitControls();
        }
        /// <summary>
        /// Init Controls
        /// </summary>
        private void InitControls()
        {
            lstEshkol.DataSource = createNewCatalogBL.Clusters;
            txtRouteNumber.Text = string.Empty;
        }
        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            createNewCatalogBL.SetCatalogNull();
            this.Close(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BaseTableEntity cluster = null;
            AccountingGroup ag = null;
            if (lstEshkol.SelectedIndex == -1)
            {
                GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseCluster);
                return;
            }
            else
            {
                cluster = lstEshkol.SelectedItem as BaseTableEntity;
                if (cluster.ID == 0)
                {
                    GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseCluster);
                    return;
                }
            }
            if (cmbAccGroup.SelectedIndex == -1)
            {
                GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseAccountingGroup);
                return;
            }
            else
            {
                ag = cmbAccGroup.SelectedItem as AccountingGroup;
                if (ag.AccountingGroupID < 0)
                {
                    GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseAccountingGroup);
                    return;
                }
            }


            if (string.IsNullOrEmpty(txtRouteNumber.Text))
            {
                GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseRouteNumber);
                return;
            }

            if (string.IsNullOrEmpty(txtCatalog.Text) || txtCatalog.Text.Length<5)
            {
                GUIBase.GuiHelper.ShowErrorMessage(Resources.ResourceGUI.ChooseCatalog);
                return;
            }

            //test if catalog already defined in the same cluster
            if (createNewCatalogBL.IsCatalogAlreadyExists(int.Parse(txtCatalog.Text), cluster.ID))
            {
                GUIBase.GuiHelper.ShowInfoMessage(string.Format(Resources.ResourceGUI.CatlogWithSameRNandClusterAlreadyDefined));
                return; 
            }

            createNewCatalogBL.SetCatalog(cluster,ag , int.Parse(txtRouteNumber.Text), int.Parse(txtCatalog.Text));
            if (createNewCatalogBL.CatalogInfoNew!=null)
            {
                if (GUIBase.GuiHelper.ShowConfirmationMessage(string.Format("המקט שנוצר הוא {0}", createNewCatalogBL.CatalogInfoNew.Catalog)))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    createNewCatalogBL.SetCatalogNull();
                }
                this.Close();
            }

        }
        /// <summary>
        /// txt Route Number Key Press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRouteNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numeric’s
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void txtCatalog_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numeric’s
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
                e.Handled = true;
            if (string.IsNullOrEmpty(txtCatalog.Text) &&  txtCatalog.Text.StartsWith("0"))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// lst Eshkol Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstEshkol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEshkol.SelectedIndex >= 0)
                cmbAccGroup.DataSource = createNewCatalogBL.GetAccountingGroupByOperatorAndCluster(lstEshkol.SelectedItem as BaseTableEntity,0);
            else
                cmbAccGroup.DataSource = null;
        }

        
    }
}
