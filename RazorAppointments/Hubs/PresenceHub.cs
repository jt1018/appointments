using Microsoft.AspNetCore.SignalR;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace RazorAppointments.Hubs
{
    public class PresenceHub : Hub
    {
    // Store online users by username only (no domain)
    private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

    public override Task OnConnectedAsync()
    {
        var fullName = Context.User?.Identity?.Name ?? "";
        var username = ExtractUsername(fullName);

        OnlineUsers[Context.ConnectionId] = username;
            Console.WriteLine($"User connected: {Context.User?.Identity?.Name ?? "Anonymous"}");
            return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        OnlineUsers.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public static bool IsUserOnline(string username)
    {
        return OnlineUsers.Values.Contains(username, StringComparer.OrdinalIgnoreCase);
    }

    private string ExtractUsername(string fullName)
    {
        if (string.IsNullOrEmpty(fullName)) return "";
        if (fullName.Contains("\\")) // DOMAIN\username
            return fullName.Split('\\')[1];
        if (fullName.Contains("@")) // email
            return fullName.Split('@')[0];
            Console.WriteLine("FULLNAME +++++++++++++++++++++++++++++++++++++++++++++++++++");
        return fullName;
    }
    }
}