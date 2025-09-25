using System.Net.Http.Json;
using System.Text.Json;

const string SERVER_URL = "http://localhost:5153";
const int TIME_INTERVAL_MINUTES = 10;

using var httpClient = new HttpClient { BaseAddress = new Uri(SERVER_URL) };
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

Console.WriteLine($"Client 3: Press Enter to request messages for last {TIME_INTERVAL_MINUTES} minutes. Press Ecs to exit.");

while (Console.ReadKey(true).Key != ConsoleKey.Escape)
{
    var to = DateTime.UtcNow;
    var from = to.AddMinutes(-TIME_INTERVAL_MINUTES);
    var url = $"/api/messages?from={from:o}&to={to:o}";

    try
    {
        Console.WriteLine($"\n=== REQUESTING MESSAGES FROM {from:T} TO {to:T} ===");
        var messages = await httpClient.GetFromJsonAsync<JsonElement[]>(url);
        if (messages != null && messages.Length > 0)
        {
            foreach (var message in messages)
            {
                Console.WriteLine(JsonSerializer.Serialize(message, jsonOptions));
            }
        }
        else
        {
            Console.WriteLine("Messages were not found.");
        }
        Console.WriteLine("=== REQUEST COMPLETED ===\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while receiving messages: {ex.Message}");
    }
}