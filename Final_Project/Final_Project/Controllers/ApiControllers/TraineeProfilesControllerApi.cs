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
    [Route("api/Profiles")]
    [ApiController]
    public class TraineeProfilesControllerApi : ControllerBase
    {
        private readonly ISpartaApiService<TraineeProfile> _service;

        public TraineeProfilesControllerApi(ISpartaApiService<TraineeProfile> apiService)
        {
            _service = apiService;
        }

        // GET: api/TraineeProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraineeProfile>>> GetTraineeProfile()
        {
            var result = await _service.GetAllAsync();
            if (result is null)
            {
                return NotFound();
            }
            return result.ToList();
        }

        // GET: api/TraineeProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TraineeProfile>> GetTraineeProfile(int id)
        {
            var result = await _service.GetAsync(id);
            if (result is null)
            {
                return NotFound();
            }
            return result;
        }

        // PUT: api/TraineeProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraineeProfile(int id, TraineeProfile traineeProfile)
        {
            if (id != traineeProfile.Id)
            {
                return BadRequest();
            }
            var result = await _service.UpdateAsync(id, traineeProfile);
            if (!result) return BadRequest();
            return NoContent();
        }

        // POST: api/TraineeProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TraineeProfile>> PostTraineeProfile(TraineeProfile traineeProfile)
        {
            var result = await _service.CreateAsync(traineeProfile);
            if (!result)
            {
                return BadRequest("Error creating Trainee Profile");
            }
            return CreatedAtAction("GetTraineeProfile", new { id = traineeProfile.Id }, traineeProfile);
        }

        // DELETE: api/TraineeProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraineeProfile(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
