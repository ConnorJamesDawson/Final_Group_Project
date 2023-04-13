using Final_Project.ApiServices;
using Final_Project.Models;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spartan>>> GetSpartans()
        {
            return (await _spartaService.GetAllAsync())
            .ToList();
        }

        // GET: api/Spartan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spartan>> GetSpartan(string id)
        {
            var spartan = await _spartaService.GetAsync(id);

            if (spartan == null)
            {
                return NotFound();
            }

            return spartan;
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        [HttpPost]
        public async Task<ActionResult<Spartan>> PostSpartan(Spartan spartan)
        {
            bool created = await _spartaService.CreateAsync(spartan);

            if (created == false)
            {
                return Problem("Entity set 'NorthwindContext.Suppliers'  is null.");
            }

            return CreatedAtAction("GetSupplier", new { id = spartan.Id }, spartan);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpartan(string id)
        {
            var deleted = await _spartaService.DeleteAsync(id);

            if (deleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
