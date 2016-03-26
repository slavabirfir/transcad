using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;
using GUIBase;
using RouteExportProcess.Resources;

namespace RouteExportProcess
{
    public partial class FrmLoginsManagment : BaseForm
    {
        private readonly BlLoginsManager _blLoginsManager = new BlLoginsManager();

        public FrmLoginsManagment()
        {
            InitializeComponent();
        }

        private void BtnCloseClick(object sender, System.EventArgs e)
        {
            Close();
        }

        protected override void InitComponents()
        {
            bndSource.DataSource = _blLoginsManager.GetAllTranscadLogins();
        }

        private void BtnRefreshClick(object sender, System.EventArgs e)
        {
            InitComponents();
        }

        private void DtDataUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!e.Row.IsNewRow && bndSource.Current != null)
            {
                var deletingObject = bndSource.Current as TranscadLogin;
                if (deletingObject != null)
                {
                    var response = MessageBox.Show(string.Format(ResourceGUI.DeleteLogin, deletingObject.UserName),
                                                   GuiHelper.Caption, MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question,
                                                   MessageBoxDefaultButton.Button2);
                    if (response == DialogResult.No)
                        e.Cancel = true;
                    else
                    {
                        _blLoginsManager.DeleteLogin(deletingObject);
                        bndSource.ResumeBinding();
                        dtData.Refresh();
                    }
                }
            }
        }
    }
}
