using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AP.FamilyTree.Mail;
using AP.FamilyTree.Web.Data.Models;

namespace AP.FamilyTree.Web.Data.Services
{
    public class MailingService : BaseMailingService
    {
        public MailingService(MailSettings settings) : base(settings)
        {
        }
        public bool SendMessage(MailItem item)
        {
            var mailData = new MailData();
            var to = item.ToEmail;
            mailData.ToEmail = new MailAddress(to);   
            mailData.FromEmail = new MailAddress(mMailSettings.FROM);
            mailData.BccEmail = new MailAddress(mMailSettings.BCC);

            try
            {
                mailData.Subject = item.Subject;
                mailData.Body = GetBody(item.Text);

                mEmailClient.SendMail(mailData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var messageExc = ex.Message;
                if (ex.InnerException != null)
                {
                    messageExc += ex.InnerException;
                    throw new Exception(messageExc);
                }

                throw new Exception(messageExc);
            }

            return true;
        }

        private string GetBody(string text)
        {
            string body = string.Empty;

            return text;
        }
    }
}
