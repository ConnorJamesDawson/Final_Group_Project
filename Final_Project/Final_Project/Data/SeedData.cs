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
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
            }

            var admin = new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
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
            roleStore
                .CreateAsync(admin)
                .GetAwaiter()
                .GetResult();

            var connor = new Spartan
            {
                UserName = "connor@spartaglobal.com",
                Email = "connor@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Admin",
                Course = ""
          
            };
            var phil = new Spartan
            {
                UserName = "phil@spartaglobal.com",
                Email = "phil@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Trainee",
                Course = "Tech 205"
            };
            var laura = new Spartan
            {
                UserName = "laura@spartaglobal.com",
                Email = "laura@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Trainee",
                Course = "Tech 201"
            };
            var ruya = new Spartan
            {
                UserName = "ruya@spartaglobal.com",
                Email = "ruya@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Trainee",
                Course = "Tech 201"
            };
            var cathy = new Spartan
            {
                UserName = "cathy@spartaglobal.com",
                Email = "cathy@spartaglobal.com",
                EmailConfirmed = true,
                Role = "Trainer",
                Course = "Tech 206"
            };
            userManager
                .CreateAsync(connor, "Password1!")
                .GetAwaiter()
                .GetResult();
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
                 },
                 new IdentityUserRole<string>
                 {
                     UserId = userManager.GetUserIdAsync(connor).Result,
                     RoleId = roleStore.GetRoleIdAsync(admin).Result
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
                    Title = "Week2",
                    StopSelfFeedback = "Stop",
                    StartSelfFeedback = "Start",
                    Spartan = phil
                },
                new PersonalTracker
                {
                    Title = "Week1",
                    Spartan = laura
                },
                new PersonalTracker
                {
                    Title = "Week1",
                    Spartan = ruya
                }
            ) ;
            context.TraineeProfile.AddRange(
                new TraineeProfile
                {
                    Title = "About me: Phil!",
                    Spartan = phil
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
