using System.Net;
using System.Net.Mail;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class EmailHelper
    {
        public static void Send(string subject, string body, string to)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("ktei2008@gmail.com", "km5jpVEi");
            MailMessage mail = new MailMessage("noreply@service.ktei.com", to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }
    }
}