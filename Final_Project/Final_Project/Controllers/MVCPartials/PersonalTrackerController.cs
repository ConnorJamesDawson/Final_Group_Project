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
        public async Task<IActionResult> IndexTrainer(string search = null, string titleSearch= null)
        {
            IQueryable<string> titleQuery = from t in _context.Personal_Tracker
                                            orderby t.Title
                                            select t.Title;
            var tracker = _context.Personal_Tracker.Include(t => t.Spartan).AsQueryable();

            if(!string.IsNullOrEmpty(search))
                tracker = tracker.Where(s => s.Spartan.UserName.Contains(search));

            if(!string.IsNullOrEmpty(titleSearch))
                tracker = tracker.Where(x => x.Title ==  titleSearch);

            var titleVM = new TitleViewModel
            {
                Titles = new SelectList(await titleQuery.Distinct().ToListAsync()),
                Trackers = await tracker.ToListAsync()
            };

            return View(titleVM);


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
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> EditTrainer(int id, [Bind("TrainerComments", "Id")] PersonalTrackerVM personalTrackerVM)
        {
            if (id != personalTrackerVM.Id)
            {
                return NotFound();
            }

            var originalTracker = await _context.Personal_Tracker
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == id);

            originalTracker.TrainerComments = personalTrackerVM.TrainerComments;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(originalTracker);
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
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", originalTracker.SpartanId);
            return View(personalTrackerVM);
        }
    }
}
