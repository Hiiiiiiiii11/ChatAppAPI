using ChatRepository.Models;
using ChatRepository.Models.Request;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.Services;

namespace ChatAppAPI.Controllers.ChatAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ICurrentUserService _currentUserService;
        public MessageController(IMessageService messageService ,ICurrentUserService currentUserService) {
            _messageService = messageService;
            _currentUserService = currentUserService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound(new {message ="Message not found"});
            }
            return Ok(message);
        }
        [Authorize]
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessageByRoom(Guid conversationId, [FromQuery] int? take, [FromQuery] DateTime? before)
        {
            if(!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }   
            var currentUserId = _currentUserService.Id.Value;
            var messages = await _messageService.GetMessageByRoomIdAsync(conversationId, currentUserId, take, before);
            return Ok(messages);
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            try
            {
                var senderId = _currentUserService.Id.Value;
                var message = await _messageService.SendMessageAsync(request, senderId);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(Guid id, [FromBody] EditMessageRequest request)
        {
            await _messageService.EditMessageAsync(id, request);
            return Ok(new {message = "Edit message successfully"});
        }
        [HttpDelete("onlyuser/{id}")]
        public async Task<IActionResult> DeleteMessageOnlyUser(Guid id, [FromQuery] Guid userId)
        {
            await _messageService.DeleteMessageOnlyUserAsync(id, userId);
            return Ok(new { message = "Message deleted for current user" });
        }
        [HttpDelete("all/{id}")]
        public async Task<IActionResult> DeleteMessageWithAll(Guid id)
        {
            await _messageService.DeleteMessageWithAllAsync(id);
            return Ok(new { message = "Message deleted for all users" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            await _messageService.DeleteMessageAsync(id);
            return Ok();
        }
    }
}
