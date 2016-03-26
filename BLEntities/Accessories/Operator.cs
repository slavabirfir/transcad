using System.Collections.Generic;
using BLEntities.Entities;

namespace BLEntities.Accessories
{
    public class Operator
    {
        public int IdOperator { get; set; }
        public string OperatorName { get; set; }
        public string EnglishName { get; set; }
        public string Email { get; set; }
        public byte TransCadStatus { get; set; }
        public List<TranscadClusterConfig> ListOfTranscadClusterConfig { get; set; }
        public TranscadClusterConfig SelectedTranscadClusterConfig { get; set; }
        
        public string OperatorWorkSpace
        {
            get 
            {
                return TransCadStatus > 0  ? OperatorName :
                    string.Format("{0} ({1})", OperatorName, Resources.ValidationResource.ConvertWasNotCompleted); 
            }
        }

        public EnmOperatorGroup OperatorGroup { get; set; }
        
    }
}
