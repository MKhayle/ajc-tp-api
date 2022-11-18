using Microsoft.AspNetCore.Mvc;
using TPPizza.API.Models;
using TPPizza.Business;
using TPPizza.DAL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPPizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PateController : ControllerBase
    {
        private readonly PizzaService _pizzaService;
        public PateController(PizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        // GET: api/<PateController>
        [HttpGet]
        public List<PateViewModel> Get()
        {
            return _pizzaService.GetPates().Select(PateViewModel.FromModel).ToList();
        }

        // GET api/<PateController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PateViewModel>> Get(int id)
        {
            if (_pizzaService.PateExists(id) == true)
            {
                var pate = await _pizzaService.GetPateAsync(id);
                return PateViewModel.FromModel(pate);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<PateController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PateViewModel pate)
        {
            if (ModelState.IsValid)
            {
                await _pizzaService.CreatePateAsync(PateViewModel.ToModel(pate));
                return CreatedAtAction(nameof(Get), new { id = pate.Id }, pate);
            }
            else return BadRequest();
        }

        // PUT api/<PateController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PateViewModel pate)
        {
            if (id != pate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _pizzaService.UpdatePateAsync(PateViewModel.ToModel(pate));
                return NoContent();
            }
            else return BadRequest();
        }

        // DELETE api/<PateController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_pizzaService.PateExists(id) == false) return NotFound();

            await _pizzaService.DeletePateAsync(id);
            return NoContent();
        }
    }
}
