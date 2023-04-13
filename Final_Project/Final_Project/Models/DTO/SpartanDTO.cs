namespace Final_Project.Models.DTO
{
    public class SpartanDTO
    {
        public string Id { get; set; }

        public string NormalizedUserName { get; set; }

        public string NormalizedEmail { get; set; }

        public string? PhoneNumber { get; set; }

        public List<LinkDTO> Links = new List<LinkDTO>();
    }
}
