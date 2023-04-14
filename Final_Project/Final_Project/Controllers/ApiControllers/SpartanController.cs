﻿using Final_Project.ApiServices;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpartanController : ControllerBase
    {
        private readonly ISpartanApiService<Spartan> _spartaService;
        private readonly ISpartaApiService<PersonalTracker> _personalTrackerService;
        private readonly ISpartaApiService<TraineeProfile> _traineeProfileService;

        public SpartanController(ISpartanApiService<Spartan> spartaService, ISpartaApiService<PersonalTracker> personalTrackerService, ISpartaApiService<TraineeProfile> traineeProfileService)
        {
            _spartaService = spartaService;
            _personalTrackerService = personalTrackerService;
            _traineeProfileService = traineeProfileService;
        }

        // GET: api/Spartan
        [HttpGet(Name = nameof(GetSpartans))]
        public async Task<ActionResult<IEnumerable<SpartanDTO>>> GetSpartans()
        {
            var spartans = await _spartaService.GetAllAsync();
            
            if (spartans == null)
            {
                return NotFound();
            }
            
            var spartansDtos = spartans.Select(s => CreateSpartanLinks(Utils.SpartanToDTO(s))).ToList();

            return spartansDtos;
        }

        // GET: api/Spartan/5
        [HttpGet("{id}", Name = nameof(GetSpartan))]
        public async Task<ActionResult<SpartanDTO>> GetSpartan(string id)
        {
            var spartan = await _spartaService.GetAsync(id);

            if (spartan == null)
            {
                return NotFound();
            }

            var spartanDto = CreateSpartanLinks(Utils.SpartanToDTO(spartan));

            return spartanDto;
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutSpartan))]
        public async Task<IActionResult> PutSpartan(string id, Spartan spartan)
        {
            if (id != spartan.Id)
            {
                return BadRequest();
            }

            var updatedSuccessfully = await _spartaService.UpdateAsync(id, spartan);

            if (!updatedSuccessfully)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = nameof(PostSpartan))]
        public async Task<ActionResult<SpartanDTO>> PostSpartan(SpartanDTO spartanDto)
        {
            var spartan = new Spartan();
            spartan.UserName = spartanDto.UserName;
            spartan.Email = spartanDto.Email;
            spartan.EmailConfirmed = spartanDto.EmailConfirmed;

            var passwordHasher = new PasswordHasher<Spartan>();
            spartan.PasswordHash = passwordHasher.HashPassword(spartan, spartanDto.PasswordHash);

            _spartaService.CreateAsync(spartan);
            await _spartaService.SaveAsync();

            var createdSpartanDto = new SpartanDTO
            {
                Id = spartan.Id,
                UserName = spartan.UserName,
                Email = spartan.Email,
                EmailConfirmed = spartan.EmailConfirmed,
                Links = spartanDto.Links
            };

            return CreatedAtAction(nameof(GetSpartan), new { id = spartan.Id }, createdSpartanDto);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}", Name = nameof(DeleteSpartan))]
        public async Task<IActionResult> DeleteSpartan(string id)
        {
            var spartan = await _spartaService.GetAsync(id);
            var trackers = spartan.Personal_Trackers;
            if(trackers != null)
            {
                foreach (var tracker in trackers)
                {
                    await _personalTrackerService.DeleteAsync(tracker.Id);
                }
            }

            if(spartan.UserProfile != null)
            {
                await _traineeProfileService.DeleteAsync(spartan.UserProfile.Id);
            }
            
            var deleted = await _spartaService.DeleteAsync(id);

            if (deleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }


        private SpartanDTO CreateSpartanLinks(SpartanDTO spartan)
        {
            if (Url == null) return spartan;

            var idObj = new { id = spartan.Id };
            
            spartan.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetSpartan), idObj),
                "self",
                "GET"));

            spartan.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PostSpartan), idObj),
                "post_spartan",
                "POST"));

            spartan.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutSpartan), idObj),
                "delete_spartan",
                "DELETE"));

            return spartan;
        }
    }
}