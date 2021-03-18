using System;
using System.Net;
using System.Net.Mail;

namespace AP.FamilyTree.Mail
{
    public class MailSenderComponent
    {
        private string SmtpServer { get; }
        private int Port { get; }
        private NetworkCredential MailCredential { get; }

        public MailSenderComponent(SmtpConfig initparameters)
        {
            SmtpServer = initparameters.SmtpServer;
            Port = initparameters.Port;
            MailCredential = initparameters.NetworkCredentials;
        }

        public void SendMail(MailData mailData)
        {
            if (string.IsNullOrEmpty(mailData.FromEmail.Address))
                throw new Exception("MAIL_FROM: ist leer");

            if (string.IsNullOrEmpty(mailData.ToEmail.Address))
                throw new Exception("MAIL_TO: ist leer");

            var message = new MailMessage();
            try
            {
                message.From = mailData.FromEmail;
                message.To.Add(mailData.ToEmail);
                message.Bcc.Add(mailData.BccEmail);
                message.Subject = mailData.Subject;
                message.Body = mailData.Body;
                message.IsBodyHtml = true;

                if (mailData.AttachmentData != null)
                {
                    foreach (var att in mailData.AttachmentData)
                    {
                        message.Attachments.Add(att);
                    }
                }

                var client = new SmtpClient(this.SmtpServer, this.Port)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = this.MailCredential
                };

                client.Send(message);

            }
            catch (Exception xpt)
            {
                var messageExc = xpt.Message;
                if (xpt.InnerException != null)
                {
                    messageExc += xpt.InnerException;
                    throw new Exception(messageExc);
                }

                throw new Exception(messageExc + " " + mailData.ToEmail);
            }

            //Logger.Write("BoiMailSender.SendMail(): Mail verschickt an " + message.To);
            return;
        }
    }
}
