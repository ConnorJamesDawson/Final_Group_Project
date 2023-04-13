using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Final_Project.Models;

namespace Final_Project.Data
{
    public class SpartaDbContext : IdentityDbContext
    {
        public SpartaDbContext(DbContextOptions<SpartaDbContext> options)
            : base(options)
        {
        }
        public DbSet<PersonalTracker> Personal_Tracker { get; set; } = default!;
        public DbSet<TraineeProfile> TraineeProfile { get; set; } = default!;
        public DbSet<Spartan> Spartans { get; set; }
    }
}