namespace RazorAppointments.Models
{
    public class UserPresence
    {
        public string Username { get; set; } // Primary Key
        public DateTime LastSeen { get; set; }
    }
}

