using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Services;
using Microsoft.AspNetCore.Authorization;
using UserService.Model.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using UserRepository.Model.Request;
using UserRepository.Models;
using Share.Services;

namespace ChatAppAPI.Controllers.UserAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUploadPhotoService _photoService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly ICurrentUserService _currentUserService;

        public UserController(IUserService userService, IUploadPhotoService photoService, IEmailVerificationService emailVerificationService, IPasswordResetService passwordResetService,ICurrentUserService currentUserService)
        {
            _userService = userService;
            _photoService = photoService;
            _emailVerificationService = emailVerificationService;
            _passwordResetService = passwordResetService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { message = "Email and password are required." });
                if (!request.Email.Contains("@"))
                    return BadRequest(new { message = "Invalid email format." });
                if (!await _emailVerificationService.IsEmailVerifiedAsync(request.Email))
                    return BadRequest(new { message = "Email chưa được xác thực." });
                var userEntity = new User
                {
                    Email = request.Email,
                    PasswordHash = request.Password,
                    DisplayName = request.DisplayName
                };

                var createdUser = await _userService.AddUserAsync(userEntity);

                var UserResponse = new UserInfoResponse
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    DisplayName =createdUser.DisplayName,
                    CreatedAt = DateTime.UtcNow,
                    AvatarUrl = createdUser.AvatarUrl,
                    IsActive = createdUser.IsActive
                };
                    




                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new
                {
                    message = "User created successfully.",
                    user = UserResponse
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id )
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });
            var userRessponses = new UserInfoResponse
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = user.AvatarUrl,
                IsActive = user.IsActive
            };

            return Ok(userRessponses);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userResponses = users.Select(u => new UserInfoResponse
            {
                Id = u.Id,
                Email = u.Email,
                DisplayName = u.DisplayName,
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = u.AvatarUrl,
                IsActive = u.IsActive
            });
            return Ok(userResponses);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserRequest request)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound(new { message = "User not found" });

            //if (!_currentUserService.Id.HasValue)
            //{
            //    return Unauthorized(new { message = "User not authenticated" });
            //}

            bool isUpdated = false;

            // Update DisplayName nếu có
            if (!string.IsNullOrEmpty(request.DisplayName))
            {
                existingUser.DisplayName = request.DisplayName;
                isUpdated = true;
            }

            // Update Avatar nếu có
            if (request.AvatarFile != null)
            {
                var imageUrl = _photoService.UploadPhotoAsync(request.AvatarFile);
                existingUser.AvatarUrl = imageUrl;
                isUpdated = true;
            }

            if (!isUpdated)
                return BadRequest(new { message = "No update data provided." });

            await _userService.UpdateUserAsync(existingUser);

            return Ok(new { message = "User info updated successfully" });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] SearchUserRequest request)
        {
            if (string.IsNullOrEmpty(request.DisplayName))
                return BadRequest(new { message = "Search term is required." });
            var users = await _userService.SearchUsersAsync(request.DisplayName);
            var userResponse = users.Select(u => new UserInfoResponse
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive

            });
            return Ok(userResponse);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("unactive/{id}")]
        public async Task<IActionResult> UnActiveUser(Guid id)
        {
            try
            {
                await _userService.UnActiveUser(id);
                return Ok(new { message = "User deactivated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "User isn't exist!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "No data update provide." });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("active/{id}")]
        public async Task<IActionResult> ActiveUser(Guid id)
        {
            try
            {
                await _userService.ActiveUser(id);
                return Ok(new { message = "User activated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "User isn't exist!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "No data update provide." });
            }
        }
    }
}
