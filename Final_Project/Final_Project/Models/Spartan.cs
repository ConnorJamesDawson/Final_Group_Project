using Microsoft.AspNetCore.Identity;

namespace Final_Project.Models
{
    public class Spartan : IdentityUser
    {
        public List<PersonalTracker>? Personal_Trackers { get; set; }

        public TraineeProfile? UserProfile { get; set; }

    }
}
