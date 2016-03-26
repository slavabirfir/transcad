namespace RouteExportProcess
{
    partial class frmClusterOfOperator
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
            this.lstCluster = new System.Windows.Forms.ListBox();
            this.bndSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lstCluster
            // 
            this.lstCluster.DataSource = this.bndSource;
            this.lstCluster.FormattingEnabled = true;
            this.lstCluster.Location = new System.Drawing.Point(3, 4);
            this.lstCluster.Name = "lstCluster";
            this.lstCluster.Size = new System.Drawing.Size(225, 121);
            this.lstCluster.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(3, 129);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(77, 22);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "בחר אשכול";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(183, 129);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 22);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "בטל";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmClusterOfOperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 155);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lstCluster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClusterOfOperator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "בחר אשכול";
            ((System.ComponentModel.ISupportInitialize)(this.bndSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstCluster;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource bndSource;
    }
}