using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUIBase
{
    


    public partial class frmProcessWaiting : Form
    {
        private delegate void CallAsyncDelegate(int progressBarValue,int maxProgressBarValue, string message);

        
        public bool IsCanceledByUser { get; private set; }

        private bool alternateFlag = false;
        /// <summary>
        /// frmProcessWaiting
        /// </summary>
        public frmProcessWaiting()
        {
            InitializeComponent();
            this.lblWaitMessage.Text = Resources.ResourceGUIBase.WaiteMessage;
            this.timerControl.Enabled = true;
            this.timerControl.Tick += new EventHandler(timerControl_Tick);
        }
       
        /// <summary>
        /// InitControls
        /// </summary>
        /// <param name="showAll"></param>
        /// <param name="maxProgressBar"></param>
        /// <param name="txtMessageText"></param>
        public void InitControls(bool showAll,int progressBarInitValue ,int progressBarMaxValue ,string  progressBarMessage)
        {
            InitControls(showAll, progressBarInitValue, progressBarMaxValue, progressBarMessage,false);
        }

        public void InitControls(bool showAll, int progressBarInitValue, int progressBarMaxValue, string progressBarMessage,bool isShowCancelButton)
        {
            if (isShowCancelButton)
            {
                this.lblWaitMessage.TextAlign = HorizontalAlignment.Left;
                this.btnCancel.BringToFront();
            }
            this.prBar.Minimum = 0;
            if (!showAll)
            {
                this.Height = lblWaitMessage.Height;
            }
            else
            {
                this.prBar.Maximum = progressBarMaxValue;
                this.prBar.Minimum = 0;
                this.prBar.Value = progressBarInitValue;
                this.txtMessgae.Text = progressBarMessage;
            }
        }

        /// <summary>
        /// Change Progress Bar And Message
        /// </summary>
        /// <param name="progressBarValue"></param>
        /// <param name="message"></param>
        public void ChangeProgressBarAndMessage(int progressBarValue, int maxProgressBarValue, string message)
        {
            if (IsCanceledByUser)
            {
                this.Invoke(new Action(Close));
                return;
            }
            Invoke(new CallAsyncDelegate(UpdateUI), progressBarValue, maxProgressBarValue, message);
        }

        public void ChangeProgressBarValues(int progressBarValue, int maxProgressBarValue, string message)
        {
            this.prBar.Minimum = 0;
            this.prBar.Value = progressBarValue;
            this.prBar.Maximum = maxProgressBarValue;
            this.txtMessgae.Text = message;
        }


        /// <summary>
        /// Update UI
        /// </summary>
        /// <param name="progressBarValue"></param>
        /// <param name="message"></param>
        private void UpdateUI(int progressBarValue, int maxProgressBarValue, string message)
        {
            this.prBar.Value = progressBarValue;
            this.prBar.Maximum = maxProgressBarValue;
            this.txtMessgae.Text = message;
        } 
        /// <summary>
        /// timerControl_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void timerControl_Tick(object sender, EventArgs e)
        {

            if (!alternateFlag)
            {
                lblWaitMessage.ForeColor = Color.Yellow; 
            }
            else
            {
                lblWaitMessage.ForeColor = Color.Blue ; 
            }
            alternateFlag = !alternateFlag;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCanceledByUser = true;
        }

        private void btnCancel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = this.Cursor = Cursors.Default;
        }

        private void btnCancel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = this.Cursor = Cursors.WaitCursor;
        }

       
    }
}
