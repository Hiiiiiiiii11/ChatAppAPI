using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UserService.Model;
using UserService.Repositories;
using UserService.VerifyEmail;

namespace UserService.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        public EmailVerificationService(EmailSettings emailSettings , IEmailVerificationRepository emailVerificationRepository)
        {
            _emailSettings = emailSettings;
            _emailVerificationRepository = emailVerificationRepository;
        }
        public async Task SendVerificationCodeAsync(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var verificationEmail = new EmailVerification
            {
                Email = email,
                Code = code,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false
            };
            await _emailVerificationRepository.AddAsync(verificationEmail);


            // 3. Gửi email
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = "Email Verification Code",
                Body = $"Your verification code is: <b>{code}</b>. It will expire in 5 minutes.",
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            await client.SendMailAsync(mailMessage);
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            var verification = await _emailVerificationRepository.GetByEmailAndCodeAsync(email, code);
            if (verification == null) return false;

            if (verification.IsVerified) return true; // Đã verify trước đó
            if (verification.ExpiredAt < DateTime.UtcNow) return false; // Hết hạn

            await _emailVerificationRepository.MarkAsVerifiedAsync(verification);
            return true;
        }
    }
}
