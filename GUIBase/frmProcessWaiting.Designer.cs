namespace GUIBase
{
    partial class frmProcessWaiting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcessWaiting));
            this.timerControl = new System.Windows.Forms.Timer(this.components);
            this.prBar = new System.Windows.Forms.ProgressBar();
            this.txtMessgae = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWaitMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // timerControl
            // 
            this.timerControl.Enabled = true;
            this.timerControl.Interval = 1000;
            // 
            // prBar
            // 
            this.prBar.Location = new System.Drawing.Point(0, 58);
            this.prBar.Name = "prBar";
            this.prBar.Size = new System.Drawing.Size(411, 17);
            this.prBar.TabIndex = 1;
            // 
            // txtMessgae
            // 
            this.txtMessgae.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMessgae.Enabled = false;
            this.txtMessgae.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessgae.ForeColor = System.Drawing.Color.Navy;
            this.txtMessgae.Location = new System.Drawing.Point(0, 36);
            this.txtMessgae.Name = "txtMessgae";
            this.txtMessgae.Size = new System.Drawing.Size(410, 21);
            this.txtMessgae.TabIndex = 2;
            this.txtMessgae.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Blue;
            this.btnCancel.Location = new System.Drawing.Point(328, -1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 38);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "בטל תהליך";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.MouseLeave += new System.EventHandler(this.btnCancel_MouseLeave);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseEnter += new System.EventHandler(this.btnCancel_MouseEnter);
            // 
            // lblWaitMessage
            // 
            this.lblWaitMessage.BackColor = System.Drawing.Color.Red;
            this.lblWaitMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaitMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaitMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblWaitMessage.Location = new System.Drawing.Point(0, -2);
            this.lblWaitMessage.Name = "lblWaitMessage";
            this.lblWaitMessage.ReadOnly = true;
            this.lblWaitMessage.Size = new System.Drawing.Size(410, 38);
            this.lblWaitMessage.TabIndex = 4;
            this.lblWaitMessage.TabStop = false;
            this.lblWaitMessage.Text = "נא, המתן עד לסיום הפעולה";
            this.lblWaitMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmProcessWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 76);
            this.ControlBox = false;
            this.Controls.Add(this.txtMessgae);
            this.Controls.Add(this.prBar);
            this.Controls.Add(this.lblWaitMessage);
            this.Controls.Add(this.btnCancel);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProcessWaiting";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerControl;
        private System.Windows.Forms.ProgressBar prBar;
        private System.Windows.Forms.TextBox txtMessgae;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox lblWaitMessage;
    }
}