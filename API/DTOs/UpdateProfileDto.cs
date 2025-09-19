namespace API.Dtos
{
    public class UpdateProfileDto
    {
        public string FirstName { get; set; }
        public string MidInitial { get; set; }
        public string LastName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
