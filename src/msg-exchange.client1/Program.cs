using System.Net.Http.Json;

const int TIMEOUT_MILLISECONDS = 1000;
const string SERVER_URL = "http://localhost:5153";

using var httpClient = new HttpClient { BaseAddress = new Uri(SERVER_URL) };
long sequenceNumber = 0;

Console.WriteLine($"Client 1: Sending messages to {SERVER_URL}. Press Ctrl+C to exit.");

while (true)
{
    var message = new
    {
        Content = $"Message from client 1 at {DateTime.Now:T}",
        SequenceNumber = ++sequenceNumber
    };

    try
    {
        var response = await httpClient.PostAsJsonAsync("/api/messages", message);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Message sent #{sequenceNumber}. Status: {response.StatusCode}");
        }
        else
        {
            Console.WriteLine($"Message sending error #{sequenceNumber}. Status: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Critical message sending error: {ex.Message}");
    }
    
    await Task.Delay(TIMEOUT_MILLISECONDS); 
}