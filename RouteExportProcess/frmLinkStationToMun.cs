using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLManager;
using BLEntities.Entities;

namespace RouteExportProcess
{
    public partial class frmLinkStationToMun : GUIBase.BaseForm
    {
        private readonly BlLinkStationToCity linkStationToCityBL;
        /// <summary>
        /// ctr
        /// </summary>
        public frmLinkStationToMun()
        {
            InitializeComponent();
            linkStationToCityBL = new BlLinkStationToCity();
        }
        /// <summary>
        /// Init Components
        /// </summary>
        protected override void InitComponents()
        {
            linkStationToCityBL.InitData();
            BindStationList();
            BindCityList();
        }
        /// <summary>
        /// Bind City List
        /// </summary>
        private void BindCityList()
        {
            linkStationToCityBL.SetFilteredCityList(txtSearchCity.Text);
            lstCity.DataSource = linkStationToCityBL.SelectedCityList;
            lstCity.SelectedIndex = -1;
        }
        /// <summary>
        /// btn Close Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// txt Search Station Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchStation_TextChanged(object sender, EventArgs e)
        {
            BindStationList();
        }
        /// <summary>
        /// BindStationList
        /// </summary>
        private void BindStationList()
        {
            linkStationToCityBL.SetFilteredStationList(txtSearchStation.Text);
            lstStations.DataSource = linkStationToCityBL.SelectedStationList;
            lstStations.SelectedIndex = -1;
            lstCity.SelectedIndex = -1; 
        }
        /// <summary>
        /// txt Search City Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchCity_TextChanged(object sender, EventArgs e)
        {
            BindCityList();
        }
        
        /// <summary>
        /// lst Stations Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstStations_Click(object sender, EventArgs e)
        {
            if (lstStations.SelectedIndex >=0)
            {
                StationToCityLink stationToCityLink = lstStations.SelectedItem as StationToCityLink;
                if (stationToCityLink != null)
                {
                    City selectedCity = linkStationToCityBL.GetCity(stationToCityLink);
                    lstCity.SelectedItem = selectedCity;

                }
            }
        }
        /// <summary>
        /// btn Belong Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBelong_Click(object sender, EventArgs e)
        {
            if (!LinkUnlink(true))
            {
                lstStations_Click(null, null);
            }
        }
        /// <summary>
        /// LinkUnlink
        /// </summary>
        /// <param name="link"></param>
        private bool LinkUnlink(bool link)
        {
            if (lstStations.SelectedIndex == -1)
            {
                GUIBase.GuiHelper.ShowInfoMessage(Resources.ResourceGUI.SelectStation);
                return false; 
            }
            if (lstCity.SelectedIndex == -1)
            {
                GUIBase.GuiHelper.ShowInfoMessage(Resources.ResourceGUI.SelectCity);
                return false;
            }
            string message = string.Empty;
            bool result = link ? linkStationToCityBL.LinkStationToCity(lstCity.SelectedItem as City, lstStations.SelectedItem as StationToCityLink, ref message) :
                                 linkStationToCityBL.UnLinkStationToCity(lstCity.SelectedItem as City, lstStations.SelectedItem as StationToCityLink, ref message);

            if (!result)
                GUIBase.GuiHelper.ShowErrorMessage(message);
            return true ;
        }
        /// <summary>
        /// btn Clear Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (LinkUnlink(false))
            {
                lstCity.SelectedIndex = -1;
                BindCityList();
            }
        }
        /// <summary>
        /// btn Report Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReport_Click(object sender, EventArgs e)
        {
            linkStationToCityBL.WriteUnlinkedStations();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchStation.Text = string.Empty;  
            BindStationList();
        }
       
    }
}
