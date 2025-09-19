namespace API.Dtos
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string MidInitial { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin or Staff
    }
}
