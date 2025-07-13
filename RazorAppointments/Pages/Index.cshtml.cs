using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAppointments.Data;        // for _context
using RazorAppointments.Models;      // for AuthorizedUser


namespace RazorAppointments.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AppointmentContext _context;

        public IndexModel(ILogger<IndexModel> logger, AppointmentContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult OnGet()
        //{
        //    return RedirectToPage("/Scheduler/Index");
        //}
        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = User.Identity?.Name; // e.g., "CORP\\jdoe"

            var isAuthorized = await _context.AuthorizedUsers
                .AnyAsync(u => u.Username == currentUser);

            if (!isAuthorized)
            {
                return RedirectToPage("/AccessDenied");
            }

            return RedirectToPage("/Scheduler/Index");
        }

    }
}
