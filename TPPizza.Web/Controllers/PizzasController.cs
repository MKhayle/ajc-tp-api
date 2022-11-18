using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;
using TPPizza.Business;
using TPPizza.DAL.Model;
using TPPizza.Web.Models;

namespace TPPizza.Web.Controllers
{
    public class PizzasController : Controller
    {
        private readonly PizzaService _pizzaService;
        private readonly HttpClient _httpClient;

        public PizzasController(PizzaService pizzaService, HttpClient httpClient)
        {
            _pizzaService = pizzaService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7225/api/");
        }

        // GET: PizzasController
        public async Task<ActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync("Pizza");

            if (httpResponse.IsSuccessStatusCode)
            {
                var pizzas = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PizzaSimpleViewModel>>();
                return View(pizzas);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: PizzasController/Details/5
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            var httpResponse = await _httpClient.GetAsync($"Pizza/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var pizza = await httpResponse.Content.ReadFromJsonAsync<PizzaSimpleViewModel>();
                return View(pizza);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: PizzasController/Create
        public ActionResult Create()
        {
            PizzaViewModel vm = new();

            PopulateList(ref vm);

            return View(vm);
        }

        // POST: PizzasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name,PateId,IngredientsIds")] PizzaViewModel pizza)
        {
            if (pizza.IngredientsIds.Count is < 2 or > 5)
            {
                ModelState.AddModelError("IngredientsIds", "Trop ou pas assez d'ingrédients !");
            }

            if (_pizzaService.PizzaExists(pizza.Name))
            {
                ModelState.AddModelError("Name", "Nom de pizza déjà existante !");
            }
            if (ModelState.IsValid)
            {
                var postPizza = new StringContent(JsonConvert.SerializeObject(pizza), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PostAsync("Pizza", postPizza);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList(ref pizza);
            return View(pizza);

        }

        // GET: PizzasController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var pizza = await _pizzaService.GetPizzaAsync(id);

            if (pizza == null)
                return NotFound();

            var vm = PizzaViewModel.FromModel(pizza);

            PopulateList(ref vm);

            return View(vm);
        }

        // POST: PizzasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, [Bind("Name,Id,PateId,IngredientsIds")] PizzaViewModel pizza)
        {
            if (pizza.IngredientsIds.Count is < 2 or > 5)
            {
                ModelState.AddModelError("IngredientsIds", "Trop ou pas assez d'ingrédients !");
            }

            if (id != pizza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var putPizza = new StringContent(JsonConvert.SerializeObject(pizza), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PutAsync($"Pizza/{id}", putPizza);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList(ref pizza);
            return View(pizza);
            

        }

        // GET: PizzasController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            var httpResponse = await _httpClient.DeleteAsync($"Pizza/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        private void PopulateList(ref PizzaViewModel pizzaViewModel)
        {
            var pates = _pizzaService.GetPates();
            var ingredients = _pizzaService.GetIngredients();

            pizzaViewModel.Pates = pates.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            pizzaViewModel.Ingredients = ingredients.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        }
    }
}
