namespace FinalDern_Support.Models.Dto.RequestDtos
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public bool ISBussiness { get; set; }
    }
}
