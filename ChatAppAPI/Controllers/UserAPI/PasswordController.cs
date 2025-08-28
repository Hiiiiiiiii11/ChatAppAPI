using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRepository.Model.Request;
using UserService.Services;

namespace ChatAppAPI.Controllers.UserAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _resetService;

        public PasswordResetController(IPasswordResetService resetService)
        {
            _resetService = resetService;
        }
        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> RequestReset([FromQuery] PasswordResetRequest request)
        {
            await _resetService.RequestPasswordResetAsync(request.Email);
            return Ok(new { message = "OTP sent to your email." });
        }
        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmReset([FromBody] ConfirmResetPasswordRequest request)
        {
            await _resetService.ResetPasswordAsync(request.Email, request.Otp, request.NewPassword);
            return Ok(new { message = "Password updated successfully." });
        }
    }
}
