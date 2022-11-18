using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TPPizza.Business;
using TPPizza.Web.Models;

namespace TPPizza.Web.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly PizzaService _pizzaService;
        private readonly HttpClient _httpClient;

        public IngredientsController(PizzaService pizzaService, HttpClient httpClient)
        {
            _pizzaService = pizzaService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7225/api/");
        }

        // GET: IngredientsController
        public async Task<ActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync("Ingredient");

            if (httpResponse.IsSuccessStatusCode)
            {
                var ingredients = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<IngredientViewModel>>();
                return View(ingredients);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: IngredientsController/Details/5
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            var httpResponse = await _httpClient.GetAsync($"Ingredient/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var ingredient = await httpResponse.Content.ReadFromJsonAsync<IngredientViewModel>();
                return View(ingredient);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: IngredientsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IngredientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name")] IngredientViewModel ingredient)
        {
            if (ModelState.IsValid)
            {
                var postIngredient = new StringContent(JsonConvert.SerializeObject(ingredient), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PostAsync("Ingredient", postIngredient);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("L'appel HTTP a échoué.");
                }
            }

            return View(ingredient);
        }

        // GET: IngredientsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var ingredient = await _pizzaService.GetIngredientAsync(id);

            if (ingredient == null)
                return NotFound();

            return View(IngredientViewModel.FromModel(ingredient));
        }

        // POST: IngredientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, [Bind("Name,Id")] IngredientViewModel ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var putIngredient = new StringContent(JsonConvert.SerializeObject(ingredient), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PutAsync($"Ingredient/{id}", putIngredient);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("L'appel HTTP a échoué.");
                }
            }

            return View(ingredient);
        }

        // GET: IngredientsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            var httpResponse = await _httpClient.DeleteAsync($"Ingredient/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }
    }
}
