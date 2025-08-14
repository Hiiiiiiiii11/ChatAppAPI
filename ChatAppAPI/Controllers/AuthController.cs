using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Services;
using UserService.Models;
using UserService.Model.Request;
using UserService.Model.Response;
using UserService.Admin;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly AdminAccountSettings _adminAccountSettings ;

        public AuthController(IAuthenticationService authService,AdminAccountSettings adminAccountSettings)
        {
            _authService = authService;
            _adminAccountSettings = adminAccountSettings;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            try
            {
                var user = await _authService.AuthenticateUserAsync(request.Username, request.Password);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }
                if (user.IsActive == false)
                    return Unauthorized(new { message = "Account be unactived." });

                var role = user.Email == _adminAccountSettings.Email ? "Admin" : "User";

                var token = _authService.GenerateTokenAsync(user,role);
                var response = new AuthResponse
                {
                    Token = token,

                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}
