namespace Final_Project.Models.DTO
{
    public class SpartanDTO
    {
        public string Id { get; set; } = "______";
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }

        public List<LinkDTO>? Links = new List<LinkDTO>();
    }
}
