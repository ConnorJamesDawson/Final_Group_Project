using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Final_Project.Controllers
{
    public partial class PersonalTrackerController : Controller
    {
        private readonly SpartaDbContext _context;
        private readonly UserManager<Spartan> _userManager;

        public PersonalTrackerController(SpartaDbContext context,
            UserManager<Spartan> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Personal_Tracker
        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = _context.Personal_Tracker
                .Where(t => t.SpartanId == currentUser.Id)
                .Include(p => p.Spartan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Personal_Tracker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Personal_Tracker == null)
            {
                return NotFound();
            }

            var personal_Tracker = await _context.Personal_Tracker
                .Include(p => p.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personal_Tracker == null)
            {
                return NotFound();
            }

            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Create
        public IActionResult Create()
        {
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id");
            return View();
            
        }

        // POST: Personal_Tracker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,SpartanId")] PersonalTracker personal_Tracker)
        //public async Task<IActionResult> Create([Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,SpartanId")] PersonalTracker personal_Tracker)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                personal_Tracker.SpartanId = currentUser.Id;
                _context.Add(personal_Tracker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", personal_Tracker.SpartanId);
            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(personal_Tracker);
        }

        // POST: Personal_Tracker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,SpartanId")] PersonalTracker personal_Tracker)
        {
            if (id != personal_Tracker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personal_Tracker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Personal_TrackerExists(personal_Tracker.Id))
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
            ViewData["SpartanId"] = new SelectList(_context.Set<Spartan>(), "Id", "Id", personal_Tracker.SpartanId);
            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personal_Tracker == null)
            {
                return NotFound();
            }

            var personal_Tracker = await _context.Personal_Tracker
                .Include(p => p.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personal_Tracker == null)
            {
                return NotFound();
            }

            return View(personal_Tracker);
        }

        // POST: Personal_Tracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personal_Tracker == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Personal_Tracker'  is null.");
            }
            var personal_Tracker = await _context.Personal_Tracker.FindAsync(id);
            if (personal_Tracker != null)
            {
                _context.Personal_Tracker.Remove(personal_Tracker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Personal_TrackerExists(int id)
        {
          return (_context.Personal_Tracker?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
