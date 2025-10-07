using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.Services;


namespace ChatAppAPI.Controllers.ChatAPI
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConversationService _conversationService;

        public ParticipantController(
            IParticipantService participantService,
            ICurrentUserService currentUserService,
            IConversationService conversationService)
        {
            _participantService = participantService;
            _currentUserService = currentUserService;
            _conversationService = conversationService;
        }

        /// <summary>
        /// Lấy tất cả thành viên trong một cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetParticipants(Guid conversationId)
        {
            var participants = await _participantService.GetParticipantsByConversationIdAsync(conversationId);
            return Ok(participants);
        }

        /// <summary>
        /// Lấy thông tin một thành viên cụ thể trong cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/{userId}")]
        public async Task<IActionResult> GetParticipant(Guid conversationId, Guid userId)
        {
            var participant = await _participantService.GetParticipantAsync(conversationId, userId);
            if (participant == null)
                return NotFound(new { message = "Participant not found" });

            return Ok(participant);
        }
        /// <summary>
        /// Add thành viên cụ vào cuộc trò chuyện
        /// </summary>

        [HttpPost("{conversationId}")]
        public async Task<IActionResult> AddParticipant(Guid conversationId, [FromForm] List<Guid> userIds)
        { 
            if(userIds == null || !userIds.Any())
            {
                return BadRequest(new { message = "No Id User's prodvide" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
              if(conversation == null)
            {
                return NotFound(new { message = "Conversation not found" });
            }
            if (conversation.AdminId != currentUserId && conversation.IsPrivateGroup)
            {
                return Unauthorized(new { message = "Only admin group can add participants in private group" });
            }
            var addParticipants = await _participantService.AddParticipantToConversation(conversationId, userIds);
            if (!addParticipants.Any())
            {
                return BadRequest(new { message = "No participants were added (they may already exist)." });
            }


            return Ok(new
            {
                message = "Participants added successfully.",
                participants = addParticipants.Select(p => new { p.UserId, p.JoinAt })
            });
        }

        /// <summary>
        /// Xoá thành viên khỏi cuộc trò chuyện
        /// </summary>
        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> RemoveParticipants(Guid conversationId, [FromForm] List<Guid> userIds)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot remove yourself from the conversation." });
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if(conversation.AdminId != currentUserId && conversation.IsPrivateGroup)
            {
                return Unauthorized(new { message = "Only admin can remove participant on private group" });
            }

            var removedParticipants = await _participantService.RemoveParticipantsAsync(conversationId, userIds);
            return Ok(new { message = "Remove participant successfully" });
        }

        /// <summary>
        /// Ban chat thành viên
        /// </summary>
        [HttpPut("{conversationId}/banchat")]
        public async Task<IActionResult> BanChat(Guid conversationId, [FromForm] List<Guid> userIds)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;


            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot ban yourself from chat." });

            var banned = await _participantService.BanChatParticipantsAsync(conversationId, userIds);
            return Ok(new { message = "BanChat participant successfully" });
        }

        /// <summary>
        /// Gỡ ban chat thành viên
        /// </summary>
        [HttpPut("{conversationId}/unbanchat")]
        public async Task<IActionResult> UnBanChat(Guid conversationId, [FromForm] List<Guid> userIds)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "User not authenticated to banchat/unbanchat participants " });
            }

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot unban yourself from chat." });

            var unbanned = await _participantService.UnBanChatParticipantsAsync(conversationId, userIds);
            return Ok(new { message = "UnbanChat participant successfully" });
        }

        /// <summary>
        /// Ban thành viên (khỏi cuộc trò chuyện)
        /// </summary>
        [HttpPut("{conversationId}/ban")]
        public async Task<IActionResult> Ban(Guid conversationId, [FromForm] List<Guid> userIds)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "User not authenticated to ban/unban participants" });
            }

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot ban yourself." });

            var banned = await _participantService.BannedParticipantsAsync(conversationId, userIds);
            return Ok(new { message = "Ban participant successfully" });
        }

        /// <summary>
        /// Gỡ ban thành viên (khỏi cuộc trò chuyện)
        /// </summary>
        [HttpPut("{conversationId}/unban")]
        public async Task<IActionResult> UnBan(Guid conversationId, [FromForm] List<Guid> userIds)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "User not authenticated to ban/unban participants" });
            }

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot unban yourself." });

            var unbanned = await _participantService.UnBannedParticipantsAsync(conversationId, userIds);
            return Ok(new { message = "Unban participant successfully" });
        }

        /// <summary>
        /// Lấy danh sách thành viên bị ban trong cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/banned")]
        public async Task<IActionResult> GetBannedParticipants(Guid conversationId)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "User not authenticated to view ban/unban participants" });
            }

            var bannedParticipants = await _participantService.GetBannedParticipantsByConversationIdAsync(conversationId);
            return Ok(bannedParticipants);
        }

        /// <summary>
        /// Lấy danh sách thành viên bị banchat trong cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/bannedchat")]
        public async Task<IActionResult> GetBanChatParticipants(Guid conversationId)
        {
            if (!_currentUserService.Id.HasValue)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            var currentUserId = _currentUserService.Id.Value;
            var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                return NotFound(new { message = "Conservation isn't exist" });
            }
            if (conversation.AdminId != currentUserId)
            {
                return Unauthorized(new { message = "User not authenticated to view ban/unban participants" });
            }

            var bannedParticipants = await _participantService.GetBanChatParticipantsByConversationIdAsync(conversationId);
            return Ok(bannedParticipants);
        }

    }
}
