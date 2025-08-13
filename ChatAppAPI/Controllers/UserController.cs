using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Services;
using UserService.Models;
using UserService.Model.Request;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUploadPhotoService _photoService;

        public UserController(IUserService userService, IUploadPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
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
                    var userEntity = new User
                {
                    Email = request.Email,
                    PasswordHash = request.Password, 
                    DisplayName = request.DisplayName
                };

                var createdUser = await _userService.AddUserAsync(userEntity);

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new
                {
                    message = "User created successfully.",
                    user = createdUser
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserRequest request)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound(new { message = "User not found" });

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

            return Ok(new { message = "User updated successfully" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }
    }
}
