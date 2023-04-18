using AutoMapper;
using Final_Project.Data;
using Final_Project.Models;
using Final_Project.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Final_Project.MVCService
{
    public class PersonalTrackerService
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

        public async Task<ServiceResponse<TitleViewModel>> GetListTrainer(string search = null, string titleSearch = null, string courseSearch = null)
        {
            var response = new ServiceResponse<TitleViewModel>();

            if(contextIsNull().Result)
            {
                response.Success = false;
                response.Message = "Context is null";
                return response;
            }

            IQueryable<string> titleQuery = from t in _context.Personal_Tracker
                                            orderby t.Title
                                            select t.Title;

            var tracker = _context.Personal_Tracker.Include(t => t.Spartan).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                tracker = tracker.Where(s => s.Spartan.UserName.Contains(search));

            if (!string.IsNullOrEmpty(courseSearch))
                tracker = tracker.Where(s => s.Spartan.Course.Contains(courseSearch));

            if (!string.IsNullOrEmpty(titleSearch))
                tracker = tracker.Where(x => x.Title == titleSearch);

            response.Data = new TitleViewModel
            {
                Titles = new SelectList(await titleQuery.Distinct().ToListAsync()),
                Trackers = await tracker.ToListAsync()
            };

            return response;
        }

        // GET: Personal_Tracker/Details/5
        public async Task<PersonalTracker> Details(int? id)
        {

            var personal_Tracker = await _context.Personal_Tracker
                .Include(p => p.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);

            return personal_Tracker;
        }

        public async Task<SelectList> returnSelectList(PersonalTracker personal_Tracker)
        {
            return new SelectList(_context.Set<Spartan>(), "Id", "Id", personal_Tracker.SpartanId);

        }
        public async Task<SelectList> returnSelectListWithoutSpartanId()
        {
            return new SelectList(_context.Set<Spartan>(), "Id", "Id");

        }




        public async Task Create(PersonalTracker personal_Tracker, string currentUserId)
        //public async Task<IActionResult> Create([Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,SpartanId")] PersonalTracker personal_Tracker)
        {


            personal_Tracker.SpartanId = currentUserId;
            _context.Add(personal_Tracker);
            await _context.SaveChangesAsync();

        }

        // GET: Personal_Tracker/Edit/5
        public async Task<PersonalTracker> GetTracker(int? id)
        {

            var personal_Tracker = await _context.Personal_Tracker
           .AsNoTracking()
           .FirstOrDefaultAsync(pt => pt.Id == id);

            return personal_Tracker;
        }

        public async Task<PersonalTracker> FindAsync(int? id)
        {

            return await _context.Personal_Tracker.FindAsync(id);
        }

        public async Task<bool> contextIsNull()
        {
            return _context.Personal_Tracker.IsNullOrEmpty();
        }

        // POST: Personal_Tracker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task Edit(int id, [Bind("Id,Title,StopSelfFeedback,StartSelfFeedback,ContinueSelfFeedback,CommentsSelfFeedback,TechnicalSkills,ConsultantSkills,SpartanId")] PersonalTracker personal_Tracker)
        {

            var originalTracker = await _context.Personal_Tracker
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id);

            personal_Tracker.TrainerComments = originalTracker!.TrainerComments;

            _context.Update(personal_Tracker);
            await _context.SaveChangesAsync();


        }

        public async Task<ServiceResponse<PersonalTrackerVM>> EditTrainer(int id, [Bind("TrainerComments", "Id")] PersonalTrackerVM personalTrackerVM)
        {
            var response = new ServiceResponse<PersonalTrackerVM>();

            if (id != personalTrackerVM.Id)
            {
                response.Success = false;
                response.Message = "Id given does not match VM Id given";
                return response;
            }

            var originalTracker = await _context.Personal_Tracker
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == id);

            originalTracker.TrainerComments = personalTrackerVM.TrainerComments;

            _context.Update(originalTracker);
            await _context.SaveChangesAsync();

            return response;
        }
        // GET: Personal_Tracker/Delete/5

        public async Task UpdatePersonalTracker(PersonalTracker personal_Tracker)
        {
            _context.Update(personal_Tracker);
            await _context.SaveChangesAsync();
        }


        // POST: Personal_Tracker/Delete/5
        public async Task Delete(PersonalTracker personal_Tracker)
        {
            _context.Personal_Tracker.Remove(personal_Tracker);
            await _context.SaveChangesAsync();


        }

        public bool Personal_TrackerExists(int id)
        {
            return (_context.Personal_Tracker?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
