using Server.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.DAL;

public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        public async Task<Message> AddMessageAsync(string content, long sequenceNumber)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var message = new Message
            {
                Content = content,
                SequenceNumber = sequenceNumber,
                Timestamp = DateTime.UtcNow
            };

            var sql = "INSERT INTO messages (content, timestamp, sequence_number) VALUES (@content, @timestamp, @sequence_number) RETURNING id;";
            
            _logger.LogInformation("DAL: Executing SQL for adding message: {sql}", sql);

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("content", message.Content);
            command.Parameters.AddWithValue("timestamp", message.Timestamp);
            command.Parameters.AddWithValue("sequence_number", message.SequenceNumber);

            var id = (int)(await command.ExecuteScalarAsync() ?? 0);
            message.Id = id;
            
            _logger.LogInformation("DAL: Message successfully added with ID: {id}", id);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            var messages = new List<Message>();
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT id, content, timestamp, sequence_number FROM messages WHERE timestamp >= @from AND timestamp <= @to ORDER BY timestamp DESC;";
            _logger.LogInformation("DAL: Executing SQL for receiving messages from time interval");

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("from", NpgsqlTypes.NpgsqlDbType.TimestampTz, from.ToUniversalTime());
            command.Parameters.AddWithValue("to", NpgsqlTypes.NpgsqlDbType.TimestampTz, to.ToUniversalTime());

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new Message
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    Timestamp = reader.GetDateTime(2),
                    SequenceNumber = reader.GetInt64(3)
                });
            }
            
            _logger.LogInformation("DAL: Found {count} messages", messages.Count);
            return messages;
        }
    }