using System;
using System.Collections.Generic;
using System.Linq;
using BLManager;
using GUIBase;
using IBLManager;
using BLEntities.Entities;

namespace RouteExportProcess
{
    public partial class FrmLinkstationToMunArea : BaseForm
    {
        //private const string StationCountString = "מס' תחנות {0}";
        private IBlLinkedMunicipalityStations _blLinkedMunicipalityStations;
        public FrmLinkstationToMunArea()
        {
            InitializeComponent();
        }

        private void FrmLinkstationToMunArea_Load(object sender, EventArgs e)
        {
            _blLinkedMunicipalityStations = new BlLinkedMunicipalityStations();
            lstStations.DataSource = _blLinkedMunicipalityStations.GetSelectedStations(null, true);
            txtSN.Text = string.Format(Resources.ResourceGUI.StationCountString, _blLinkedMunicipalityStations.StationCount); 
        }

        private void CleanUp()
        {
            lstAreas.DataSource = null;
            txtMuniSel.Text = string.Empty;
            txtSN.Text = string.Empty; 
        }

        private void LstStationsSelectedIndexChanged(object sender, EventArgs e)
        {
            CleanUp();
            if (lstStations.SelectedItem == null) return;
            txtMuniSel.Text = _blLinkedMunicipalityStations.GetCityNameByPs(lstStations.SelectedItem as BaseTableEntity);
            txtSN.Text = string.Format(Resources.ResourceGUI.StationCountString, _blLinkedMunicipalityStations.StationCount); 
        }

        private void BtnShowCloseAreasClick(object sender, EventArgs e)
        {
            if (lstStations.SelectedItem == null) return;
            var processWaitingManager = new ProcessWaitingManager();
            try
            {
               processWaitingManager.ShowProcessWaitingForm();
               lstAreas.DataSource = _blLinkedMunicipalityStations.GetMunAreasByPhysicalStop(lstStations.SelectedItem as BaseTableEntity);
               processWaitingManager.CloseProcessWaitingForm();
               if (lstAreas.DataSource != null && (lstAreas.DataSource as List<City>).Any())
                   lstAreas.SelectedIndex = -1;
               else
                   GuiHelper.ShowInfoMessage(Resources.ResourceGUI.CloseAreNotFound);
               
            }
            catch (Exception)
            {
                processWaitingManager.CloseProcessWaitingForm();
                throw;
            }
        }

        private void BtnLinkStationToAreaClick(object sender, EventArgs e)
        {
            if (lstStations.SelectedIndex>=0 && lstAreas.SelectedIndex>=0)
            {
                if (_blLinkedMunicipalityStations.LinkStationtoMunArea(lstStations.SelectedItem as BaseTableEntity,
                     Convert.ToInt32(lstAreas.SelectedValue)))
                {
                    LstStationsSelectedIndexChanged(null, null);
                    GuiHelper.ShowInfoMessage(Resources.ResourceGUI.StationWasSuccesefullyLinked);
                }
            }
        }

        private void TxtStationCatalogSearchTextChanged(object sender, EventArgs e)
        {
            CleanUp();
            lstStations.DataSource = _blLinkedMunicipalityStations.GetSelectedStations(txtStationCatalogSearch.Text,
                                                                                       chAllNonLinked.Checked);
            txtSN.Text = string.Format(Resources.ResourceGUI.StationCountString, _blLinkedMunicipalityStations.StationCount); 
        }

        private void ChAllNonLinkedCheckedChanged(object sender, EventArgs e)
        {
            CleanUp();
            lstStations.DataSource = _blLinkedMunicipalityStations.GetSelectedStations(txtStationCatalogSearch.Text,
                                                                                       chAllNonLinked.Checked);
            txtSN.Text = string.Format(Resources.ResourceGUI.StationCountString, _blLinkedMunicipalityStations.StationCount); 
        }

        private void CmdCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnShowStationOnMapClick(object sender, EventArgs e)
        {
            if (lstStations.SelectedIndex >= 0)
            {
                _blLinkedMunicipalityStations.ShowStationOnMap(lstStations.SelectedItem as BaseTableEntity);
            }
        }

       
    }
}
