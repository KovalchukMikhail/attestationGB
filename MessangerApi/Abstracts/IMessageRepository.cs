using MessangerApi.Data.Dto;

namespace MessangerApi.Abstracts
{
    public interface IMessageRepository
    {
        List<MessageDto> GetNotDeliveredMessages(Guid guid);
        void SendMessages(MessageDto messageDto);
    }
}
