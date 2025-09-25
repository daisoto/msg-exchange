namespace Server.Models
{
    public class Message
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public long SequenceNumber { get; set; }
    }
}