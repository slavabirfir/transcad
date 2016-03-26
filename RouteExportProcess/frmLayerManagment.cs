using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLManager;
using BLEntities.Entities;
using Utilities;
using GUIBase;
using System.Linq;
using BLEntities.Model;
namespace RouteExportProcess
{
    /// <summary>
    /// frm Layer Managment
    /// </summary>
    public partial class frmLayerManagment : GUIBase.BaseForm
    {
        #region Var
        /// <summary>
        /// layer BL
        /// </summary>
        private LayerManagerBL layerBL = new LayerManagerBL();
        
        #endregion
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            bndSourceLayers.DataSource = layerBL.GetUserList;
            lstData.DataSource = bndSourceLayers;
            if (bndSourceLayers.DataSource != null)
            {
                for(int i=0;i<lstData.Items.Count;i++ )
                {
                    Layer layer = (Layer)lstData.Items[i];
                    lstData.SetItemChecked(i, layer.IsSelectedByUser);
                }
            }
        }
        /// <summary>
        /// Constractor
        /// </summary>
        public frmLayerManagment()
        {
            InitializeComponent();
            InitData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// lstData Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bndSourceLayers.Current != null)
            {
                Layer currentLayer = bndSourceLayers.Current as Layer;
                List<string> list_string = layerBL.GetLayerFields(bndSourceLayers.Current as Layer);
                if (list_string.IsListFull())
                   dtGFields.DataSource = list_string.Select(x => new { Field = x }).ToList(); 
                else
                    dtGFields.DataSource = null;
                grpFields.Enabled = currentLayer.IsSelectedByUser;//  currentLayer.FieldList.IsListFull();
                //dtGFields.Refresh();
                //dtGFields.ResumeLayout();
            }
            else
            {
                
                dtGFields.DataSource = null;
                pcbLayerColor.BackColor = Color.White;
            }
        }

        
        /// <summary>
        /// lstData Item Check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (lstData.SelectedIndex >= 0)
            {
                Layer layer = lstData.Items[e.Index] as Layer;
                if (e.NewValue == CheckState.Checked)
                {
                    layer.IsSelectedByUser = true; 
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    layer.IsSelectedByUser = false; 
                }
            }
        }
        /// <summary>
        /// btn Approve Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.ChangeLayers))
            {
                var processWaitingManager = new ProcessWaitingManager();
                processWaitingManager.ShowProcessWaitingForm();
                layerBL.UpdateMap();
                processWaitingManager.CloseProcessWaitingForm();
                Close();
                //InitData();
                //if (lstData.Items!=null)
                //{
                //    if (lstData.SelectedIndex >= 0)
                //    {
                //        lstData_SelectedIndexChanged(null, null);
                //    }
                //}
            }

        }
        /// <summary>
        /// btn Layer Color Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayerColor_Click(object sender, EventArgs e)
        {
            if (lstData.SelectedIndex >= 0)
            {
                Layer layer = lstData.Items[lstData.SelectedIndex] as Layer;
                if (layer.IsSelectedByUser)
                {
                    ColorDialog dialog = new ColorDialog();
                    // Keeps the user from selecting a custom color.
                    dialog.AllowFullOpen = true;
                    // Allows the user to get help. (The default is false.)
                    dialog.ShowHelp = false;
                    // Sets the initial color select to the current text color.
                    Color selectedColor;
                    // Update the text box color if the user clicks OK 
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedColor = dialog.Color;
                        pcbLayerColor.BackColor = dialog.Color;
                        layer.TransCadColorValue = new TransCadColor { Red = selectedColor.R, Grean = selectedColor.G, Blue = selectedColor.B };
                    }
                }
            }
        }
        /// <summary>
        /// frm Layer Managment Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLayerManagment_FormClosing(object sender, FormClosingEventArgs e)
        {
            layerBL.SetIsMunSelected();
        }
        
    }
}
