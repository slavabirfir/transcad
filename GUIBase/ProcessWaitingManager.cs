using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUIBase
{
    /// <summary>
    /// Comleted
    /// </summary>
    public delegate void Comleted();
    /// <summary>
    /// Process Waiting Manager
    /// </summary>
    public class ProcessWaitingManager
    {
         
        public bool ShowProgressBar { get; set; }
        public int ProgressBarInitValue { get; set; }
        public int ProgressBarMaxValue { get; set; }
        public string  ProgressBarMessage { get; set; }
        public bool ShowCancelButton { get; set; }
        /// <summary>
        /// process Waiting
        /// </summary>
        private frmProcessWaiting processWaiting = null;
        /// <summary>
        /// Close Dialog Form
        /// </summary>
        private void CloseDialogForm()
        {
            try
            {
                ClearParams();
                processWaiting.Close();
            }
            catch
            {

            }
        }

        private void ClearParams()
        {
            ShowProgressBar = false;
            ProgressBarInitValue = 0;
            ProgressBarMaxValue = 0;
            ProgressBarMessage = string.Empty;
            ShowCancelButton = false; 
        }

        ///// <summary>
        ///// Show Dialog Form
        ///// </summary>
        //private void ShowDialogForm()
        //{
        //    ShowDialogForm(false);
        //}

        private void ShowDialogForm()
        {
            try
            {
                processWaiting = new frmProcessWaiting();
                processWaiting.InitControls(ShowProgressBar, ProgressBarInitValue, ProgressBarMaxValue, ProgressBarMessage, ShowCancelButton);
                processWaiting.ShowDialog();
            }
            catch
            {

            }
        }
        static Thread thread = null;
        public void ShowProcessWaitingForm()
        {
            thread = new Thread(ShowDialogForm);
            thread.Start();
        }

        public bool IsCanceledByUser
        {
            get
            {
                return processWaiting!=null && processWaiting.IsCanceledByUser;
            }
        }
        /// <summary>
        /// Close Process Waiting Form
        /// </summary>
        public void CloseProcessWaitingForm()
        {
            try
            {
                Comleted c = new Comleted(CloseDialogForm);
                processWaiting.BeginInvoke(c);
            }
            catch
            {
 
            }
        }
        /// <summary>
        /// ChangeProgressBar
        /// </summary>
        /// <param name="progressBarValue"></param>
        /// <param name="txtMessageText"></param>
        public void ChangeProgressBar(int progressBarValue,int maxprogressBarValue, string txtMessageText)
        {
            if (processWaiting == null)
                processWaiting = new frmProcessWaiting(); 
            processWaiting.ChangeProgressBarAndMessage(progressBarValue,maxprogressBarValue, txtMessageText);
        }
    }

 }
