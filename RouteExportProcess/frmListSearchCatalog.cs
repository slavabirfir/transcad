using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IBLManager;
using BLManager;
using BLEntities.Entities;

namespace RouteExportProcess
{
    public partial class frmListSearchCatalog : GUIBase.BaseForm
    {
        private const string delimiter = ",";
        private FastSearchBLManagerPresenterCatalog fastSearchBLManagerPresenterCatalog = null;
        /// <summary>
        /// Results
        /// </summary>
        public string Results
        {
            get
            {
                return txtResults.Text;
            }
        }
        
        /// <summary>
        /// frmListSearch
        /// </summary>
        public frmListSearchCatalog()
        {
            InitializeComponent();
        }

       public frmListSearchCatalog(List<CatalogInfo> data, string caption, string initData)
        {
            InitializeComponent();
            fastSearchBLManagerPresenterCatalog = new FastSearchBLManagerPresenterCatalog(data); 
            lstData.DataSource = data;
            
            txtResults.Text = initData;
            this.Text = caption;
            SetCheckedData(initData);
            fastSearchBLManagerPresenterCatalog.UserSelectedData = initData;
            this.lstData.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstData_ItemCheck);
            lstData.SelectedIndex = -1;
        }

        /// <summary>
        /// SetChechekdData
        /// </summary>
        /// <param name="initData"></param>
        private void SetCheckedData(string initData)
        {
            for (int i = 0; i < lstData.Items.Count; i++)
            {
                lstData.SetItemCheckState(i, CheckState.Unchecked);
            }
            lstData.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(initData))
            {
                string initCorrectData = initData.StartsWith(",") ? initData.Substring(1) : initData;
                string[] strList = initCorrectData.Split(delimiter.ToCharArray());
                

                for (int i = 0; i < lstData.Items.Count; i++)
                {

                    foreach (var str in strList)
                    {
                        string lstValueCatalog = (lstData.Items[i] as CatalogInfo).Catalog.ToString();
                        if (!string.IsNullOrEmpty(str) && str.Equals(lstValueCatalog))
                        {
                            lstData.SetItemCheckState(i, CheckState.Checked);
                            break;
                        }
                        
                    }

                }
            }
        }

        /// <summary>
        /// txtSearch TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
            lstData.DataSource  = fastSearchBLManagerPresenterCatalog.GetFilteredList(txtSearch.Text);
            SetCheckedData(txtResults.Text);
        }
        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// btnClear Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearResult(true,true); 
        }

        private void ClearResult(bool resultBox, bool searchBox)
        {
            if (resultBox)
                txtResults.Text = string.Empty;
            if (searchBox)
                txtSearch.Text = string.Empty;
            if (lstData.Items != null && lstData.Items.Count > 0)
            {
                for (int i = 0; i < lstData.Items.Count; i++)
                {
                    lstData.SetItemChecked(i, false);
                }
            }
        }

        
        /// <summary>
        /// btnSearchClear Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;  
        }
        

        private void lstData_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (lstData.SelectedIndex >= 0)
            {
                string valueSelected = (lstData.SelectedItem as CatalogInfo).Catalog.ToString();
                if (e.NewValue == CheckState.Checked)
                {
                    if (!fastSearchBLManagerPresenterCatalog.IsItemExists(valueSelected))
                    {
                        if (txtResults.Text.IndexOf(lstData.SelectedValue.ToString())==-1)
                        {
                          txtResults.Text = string.Format("{0}{1}{2}", txtResults.Text, delimiter, valueSelected);
                          fastSearchBLManagerPresenterCatalog.UserSelectedData = txtResults.Text;
                        }
                    }
                }
                else if(e.NewValue == CheckState.Unchecked) 
                {
                    if (txtResults.Text.IndexOf(valueSelected) >= 0)
                    {
                        txtResults.Text = txtResults.Text.Replace(string.Format("{1}{0}", valueSelected, delimiter), string.Empty);
                    }
                    if (fastSearchBLManagerPresenterCatalog.IsItemExists(lstData.SelectedValue.ToString()))
                    {
                        txtResults.Text = txtResults.Text.Replace(string.Concat(valueSelected, delimiter), string.Empty);
                        txtResults.Text = txtResults.Text.Replace(string.Concat(delimiter, valueSelected), string.Empty);
                        fastSearchBLManagerPresenterCatalog.UserSelectedData = txtResults.Text;
                    }
                }
            }
        }

        
    }
}
