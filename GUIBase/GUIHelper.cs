using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Logger;

namespace GUIBase
{
    public static class GuiHelper
    {
        public static string Caption = Resources.ResourceGUIBase.Caption;
        /// <summary>
        /// Show Info Message
        /// </summary>
        /// <param name="message"></param>
        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message,Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Show Error Message
        /// </summary>
        /// <param name="message"></param>
        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            LoggerManager.WriteToLog(message);
        }
        /// <summary>
        /// Show Confirmation Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ShowConfirmationMessage(string message)
        {
          return MessageBox.Show(message, Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ;
        }
    }
}
