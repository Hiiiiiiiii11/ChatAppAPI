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

        public ParticipantController(
            IParticipantService participantService,
            ICurrentUserService currentUserService)
        {
            _participantService = participantService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy tất cả thành viên trong một cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/participants")]
        public async Task<IActionResult> GetParticipants(Guid conversationId)
        {
            var participants = await _participantService.GetParticipantsByConversationIdAsync(conversationId);
            return Ok(participants);
        }

        /// <summary>
        /// Lấy thông tin một thành viên cụ thể trong cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/participants/{userId}")]
        public async Task<IActionResult> GetParticipant(Guid conversationId, Guid userId)
        {
            var participant = await _participantService.GetParticipantAsync(conversationId, userId);
            if (participant == null)
                return NotFound(new { message = "Participant not found" });

            return Ok(participant);
        }

        /// <summary>
        /// Xoá thành viên khỏi cuộc trò chuyện
        /// </summary>
        [HttpDelete("{conversationId}/participants")]
        public async Task<IActionResult> RemoveParticipants(Guid conversationId, [FromQuery] List<Guid> userIds)
        {
            if (!ValidateCurrentUser(out var currentUserId)) return Unauthorized();

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot remove yourself from the conversation." });

            var removedParticipants = await _participantService.RemoveParticipantsAsync(conversationId, userIds);
            return Ok(removedParticipants);
        }

        /// <summary>
        /// Ban chat thành viên
        /// </summary>
        [HttpPut("{conversationId}/participants/banchat")]
        public async Task<IActionResult> BanChat(Guid conversationId, [FromQuery] List<Guid> userIds)
        {
            if (!ValidateCurrentUser(out var currentUserId)) return Unauthorized();

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot ban yourself from chat." });

            var banned = await _participantService.BanChatParticipantsAsync(conversationId, userIds);
            return Ok(banned);
        }

        /// <summary>
        /// Gỡ ban chat thành viên
        /// </summary>
        [HttpPut("{conversationId}/participants/unbanchat")]
        public async Task<IActionResult> UnBanChat(Guid conversationId, [FromQuery] List<Guid> userIds)
        {
            if (!ValidateCurrentUser(out var currentUserId)) return Unauthorized();

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot unban yourself from chat." });

            var unbanned = await _participantService.UnBanChatParticipantsAsync(conversationId, userIds);
            return Ok(unbanned);
        }

        /// <summary>
        /// Ban thành viên (khỏi cuộc trò chuyện)
        /// </summary>
        [HttpPut("{conversationId}/participants/ban")]
        public async Task<IActionResult> Ban(Guid conversationId, [FromQuery] List<Guid> userIds)
        {
            if (!ValidateCurrentUser(out var currentUserId)) return Unauthorized();

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot ban yourself." });

            var banned = await _participantService.BannedParticipantsAsync(conversationId, userIds);
            return Ok(banned);
        }

        /// <summary>
        /// Gỡ ban thành viên (khỏi cuộc trò chuyện)
        /// </summary>
        [HttpPut("{conversationId}/participants/unban")]
        public async Task<IActionResult> UnBan(Guid conversationId, [FromQuery] List<Guid> userIds)
        {
            if (!ValidateCurrentUser(out var currentUserId)) return Unauthorized();

            if (userIds.Contains(currentUserId))
                return BadRequest(new { message = "You cannot unban yourself." });

            var unbanned = await _participantService.UnBannedParticipantsAsync(conversationId, userIds);
            return Ok(unbanned);
        }

        /// <summary>
        /// Lấy danh sách thành viên bị ban trong cuộc trò chuyện
        /// </summary>
        [HttpGet("{conversationId}/participants/banned")]
        public async Task<IActionResult> GetBannedParticipants(Guid conversationId)
        {
            var bannedParticipants = await _participantService.GetBannedParticipantsByConversationIdAsync(conversationId);
            return Ok(bannedParticipants);
        }

        /// <summary>
        /// Kiểm tra người dùng hiện tại
        /// </summary>
        private bool ValidateCurrentUser(out Guid currentUserId)
        {
            currentUserId = Guid.Empty;
            if (!_currentUserService.Id.HasValue) return false;
            currentUserId = _currentUserService.Id.Value;
            return true;
        }
    }
}
