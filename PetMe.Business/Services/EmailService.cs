using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PetMe.Data.Entities;

namespace PetMe.Business.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient(_smtpSettings.Host);

            message.From = new MailAddress(_smtpSettings.FromEmail);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            smtp.Port = _smtpSettings.Port;
            smtp.Credentials = new NetworkCredential(_smtpSettings.FromEmail, _smtpSettings.Password);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(message);
        }
    }
}
