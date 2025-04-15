using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortfolioApp.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:From"], _configuration["Email:DisplayName"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                using (var client = new SmtpClient(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"])))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);
                    client.EnableSsl = bool.Parse(_configuration["Email:EnableSsl"]);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"Email sending failed: {ex.Message}");
            }
        }
    }
}