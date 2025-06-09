using System;

namespace RazorAppointments.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string Username { get; set; }
        public string AppointmentType { get; set; }
        public string Status { get; set; }
        public string? Room { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public bool? Acknowledged { get; set; }
        public bool Notified { get; set; }
    }
}

