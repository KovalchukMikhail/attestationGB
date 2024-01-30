namespace MessangerApi.Data.Model
{
    public class Message
    {
        public Guid Guid { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Text { get; set; } = null!;
        public MessageStatus MessageStatus { get; set; }
        public virtual Status Status { get; set; }
    }
}
