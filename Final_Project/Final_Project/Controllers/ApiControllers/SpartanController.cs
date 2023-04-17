using Final_Project.ApiServices;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Final_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public partial class SpartanController : Controller
    {
        private readonly ISpartanApiService<Spartan> _spartaService;
        private readonly ISpartaApiService<PersonalTracker> _personalTrackerService;
        private readonly ISpartaApiService<TraineeProfile> _traineeProfileService;
        private readonly ISpartanApiService<IdentityUserRole<string>> _userRoleService;
        private readonly ISpartanApiService<IdentityRole> _roleService;
        private readonly UserManager<Spartan> _userManager;

        public SpartanController(ISpartanApiService<Spartan> spartaService, ISpartaApiService<PersonalTracker> personalTrackerService, ISpartaApiService<TraineeProfile> traineeProfileService, ISpartanApiService<IdentityUserRole<string>> userRoleService, ISpartanApiService<IdentityRole> roleService, UserManager<Spartan> userManager)
        {
            _spartaService = spartaService;
            _personalTrackerService = personalTrackerService;
            _traineeProfileService = traineeProfileService;
            _userRoleService = userRoleService;
            _roleService = roleService;
            _userManager = userManager;
        }

        // GET: api/Spartan
        [HttpGet(Name = nameof(GetSpartans)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<SpartanDTO>>> GetSpartans()
        {
            var spartans = await _spartaService.GetAllAsync();

            if (spartans == null)
            {
                return NotFound();
            }

            //var spartansDtos = spartans.Select(s => CreateSpartanLinks(s)).ToList();
            var spartansDtos = new List<SpartanDTO>();
            foreach (var spartan in spartans)
            {
                var spartanDto = CreateSpartanLinks(spartan);
                spartanDto.Role = _userManager.GetRolesAsync(spartan).Result.ToList();
                spartansDtos.Add(spartanDto);
            }

            return spartansDtos;
        }

        // GET: api/Spartan/5
        [HttpGet("{id}", Name = nameof(GetSpartan)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SpartanDTO>> GetSpartan(string id)
        {
            var spartan = await _spartaService.GetAsync(id);

            if (spartan == null)
            {
                return NotFound();
            }

            var spartanDto = CreateSpartanLinks(spartan);

            spartanDto.Role = _userManager.GetRolesAsync(spartan).Result.ToList();

            return spartanDto;
        }

        // PUT: api/Spartans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(PutSpartan)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SpartanDTO>> PutSpartan(string id, SpartanDTO spartanDto)
        {
            var spartan = await _spartaService.GetAsync(id);

            if (spartan == null)
            {
                return NotFound();
            }

            spartan.UserName = spartanDto.UserName;
            spartan.FirstName = spartanDto.FirstName;
            spartan.LastName = spartanDto.LastName;
            spartan.Email = spartanDto.Email;
            spartan.PhoneNumber = spartanDto.PhoneNumber;
            spartan.EmailConfirmed = spartanDto.EmailConfirmed;


            if (!string.IsNullOrEmpty(spartanDto.PasswordHash))
            {
                var passwordHasher = new PasswordHasher<Spartan>();
                spartan.PasswordHash = passwordHasher.HashPassword(spartan, spartanDto.PasswordHash);
            }
            
            await _userManager.RemoveFromRolesAsync(spartan, _userManager.GetRolesAsync(spartan).Result);
            await _userManager.AddToRolesAsync(spartan, spartanDto.Role);

            var result = await _spartaService.UpdateAsync(spartan.Id, spartan);

            if (!result)
            {
                return BadRequest(result);
            }

            var createdSpartanDto = Utils.SpartanToDTO(spartan);
            createdSpartanDto.Role = _userManager.GetRolesAsync(spartan).Result.ToList();

            return Ok(createdSpartanDto);
        }

        // POST: api/Spartan
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = nameof(PostSpartan)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SpartanDTO>> PostSpartan(SpartanDTO spartanDto)
        {
            var spartan = new Spartan();

            spartan = Utils.DTOToSpartan(spartanDto);

            var passwordHasher = new PasswordHasher<Spartan>();
            spartan.PasswordHash = passwordHasher.HashPassword(spartan, spartanDto.PasswordHash);

            await _spartaService.CreateAsync(spartan);
            await _spartaService.SaveAsync();

            var roles = _roleService.GetAllAsync().Result.Select(r => r.NormalizedName).ToList();
            
            bool allAreRoles = true;

            foreach (var role in spartanDto.Role)
            {
                if (roles.Contains(role.ToUpper())) continue;

                allAreRoles = false;
            }

            if (!allAreRoles)
            {
                return BadRequest("Role does not exist");
            }

            await _userManager.AddToRolesAsync(spartan, spartanDto.Role);

            var createdSpartanDto = Utils.SpartanToDTO(spartan);
            createdSpartanDto.Role = _userManager.GetRolesAsync(spartan).Result.ToList();

            return CreatedAtAction(nameof(GetSpartan), new { id = spartan.Id }, createdSpartanDto);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}", Name = nameof(DeleteSpartan)), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteSpartan(string id)
        {
            var spartan = await _spartaService.GetAsync(id);
            if (spartan == null)
            {
                return NotFound();
            }
            var trackers = spartan.Personal_Trackers;
            if (trackers != null)
            {
                foreach (var tracker in trackers)
                {
                    await _personalTrackerService.DeleteAsync(tracker.Id);
                }
            }

            if (spartan.UserProfile != null)
            {
                await _traineeProfileService.DeleteAsync(spartan.UserProfile.Id);
            }

            await _userManager.RemoveFromRolesAsync(spartan, _userManager.GetRolesAsync(spartan).Result);
            var deleted = await _spartaService.DeleteAsync(id);

            if (deleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        private SpartanDTO CreateSpartanLinks(Spartan spartan)
        {
            var spartanDto = Utils.SpartanToDTO(spartan);
            if (Url == null) return spartanDto;

            var idObj = new { id = spartan.Id };

            var profile = _traineeProfileService.GetAllAsync().Result.Where(p => p.SpartanId == spartan.Id).FirstOrDefault();

            if (profile != null)
            {
                spartanDto.Profile = Url.Link("GetTraineeProfile", new { id = profile.Id });
            }

            var trackers = _personalTrackerService.GetAllAsync().Result.Where(t => t.SpartanId == spartan.Id);

            if (trackers != null)
            {
                foreach (var tracker in trackers)
                {
                    spartanDto.Trackers.Add(Url.Link("GetPersonalTracker", new { id = tracker.Id }));
                }
            }

            spartanDto.Links.Add(
                new LinkDTO(Url.Link(nameof(this.GetSpartan), idObj),
                "self",
                "GET"));

            spartanDto.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PostSpartan), idObj),
                "post_spartan",
                "POST"));

            spartanDto.Links.Add(
                new LinkDTO(Url.Link(nameof(this.PutSpartan), idObj),
                "delete_spartan",
                "DELETE"));

            return spartanDto;
        }
    }
}
