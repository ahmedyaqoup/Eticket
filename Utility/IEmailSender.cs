using System.Net.Mail;
using System.Net;

namespace Eticket.Utility
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("yaqoupahmed16@gmail.com", "kule tcfi bjdz cnfc")
            };

            return client.SendMailAsync(
                new MailMessage(from: "yaqoupahmed16@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
