using Microsoft.EntityFrameworkCore;
using RazorAppointments.Models;

namespace RazorAppointments.Data
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options)
            : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Room> Rooms { get; set; }

    }
}