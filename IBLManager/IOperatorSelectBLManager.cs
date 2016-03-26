using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEntities.Accessories;
using BLEntities.Entities;

namespace IBLManager
{
    public interface IOperatorSelectBlManager
    {
        List<Operator> GetOperatorList();
        List<Operator> GetActiveDirectoryGroupsOperatorList();
        void SetActiveOperator(Operator oper);
        bool OpenTranscadByWsPath(string workSpacePath, ref string message);
        bool UpdateOperator(Operator operatorEntity);
        Operator SelectedOperator { get; set; }
        void SetSelectedTranscadClusterConfig(Operator operatorEntity, BaseTableEntity selectedCluster);
        bool IsConstraintViolationFolderFull();
        void ShowConstraintViolationFolder();
        bool IsShowUpdateOperatorAttributeButton();
        List<BaseTableEntity> ClustersByOperator { get; set; }
        bool SetSelectedTranscadClusterConfigAndTestClustersByOperatorIsListFull(Operator operatorEntity);
    }
}
