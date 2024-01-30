namespace MessangerApi.Data.Model
{
    public class Status
    {
        public MessageStatus MessageStatus { get; set; }
        public string Name { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
