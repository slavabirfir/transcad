using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLEntities.Entities;
using BLManager;
using GUIBase;

namespace RouteExportProcess
{
    public partial class frmZoneImportAreaChanged : GUIBase.BaseForm
    {

        private BLZoneImportAreaChanged zoneImportAreaChangedBL;

        public frmZoneImportAreaChanged()
        {
            InitializeComponent();
        }

        public frmZoneImportAreaChanged(List<PriceArea> changedList):this()
        {
            zoneImportAreaChangedBL = new BLZoneImportAreaChanged(changedList); 
            ShowDataGrid();
        }

        private void ShowDataGrid()
        {

            bnSource.DataSource = zoneImportAreaChangedBL.ChangedListBLPriceArea;
            SelectColumnConfiguration();
            dgData.Refresh();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dgData_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = this.dgData.HitTest(e.X, e.Y);

            if (hti.ColumnIndex > -1 && hti.ColumnIndex > 1)
            {
                if (hti.RowIndex >= 0)
                {
                    dgData.Rows[hti.RowIndex].Selected = true;
                    bnSource.Position = hti.RowIndex;
                }
                if (hti.ColumnIndex == dgData.ColumnCount - 1 && bnSource.Current != null && hti.RowIndex >= 0)
                {
                    (bnSource.Current as BLPriceArea).ReplaceZoneCode = !(bnSource.Current as BLPriceArea).ReplaceZoneCode;
                    dgData.Refresh();
                }
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            zoneImportAreaChangedBL.UpdateList();
            this.DialogResult = DialogResult.OK;
        }

        private void SelectColumnConfiguration()
        {
            dgData.Columns[dgData.ColumnCount - 1].HeaderCell = new DGVColumnHeaderPriceArea { Caption = "האם להחליף קוד", SelectedColumnIndex = dgData.ColumnCount - 1 };
            (dgData.Columns[dgData.ColumnCount - 1].HeaderCell as DGVColumnHeaderPriceArea).IsSelectabeFunction = zoneImportAreaChangedBL.IsSelectablePriceArea;
           
           
        }
    }
}
