using System;
using System.Windows.Forms;
using GUIBase;
using IBLManager;
using BLManager;
using BLEntities.Accessories;

namespace RouteExportProcess
{
    public partial class FrmOperatorList : BaseForm
    {

        private readonly IOperatorSelectBlManager _operatorSelectBlManager = new OperatorSelectBlManager();

        /// <summary>
        /// ctr
        /// </summary>
        public FrmOperatorList()
        {
            InitializeComponent();
            bndSource.DataSource = _operatorSelectBlManager.GetActiveDirectoryGroupsOperatorList();
            if (lstOperatorList.Items.Count>0)
                lstOperatorList.SelectedIndex = 0;
            btnExportErrors.Visible = _operatorSelectBlManager.IsConstraintViolationFolderFull();
           // btnUpdate.Visible = _operatorSelectBlManager.IsShowUpdateOperatorAttributeButton();
        }
        /// <summary>
        /// btn Select Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectClick(object sender, EventArgs e)
        {
            if (lstOperatorList.SelectedIndex == -1)
            {
                if (!GuiHelper.ShowConfirmationMessage(Resources.ResourceGUI.OperatorWasNotSelected))
                    DialogResult = DialogResult.Ignore;
            }
            else
            {
                var message = string.Empty;
                var oper = bndSource.Current as Operator;
                if (oper != null)
                {
                    if (oper.TransCadStatus == 0)
                    {
                        GuiHelper.ShowErrorMessage(Resources.ResourceGUI.OperatorWasNotConverted);
                        return;
                    }
                    _operatorSelectBlManager.SetActiveOperator(oper);
                    if (_operatorSelectBlManager.SetSelectedTranscadClusterConfigAndTestClustersByOperatorIsListFull(oper))
                    {
                        var clusterOfOperator = new frmClusterOfOperator(_operatorSelectBlManager, oper);
                        var resclusterOfOperator = clusterOfOperator.ShowDialog();
                        clusterOfOperator.Close();
                        if (resclusterOfOperator == DialogResult.OK)
                        {
                            if (oper.SelectedTranscadClusterConfig != null)
                            {
                                if (!_operatorSelectBlManager.OpenTranscadByWsPath(oper.SelectedTranscadClusterConfig.PathToRSTWorkSpace, ref message))
                                {
                                    GuiHelper.ShowErrorMessage(message);
                                    DialogResult = DialogResult.Ignore;
                                }
                                else DialogResult = DialogResult.OK;
                            }
                        }
                        else if (resclusterOfOperator == DialogResult.Yes)
                        {
                            if (!_operatorSelectBlManager.OpenTranscadByWsPath(oper.SelectedTranscadClusterConfig.PathToRSTWorkSpace, ref message))
                            {
                                GuiHelper.ShowErrorMessage(message);
                                DialogResult = DialogResult.Ignore;
                            }
                            else DialogResult = DialogResult.OK;
                        }
                        else if (resclusterOfOperator == DialogResult.Cancel)
                        {
                           DialogResult = DialogResult.Ignore;
                        }
                    }
                    else
                    {
                        if (!_operatorSelectBlManager.OpenTranscadByWsPath(oper.SelectedTranscadClusterConfig.PathToRSTWorkSpace, ref message))
                        {
                            GuiHelper.ShowErrorMessage(message);
                            DialogResult = DialogResult.Ignore;
                        }
                        else DialogResult = DialogResult.OK;
                    }
                }
            }
        }
        /// <summary>
        /// btnCancel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, EventArgs e)
        {
            //ProcessWaitingManager processWaitingManager = new ProcessWaitingManager { ShowProgressBar = true, MaxProgressBar = 10, TxtMessageText = "שלום חבר אתה חסר" };
            //processWaitingManager.ShowProcessWaitingForm();
            //for (int i = 1; i <= 10; i++)
            //{
            //    Thread.Sleep(1000);
            //    processWaitingManager.ChangeProgressBar(i,10, "שלב מספר הבא " + i.ToString());
            //}
            DialogResult = DialogResult.Ignore;
        }
        /// <summary>
        /// btnUpdate Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdateClick(object sender, EventArgs e)
        {
            var oper = bndSource.Current as Operator;
            if (lstOperatorList.SelectedIndex >= 0 && oper!=null)
            {
                var operatorEntity = new frmOperatorEntity(oper);
                if (operatorEntity.ShowDialog() == DialogResult.OK)
                {
                    bndSource.ResetBindings(false); 
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportErrorsClick(object sender, EventArgs e)
        {
            _operatorSelectBlManager.ShowConstraintViolationFolder();
            GuiHelper.ShowInfoMessage(Resources.ResourceGUI.DeleteFolderContentAfterRunningBaseExport);
        }
    }
}
