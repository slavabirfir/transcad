using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Utilities
{
    public static class MailSender
    {
        //sbirfir@yahoo.com
        public static void SendMail(string body, string message, string from, string to,string mailUser, 
            string mailPassword,
            string mailServer,
            int mailPort,
            bool mailEnableSsl,
            bool mailUseDefaultCredentials
            )
        {
            //Create Mail Message Object with content that you want to send with mail.
            MailMessage MyMailMessage = new System.Net.Mail.MailMessage(from, to,
            body, message);

            MyMailMessage.IsBodyHtml = false;

            //Proper Authentication Details need to be passed when sending email from gmail
            System.Net.NetworkCredential mailAuthentication = new
            System.Net.NetworkCredential(mailUser, mailPassword);

            //Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
            //For different server like yahoo this details changes and you can
            //get it from respective server.
            System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(mailServer, mailPort );
            //Enable SSL
            mailClient.EnableSsl = mailEnableSsl;
            mailClient.UseDefaultCredentials = mailUseDefaultCredentials;
            mailClient.Credentials = mailAuthentication;
            mailClient.Send(MyMailMessage);
        }
    }
}
