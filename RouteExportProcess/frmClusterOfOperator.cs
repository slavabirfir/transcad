using BLEntities.Entities;
using IBLManager;
using BLEntities.Accessories;

namespace RouteExportProcess
{
    public partial class frmClusterOfOperator : GUIBase.BaseForm
    {
        private readonly IOperatorSelectBlManager _operatorSelectBlManager;
        private readonly Operator _operatorEntity;
        public frmClusterOfOperator()
        {
            InitializeComponent();
        }

        public frmClusterOfOperator(IOperatorSelectBlManager operatorSelectBlManager,Operator operatorEntity)
        {
            InitializeComponent();
            _operatorSelectBlManager = operatorSelectBlManager;
            _operatorEntity = operatorEntity;
            bndSource.DataSource = _operatorSelectBlManager.ClustersByOperator;
            if (lstCluster.Items.Count > 0)
                lstCluster.SelectedIndex = 0;
        }

        private void BtnOkClick(object sender, System.EventArgs e)
        {
            if (bndSource!=null && bndSource.Current!=null)
            {
                _operatorSelectBlManager.SetSelectedTranscadClusterConfig(_operatorEntity,bndSource.Current as BaseTableEntity);
            }
           
        }

       

       
    }
}
