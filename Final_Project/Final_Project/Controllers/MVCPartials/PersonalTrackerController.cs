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
        public async Task<IActionResult> IndexTrainer(string search = null, string titleSearch= null, string courseSearch = null)
        {
            var response = _personalTrackerService.GetListTrainer(search, titleSearch, courseSearch).Result;

            if(response.Success == false)
            {
                return Problem(response.Message);
            }

            return View(response.Data);

        }

        //GET
        public async Task<IActionResult> EditTrainer(int? id)
        {

            var personal_Tracker = await _personalTrackerService.FindAsync(id);

            if (personal_Tracker == null)
            {
                return NotFound("Could not find tracker");
            }

            return View(_mapper.Map<PersonalTrackerVM>(personal_Tracker));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> EditTrainer(int id, [Bind("TrainerComments", "Id")] PersonalTrackerVM personalTrackerVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _personalTrackerService.EditTrainer(id, personalTrackerVM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_personalTrackerService.Personal_TrackerExists(personalTrackerVM.Id))
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
            return View(personalTrackerVM);
        }
    }
}
