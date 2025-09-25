using Server.Models;

namespace Server.DAL;

public interface IMessageRepository
{
    Task<Message> AddMessageAsync(string content, long sequenceNumber);
    Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to);
}