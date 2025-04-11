using LibraryProject.Application.Interface.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AppEmailSender = LibraryProject.Application.Interface.Identity.IEmailSender;
using IdentityEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;

namespace LibraryProject.Application.Services.Identity
{
    public class EmailSender : AppEmailSender, IdentityEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:From"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            using (var client = new SmtpClient(
                _configuration["EmailSettings:Host"],
                int.Parse(_configuration["EmailSettings:Port"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
