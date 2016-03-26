﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using BLEntities.Entities;

namespace GUIBase
{
    public delegate bool IsSelectable(RouteStop rs);
    

    public class DGVColumnHeaderRouteStop : DataGridViewColumnHeaderCell
    {
        public IsSelectable IsSelectabeFunction {get ;set;}

        
        private Rectangle CheckBoxRegion;
        private bool checkAll = false;
        public String Caption { get; set; }
        public int SelectedColumnIndex { get; set; }
        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value,
                formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            graphics.FillRectangle(new SolidBrush(cellStyle.BackColor), cellBounds);

            CheckBoxRegion = new Rectangle(
                cellBounds.Location.X + 1,
                cellBounds.Location.Y + 2,
                25, cellBounds.Size.Height - 4);


            if (this.checkAll)
                ControlPaint.DrawCheckBox(graphics, CheckBoxRegion, ButtonState.Checked);
            else
                ControlPaint.DrawCheckBox(graphics, CheckBoxRegion, ButtonState.Normal);

            Rectangle normalRegion =
                new Rectangle(
                cellBounds.Location.X + 1 + 25,
                cellBounds.Location.Y,
                cellBounds.Size.Width - 26,
                cellBounds.Size.Height);

            graphics.DrawString(Caption, cellStyle.Font, new SolidBrush(cellStyle.ForeColor), normalRegion);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            //Convert the CheckBoxRegion 
            Rectangle rec = new Rectangle(new Point(0, 0), this.CheckBoxRegion.Size);
            this.checkAll = !this.checkAll;
            if (rec.Contains(e.Location))
            {
                this.DataGridView.Invalidate();
                this.DataGridView.EndEdit();
                if (e.ColumnIndex == SelectedColumnIndex)
                {
                    for (int i = 0; i < this.DataGridView.Rows.Count; i++)
                    {
                        if (IsSelectabeFunction(this.DataGridView.Rows[i].DataBoundItem as RouteStop))
                           this.DataGridView.Rows[i].Cells[SelectedColumnIndex].Value = this.checkAll;
                    }
                }
            }
            base.OnMouseClick(e);
        }

        public bool CheckAll
        {
            get { return this.checkAll; }
            set { this.checkAll = value; }
        }
    }
}
