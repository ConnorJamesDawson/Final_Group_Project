namespace Final_Project.Models.DTO
{
    public class SpartanDTO
    {
        public string Id { get; set; } = "guid";
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public List<string>? Role { get; set; }
        public string? Profile { get; set; }
        public List<string>? Trackers { get; set; } = new List<string>();

        public List<LinkDTO>? Links = new List<LinkDTO>();
    }
}