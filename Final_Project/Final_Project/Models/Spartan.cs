using Microsoft.AspNetCore.Identity;

namespace Final_Project.Models
{
    public class Spartan : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<PersonalTracker>? Personal_Trackers { get; set; }

        public TraineeProfile? UserProfile { get; set; }

    }
}
