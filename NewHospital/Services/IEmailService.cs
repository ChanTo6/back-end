using NewHospital.Models;


namespace SimpleEmailApp.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request, string verificationCode);
    }
}