using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NewHospital.Models;

namespace SimpleEmailApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void SendEmail(EmailDto request, string verificationCode)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(request.To));

                string subject = "Authorization Code";
                email.Subject = subject;

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"{request.Body}<br><br>Your verification code: {verificationCode}"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; 
            }
        }

        private string GenerateVerificationCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var verificationCode = new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return verificationCode;
        }
    }
}
