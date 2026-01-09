using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class ChatHub : Hub
{
    // Mapăm UserId -> List<ConnectionId> (un user poate fi logat de pe telefon și PC simultan)
    private static readonly ConcurrentDictionary<string, List<string>> UserConnections = new();

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Curățăm conexiunile vechi când cineva închide pagina
        foreach (var user in UserConnections)
        {
            user.Value.Remove(Context.ConnectionId);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task RegisterUser(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return;

        var connections = UserConnections.GetOrAdd(userId, _ => new List<string>());
        lock (connections)
        {
            if (!connections.Contains(Context.ConnectionId))
            {
                connections.Add(Context.ConnectionId);
            }
        }
        Console.WriteLine($"[HUB] Utilizator {userId} înregistrat cu succes.");
    }

    public async Task SendPrivateMessage(string targetUserId, string message, string myId, string myName)
    {
        // 1. Trimitem la destinatar (căutăm toate ferestrele lui deschise)
        if (UserConnections.TryGetValue(targetUserId, out var targetConnections))
        {
            await Clients.Clients(targetConnections).SendAsync("ReceiveMessage", myId, myName, message);
        }

        // 2. Trimitem înapoi la expeditor (mie)
        await Clients.Caller.SendAsync("ReceiveMessage", myId, "Eu", message);
    }
}