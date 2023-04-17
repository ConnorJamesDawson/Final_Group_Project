using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models.DTO
{
    public class PersonalTrackerDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? StopSelfFeedback { get; set; }
        public string? StartSelfFeedback { get; set; }
        public string? ContinueSelfFeedback { get; set; }
        public string? CommentsSelfFeedback { get; set; }
        public string? TrainerComments { get; set; }
        public string TechnicalSkills { get; set; } = "Unskilled";
        public string ConsultantSkills { get; set; } = "Unskilled";
        public string SpartanId { get; set; } = null!;

        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
