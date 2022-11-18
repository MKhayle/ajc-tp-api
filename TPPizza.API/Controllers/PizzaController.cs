using Microsoft.AspNetCore.Mvc;
using TPPizza.Business;
using TPPizza.API.Models;
using TPPizza.DAL.Model;
using System.Net;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPPizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaService _pizzaService;
        public PizzaController(PizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        // GET: api/<PizzaController>
        [HttpGet]
        public IEnumerable<PizzaSimpleViewModel> Get()
        {
            return _pizzaService.GetPizzas().Select(PizzaSimpleViewModel.FromModel).ToList();
        }

        // GET api/<PizzaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaSimpleViewModel>> Get(int id)
        {
            if (_pizzaService.PizzaExists(id) == true)
            {
                var pizza = await _pizzaService.GetPizzaAsync(id);
                return PizzaSimpleViewModel.FromModel(pizza);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<PizzaController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PizzaViewModel pizza)
        {
            if (pizza.IngredientsIds.Count is < 2 or > 5)
            {
                ModelState.AddModelError("IngredientsIds", "Trop ou pas assez d'ingrédients !");
            }

            if (_pizzaService.PizzaExists(pizza.Name))
            {
                ModelState.AddModelError("Name", "Nom de pizza déjà existante !");
            }

            if (_pizzaService.PateExists(pizza.PateId) == false) ModelState.AddModelError("PateId", "La pâte sélectionnée n'existe pas.");

            foreach (int ingredient in pizza.IngredientsIds)
            {
                if (_pizzaService.IngredientExists(ingredient) == false) ModelState.AddModelError("IngredientsIds", "Au moins l'un des ingrédients sélectionnés existe pas.");
            }

            if (ModelState.IsValid)
            {
                await _pizzaService.CreatePizzaAsync(PizzaViewModel.ToModel(pizza), pizza.IngredientsIds);
                return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<PizzaController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PizzaViewModel pizza)
        {
            if (_pizzaService.PizzaExists(pizza.Id) == false)
            {
                return NotFound();
            }

            if (pizza.IngredientsIds.Count is < 2 or > 5)
            {
                ModelState.AddModelError("IngredientsIds", "Trop ou pas assez d'ingrédients.");
            }

            if (_pizzaService.PateExists(pizza.PateId) == false) ModelState.AddModelError("PateId", "La pâte sélectionnée n'existe pas.");

            foreach(int ingredient in pizza.IngredientsIds)
            {
                if (_pizzaService.IngredientExists(ingredient) == false) ModelState.AddModelError("IngredientsIds", "Au moins l'un des ingrédients sélectionnés existe pas.");
            }

            if (ModelState.IsValid)
            {
                await _pizzaService.UpdatePizzaAsync(PizzaViewModel.ToModel(pizza), pizza.IngredientsIds);
                return NoContent();
            }
            else return BadRequest(ModelState);
        }

        // DELETE api/<PizzaController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_pizzaService.PizzaExists(id) == false) return NotFound();

            await _pizzaService.DeletePizzaAsync(id);
            return NoContent();
        }
    }
}
