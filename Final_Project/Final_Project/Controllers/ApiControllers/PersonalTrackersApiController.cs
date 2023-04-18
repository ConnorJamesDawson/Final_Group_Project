using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data;
using Final_Project.Models;
using Final_Project.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Final_Project.Models.DTO;

namespace Final_Project.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/personaltrackers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonalTrackersApiController : ControllerBase
    {
        private readonly ISpartaApiService<PersonalTracker> _service;

        public PersonalTrackersApiController(ISpartaApiService<PersonalTracker> service)
        {
            _service = service;
        }

        // GET: api/PersonalTrackersApi
        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<PersonalTrackerDTO>>> GetPersonal_Tracker()
        {
            var trackers = await _service.GetAllAsync();
            if (trackers == null)
            {
                return NotFound();
            }

            var result = new List<PersonalTrackerDTO>();

            foreach (var tracker in trackers)
            {
                result.Add(CreateTrackerLinks(tracker));
            }
            return result.ToList();
        }

        // GET: api/PersonalTrackersApi/5
        [HttpGet("{id}", Name = nameof(GetPersonalTracker)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PersonalTrackerDTO>> GetPersonalTracker(int id)
        {
            if (_service.GetAllAsync() == null)
            {
                return NotFound();
            }
            var personalTracker = await _service.GetAsync(id);

            if (personalTracker == null)
            {
                return NotFound();
            }

            return CreateTrackerLinks(personalTracker);
        }

        // PUT: api/PersonalTrackersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutPersonalTracker(int id, 
            [Bind("Id, Title, StopSelfFeedback, StartSelfFeedback, ContinueSelfFeedback, CommentsSelfFeedback, TrainerComments, TechnicalSkills, ConsultantSkills, SpartanId")]PersonalTracker personalTracker)
        {
            if (id != personalTracker.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(id, personalTracker);

            if (result)
            {
                return NoContent();
            }


            return NotFound();
        }

        // POST: api/PersonalTrackersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = nameof(PostPersonalTracker)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PersonalTrackerDTO>> PostPersonalTracker([Bind("Id, Title, StopSelfFeedback, StartSelfFeedback, ContinueSelfFeedback, CommentsSelfFeedback, TrainerComments, TechnicalSkills, ConsultantSkills, SpartanId")] PersonalTracker personalTracker)
        {

            var result = await _service.CreateAsync(personalTracker);

            if (result)
            {
                return CreatedAtAction("GetPersonalTracker", new { id = personalTracker.Id }, CreateTrackerLinks(personalTracker));
            }
            else
            {
                return Problem($"Entity set 'SpartaDbContext.PersonalTracker'  is null or entity with id: {personalTracker.Id} already exists");
            }
        }

        // DELETE: api/PersonalTrackersApi/5
        [HttpDelete("{id}", Name = nameof(DeletePersonalTracker)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePersonalTracker(int id)
        {
            if (_service == null)
            {
                return NotFound();
            }
            var result = await _service.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        private PersonalTrackerDTO CreateTrackerLinks(PersonalTracker tracker)
        {
            PersonalTrackerDTO output = Utils.PersonalTrackerToDTO(tracker);
            if (Url is null) return output;

            var idObj = new { id = tracker.Id };
      
            output.Links.Add(
                new LinkDTO(Url.Link(nameof(GetPersonalTracker), idObj),
                "self",
                "GET"));

            output.Links.Add(
                new LinkDTO(Url.Link(nameof(PostPersonalTracker), null),
                "post_tracker",
                "POST"));

            output.Links.Add(
                new LinkDTO(Url.Link(nameof(DeletePersonalTracker), idObj),
                "delete_tracker",
                "DELETE"));
            output.Links.Add(
                new LinkDTO(Url.Link(nameof(SpartanController.GetSpartan), new { id = output.SpartanId }),
                "owner",
                "GET"));
            return output;
        }
    }
}
