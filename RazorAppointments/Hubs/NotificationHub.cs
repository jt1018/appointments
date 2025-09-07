using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RazorAppointments.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationToUser(string username, string message)
        {
            await Clients.User(username).SendAsync("ReceiveNotification", message);
        }
    }
}

