using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models.ViewModels
{
    public class PersonalTrackerVM
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? StopSelfFeedback { get; set; }
        public string? StartSelfFeedback { get; set; }
        public string? ContinueSelfFeedback { get; set; }
        public string? CommentsSelfFeedback { get; set; }
        public string? TrainerComments { get; set; }
        public string TechnicalSkills { get; set; } = "Unskilled";
        public string ConsultantSkills { get; set; } = "Unskilled";
    }
}
