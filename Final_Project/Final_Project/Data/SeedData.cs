using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Final_Project.Data
{
    public class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SpartaDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Spartan>>();
            var roleStore = new RoleStore<IdentityRole>(context);



            if (context.Spartans.Any())
            {
                context.Spartans.RemoveRange(context.Spartans);
                context.UserRoles.RemoveRange(context.UserRoles);
                context.Roles.RemoveRange(context.Roles);
                context.SaveChanges();
            }

            var trainer = new IdentityRole
            {
                Name = "Trainer",
                NormalizedName = "TRAINER"
            };
            var trainee = new IdentityRole
            {
                Name = "Trainee",
                NormalizedName = "TRAINEE"
            };

            roleStore
                .CreateAsync(trainer)
                .GetAwaiter()
                .GetResult();
            roleStore
                .CreateAsync(trainee)
                .GetAwaiter()
                .GetResult();

            var phil = new Spartan
            {
                UserName = "phil@spartaglobal.com",
                Email = "phil@spartaglobal.com",
                EmailConfirmed = true
            };
            var laura = new Spartan
            {
                UserName = "laura@spartaglobal.com",
                Email = "laura@spartaglobal.com",
                EmailConfirmed = true,
            };
            var cathy = new Spartan
            {
                UserName = "cathy@spartaglobal.com",
                Email = "cathy@spartaglobal.com",
                EmailConfirmed = true,
            };


            userManager
                .CreateAsync(phil, "Password1!")
                .GetAwaiter()
                .GetResult();
            userManager
                .CreateAsync(laura, "Password1!")
                .GetAwaiter()
                .GetResult();
            userManager
                .CreateAsync(cathy, "Password1!")
                .GetAwaiter()
                .GetResult();



            context.UserRoles.AddRange(new IdentityUserRole<string>[]
            {
             new IdentityUserRole<string>
             {
                 UserId = userManager.GetUserIdAsync(phil).Result,
                 RoleId = roleStore.GetRoleIdAsync(trainee).Result
             },
             new IdentityUserRole<string>
             {
                 UserId = userManager.GetUserIdAsync(laura).Result,
                 RoleId = roleStore.GetRoleIdAsync(trainee).Result
             },
             new IdentityUserRole<string>
             {
                 UserId = userManager.GetUserIdAsync(cathy).Result,
                 RoleId = roleStore.GetRoleIdAsync(trainer).Result
             }
            });



            context.Personal_Tracker.AddRange(
                new PersonalTracker
                {
                    Title = "Week1",
                    StopSelfFeedback = "Stop",
                    StartSelfFeedback = "Start",
                    Spartan = phil
                },
                new PersonalTracker
                {
                    Title = "Week1",
                    Spartan = cathy
                },
                new PersonalTracker
                {
                    Title = "Week1",
                    Spartan = laura
                }
            );
            context.TraineeProfile.AddRange(
                new TraineeProfile
                {
                    Title = "About me: Phil!",
                    Spartan = phil
                },
                new TraineeProfile
                {
                    Title = "About me: Cathy!",
                    Spartan = cathy
                },
                new TraineeProfile
                {
                    Title = "About me: Laura!",
                    Spartan = laura
                }
            );
            context.SaveChanges();
        }
    }
}
