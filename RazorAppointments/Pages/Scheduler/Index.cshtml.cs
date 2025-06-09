using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorAppointments.Data;
using RazorAppointments.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorAppointments.Pages.Scheduler
{
    public class IndexModel : PageModel
    {
        private readonly AppointmentContext _context;

        public IndexModel(AppointmentContext context)
        {
            _context = context;
        }

        public List<Appointment> Appointments { get; set; }
        public List<SelectListItem> RoomsList { get; set; } = new();
        [BindProperty]
        public int SelectedRoomID { get; set; }
        public List<Appointment> AppointmentsInSelectedRoom { get; set; } = new();
        public async Task OnGetAsync([FromQuery] int? selectedRoomId)
        {
            SelectedRoomID = selectedRoomId ?? 0;

            Appointments = await _context.Appointments
                .Where(a => a.Status == "Pending" || a.Status == "In Progress")
                .OrderBy(a => a.Status == "Pending" ? 1 : 0) // "In Progress" gets 0, comes first
                .ThenBy(a => a.CreatedAt) // optional: order within each group by creation time
                .ToListAsync();
            RoomsList = await _context.Rooms
               .Select(r => new SelectListItem
               {
                   Value = r.RoomID.ToString(),
                   Text = r.RoomName
               })
               .ToListAsync();
            if (SelectedRoomID > 0)
            {
                var selectedRoom = await _context.Rooms.FindAsync(SelectedRoomID);
                if (selectedRoom != null)
                {
                    AppointmentsInSelectedRoom = await _context.Appointments
                        .Where(a => a.Room == selectedRoom.RoomName &&
                                    (a.Status == "In Progress" || a.Status == "Pending"))
                        .OrderBy(a => a.CreatedAt)
                        .ToListAsync();
                }
            }
        }
        public async Task<IActionResult> OnPostAcceptAsync(int appointmentId, int selectedRoomId)
        {
            if (selectedRoomId == 0)
            {
                TempData["Error"] = "Please select a room before accepting an appointment.";
                return RedirectToPage(new { selectedRoomId });
            }

            var appt = await _context.Appointments.FindAsync(appointmentId);
            if (appt == null || appt.Status != "Pending")
            {
                // Either doesn't exist or already accepted
                return RedirectToPage(new { selectedRoomId });
            }
            // Check for existing "In Progress" appointment for this user in a different room
            var selectedRoom = await _context.Rooms.FindAsync(SelectedRoomID);
            var selectedRoomName = selectedRoom?.RoomName;

            var hasActiveAppointmentInOtherRoom = await _context.Appointments.AnyAsync(a =>
                a.Username == appt.Username &&
                a.Status == "In Progress" &&
                a.Room != null &&
                a.Room != selectedRoomName
            );

            if (hasActiveAppointmentInOtherRoom)
            {
                // Prevent acceptance — could optionally display an error message
                TempData["Error"] = "User already has an active appointment in a different room.";
                return RedirectToPage(new { selectedRoomId });
            }


            if (appt != null && appt.Status == "Pending")
            {
                var room = await _context.Rooms.FindAsync(selectedRoomId);
                if (room != null)
                {
                    appt.Status = "In Progress";
                    appt.StartedAt = DateTime.Now;
                    appt.Room = room.RoomName;
                    await _context.SaveChangesAsync();
                }               
            }

            return RedirectToPage(new { selectedRoomId }); // Refresh the page
        }
        public async Task<IActionResult> OnPostCancelAsync(int id, int SelectedRoomID)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null && appointment.Status != "Canceled" && appointment.Status != "Complete")
            {
                appointment.Status = "Canceled";
                appointment.CanceledAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { selectedRoomId = SelectedRoomID });
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id, int SelectedRoomID)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null && appointment.Status != "Canceled" && appointment.Status != "Complete")
            {
                appointment.Status = "Complete";
                appointment.CompletedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { selectedRoomId = SelectedRoomID });
        }


    }
}