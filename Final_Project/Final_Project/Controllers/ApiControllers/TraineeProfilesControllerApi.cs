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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Final_Project.Models.DTO;

namespace Final_Project.Controllers.ApiControllers
{
    [Route("api/Profiles")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TraineeProfilesControllerApi : ControllerBase
    {
        private readonly ISpartaApiService<TraineeProfile> _service;

        public TraineeProfilesControllerApi(ISpartaApiService<TraineeProfile> apiService)
        {
            _service = apiService;
        }

        // GET: api/TraineeProfiles
        [HttpGet() , Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<TraineeProfileDTO>>> GetTraineeProfile()
        {
            var result = await _service.GetAllAsync();
            if (result is null)
            {
                return NotFound();
            }
            return result.Select(pr => CreateProfileLinks(pr)).ToList();
        }

        // GET: api/TraineeProfiles/5
        [HttpGet("{id}", Name = nameof(GetTraineeProfile)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<TraineeProfileDTO>> GetTraineeProfile(int id)
        {
            var result = await _service.GetAsync(id);
            if (result is null)
            {
                return NotFound();
            }
            return CreateProfileLinks(result);
        }

        // PUT: api/TraineeProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpPost(Name = nameof(PostTraineeProfile)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpDelete("{id}", Name = nameof(DeleteTraineeProfile)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteTraineeProfile(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        private TraineeProfileDTO CreateProfileLinks(TraineeProfile profile)
        {
            TraineeProfileDTO output = Utils.ProfileToDTO(profile);
            if (Url is null) return output;

            var idObj = new { id = profile.Id };

            output.Spartan = Url.Link("GetSpartan", new { id = output.SpartanId });

            output.Links.Add(
                new LinkDTO(Url.Link(nameof(GetTraineeProfile), idObj),
                "self",
                "GET"));

            output.Links.Add(
                new LinkDTO(Url.Link(nameof(PostTraineeProfile), null),
                "post_profile",
                "POST"));

            output.Links.Add(
                new LinkDTO(Url.Link(nameof(DeleteTraineeProfile), idObj),
                "delete_profile",
                "DELETE"));

            return output;
        }
    }
}
