using System.Net.Mail;
using System.Net;
using UserServiceDAL.InterfaceServices;
using Microsoft.Extensions.Options;
using UserServiceDAL.Helpers;
using UserServiceDAL.Model.Email;

namespace UserServiceDAL.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpConfig _smtpConfig;

        public EmailSenderService(IOptions<SmtpConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        public async Task<bool> SendVerificationEmailAsync(SendEmailVerification email)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_smtpConfig.FromAddressEmail, _smtpConfig.FromAddressName);
                message.To.Add(email.ToAddress);
                message.Subject = email.Subject;
                message.Body = email.Body;
                message.IsBodyHtml = email.IsBodyHtml;

                using (var smtpClient = new SmtpClient(_smtpConfig.SmtpServer, _smtpConfig.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpConfig.SmtpUsername, _smtpConfig.SmtpPassword);
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(message);
                }
            }
            return true;
        }
    }
}
