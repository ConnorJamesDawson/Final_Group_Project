using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class PersonalTracker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        public string? Stop_SelfFeedback { get; set; }
        public string? Start_SelfFeedback { get; set; }
        public string? Continue_SelfFeedback { get; set; }
        public string? Comments_SelfFeedback { get; set; }

        public SkillLevel TechnicalSkills = SkillLevel.Unskilled;
        public SkillLevel ConsultantSkills = SkillLevel.Unskilled;

        [ValidateNever]
        [ForeignKey("Spartan")]
        public string SpartanId { get; set; } = null!;

        public Spartan? Spartan { get; set; }
    }
}
