using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class PersonalTracker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [StringLength(50)]
        public string Title { get; set; } = null!;
        [Display(Name = "Stop")]
        public string? StopSelfFeedback { get; set; }
        [Display(Name = "Start")]
        public string? StartSelfFeedback { get; set; }
        [Display(Name = "Continue")]
        public string? ContinueSelfFeedback { get; set; }
        [Display(Name = "Comments")]
        public string? CommentsSelfFeedback { get; set; }
        [Display(Name = "Trainer Comments")]
        public string? TrainerComments { get; set; }

        public static string[] SkillLevelOptionsArray { get; } = new string[] { "Unskilled", "Low Skilled", "Partially Skilled", "Skilled" };

        public SelectList SkillLevelOptions = new SelectList(SkillLevelOptionsArray);


        [Display(Name = "Technical Skills")]
        public string TechnicalSkills { get; set; } = "Unskilled";
        [Display(Name = "Consultant Skills")]
        public string ConsultantSkills { get; set; } = "Unskilled";


        [ValidateNever]
        [ForeignKey("Spartan")]
        public string SpartanId { get; set; } = null!;

        public Spartan? Spartan { get; set; }
    }
}
