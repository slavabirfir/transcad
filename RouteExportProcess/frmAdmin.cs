using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLManager;
using GUIBase;
using RouteExportProcess.Resources;
using ExportConfiguration;

namespace RouteExportProcess
{
    /// <summary>
    /// frmAdmin
    /// </summary>
    public partial class frmAdmin : GUIBase.BaseForm
    {

        private AdministrationBl administrationBL = new AdministrationBl();
        private ProcessWaitingManager processWaitingManager = new ProcessWaitingManager();
        /// <summary>
        /// constractor
        /// </summary>
        public frmAdmin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                processWaitingManager.ShowProcessWaitingForm();
                if (rdConfig.Checked)
                {
                    administrationBL.DecryptConfigFile();
                }
                else if (rdSetBasedLine.Checked)
                {
                    administrationBL.SetBaseRouteLines();
                    processWaitingManager.CloseProcessWaitingForm();
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                        Application.Exit(); 
                }
                //else if (rdCatalogTo7Pos.Checked)
                //{
                //    ConvertCatalogsTo7Positions();
                //    if (GUIHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                //        Application.Exit(); 
                //}
                else if (rdHorada.Checked)
                {
                   CalcStationHorada();
                   processWaitingManager.CloseProcessWaitingForm();
                   if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                       Application.Exit(); 
                }
                else if (rdUpdateEndPoints.Checked)
                {
                    administrationBL.UpdateEndPoints();
                    processWaitingManager.CloseProcessWaitingForm();
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                        Application.Exit(); 
                }
                else if (rdUpdatePhysicalStops.Checked)
                {
                    administrationBL.UpdatePhysicalStops();
                    processWaitingManager.CloseProcessWaitingForm();
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                        Application.Exit(); 
                }

                else if (rdJunctionVersion.Checked)
                {

                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    openFileDialog1.Filter = string.Format("(*.{0})|*.{0}", "csv");
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.Title = Resources.ResourceGUI.CSVFileJunctionVersionNonHeader;
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                    {
                        string filename = openFileDialog1.FileName;
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.ShowNewFolderButton = true;
                        fbd.Description = Resources.ResourceGUI.ChooseJunctionVersionFileBuilddFolder;
                        if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                        {
                            string folder = fbd.SelectedPath;
                            //if (Convert.ToBoolean(ExportConfiguration.ExportConfigurator.GetConfig().DevelopmentEnv))
                            administrationBL.BuildJunctionVersionFileAndWriteToSqlServer(filename, folder);
                            //else
                            //    administrationBL.BuildJunctionVersionFileAndWriteInText(filename, folder);
                        }
                    }
                    processWaitingManager.CloseProcessWaitingForm();
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                        Application.Exit(); 
                }


                else if (rdExportEggedStations.Checked)
                {
                    administrationBL.ExportEggedStations(ExportConfigurator.GetConfig().EggedExportSationFolder);
                    processWaitingManager.CloseProcessWaitingForm();
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                        Application.Exit();
                    
                }
                else if (rdExportAdmin.Checked)
                {
                    if (GuiHelper.ShowConfirmationMessage(ResourceGUI.AdminExportQuestions))
                    {
                        this.Refresh();
                        administrationBL.ExportMapForCompare();
                        processWaitingManager.CloseProcessWaitingForm();
                        if (GuiHelper.ShowConfirmationMessage(ResourceGUI.ChangeAdminDone))
                            Application.Exit();
                    }
                }
            }
            finally
            {
                        processWaitingManager.CloseProcessWaitingForm();
                        this.Cursor = Cursors.Default;
                        this.Activate();
                        this.Close();
                        
            }
        }
        /// <summary>
        /// Calc Station Horada
        /// </summary>
        private void CalcStationHorada()
        {
            administrationBL.CalcStationHorada();
        }
        /// <summary>
        /// Convert Catalogs To7 Positions
        /// </summary>
        private void ConvertCatalogsTo7Positions()
        {
            string folderName = null;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = Resources.ResourceGUI.ChooseConvertCatalogFolder;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                folderName = fbd.SelectedPath;
            }
            if (String.IsNullOrEmpty(folderName))
            {
                MessageBox.Show(Resources.ResourceGUI.PleaseChooseConvertCatalogFolder, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {
                string message = string.Empty;  
                administrationBL.ConvertCatalogsTo7Positions(folderName, ref message);
                if (string.IsNullOrEmpty(message))
                   MessageBox.Show(Resources.ResourceGUI.CatalogConvertedFileWasCreatedSuccesefully, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
       
    }
}
