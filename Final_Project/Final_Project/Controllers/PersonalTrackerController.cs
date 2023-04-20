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
using AutoMapper;
using Final_Project.Models.ViewModels;
using Final_Project.MVCService;

namespace Final_Project.Controllers
{
    public partial class PersonalTrackerController : Controller
    {
        private readonly UserManager<Spartan> _userManager;
        private readonly IMapper _mapper;
        private PersonalTrackerService _personalTrackerService;

        public PersonalTrackerController(UserManager<Spartan> userManager, IMapper mapper, PersonalTrackerService personalTrackerService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _personalTrackerService = personalTrackerService;
        }

        private string? GetRole()
        {
            return HttpContext.User.IsInRole("Trainee") ? "Trainee" : "Trainer";
        }

        // GET: Personal_Tracker
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var role = GetRole();
            var applicationDbContext = _personalTrackerService.GetList(currentUser.Id);
                
            return View(await applicationDbContext);
        }

        // GET: Personal_Tracker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _personalTrackerService.contextIsNull())
            {
                return NotFound();
            }

            var personal_Tracker = await _personalTrackerService.Details(id);

            if (personal_Tracker == null)
            {
                return NotFound();
            }

            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Create
        public IActionResult Create()
        {
            ViewData["SpartanId"] =  _personalTrackerService.returnSelectListWithoutSpartanId();
            return View();
            
        }

        // POST: Personal_Tracker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonalTracker personal_Tracker)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                await _personalTrackerService.Create(personal_Tracker, currentUser.Id);
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpartanId"] = await _personalTrackerService.returnSelectList(personal_Tracker);
            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _personalTrackerService.contextIsNull() )
            {
                return NotFound();
            }

            var personal_Tracker = await _personalTrackerService.GetTracker(id);

            if (personal_Tracker == null)
            {
                return NotFound();
            }
            ViewData["SpartanId"] = await _personalTrackerService.returnSelectList(personal_Tracker);
            return View(personal_Tracker);
        }

        // POST: Personal_Tracker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,TechnicalSkills,ConsultantSkills,SpartanId")] PersonalTracker personal_Tracker)
        {
            if (id != personal_Tracker.Id)
            {
                return NotFound();
            }
            var originalTracker = await _personalTrackerService.GetTracker(id);

            personal_Tracker.TrainerComments = originalTracker!.TrainerComments;

            if (ModelState.IsValid)
            {
                try
                {
                   await _personalTrackerService.UpdatePersonalTracker(personal_Tracker);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_personalTrackerService.Personal_TrackerExists(personal_Tracker.Id))
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
            ViewData["SpartanId"] = await _personalTrackerService.returnSelectList(personal_Tracker);
            return View(personal_Tracker);
        }

        // GET: Personal_Tracker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _personalTrackerService.contextIsNull() )
            {
                return NotFound();
            }

            var personal_Tracker = await _personalTrackerService.GetTracker(id);

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
            if (await _personalTrackerService.contextIsNull())
            {
                return Problem("Entity set 'ApplicationDbContext.Personal_Tracker'  is null.");
            }
            var personal_Tracker = await _personalTrackerService.FindAsync(id);
            if (personal_Tracker != null)
            {
               await _personalTrackerService.Delete(personal_Tracker);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
