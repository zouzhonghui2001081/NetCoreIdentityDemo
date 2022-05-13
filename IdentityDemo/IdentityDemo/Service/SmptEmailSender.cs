using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityDemo.Service
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpOptions> _smtpOptions;

        public SmtpEmailSender(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions;
        }
        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);
            using(var client = new SmtpClient(_smtpOptions.Value.Host, _smtpOptions.Value.Port)
            {
                Credentials = new NetworkCredential(_smtpOptions.Value.Username, _smtpOptions.Value.Password)
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
