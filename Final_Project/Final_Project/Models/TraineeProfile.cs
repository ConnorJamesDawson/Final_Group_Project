using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models;

public class TraineeProfile
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is Required")]
    [StringLength(50)]
    public string Title { get; set; } = null!;

    public string? PictureURL { get; set; }
    [Display(Name = "About Me")]
    [StringLength(2000)]
    public string? AboutMe { get; set; }

    [Display(Name = "Work Experience")]
    [StringLength(2000)]
    public string? WorkExperience { get; set; }

    [Display(Name = "Complete?")]
    public bool Complete { get; set; }

    [ValidateNever]
    [ForeignKey("Spartan")]
    public string SpartanId { get; set; } = null!;

    public Spartan? Spartan { get; set; }
}
