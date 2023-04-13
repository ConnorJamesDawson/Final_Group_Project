using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data;
using Final_Project.Models;

namespace Final_Project.Controllers
{
    public class TraineeProfilesController : Controller
    {
        private readonly SpartaDbContext _context;

        public TraineeProfilesController(SpartaDbContext context)
        {
            _context = context;
        }

        // GET: TraineeProfiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TraineeProfile.Include(t => t.Spartan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TraineeProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TraineeProfile == null)
            {
                return NotFound();
            }

            var traineeProfile = await _context.TraineeProfile
                .Include(t => t.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (traineeProfile == null)
            {
                return NotFound();
            }

            return View(traineeProfile);
        }

        // GET: TraineeProfiles/Create
        public IActionResult Create()
        {
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id");
            return View();
        }

        // POST: TraineeProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,AboutMe,WorkExperience,Complete,SpartanId")] TraineeProfile traineeProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(traineeProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", traineeProfile.SpartanId);
            return View(traineeProfile);
        }

        // GET: TraineeProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TraineeProfile == null)
            {
                return NotFound();
            }

            var traineeProfile = await _context.TraineeProfile.FindAsync(id);
            if (traineeProfile == null)
            {
                return NotFound();
            }
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", traineeProfile.SpartanId);
            return View(traineeProfile);
        }

        // POST: TraineeProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AboutMe,WorkExperience,Complete,SpartanId")] TraineeProfile traineeProfile)
        {
            if (id != traineeProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(traineeProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeProfileExists(traineeProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", traineeProfile.SpartanId);
            return View(traineeProfile);
        }

        // GET: TraineeProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TraineeProfile == null)
            {
                return NotFound();
            }

            var traineeProfile = await _context.TraineeProfile
                .Include(t => t.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (traineeProfile == null)
            {
                return NotFound();
            }

            return View(traineeProfile);
        }

        // POST: TraineeProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TraineeProfile == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TraineeProfile'  is null.");
            }
            var traineeProfile = await _context.TraineeProfile.FindAsync(id);
            if (traineeProfile != null)
            {
                _context.TraineeProfile.Remove(traineeProfile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeProfileExists(int id)
        {
          return (_context.TraineeProfile?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
