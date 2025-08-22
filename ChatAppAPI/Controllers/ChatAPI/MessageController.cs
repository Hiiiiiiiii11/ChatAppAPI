using ChatService.Models;
using ChatService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppAPI.Controllers.ChatAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService) {
            _messageService = messageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            var message = _messageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessageByRoom(Guid conversationId, [FromQuery] int? take, [FromQuery] DateTime? before)
        {
            var messages = _messageService.GetMessageByRoomIdAsync(conversationId, take, before);
            return Ok(messages);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Messages message)
        {
            var created = await _messageService.CreateMessageAsync(message);
            return CreatedAtAction(nameof(GetMessage), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] Messages message)
        {
            if (id != message.Id) return BadRequest("ID mismatch");

            await _messageService.UpdateMessageAsync(message);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            await _messageService.DeleteMessageAsync(id);
            return Ok();
        }
    }
}
