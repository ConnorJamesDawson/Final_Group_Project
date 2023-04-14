using Final_Project.ApiServices;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpartanController : ControllerBase
    {
        private readonly ISpartanApiService<Spartan> _spartaService;

        public SpartanController(ISpartanApiService<Spartan> spartaService)
        {
            _spartaService = spartaService;
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
        public async Task<ActionResult<Spartan>> PostSpartan(Spartan spartan)
        {
            bool created = await _spartaService.CreateAsync(spartan);

            if (created == false)
            {
                return Problem("Entity set 'NorthwindContext.Suppliers'  is null.");
            }

            return CreatedAtAction("GetSupplier", new { id = spartan.Id }, CreateSpartanLinks(Utils.SpartanToDTO(spartan)));
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}", Name = nameof(DeleteSpartan))]
        public async Task<IActionResult> DeleteSpartan(string id)
        {
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
