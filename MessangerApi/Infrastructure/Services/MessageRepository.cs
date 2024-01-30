using AutoMapper;
using MessangerApi.Abstracts;
using MessangerApi.Data;
using MessangerApi.Data.Dto;
using MessangerApi.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MessangerApi.Infrastructure.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<AppDbContext> _options;
        public MessageRepository(IMapper mapper, DbContextOptions<AppDbContext> options)
        {
            _mapper = mapper;
            _options = options;
        }
        public List<MessageDto> GetNotDeliveredMessages(Guid guid)
        {
            using (var context = new AppDbContext(_options))
            {
                List<Message> messages = context.Messages.Where(m => m.ToUserId == guid && m.MessageStatus == MessageStatus.NotDelivered).ToList();
                List<MessageDto> messagesDto = messages.Select(m => _mapper.Map<MessageDto>(m)).ToList();
                messages.ForEach(m => m.MessageStatus = MessageStatus.Delivered);
                context.SaveChanges();
                return messagesDto;
            }
        }

        public void SendMessages(MessageDto messageDto)
        {
            using(var context = new AppDbContext(_options))
            {
                Message message = _mapper.Map<Message>(messageDto);
                message.MessageStatus = MessageStatus.NotDelivered;
                context.Messages.Add(message);
                context.SaveChanges();
            }
        }
    }
}
