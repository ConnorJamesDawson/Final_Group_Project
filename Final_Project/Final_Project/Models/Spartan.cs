using Microsoft.AspNetCore.Identity;

namespace Final_Project.Models
{
    public class Spartan : IdentityUser
    {
        public List<Personal_Tracker>? Personal_Trackers { get; set; }

        public Profile? UserProfile { get; set; }

    }
}
