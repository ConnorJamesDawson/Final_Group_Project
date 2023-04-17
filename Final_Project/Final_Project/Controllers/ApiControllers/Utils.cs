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
            PictureURL = profile.PictureURL,
        };

        public static SpartanDTO SpartanToDTO(Spartan spartan) => new SpartanDTO()
        {
            Id = spartan.Id,
            UserName = spartan.UserName,
            FirstName = spartan.FirstName,
            LastName = spartan.LastName,
            Email = spartan.Email,
            EmailConfirmed = spartan.EmailConfirmed,
            PhoneNumber = spartan.PhoneNumber,
            PasswordHash = spartan.PasswordHash
        };

        public static Spartan DTOToSpartan(SpartanDTO spartanDto) => new Spartan()
        {
            UserName = spartanDto.UserName,
            FirstName = spartanDto.FirstName,
            LastName = spartanDto.LastName,
            Email = spartanDto.Email,
            EmailConfirmed = spartanDto.EmailConfirmed,
            PhoneNumber = spartanDto.PhoneNumber
        };
        public static PersonalTrackerDTO PersonalTrackerToDTO(PersonalTracker personalTracker) => new PersonalTrackerDTO()
        {
            Id = personalTracker.Id,
            Title = personalTracker.Title,
            StopSelfFeedback = personalTracker.StopSelfFeedback,
            StartSelfFeedback = personalTracker.StartSelfFeedback,
            ContinueSelfFeedback = personalTracker.ContinueSelfFeedback,
            CommentsSelfFeedback = personalTracker.CommentsSelfFeedback,
            TrainerComments = personalTracker.TrainerComments,
            TechnicalSkills = personalTracker.TechnicalSkills,
            ConsultantSkills = personalTracker.ConsultantSkills,
            SpartanId = personalTracker.SpartanId,
        };
    }
}
