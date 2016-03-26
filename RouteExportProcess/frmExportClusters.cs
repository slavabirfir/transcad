using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLEntities.Entities;
using Utilities; 
using BLManager;
using BLEntities.Model;

namespace RouteExportProcess
{
    public partial class frmExportClusters : GUIBase.BaseForm
    {
        BLExportClusters exportClustersBL;
        public List<BaseTableEntity> SelectedCusters { get { return exportClustersBL.SelectedClusters; } }
        public frmExportClusters()
        {
            InitializeComponent();
            exportClustersBL = new BLExportClusters();
            Init();

        }

        private void Init()
        {
            
            
            lstData.DataSource = this.exportClustersBL.AllClusters;
            for (int i = 0; i < lstData.Items.Count; i++)
            {
                lstData.SetItemCheckState(i, CheckState.Checked);
            }
            lstData.SelectedIndex = -1;
        
        }

        

        private void cmdContinue_Click(object sender, EventArgs e)
        {
            if (lstData.CheckedItems.Count>0)
            {
                this.exportClustersBL.SelectedClusters = new List<BaseTableEntity>();
                for (int i = 0; i < lstData.CheckedItems.Count; i++)
                    this.exportClustersBL.SelectedClusters.Add(lstData.CheckedItems[i] as BaseTableEntity);
            }
        }
    }
}
