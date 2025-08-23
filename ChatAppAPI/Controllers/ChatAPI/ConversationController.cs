using ChatService.Model.Request;
using ChatService.Models;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using UserService.Models;
using UserService.Services;

namespace ChatAppAPI.Controllers.ChatAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly ICurrentUserService _currentUserService;

        public ConversationController(IConversationService conversationService, ICurrentUserService currentUserService)
        {
            _conversationService = conversationService;
            _currentUserService = currentUserService;
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
        public async Task<IActionResult> CreateConversation([FromQuery] ConversationCreateRequest request, [FromQuery] List<Guid> participants)
        {

            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;

            // Nếu là private chat thì phải chỉ có đúng 1 participant (ngoài currentUser)
            if (request.IsPrivate && !request.IsGroup)
            {
                if (participants == null || participants.Count != 1)
                {
                    return BadRequest(new { message = "Private conversation must include exactly one other participant." });
                }

                var conversation = new Conversations
                {
                    IsGroup = false,
                    IsPrivate = true,
                    Name = null,           // private chat ko cần name
                    AdminId = null         // private chat ko có admin
                };

                var conv = await _conversationService.CreateConversationAsync(conversation, currentUserId, participants);
                return Ok(conv);
            }
            // Nếu là group chat
            if (request.IsGroup)
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(new { message = "Group conversation must have a name." });
                }

                var conversation = new Conversations
                {
                    IsGroup = true,
                    IsPrivate = false,
                    IsPrivateGroup = request.IsPrivateGroup ? true : false,
                    Name = request.Name,
                    AdminId = currentUserId  // người tạo group là admin
                };

                var conv = await _conversationService.CreateConversationAsync(conversation, currentUserId, participants);
                return Ok(conv);
            }
            return BadRequest(new { message = "Invalid conversation type." });

        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConversation(Guid id, [FromQuery] ConversationGroupUpdateRequest request)
        {
            if(!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;

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
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
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
