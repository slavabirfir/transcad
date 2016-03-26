﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="TransCAD")]
	public partial class DataBaseDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertbaseRouteType(baseRouteType instance);
    partial void UpdatebaseRouteType(baseRouteType instance);
    partial void DeletebaseRouteType(baseRouteType instance);
    partial void InsertbaseSeasonal(baseSeasonal instance);
    partial void UpdatebaseSeasonal(baseSeasonal instance);
    partial void DeletebaseSeasonal(baseSeasonal instance);
    partial void InsertbaseServiceType(baseServiceType instance);
    partial void UpdatebaseServiceType(baseServiceType instance);
    partial void DeletebaseServiceType(baseServiceType instance);
    partial void InsertbaseVarConverter(baseVarConverter instance);
    partial void UpdatebaseVarConverter(baseVarConverter instance);
    partial void DeletebaseVarConverter(baseVarConverter instance);
    partial void InsertbaseVehicleSize(baseVehicleSize instance);
    partial void UpdatebaseVehicleSize(baseVehicleSize instance);
    partial void DeletebaseVehicleSize(baseVehicleSize instance);
    partial void InsertbaseVehicleType(baseVehicleType instance);
    partial void UpdatebaseVehicleType(baseVehicleType instance);
    partial void DeletebaseVehicleType(baseVehicleType instance);
    partial void InsertbaseZone(baseZone instance);
    partial void UpdatebaseZone(baseZone instance);
    partial void DeletebaseZone(baseZone instance);
    partial void InsertbaseStationType(baseStationType instance);
    partial void UpdatebaseStationType(baseStationType instance);
    partial void DeletebaseStationType(baseStationType instance);
    #endregion
		
		public DataBaseDataContext() : 
				base(global::DAL.Properties.Settings.Default.TransCADConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public DataBaseDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataBaseDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataBaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataBaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<baseRouteType> baseRouteTypes
		{
			get
			{
				return this.GetTable<baseRouteType>();
			}
		}
		
		public System.Data.Linq.Table<baseSeasonal> baseSeasonals
		{
			get
			{
				return this.GetTable<baseSeasonal>();
			}
		}
		
		public System.Data.Linq.Table<baseServiceType> baseServiceTypes
		{
			get
			{
				return this.GetTable<baseServiceType>();
			}
		}
		
		public System.Data.Linq.Table<baseVarConverter> baseVarConverters
		{
			get
			{
				return this.GetTable<baseVarConverter>();
			}
		}
		
		public System.Data.Linq.Table<baseVehicleSize> baseVehicleSizes
		{
			get
			{
				return this.GetTable<baseVehicleSize>();
			}
		}
		
		public System.Data.Linq.Table<baseVehicleType> baseVehicleTypes
		{
			get
			{
				return this.GetTable<baseVehicleType>();
			}
		}
		
		public System.Data.Linq.Table<baseZone> baseZones
		{
			get
			{
				return this.GetTable<baseZone>();
			}
		}
		
		public System.Data.Linq.Table<baseStationType> baseStationTypes
		{
			get
			{
				return this.GetTable<baseStationType>();
			}
		}
		
		[Function(Name="dbo.tblOperatorClusterGetClustersByIdOperator")]
		public ISingleResult<tblOperatorClusterGetClustersByIdOperatorResult> tblOperatorClusterGetClustersByIdOperator([Parameter(Name="IdOperator", DbType="Int")] System.Nullable<int> idOperator)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idOperator);
			return ((ISingleResult<tblOperatorClusterGetClustersByIdOperatorResult>)(result.ReturnValue));
		}
		
		[Function(Name="dbo.tblGetOperatorIdOperator")]
		public ISingleResult<tblGetOperatorIdOperatorResult> tblGetOperatorIdOperator([Parameter(Name="IdOperator", DbType="Int")] System.Nullable<int> idOperator)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idOperator);
			return ((ISingleResult<tblGetOperatorIdOperatorResult>)(result.ReturnValue));
		}
		
		[Function(Name="dbo.procGetAlltblOperator")]
		public ISingleResult<procGetAlltblOperatorResult> procGetAlltblOperator()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<procGetAlltblOperatorResult>)(result.ReturnValue));
		}
		
		[Function(Name="dbo.procUpdateinternalUser")]
		public int procUpdateinternalUser([Parameter(Name="IdUser", DbType="Int")] System.Nullable<int> idUser, [Parameter(Name="IdOperator", DbType="Int")] System.Nullable<int> idOperator, [Parameter(Name="UserName", DbType="VarChar(50)")] string userName, [Parameter(Name="IsSuperViser", DbType="Bit")] System.Nullable<bool> isSuperViser, [Parameter(Name="FirstName", DbType="VarChar(50)")] string firstName, [Parameter(Name="LastName", DbType="VarChar(50)")] string lastName, [Parameter(Name="FirmName", DbType="VarChar(50)")] string firmName, [Parameter(Name="Telephone", DbType="VarChar(50)")] string telephone)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idUser, idOperator, userName, isSuperViser, firstName, lastName, firmName, telephone);
			return ((int)(result.ReturnValue));
		}
		
		[Function(Name="dbo.internalUserGetByUserName")]
		public ISingleResult<internalUserGetByUserNameResult> internalUserGetByUserName([Parameter(Name="UserName", DbType="VarChar(50)")] string userName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userName);
			return ((ISingleResult<internalUserGetByUserNameResult>)(result.ReturnValue));
		}
	}
	
	[Table(Name="dbo.baseRouteType")]
	public partial class baseRouteType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdRouteType;
		
		private string _RouteTypeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdRouteTypeChanging(int value);
    partial void OnIdRouteTypeChanged();
    partial void OnRouteTypeNameChanging(string value);
    partial void OnRouteTypeNameChanged();
    #endregion
		
		public baseRouteType()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdRouteType", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdRouteType
		{
			get
			{
				return this._IdRouteType;
			}
			set
			{
				if ((this._IdRouteType != value))
				{
					this.OnIdRouteTypeChanging(value);
					this.SendPropertyChanging();
					this._IdRouteType = value;
					this.SendPropertyChanged("IdRouteType");
					this.OnIdRouteTypeChanged();
				}
			}
		}
		
		[Column(Storage="_RouteTypeName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string RouteTypeName
		{
			get
			{
				return this._RouteTypeName;
			}
			set
			{
				if ((this._RouteTypeName != value))
				{
					this.OnRouteTypeNameChanging(value);
					this.SendPropertyChanging();
					this._RouteTypeName = value;
					this.SendPropertyChanged("RouteTypeName");
					this.OnRouteTypeNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseSeasonal")]
	public partial class baseSeasonal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdSeasonal;
		
		private string _SeasonalName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdSeasonalChanging(int value);
    partial void OnIdSeasonalChanged();
    partial void OnSeasonalNameChanging(string value);
    partial void OnSeasonalNameChanged();
    #endregion
		
		public baseSeasonal()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdSeasonal", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdSeasonal
		{
			get
			{
				return this._IdSeasonal;
			}
			set
			{
				if ((this._IdSeasonal != value))
				{
					this.OnIdSeasonalChanging(value);
					this.SendPropertyChanging();
					this._IdSeasonal = value;
					this.SendPropertyChanged("IdSeasonal");
					this.OnIdSeasonalChanged();
				}
			}
		}
		
		[Column(Storage="_SeasonalName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string SeasonalName
		{
			get
			{
				return this._SeasonalName;
			}
			set
			{
				if ((this._SeasonalName != value))
				{
					this.OnSeasonalNameChanging(value);
					this.SendPropertyChanging();
					this._SeasonalName = value;
					this.SendPropertyChanged("SeasonalName");
					this.OnSeasonalNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseServiceType")]
	public partial class baseServiceType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdServiceType;
		
		private string _ServiceTypeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdServiceTypeChanging(int value);
    partial void OnIdServiceTypeChanged();
    partial void OnServiceTypeNameChanging(string value);
    partial void OnServiceTypeNameChanged();
    #endregion
		
		public baseServiceType()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdServiceType", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdServiceType
		{
			get
			{
				return this._IdServiceType;
			}
			set
			{
				if ((this._IdServiceType != value))
				{
					this.OnIdServiceTypeChanging(value);
					this.SendPropertyChanging();
					this._IdServiceType = value;
					this.SendPropertyChanged("IdServiceType");
					this.OnIdServiceTypeChanged();
				}
			}
		}
		
		[Column(Storage="_ServiceTypeName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ServiceTypeName
		{
			get
			{
				return this._ServiceTypeName;
			}
			set
			{
				if ((this._ServiceTypeName != value))
				{
					this.OnServiceTypeNameChanging(value);
					this.SendPropertyChanging();
					this._ServiceTypeName = value;
					this.SendPropertyChanged("ServiceTypeName");
					this.OnServiceTypeNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseVarConverter")]
	public partial class baseVarConverter : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private short _VarNum;
		
		private string _VarChar;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnVarNumChanging(short value);
    partial void OnVarNumChanged();
    partial void OnVarCharChanging(string value);
    partial void OnVarCharChanged();
    #endregion
		
		public baseVarConverter()
		{
			OnCreated();
		}
		
		[Column(Storage="_VarNum", DbType="SmallInt NOT NULL", IsPrimaryKey=true)]
		public short VarNum
		{
			get
			{
				return this._VarNum;
			}
			set
			{
				if ((this._VarNum != value))
				{
					this.OnVarNumChanging(value);
					this.SendPropertyChanging();
					this._VarNum = value;
					this.SendPropertyChanged("VarNum");
					this.OnVarNumChanged();
				}
			}
		}
		
		[Column(Storage="_VarChar", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string VarChar
		{
			get
			{
				return this._VarChar;
			}
			set
			{
				if ((this._VarChar != value))
				{
					this.OnVarCharChanging(value);
					this.SendPropertyChanging();
					this._VarChar = value;
					this.SendPropertyChanged("VarChar");
					this.OnVarCharChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseVehicleSize")]
	public partial class baseVehicleSize : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdVehicleSize;
		
		private string _VehicleSizeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdVehicleSizeChanging(int value);
    partial void OnIdVehicleSizeChanged();
    partial void OnVehicleSizeNameChanging(string value);
    partial void OnVehicleSizeNameChanged();
    #endregion
		
		public baseVehicleSize()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdVehicleSize", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdVehicleSize
		{
			get
			{
				return this._IdVehicleSize;
			}
			set
			{
				if ((this._IdVehicleSize != value))
				{
					this.OnIdVehicleSizeChanging(value);
					this.SendPropertyChanging();
					this._IdVehicleSize = value;
					this.SendPropertyChanged("IdVehicleSize");
					this.OnIdVehicleSizeChanged();
				}
			}
		}
		
		[Column(Storage="_VehicleSizeName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string VehicleSizeName
		{
			get
			{
				return this._VehicleSizeName;
			}
			set
			{
				if ((this._VehicleSizeName != value))
				{
					this.OnVehicleSizeNameChanging(value);
					this.SendPropertyChanging();
					this._VehicleSizeName = value;
					this.SendPropertyChanged("VehicleSizeName");
					this.OnVehicleSizeNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseVehicleType")]
	public partial class baseVehicleType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdVehicleType;
		
		private string _VehicleTypeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdVehicleTypeChanging(int value);
    partial void OnIdVehicleTypeChanged();
    partial void OnVehicleTypeNameChanging(string value);
    partial void OnVehicleTypeNameChanged();
    #endregion
		
		public baseVehicleType()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdVehicleType", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdVehicleType
		{
			get
			{
				return this._IdVehicleType;
			}
			set
			{
				if ((this._IdVehicleType != value))
				{
					this.OnIdVehicleTypeChanging(value);
					this.SendPropertyChanging();
					this._IdVehicleType = value;
					this.SendPropertyChanged("IdVehicleType");
					this.OnIdVehicleTypeChanged();
				}
			}
		}
		
		[Column(Storage="_VehicleTypeName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string VehicleTypeName
		{
			get
			{
				return this._VehicleTypeName;
			}
			set
			{
				if ((this._VehicleTypeName != value))
				{
					this.OnVehicleTypeNameChanging(value);
					this.SendPropertyChanging();
					this._VehicleTypeName = value;
					this.SendPropertyChanged("VehicleTypeName");
					this.OnVehicleTypeNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseZone")]
	public partial class baseZone : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdZone;
		
		private string _ZoneName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdZoneChanging(int value);
    partial void OnIdZoneChanged();
    partial void OnZoneNameChanging(string value);
    partial void OnZoneNameChanged();
    #endregion
		
		public baseZone()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdZone", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdZone
		{
			get
			{
				return this._IdZone;
			}
			set
			{
				if ((this._IdZone != value))
				{
					this.OnIdZoneChanging(value);
					this.SendPropertyChanging();
					this._IdZone = value;
					this.SendPropertyChanged("IdZone");
					this.OnIdZoneChanged();
				}
			}
		}
		
		[Column(Storage="_ZoneName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ZoneName
		{
			get
			{
				return this._ZoneName;
			}
			set
			{
				if ((this._ZoneName != value))
				{
					this.OnZoneNameChanging(value);
					this.SendPropertyChanging();
					this._ZoneName = value;
					this.SendPropertyChanged("ZoneName");
					this.OnZoneNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.baseStationType")]
	public partial class baseStationType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _IdStationType;
		
		private string _StationTypeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdStationTypeChanging(int value);
    partial void OnIdStationTypeChanged();
    partial void OnStationTypeNameChanging(string value);
    partial void OnStationTypeNameChanged();
    #endregion
		
		public baseStationType()
		{
			OnCreated();
		}
		
		[Column(Storage="_IdStationType", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int IdStationType
		{
			get
			{
				return this._IdStationType;
			}
			set
			{
				if ((this._IdStationType != value))
				{
					this.OnIdStationTypeChanging(value);
					this.SendPropertyChanging();
					this._IdStationType = value;
					this.SendPropertyChanged("IdStationType");
					this.OnIdStationTypeChanged();
				}
			}
		}
		
		[Column(Storage="_StationTypeName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string StationTypeName
		{
			get
			{
				return this._StationTypeName;
			}
			set
			{
				if ((this._StationTypeName != value))
				{
					this.OnStationTypeNameChanging(value);
					this.SendPropertyChanging();
					this._StationTypeName = value;
					this.SendPropertyChanged("StationTypeName");
					this.OnStationTypeNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class tblOperatorClusterGetClustersByIdOperatorResult
	{
		
		private int _IdCluster;
		
		private string _ClusterName;
		
		public tblOperatorClusterGetClustersByIdOperatorResult()
		{
		}
		
		[Column(Storage="_IdCluster", DbType="Int NOT NULL")]
		public int IdCluster
		{
			get
			{
				return this._IdCluster;
			}
			set
			{
				if ((this._IdCluster != value))
				{
					this._IdCluster = value;
				}
			}
		}
		
		[Column(Storage="_ClusterName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string ClusterName
		{
			get
			{
				return this._ClusterName;
			}
			set
			{
				if ((this._ClusterName != value))
				{
					this._ClusterName = value;
				}
			}
		}
	}
	
	public partial class tblGetOperatorIdOperatorResult
	{
		
		private int _IdOperator;
		
		private string _OperatorName;
		
		private string _PathToRSTFile;
		
		private string _PathToRSTRouteLineExportFolder;
		
		private string _EnglishOperatorName;
		
		private string _Email;
		
		public tblGetOperatorIdOperatorResult()
		{
		}
		
		[Column(Storage="_IdOperator", DbType="Int NOT NULL")]
		public int IdOperator
		{
			get
			{
				return this._IdOperator;
			}
			set
			{
				if ((this._IdOperator != value))
				{
					this._IdOperator = value;
				}
			}
		}
		
		[Column(Storage="_OperatorName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string OperatorName
		{
			get
			{
				return this._OperatorName;
			}
			set
			{
				if ((this._OperatorName != value))
				{
					this._OperatorName = value;
				}
			}
		}
		
		[Column(Storage="_PathToRSTFile", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string PathToRSTFile
		{
			get
			{
				return this._PathToRSTFile;
			}
			set
			{
				if ((this._PathToRSTFile != value))
				{
					this._PathToRSTFile = value;
				}
			}
		}
		
		[Column(Storage="_PathToRSTRouteLineExportFolder", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string PathToRSTRouteLineExportFolder
		{
			get
			{
				return this._PathToRSTRouteLineExportFolder;
			}
			set
			{
				if ((this._PathToRSTRouteLineExportFolder != value))
				{
					this._PathToRSTRouteLineExportFolder = value;
				}
			}
		}
		
		[Column(Storage="_EnglishOperatorName", DbType="VarChar(50)")]
		public string EnglishOperatorName
		{
			get
			{
				return this._EnglishOperatorName;
			}
			set
			{
				if ((this._EnglishOperatorName != value))
				{
					this._EnglishOperatorName = value;
				}
			}
		}
		
		[Column(Storage="_Email", DbType="VarChar(50)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this._Email = value;
				}
			}
		}
	}
	
	public partial class procGetAlltblOperatorResult
	{
		
		private int _IdOperator;
		
		private string _OperatorName;
		
		private string _PathToRSTFile;
		
		private string _PathToRSTRouteLineExportFolder;
		
		private string _EnglishOperatorName;
		
		private string _Email;
		
		private System.Nullable<long> _RowNumber;
		
		public procGetAlltblOperatorResult()
		{
		}
		
		[Column(Storage="_IdOperator", DbType="Int NOT NULL")]
		public int IdOperator
		{
			get
			{
				return this._IdOperator;
			}
			set
			{
				if ((this._IdOperator != value))
				{
					this._IdOperator = value;
				}
			}
		}
		
		[Column(Storage="_OperatorName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string OperatorName
		{
			get
			{
				return this._OperatorName;
			}
			set
			{
				if ((this._OperatorName != value))
				{
					this._OperatorName = value;
				}
			}
		}
		
		[Column(Storage="_PathToRSTFile", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string PathToRSTFile
		{
			get
			{
				return this._PathToRSTFile;
			}
			set
			{
				if ((this._PathToRSTFile != value))
				{
					this._PathToRSTFile = value;
				}
			}
		}
		
		[Column(Storage="_PathToRSTRouteLineExportFolder", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string PathToRSTRouteLineExportFolder
		{
			get
			{
				return this._PathToRSTRouteLineExportFolder;
			}
			set
			{
				if ((this._PathToRSTRouteLineExportFolder != value))
				{
					this._PathToRSTRouteLineExportFolder = value;
				}
			}
		}
		
		[Column(Storage="_EnglishOperatorName", DbType="VarChar(50)")]
		public string EnglishOperatorName
		{
			get
			{
				return this._EnglishOperatorName;
			}
			set
			{
				if ((this._EnglishOperatorName != value))
				{
					this._EnglishOperatorName = value;
				}
			}
		}
		
		[Column(Storage="_Email", DbType="VarChar(50)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this._Email = value;
				}
			}
		}
		
		[Column(Storage="_RowNumber", DbType="BigInt")]
		public System.Nullable<long> RowNumber
		{
			get
			{
				return this._RowNumber;
			}
			set
			{
				if ((this._RowNumber != value))
				{
					this._RowNumber = value;
				}
			}
		}
	}
	
	public partial class internalUserGetByUserNameResult
	{
		
		private int _IdUser;
		
		private int _IdOperator;
		
		private string _UserName;
		
		private bool _IsSuperViser;
		
		private string _FirstName;
		
		private string _LastName;
		
		private string _FirmName;
		
		private string _Telephone;
		
		public internalUserGetByUserNameResult()
		{
		}
		
		[Column(Storage="_IdUser", DbType="Int NOT NULL")]
		public int IdUser
		{
			get
			{
				return this._IdUser;
			}
			set
			{
				if ((this._IdUser != value))
				{
					this._IdUser = value;
				}
			}
		}
		
		[Column(Storage="_IdOperator", DbType="Int NOT NULL")]
		public int IdOperator
		{
			get
			{
				return this._IdOperator;
			}
			set
			{
				if ((this._IdOperator != value))
				{
					this._IdOperator = value;
				}
			}
		}
		
		[Column(Storage="_UserName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[Column(Storage="_IsSuperViser", DbType="Bit NOT NULL")]
		public bool IsSuperViser
		{
			get
			{
				return this._IsSuperViser;
			}
			set
			{
				if ((this._IsSuperViser != value))
				{
					this._IsSuperViser = value;
				}
			}
		}
		
		[Column(Storage="_FirstName", DbType="VarChar(50)")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this._FirstName = value;
				}
			}
		}
		
		[Column(Storage="_LastName", DbType="VarChar(50)")]
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				if ((this._LastName != value))
				{
					this._LastName = value;
				}
			}
		}
		
		[Column(Storage="_FirmName", DbType="VarChar(50)")]
		public string FirmName
		{
			get
			{
				return this._FirmName;
			}
			set
			{
				if ((this._FirmName != value))
				{
					this._FirmName = value;
				}
			}
		}
		
		[Column(Storage="_Telephone", DbType="VarChar(50)")]
		public string Telephone
		{
			get
			{
				return this._Telephone;
			}
			set
			{
				if ((this._Telephone != value))
				{
					this._Telephone = value;
				}
			}
		}
	}
}
#pragma warning restore 1591