using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Model;
using UserService.Repositories;

namespace UserService.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationService _emailService;
        private readonly IPasswordResetTokenRepository _tokenRepo;

        public PasswordResetService(
            IUserRepository userRepository,
            IEmailVerificationService emailService,
            IPasswordResetTokenRepository tokenRepo)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenRepo = tokenRepo;
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email)
                ?? throw new Exception("Email not found");

            var otp = GenerateOtp();
            var token = new PasswordResetToken
            {
                UserId = user.Id,
                Token = otp,
                ExpiredAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _tokenRepo.AddAsync(token);
            await _tokenRepo.SaveChangesAsync();

            await _emailService.SendPasswordResetCodeAsync(user.Email, otp);
        }

        public async Task ResetPasswordAsync(string email, string otp, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email)
                ?? throw new Exception("User not found");

            var token = await _tokenRepo.GetUnusedValidTokenAsync(user.Id, otp)
                ?? throw new Exception("Invalid or expired OTP");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            token.IsUsed = true;
            await _tokenRepo.UpdateAsync(token);
            await _tokenRepo.SaveChangesAsync();

            await _emailService.SendPasswordChangedNotificationAsync(email);
        }

        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }


}
