using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models.DTO
{
    public class TraineeProfileDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? PictureURL { get; set; }

        public string? AboutMe { get; set; }

        public string? WorkExperience { get; set; }

        public bool Complete { get; set; }

        public string SpartanId { get; set; } = null!;

        public string? Spartan { get; set; }

        public List<LinkDTO> Links = new();

    }
}
