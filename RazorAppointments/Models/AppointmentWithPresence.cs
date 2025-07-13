namespace RazorAppointments.Models
{
    public class AppointmentWithPresence
    {
        public Appointment? Appointment { get; set; }
        public bool IsOnline { get; set; }
    }
}
