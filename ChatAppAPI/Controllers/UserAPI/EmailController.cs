using Microsoft.AspNetCore.Mvc;
using UserService.Services;
using System.Threading.Tasks;
using UserRepository.Model.Request;

namespace ChatAppAPI.Controllers.UserAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public EmailController(IEmailVerificationService emailVerificationService)
        {
            _emailVerificationService = emailVerificationService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromQuery] string email)
        {
            await _emailVerificationService.SendVerificationCodeAsync(email);
            return Ok(new { message = "OTP sent to your email." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPRequest request)
        {
            var result = await _emailVerificationService.VerifyCodeAsync(request);
            if (!result) return BadRequest(new { message = "Invalid or expired code." });
            return Ok(new { message = "Email verified successfully." });
        }
    }
}
