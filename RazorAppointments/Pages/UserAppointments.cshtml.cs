using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAppointments.Data;

namespace RazorAppointments.Pages
{   
    public class UserAppointmentsModel(AppointmentContext context) : PageModel
    {
        private readonly AppointmentContext _context = context;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostRespondAsync(int id, bool ready)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt != null && appt.Status == "Pending")
            {
                appt.Status = ready ? "In Progress" : "Pending";
                appt.Notified = true;
                await _context.SaveChangesAsync();
            }
            return new JsonResult(new { success = true });
        }

    }
}
