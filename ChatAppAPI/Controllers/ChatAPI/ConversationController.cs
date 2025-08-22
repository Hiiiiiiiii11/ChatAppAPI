using ChatService.Model.Request;
using ChatService.Models;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ChatAppAPI.Controllers.ChatAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            var conversation = await _conversationService.GetConversationByIdAsync(id);
            if (conversation == null)
            {
                return NotFound(new { message = "Conversation not found" });
            }
            return Ok(conversation);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserConversations(Guid userId)
        {
            var conversations = await _conversationService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] ConversationCreateRequest request, [FromQuery] List<Guid> participants)
        {

            var userIdClaim = User.FindFirst("id");
            Guid userId = Guid.Parse(userIdClaim.Value);
            var conversation = new Conversations
            {
                Name = request.Name,
                IsGroup = request.IsGroup,
                IsPrivate = request.IsPrivate,
                AdminId = request.IsGroup ? userId : null
            };

            var conv = await _conversationService.CreateConversationAsync(conversation , userId, participants);
            return Ok(conv);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConversation(Guid id, [FromQuery] ConversationGroupUpdateRequest request)
        {
            var userIdClaim = User.FindFirst("id");
            Guid currentUserId = Guid.Parse(userIdClaim.Value);

            var conversation = await _conversationService.GetConversationByIdAsync(id);
            if(conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            // chỉ admin hiện tại mới có quyền update
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "Only admin can update info on group " });
            }
            //đổi tên nhóm nếu cần
            if (!string.IsNullOrEmpty(request.Name))
            {
                conversation.Name = request.Name;
            }
            conversation.IsPrivateGroup = request.IsPrivateGroup;

            //chuyển quyền admin nếu có 
            if(request.AdminId.HasValue && request.AdminId.Value != conversation.AdminId)
            {
                bool isPatcipant = conversation.Participants.Any(p => p.UserId == request.AdminId.Value);
                if (!isPatcipant)
                {
                    return BadRequest(new { message = "The new admin must be a participant of the group." });
                }
                conversation.AdminId = request.AdminId.Value;
            }
            await _conversationService.UpdateConversationAsync(conversation);
            return Ok(new { message = "Conversation updated successfully" });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupConversation(Guid id)
        {
            var userIdClaim = User.FindFirst("id");
            Guid currentUserId = Guid.Parse(userIdClaim.Value);
            var conversation = await _conversationService.GetConversationByIdAsync(id);
            if (conversation == null)
            {
                return NotFound(new { message = "Conversation not found" });
            }

            // Nếu là group thì chỉ admin mới có quyền xóa
            if (conversation.IsGroup && conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "Only admin can delete this group conversation" });
            }
            await  _conversationService.DeleteConversationAsync(id);
            
            return Ok(new {message = "Delete Conversation success"});
        }
    }
}
