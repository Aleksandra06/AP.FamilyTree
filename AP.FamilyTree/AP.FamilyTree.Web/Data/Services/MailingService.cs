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
            //mailData.ToEmail = new MailAddress(@"arthur.fischer@alfatraining.de"); //For TEST-CASE!!!      
            mailData.FromEmail = new MailAddress(mMailSettings.FROM);
            mailData.BccEmail = new MailAddress(mMailSettings.BCC);

            try
            {
                mailData.Subject = item.Subject;
                mailData.Body = GetBody(item.Text);
                //mailData.AttachmentData = AddAttachmentList(item).GetAwaiter().GetResult();

                mEmailClient.SendMail(mailData);
            }
            catch (Exception ex)
            {
                //TODO: Logging
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

        //private async Task<List<Attachment>> AddAttachmentList(MailItem item)
        //{
        //    if (!(item.DocumentList?.Count() > 0))
        //    {
        //        return null;
        //    }

        //    return item.DocumentList.Select(doc => new Attachment(new MemoryStream(doc.Data), doc.FileName, doc.ContentType)).ToList();
        //}

        private string GetBody(string text)
        {
            string body = string.Empty;
           // body += item.AnschreibenModel.Text;

            //var tnKennung = "Ihre alfatraining Kundennummer:   " + item.MailingListeZugangsdaten.TnKennung +
            //                 Environment.NewLine + "Ihr Passwort:   " + item.MailingListeZugangsdaten.Passwort;

            ////body = body.Replace("XXXZUGANGSDATENXXX", tnKennung);

            return text;
        }
    }
}
