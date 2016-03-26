using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IBLManager;
using BLManager;
using BLEntities.Accessories;

namespace RouteExportProcess
{
    public partial class frmOperatorEntity : GUIBase.BaseForm
    {
        /// <summary>
        /// operator Select BL Manager
        /// </summary>
        private IOperatorSelectBlManager operatorSelectBLManager = null; 
        /// <summary>
        /// frm Operator Entity
        /// </summary>
        /// <param name="oper"></param>
        public frmOperatorEntity(Operator oper)
        {
            InitializeComponent();
            operatorSelectBLManager = new OperatorSelectBlManager(oper);
            this.bndSource.DataSource = oper;
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
        /// Get File
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string GetFile(string extension)
        {
           
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            openFileDialog1.Filter = string.Format("(*.{0})|*.{0}",extension) ;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                return openFileDialog1.FileName;
            else
                return string.Empty;  
        }

        /// <summary>
        /// btn ChooseRS Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseRS_Click(object sender, EventArgs e)
        {
            this.operatorSelectBLManager.SelectedOperator.SelectedTranscadClusterConfig.PathToRSTFile = GetFile("rts");
            this.bndSource.ResetCurrentItem();
        }
        /// <summary>
        /// btn Choose WS Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseWS_Click(object sender, EventArgs e)
        {
            this.operatorSelectBLManager.SelectedOperator.SelectedTranscadClusterConfig.PathToRSTWorkSpace = GetFile("wrk");
            this.bndSource.ResetCurrentItem();
        }
        /// <summary>
        /// btnSave Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            operatorSelectBLManager.UpdateOperator(operatorSelectBLManager.SelectedOperator);
            DialogResult = DialogResult.OK;
            Close(); 
        }
    }
}
