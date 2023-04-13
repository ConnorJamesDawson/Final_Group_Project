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

namespace Final_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalTrackersApiController : ControllerBase
    {
        private readonly ISpartaApiService<PersonalTracker> _service;

        public PersonalTrackersApiController(ISpartaApiService<PersonalTracker> service)
        {
            _service = service;
        }

        // GET: api/PersonalTrackersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalTracker>>> GetPersonal_Tracker()
        {
          //if (_service.GetAllAsync() == null)
          //{
          //    return NotFound();

          //}
            var result = await _service.GetAllAsync();

            return result.ToList();
        }

        // GET: api/PersonalTrackersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalTracker>> GetPersonalTracker(int id)
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

            return personalTracker;
        }

        // PUT: api/PersonalTrackersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalTracker(int id, PersonalTracker personalTracker)
        {
            if (id != personalTracker.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(id, personalTracker);

            if(result)
            {
                return NoContent();
            }
     

            return NotFound();
        }

        // POST: api/PersonalTrackersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonalTracker>> PostPersonalTracker(PersonalTracker personalTracker)
        {

            var result = await _service.CreateAsync(personalTracker);

            if(result)
            {
                return CreatedAtAction("GetPersonalTracker", new { id = personalTracker.Id }, personalTracker);
            }
            else {
                return Problem($"Entity set 'SpartaDbContext.PersonalTracker'  is null or entity with id: {personalTracker.Id} already exists");
                  }
        }

        // DELETE: api/PersonalTrackersApi/5
        [HttpDelete("{id}")]
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
    }
}
