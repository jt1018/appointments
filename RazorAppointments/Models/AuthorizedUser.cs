namespace RazorAppointments.Models
{
    public class AuthorizedUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

}

//CREATE TABLE AuthorizedUsers (
//    Id INT IDENTITY PRIMARY KEY,
//    Username NVARCHAR(255) NOT NULL UNIQUE
//);
