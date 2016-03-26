using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLManager;
using IBLManager;
using Utilities;
using BLEntities.Accessories;

namespace RouteExportProcess
{
    public partial class frmRecreateTableStructure : GUIBase.BaseForm
    {
        private ITransCadBLManager manager = null;
             
        public frmRecreateTableStructure()
        {
            InitializeComponent();
            manager = new TransCadBlManager();
        }
        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }
        /// <summary>
        /// InitComponents
        /// </summary>
        protected override void InitComponents()
        {
            lstRouteLinesFields.DataSource = manager.GetRouteLineLayerFields();
            lstRouteStopField.DataSource = manager.GetRouteStopsLayerFields();
            bool isDataRecreated = false; 
            if (lstRouteLinesFields.DataSource != null)
            {
                isDataRecreated = manager.GetRouteLineLayerFields().Exists(it => it == "Catalog");
            }
            
            btnRun.Enabled = !isDataRecreated;
            if (isDataRecreated)
               GUIBase.GuiHelper.ShowInfoMessage("The Route System already was updated by new format");
        }
        /// <summary>
        /// btnRun Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            lstRouteLines.Items.Clear(); 
            manager.RouteLineWasRecreated += new EventHandler<RecreatedArgs>(manager_RouteLineWasRecreated);
            manager.ReCreateTableStructure();
            btnRun.Enabled = false;
            lstRouteLinesFields.DataSource = manager.GetRouteLineLayerFields();
            lstRouteStopField.DataSource = manager.GetRouteStopsLayerFields();
        }
        /// <summary>
        /// manager RouteLineWasRecreated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void manager_RouteLineWasRecreated(object sender, RecreatedArgs e)
        {
            lstRouteLines.Items.Add(e.ErrorString);
        }
        /// <summary>
        /// btnShowRouteLineFile Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowRouteLineFile_Click(object sender, EventArgs e)
        {
            manager.ShowRecreatedRouteLineReport();
        }
            /// <summary>
            /// frmRecreateTableStructure FormClosed
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void frmRecreateTableStructure_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }
        

        
    }
}
