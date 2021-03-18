using System;

namespace AP.FamilyTree.Mail
{
    public class BaseMailingService
    {
        protected MailSenderComponent mEmailClient;
        protected MailSettings mMailSettings;
        public BaseMailingService(MailSettings mailSettings)
        {
            try
            {
                mMailSettings = mailSettings;

                mEmailClient = new MailSenderComponent(new SmtpConfig(
                    mailSettings.SMTP_SERVER, int.Parse(mailSettings.SMTP_PORT),
                    mailSettings.SMTP_USERNAME, mailSettings.SMTP_PASSWORD));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
