using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace Final_Project.Controllers.ApiControllers
{
    public class Utils
    {
        public static TraineeProfileDTO ProfileToDTO(TraineeProfile profile) => new TraineeProfileDTO()
        {
            Id = profile.Id,
            Title = profile.Title,
            AboutMe = profile.AboutMe,
            Complete = profile.Complete,
            SpartanId = profile.SpartanId,
            WorkExperience = profile.WorkExperience,
            PictureURL = profile.PictureURL
        };

        public static SpartanDTO SpartanToDTO(Spartan spartan) => new SpartanDTO()
        {
            Id = spartan.Id,
            UserName = spartan.UserName,
            Email = spartan.Email,
            EmailConfirmed = spartan.EmailConfirmed,
            PasswordHash = spartan.PasswordHash
        };
    }
}
