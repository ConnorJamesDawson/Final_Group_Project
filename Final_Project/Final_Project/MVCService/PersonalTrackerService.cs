using AutoMapper;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Final_Project.MVCService
{
    public class PersonalTrackerService : Controller
    {

        private readonly SpartaDbContext _context;
        private readonly UserManager<Spartan> _userManager;
        private readonly IMapper _mapper;

        public PersonalTrackerService(SpartaDbContext context,
            UserManager<Spartan> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }


        // GET: Personal_Tracker
        public async Task<List<PersonalTracker>> GetList(string currentUserId)
        {

            var applicationDbContext = _context.Personal_Tracker
                .Where(t => t.SpartanId == currentUserId)
                .Include(p => p.Spartan);
            
            return await applicationDbContext.ToListAsync();
        }

        // GET: Personal_Tracker/Details/5
        public async Task<PersonalTracker> Details(int? id)
        {

            var personal_Tracker = await _context.Personal_Tracker
                .Include(p => p.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return  personal_Tracker;
        }



   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create(PersonalTracker personal_Tracker, string currentUserId)
        //public async Task<IActionResult> Create([Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,SpartanId")] PersonalTracker personal_Tracker)
        {

            if (ModelState.IsValid)
            {
                personal_Tracker.SpartanId = currentUserId;
                _context.Add(personal_Tracker);
                await _context.SaveChangesAsync();
            }
        }

        // GET: Personal_Tracker/Edit/5
        public async Task<PersonalTracker> GetTracker(int? id)
        {

             var personal_Tracker = await _context.Personal_Tracker
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id);

            return personal_Tracker;
        }

        public async Task<bool> contextIsNull()
        {
            return _context.Personal_Tracker.IsNullOrEmpty();
        }

        // POST: Personal_Tracker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,TechnicalSkills,ConsultantSkills,SpartanId")] PersonalTracker personal_Tracker)
        {

            var originalTracker = await _context.Personal_Tracker
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id);

            personal_Tracker.TrainerComments = originalTracker!.TrainerComments;

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

        public async Task UpdatePersonalTracker(PersonalTracker personal_Tracker)
        {
            _context.Update(personal_Tracker);
            await _context.SaveChangesAsync();
        }
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
