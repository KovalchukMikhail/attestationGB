using MessangerApi.Data.Model;

namespace MessangerApi.Data.Dto
{
    public class MessageDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Text { get; set; } = null!;
    }
}
