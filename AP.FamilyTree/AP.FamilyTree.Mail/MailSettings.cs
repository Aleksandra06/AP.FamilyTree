namespace AP.FamilyTree.Mail
{
    public class MailSettings
    {
        public string SMTP_SERVER { get; set; }
        public string SMTP_PORT { get; set; }
        public string SMTP_USERNAME { get; set; }
        public string SMTP_PASSWORD { get; set; }
        public string FROM { get; set; }
        public string BCC { get; set; }
    }
}