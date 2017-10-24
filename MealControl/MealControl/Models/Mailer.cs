using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace MealControl.Models
{
    public class Mailer
    {
        private const string SmtpHost = "smtp.live.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "ricardogomessaraiva@hotmail.com";
        private const string SmtpPassword = "";
        private const string MailFrom = "ricardogomessaraiva@hotmail.com";
        private string Mailto { get; set; }
        private string Subject { get; set; }
        private string Body { get; set; }

        public Mailer(Email e)
        {
            Mailto = e.Mailto;
            Subject = e.Subject;
            Body = e.Body;

        }

        public void Send()
        {
            SmtpClient SmtpServer = new SmtpClient(SmtpHost);
            var mail = new MailMessage();
            mail.From = new MailAddress(SmtpUsername);
            mail.To.Add(Mailto);
            mail.Subject = Mailto;
            mail.IsBodyHtml = true;
            mail.Body = Body;
            SmtpServer.Port = SmtpPort;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
    }
}