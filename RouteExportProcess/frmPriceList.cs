using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLManager;
using BLEntities.Entities;

namespace RouteExportProcess
{
    public partial class frmPriceList : GUIBase.BaseForm
    {
        #region private
        PriceListPolygonBL priceListPolygonBL = null;
        #endregion

        /// <summary>
        /// .ctr
        /// </summary>
        public frmPriceList()
        {
            InitializeComponent();
            priceListPolygonBL = new PriceListPolygonBL();
            lstData.DataSource = priceListPolygonBL.DataPriceArea ;
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
        /// lst Data Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstData.SelectedIndex >= 0)
            {
                PriceArea priceArea = lstData.SelectedItem as PriceArea;
                if (priceArea.FromDate == DateTime.MinValue)
                {
                    dtpFromDate.CustomFormat = " ";
                    
                }
                else
                {
                    dtpFromDate.CustomFormat = "dd/MM/yyyy";
                    dtpFromDate.Value = priceArea.FromDate;
                }
                dtpFromDate.Format = DateTimePickerFormat.Custom;
                
            }
        }
        /// <summary>
        /// dtp From Date Value Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            dtpFromDate.CustomFormat = "dd/MM/yyyy";
        }
        

        /// <summary>
        /// txt Search Text Changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lstData.DataSource = priceListPolygonBL.Search(txtSearch.Text);
        }
        /// <summary>
        /// btnSaveDates_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveDates_Click(object sender, EventArgs e)
        {
            if (lstData.SelectedIndex >= 0)
            {
                //PriceArea priceArea = lstData.SelectedItem as PriceArea;
                //priceArea.FromDate = string.IsNullOrEmpty(dtpFromDate.Text.Trim()) ? DateTime.MinValue :  dtpFromDate.Value;
                //string message = string.Empty;
                //if (!priceListPolygonBL.Update(lstData.SelectedItem as PriceArea, ref message))
                //{
                //    GUIBase.GUIHelper.ShowErrorMessage(message);
                //}
            }
        }

        

    }
}
