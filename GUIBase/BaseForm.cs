using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLEntities;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Utilities;
using BLEntities.Entities;
using BLEntities.Accessories;
using System.Threading;



namespace GUIBase
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
           
        }
        public BaseForm(string caption)
            : this()
        {
            this.Text = caption; 

        }

        protected virtual void InitComponents()
        {
        }

        protected void InitBaseCombo(ComboBox cmb, List<BaseTableEntity> lst, int value)
        {
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "ID";
            cmb.DataSource = lst;
            foreach (BaseTableEntity item in cmb.Items)
            {
                if (item.ID == value)
                {
                    cmb.SelectedValue = value;
                    break;
                }
            }
        }


        protected  void InitBaseCombo(ComboBox cmb, List<BaseTableEntity> lst)
        {
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "ID";
            cmb.DataSource = lst;
        }
        private void BaseForm_Load(object sender, EventArgs e)
        {
            InitComponents();
        }
        //public void CheckIntegrity(ValidationResults results, BaseClass baseClass)
        //{
        //    RouteStop rs = baseClass as RouteStop;
        //    if (rs.ID > 4)
        //    {
        //        ValidationResult result = new ValidationResult
        //          ("DDDDDDDDDDDDDDDD", this, "ErrorCodes", null, null);
        //        results.AddResult(result);
        //    }

        //}
    }
}
