using Microsoft.AspNetCore.Mvc;
using TPPizza.API.Models;
using TPPizza.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPPizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly PizzaService _pizzaService;
        public IngredientController(PizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        // GET: api/<IngredientController>
        [HttpGet]
        public List<IngredientViewModel> Get()
        {
            return _pizzaService.GetIngredients().Select(IngredientViewModel.FromModel).ToList();
        }

        // GET api/<IngredientController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientViewModel>> Get(int id)
        {
            if (_pizzaService.IngredientExists(id) == true)
            {
                var ingredient = await _pizzaService.GetIngredientAsync(id);
                return IngredientViewModel.FromModel(ingredient);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<IngredientController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IngredientViewModel ingredient)
        {
            if (ModelState.IsValid)
            {
                await _pizzaService.CreateIngredientAsync(IngredientViewModel.ToModel(ingredient));
                return CreatedAtAction(nameof(Get), new { id = ingredient.Id }, ingredient);
            }
            else return BadRequest();
        }

        // PUT api/<IngredientController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] IngredientViewModel ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _pizzaService.UpdateIngredientAsync(IngredientViewModel.ToModel(ingredient));
                return NoContent();
            }
            else return BadRequest();
        }

        // DELETE api/<IngredientController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_pizzaService.IngredientExists(id) == false) return NotFound();

            await _pizzaService.DeleteIngredientAsync(id);
            return NoContent();
        }
    }
}
