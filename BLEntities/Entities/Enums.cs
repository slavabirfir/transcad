namespace BLEntities.Entities
{
    public enum enmLinkStationCity
    {
        NonLinked = 0,
        Manual = 1,
        Geography = 2
    }

    public enum EnmOperatorGroup
    {
        Operator=1,
        PlanningFirm=2
    }

    public enum StationOrderConst
    {
        First = 1,
        Last = 2,
        Regular = 3
    }
    /// <summary>
    /// Layer Type
    /// </summary>
    public enum LayerType
    {
        Point = 1,
        Line = 2,
        Area = 3
    }
    /// <summary>
    /// Export To SQL Type
    /// </summary>
    public enum ExportToSQLType
    {
        PeriodBase =1,
        NotPeriod = 2,
        Events =3
    }

}
