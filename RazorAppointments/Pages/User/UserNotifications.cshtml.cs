using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAppointments.Data;
using RazorAppointments.Models;
using System.Threading.Tasks;

namespace RazorAppointments.Pages.User
{
    public class UserNotificationsModel : PageModel
    {
        private readonly AppointmentContext _context;

        public UserNotificationsModel(AppointmentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostRespondAsync([FromBody] ResponseModel response)
        {
            var username = User.Identity?.Name;

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Username == username && a.AppointmentID == response.AppointmentId);

            if (appointment != null)
            {
                // Update status based on user response
                appointment.Status = response.Confirmed ? "In Progress" : "Pending";
                await _context.SaveChangesAsync();
            }

            return new JsonResult(new { success = true });
        }

        public class ResponseModel
        {
            public int AppointmentId { get; set; }
            public bool Confirmed { get; set; }
        }
    }
}
