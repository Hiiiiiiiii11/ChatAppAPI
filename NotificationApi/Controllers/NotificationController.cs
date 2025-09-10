using Microsoft.AspNetCore.Mvc;
using NotificationRepository.Model.Request;
using NotificationRepository.Model.Response;
using NotificationRepository.Models;
using NotificationService.Services;
using Share.Services;

namespace NotificationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }   

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found" });
            }
            return Ok(notification);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }
        // Tạo thông báo khi có tin nhắn mới
        [HttpPost("message")]
        public async Task<ActionResult<NotificationMessageResponse>> CreateMessageNotification([FromBody] CreateMessageNotificationRequest request)
        {
            var createdNotification = await _notificationService.CreateMessageNotificationAsync(request);

            return CreatedAtAction(
                nameof(GetNotificationById),
                new { id = createdNotification.Id },
                createdNotification
            );
        }
        //tạo thông báo cho user
        [HttpPost("user")]
        public async Task<ActionResult<NotificationMessageResponse>> CreateNotificationForUser([FromBody] CreateUserNotificationRequest request)
        {
            var createdNotification = await _notificationService.CreateNotificationForUserAsync(request);

            return CreatedAtAction(
                nameof(GetNotificationById),
                new { id = createdNotification.Id },
                createdNotification
            );
        }
        [HttpPost("markasread/{id}")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found" });
            }
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(Guid id, [FromBody] Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }
            var existingNotification = await _notificationService.GetNotificationByIdAsync(id);
            if (existingNotification == null)
            {
                return NotFound(new { message = "Notification not found" });
            }
            await _notificationService.UpdateNotificationAsync(notification);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found" });
            }
            await _notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }


    }
}
