//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using System.Text.Json;

//public class NotificationsModel : PageModel
//{
//    private readonly string _connectionString = "YourConnectionStringHere";

//    public void OnGet() { }

//    public async Task<IActionResult> OnPostRespondAsync()
//    {
//        using var reader = new System.IO.StreamReader(Request.Body);
//        var body = await reader.ReadToEndAsync();
//        var data = JsonSerializer.Deserialize<ResponseDto>(body);

//        using var conn = new SqlConnection(_connectionString);
//        await conn.OpenAsync();

//        var query = "UPDATE Appointments SET Status = @Status WHERE AppointmentID = @AppointmentID";
//        using var cmd = new SqlCommand(query, conn);
//        cmd.Parameters.AddWithValue("@Status", data.ready ? "Confirmed" : "Declined");
//        cmd.Parameters.AddWithValue("@AppointmentID", data.appointmentId);

//        await cmd.ExecuteNonQueryAsync();

//        return new JsonResult(new { success = true });
//    }

//    public class ResponseDto
//    {
//        public int appointmentId { get; set; }
//        public bool ready { get; set; }
//    }
//}
