using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

using SendGrid;
using System.Net.Mime;
using System.Web;
using System.Reflection;
using System;
using System.IO;
using System.Linq;

namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// Handles send grid related activities
    /// </summary>
    public static class SendGridUtilities
    {
        /// <summary>
        /// Sends Email
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendEmail(List<string> recipients, string subject, string body)
        {
            Web transportWeb;
            var message = GetEmailMessage(out transportWeb, recipients, subject, body);

            // Send the email, which returns an awaitable task.
            transportWeb.DeliverAsync(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachment"></param>
        /// <param name="fileName"></param>
        public static void SendEmailWithAttachment(List<string> recipients, string subject, string body, System.IO.Stream attachment, string fileName)
        {
            Web transportWeb;
            var message = GetEmailMessage(out transportWeb, recipients, subject, body);

            message.AddAttachment(attachment, fileName);
            transportWeb.DeliverAsync(message);
        }

        /// <summary>
        /// Generates te email and returns the content
        /// </summary>
        /// <param name="transportWeb"></param>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static SendGridMessage GetEmailMessage(out Web transportWeb, List<string> recipients, string subject, string body)
        {
            var apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"].ToString();
            // create a Web transport, using API Key
            transportWeb = new Web(apiKey);
            SendGridMessage message = ConstructEmailMessage(recipients, subject, body);
            return message;
        }

        /// <summary>
        /// Consructs the Email Message
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static SendGridMessage ConstructEmailMessage(List<string> recipients, string subject, string body)
        {
            var myMessage = new SendGridMessage();
            // Add the message properties.
            myMessage.From = new MailAddress(ConfigurationManager.AppSettings["SENDGRID_FROMADD"]);

            // Add multiple addresses to the To field.
            myMessage.AddTo(recipients);
            myMessage.Subject = subject;
            myMessage.Html = body;
            return myMessage;
        }

    }
}