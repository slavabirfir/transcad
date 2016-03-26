using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TvunaExport
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// 

        private bool isCloseByApproved = false ; 

        public frmMain()
        {
            InitializeComponent();
            this.bndSource.DataSource = BL.Global.ImportControlOperator;
        }
        /// <summary>
        /// btn Approve Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApprove_Click(object sender, EventArgs e)
        {
            Console.WriteLine(BL.Global.GetJsonOfImportControlOperator());
            isCloseByApproved = true; 
            Application.Exit(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isCloseByApproved)
            {
                Console.WriteLine(string.Empty);
                Application.Exit(); 
            }
        }
    }
}
