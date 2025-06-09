using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAppointments.Data;
using RazorAppointments.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorAppointments.Pages.Appointments 
{
    public class IndexModel : PageModel
    {
        private readonly AppointmentContext _context;

        public IndexModel(AppointmentContext context)
        {
            _context = context;
        }

        public List<Appointment> Appointments { get; set; }

        public async Task OnGetAsync()
        {
            Appointments = await _context.Appointments
                .Where(a => a.Status == "Pending" || a.Status == "In Progress")
                .OrderBy(a => a.Status == "Pending" ? 1 : 0) // "In Progress" gets 0, comes first
                .ThenBy(a => a.CreatedAt) // optional: order within each group by creation time
                .ToListAsync();


        }
    }
}