using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLEntities.Accessories;
using BLManager;
using GUIBase;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public partial class frmExportToSQLServer : GUIBase.BaseForm
    {
        /// <summary>
        /// exportToSQLBL 
        /// </summary>
        private SQLExportManager exportToSQLBL = new SQLExportManager(); 

        /// <summary>
        /// frm Export To SQL Server
        /// </summary>
        public frmExportToSQLServer()
        {
            InitializeComponent();
            cmbYears.DataSource = exportToSQLBL.YearsList;
            bndSourceExportToSQL.DataSource = exportToSQLBL.ExportToSqlInfo;
        }
        /// <summary>
        /// btn Close Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// btn Load Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.ExportToSQLServer))
                return;

            ProcessWaitingManager processWaitingManager = new ProcessWaitingManager();
            string message = string.Empty;
            try
            {
                processWaitingManager.ShowProcessWaitingForm();
                if (exportToSQLBL.ExportData(ref message))
                {
                    processWaitingManager.CloseProcessWaitingForm();
                    GUIBase.GuiHelper.ShowInfoMessage(ResourceGUI.SQLServerExportWasSuccesefullyFinished);
                    this.Close();
                }
                else
                {
                    processWaitingManager.CloseProcessWaitingForm();
                    GUIBase.GuiHelper.ShowInfoMessage(string.Format(ResourceGUI.SQLServerExportWasFailed, message));
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        /// <summary>
        /// btn Save Infra A sLocal Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveInfraAsLocal_Click(object sender, EventArgs e)
        {
            if (!GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.SaveInfraToLocalStorage))
                return;

            ProcessWaitingManager processWaitingManager = new ProcessWaitingManager();
            string message = string.Empty;
            try
            {
                processWaitingManager.ShowProcessWaitingForm();
                exportToSQLBL.WriteInfractructure();
                processWaitingManager.CloseProcessWaitingForm();
                GUIBase.GuiHelper.ShowInfoMessage(ResourceGUI.SaveImportToLocalWasSuccesefullyFinished);
            }
            catch (Exception exc)
            {
                processWaitingManager.CloseProcessWaitingForm();
                throw exc;
            }
        }
    }
}
