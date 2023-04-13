using Final_Project.Models;
using Final_Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public partial class PersonalTrackerController : Controller
    {
        public async Task<IActionResult> IndexTrainer()
        {
            var applicationDbContext = _context.Personal_Tracker.Include(p => p.Spartan);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET
        public async Task<IActionResult> EditTrainer(int? id)
        {
            if (id == null || _context.Personal_Tracker == null)
            {
                return NotFound();
            }

            var personal_Tracker = await _context.Personal_Tracker.FindAsync(id);
            if (personal_Tracker == null)
            {
                return NotFound();
            }
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", personal_Tracker.SpartanId);
            return View(_mapper.Map<PersonalTrackerVM>(personal_Tracker));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize (Roles = "Trainer")]
        public async Task<IActionResult> EditTrainer(int id, [Bind("TrainerComments","Id")] PersonalTrackerVM personalTrackerVM)
        {
            if (id != personalTrackerVM.Id)
            {
                return NotFound();
            }

            var personalTracker = _mapper.Map<PersonalTracker>(personalTrackerVM);
            personalTracker.SpartanId = (await _context.Personal_Tracker.FindAsync(id)).SpartanId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalTracker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Personal_TrackerExists(personalTrackerVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexTrainer));
            }
            //ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", personalTracker.SpartanId);
            return View(personalTrackerVM);
        }
    }
}
