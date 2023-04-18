using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Final_Project.MVCService
{
    public class TraineeProfilesService
    {
        private readonly SpartaDbContext _context;
        private readonly UserManager<Spartan> _userManager;
        

        public TraineeProfilesService(SpartaDbContext context, UserManager<Spartan> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IIncludableQueryable<TraineeProfile, Spartan?>> ReturnIndex(string role, string currentUserId)
        {
            var applicationDbContext = _context.TraineeProfile
                    .Include(t => t.Spartan);

            if (role == "Trainee")
            {
                applicationDbContext = _context.TraineeProfile
                .Where(t => t.SpartanId == currentUserId)
                .Include(t => t.Spartan);
            }
            else
            {
                applicationDbContext = _context.TraineeProfile
                   .Include(t => t.Spartan);
            }

            return applicationDbContext;

        }

        public async Task<TraineeProfile> FindAsync(int? id)
        {
            return  _context.TraineeProfile.FindAsync(id).Result;
        }


        public async Task<bool> isContextNull()
        {
            return _context.TraineeProfile == null;
        }

        public async Task<SelectList> ReturnSelectListAsync(TraineeProfile traineeProfile)
        {
           return new SelectList(_context.Set<Spartan>(), "Id", "Id", traineeProfile.SpartanId);
        }


        public async Task Update(TraineeProfile traineeProfile)
        {
            _context.Update(traineeProfile);
            await _context.SaveChangesAsync();
        }
    }

}
