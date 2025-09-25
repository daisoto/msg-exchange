using Server.DAL;
using Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessagesController(ILogger<MessagesController> logger, IMessageRepository messageRepository, IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] PostMessageRequest request)
        {
            _logger.LogInformation("API: Received request for POST message with sequence number: {seq}", request.SequenceNumber);

            var newMessage = await _messageRepository.AddMessageAsync(request.Content, request.SequenceNumber);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", newMessage);
            _logger.LogInformation("API: Message {id} sent to clients via SignalR.", newMessage.Id);

            return CreatedAtAction(nameof(GetMessages), new { from = DateTime.UtcNow.AddMinutes(-10), to = DateTime.UtcNow }, newMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            _logger.LogInformation("API: Received request for GET messages from {from} to {to}", from, to);
            var messages = await _messageRepository.GetMessagesAsync(from, to);
            
            return Ok(messages);
        }
    }

    public class PostMessageRequest
    {
        [Required]
        [MaxLength(128)]
        public required string Content { get; set; }
        
        [Required]
        public long SequenceNumber { get; set; }
    }
}