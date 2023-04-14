
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public partial class TraineeProfilesController
    {
        public async Task<IActionResult> IndexTrainer()
        {
            var applicationDbContext = _context.TraineeProfile.Include(p => p.Spartan);
            return View(await applicationDbContext.ToListAsync());
        }
    }
}
