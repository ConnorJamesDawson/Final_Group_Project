using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Spartan : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Course { get; set; }

        public List<PersonalTracker>? Personal_Trackers { get; set; }

        public TraineeProfile? UserProfile { get; set; }
    }
}
