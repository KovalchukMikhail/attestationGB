using MessangerApi.Abstracts;
using MessangerApi.Data.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessangerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        [HttpGet]
        [Route("GetMessages")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult<List<MessageDto>> GetMessages()
        {
            try
            {
                Guid userGuid = GetCurrentUserGuid();
                List<MessageDto> messages = _messageRepository.GetNotDeliveredMessages(userGuid);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("SendMessage")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult SendMessage(string toUserGuid, string messageText)
        {
            try
            {
                Guid toUser = new Guid(toUserGuid);
                Guid userGuid = GetCurrentUserGuid();
                MessageDto message = new MessageDto() {FromUserId = userGuid, ToUserId = toUser, Text = messageText };
                _messageRepository.SendMessages(message);
                return Ok("Сообщение отправлено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private Guid GetCurrentUserGuid()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new Guid(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            return default;
        }
    }
}
