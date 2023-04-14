using Final_Project.ApiServices;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers.MVCPartials
{
    public class UserController : Controller
    {
        private readonly ISpartanApiService<Spartan> _spartaService;
        private readonly UserManager<Spartan> _userManager;
        private readonly SpartaDbContext _context;


        public UserController(ISpartanApiService<Spartan> spartaService, ISpartaApiService<PersonalTracker> personalTrackerService, ISpartaApiService<TraineeProfile> traineeProfileService, UserManager<Spartan> userManager, SpartaDbContext context = null)
        {
            _spartaService = spartaService;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var spartans = await _spartaService.GetAllAsync();

            if (spartans == null)
            {
                return NotFound();
            }

            return View(spartans);
        }
        // GET: Personal_Tracker/Create
        public IActionResult CreateTrainee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainee(Spartan spartan)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (ModelState.IsValid)
            {
                spartan.EmailConfirmed = true;
                
                _userManager
                    .CreateAsync(spartan, "Password1!")
                    .GetAwaiter()
                    .GetResult();

                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = _userManager.GetUserIdAsync(spartan).Result,
                    RoleId =  roleStore.GetRoleIdAsync(await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Trainee")!).Result
                });

                _context.TraineeProfile.Add(
                new TraineeProfile
                {
                    Title = $"About me: {spartan.UserName}!",
                    Spartan = spartan
                } );

                await _spartaService.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(spartan);
        }
        // GET: Personal_Tracker/Create
        public IActionResult CreateTrainer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(Spartan spartan)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (ModelState.IsValid)
            {
                spartan.EmailConfirmed = true;

                _userManager
                    .CreateAsync(spartan, "Password1!")
                    .GetAwaiter()
                    .GetResult();

                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = _userManager.GetUserIdAsync(spartan).Result,
                    RoleId = roleStore.GetRoleIdAsync(await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Trainer")!).Result
                });

                await _spartaService.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(spartan);
        }
    }
}
