using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

const string SERVER_URL = "http://localhost:5153";

var connection = new HubConnectionBuilder()
    .WithUrl(SERVER_URL)
    .WithAutomaticReconnect()
    .Build();

connection.On<JsonElement>("ReceiveMessage", (message) =>
{
    Console.WriteLine($"[Real-time] New message: {message}");
});

connection.Reconnecting += error =>
{
    Console.WriteLine($"Connection lost, reconnecting... Error: {error?.Message}");
    return Task.CompletedTask;
};

connection.Reconnected += connectionId =>
{
    Console.WriteLine($"Reconnected. Connection ID: {connectionId}");
    return Task.CompletedTask;
};

Console.WriteLine("Client 2: Connecting to SignalR' hub...");
try
{
    await connection.StartAsync();
    Console.WriteLine("Connected. Waiting for messages...");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failure: {ex.Message}");
}

await new TaskCompletionSource<object>().Task;