using System;
using System.Collections.Generic;
using IBLManager;
using BLEntities.Entities;
using BLEntities.Model;
using Utilities;
namespace BLManager
{
    public class RouteStopDetailPresenter : IRouteStopDetailPresenter
    {
        private readonly IDataSearchAndManipulateBLManager _dataSearchAndManipulateBlManager;
        private readonly ITransCadBLManager _transCadBlManager ;
        public RouteStopDetailPresenter()
        {
            _dataSearchAndManipulateBlManager = new DataSearchAndManipulateBLManager();

            _transCadBlManager = new TransCadBlManager();
            if (_transCadBlManager.IsEgedOperator)
            {
                _transCadBlManager = new EgedBlManager();
            }
        }
        #region IRouteStopDetailPresenter Members
        /// <summary>
        /// Save Route Stop
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool SaveRouteStop(RouteStop rs)
        {
            if (!_dataSearchAndManipulateBlManager.IsRouteStopValid(rs))
            {
                return false;
            }
            if (_transCadBlManager.SaveRouteStop(rs))
            {
                _transCadBlManager.SetRouteStopsDataInSpecificRouteLine(rs.RouteLine, StationOrderConst.Regular);

                return true; 
            }
            else
            {
                return false; 
            }
        }
        /// <summary>
        /// IsRouteStopValid
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public bool IsRouteStopValid(RouteStop routeStop)
        {
            return _dataSearchAndManipulateBlManager.IsRouteStopValid(routeStop);
        }

        /// <summary>
        /// SetNotInUseFloorAndPlatform
        /// </summary>
        /// <param name="routeStop"></param>
        
        public void SetNotInUseFloorAndPlatform(RouteStop routeStop)
        {
            if (routeStop!=null)
            {
                routeStop.Platform = "0";
            }
        }

        public bool IsExistsHoradaStationInLine(int routeId)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                var routeLine = GlobalData.RouteModel.RouteLineList.FindLast(rl => rl.ID == routeId);
                if (routeLine != null)
                    return _transCadBlManager.IsExistsHoradaStationInLine(routeLine);
            }
            return false;
        }

        /// <summary>
        /// Get Last FilterdItem
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteStop GetLastFilterdItem(List<RouteStop> filteredList, RouteStop current)
        {
            if (filteredList == null) return null;
            return filteredList[0];
        }
        /// <summary>
        /// Get First Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteStop GetFirstFilterdItem(List<RouteStop> filteredList, RouteStop current)
        {
            if (filteredList == null) return null;
            return filteredList[filteredList.Count - 1];
        }
        /// <summary>
        /// Get Next Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteStop GetNextFilterdItem(List<RouteStop> filteredList, RouteStop current)
        {
            if (filteredList == null || current == null) return null;
            RouteStop retValue = current;

            int index = filteredList.FindIndex(p => p.ID == current.ID);
            if (index < filteredList.Count - 1)
            {
                retValue = filteredList[++index];
            }
            return retValue;
        }
        /// <summary>
        /// Get Prev Filterd Item
        /// </summary>
        /// <param name="filteredList"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public RouteStop GetPrevFilterdItem(List<RouteStop> filteredList, RouteStop current)
        {
            if (filteredList == null || current == null) return null;
            RouteStop retValue = current;

            int index = filteredList.FindIndex(p => p.ID == current.ID);
            if (index > 0)
            {
                retValue = filteredList[--index];
            }
            return retValue;
        }
        /// <summary>
        /// GetServiceType
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetStationType()
        {
            return _dataSearchAndManipulateBlManager.GetStationType();
        }
        /// <summary>
        /// GetStationOrder
        /// </summary>
        /// <param name="routeStop"></param>
        /// <returns></returns>
        public StationOrderConst GetStationOrder(RouteStop routeStop)
        {
            return _dataSearchAndManipulateBlManager.GetStationOrder(routeStop);
        }
        
        /// <summary>
        /// GetStationCatalogHorada
        /// </summary>
        /// <returns></returns>
        public List<BaseTableEntity> GetStationCatalogHorada(RouteStop routeStop)
        {
            return _dataSearchAndManipulateBlManager.GetStationCatalogHorada(routeStop);
        }

        /// <summary>
        /// Set Horada In Specific RouteLine
        /// </summary>
        public void SetHoradaInSpecificRouteLine(int routeLineId)
        {
            if (GlobalData.RouteModel.RouteLineList.IsListFull())
            {
                RouteLine routeLine = GlobalData.RouteModel.RouteLineList.FindLast(rl => rl.ID == routeLineId);
                if (routeLine!=null)
                    _transCadBlManager.SetHoradaInSpecificRouteLine(routeLine);
            }
        }
      /// <summary>
        /// IsLoweringStation
      /// </summary>
      /// <param name="be"></param>
      /// <returns></returns>
        public bool IsLoweringStation(BaseTableEntity be)
        {
            return be!=null && (be.ID == 2 || be.ID == 4);
        }
        

        #endregion
    }
}
