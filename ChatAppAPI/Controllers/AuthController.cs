using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Services;
using UserService.Models;
using UserService.Model.Request;
using UserService.Model.Response;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
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
                var token = _authService.GenerateTokenAsync(user);
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
