using System.Net;
using System.Net.Mail;

namespace AP.FamilyTree.Web.Globals
{
    public static class EmailVerification
    {
        public static bool RemoteMailExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                var statusCode = response.StatusCode;
                response.Close();
                return (statusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        public static string GetNotCorrectStatusEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "Email незаполнен";
            }

            try
            {
                email = email.Trim();
                var addr = new MailAddress(email);
                var isEmail = addr.Address == email;

                return !isEmail ? "Ошибка в написании Email" : null;
            }
            catch
            {
                return "Ошибка в написании Email";
            }
        }
    }
}
