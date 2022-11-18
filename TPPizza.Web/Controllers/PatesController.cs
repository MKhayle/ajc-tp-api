using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TPPizza.Business;
using TPPizza.DAL.Model;
using TPPizza.Web.Models;

namespace TPPizza.Web.Controllers
{
    public class PatesController : Controller
    {
        private readonly PizzaService _pizzaService;
        private readonly HttpClient _httpClient;

        public PatesController(PizzaService pizzaService, HttpClient httpClient)
        {
            _pizzaService = pizzaService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7225/api/");
        }

        // GET: PatesController
        public async Task<ActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync("Pate");

            if (httpResponse.IsSuccessStatusCode)
            {
                var pates = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PateViewModel>>();
                return View(pates);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: PatesController/Details/5
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            var httpResponse = await _httpClient.GetAsync($"Pate/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var pate = await httpResponse.Content.ReadFromJsonAsync<PateViewModel>();
                return View(pate);
            }
            else
            {
                return BadRequest("L'appel HTTP a échoué.");
            }
        }

        // GET: PatesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name")] PateViewModel pate)
        {
            if (ModelState.IsValid)
            {
                var postPate = new StringContent(JsonConvert.SerializeObject(pate), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PostAsync("Pate", postPate);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("L'appel HTTP a échoué.");
                }
            }

            return View(pate);
        }

        // GET: PatesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var pate = await _pizzaService.GetPateAsync(id);

            if (pate == null)
                return NotFound();

            return View(PateViewModel.FromModel(pate));
        }

        // POST: PatesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, [Bind("Name,Id")] PateViewModel pate)
        {
            if (id != pate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
                {
                    var putPate = new StringContent(JsonConvert.SerializeObject(pate), Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PutAsync($"Pate/{id}", putPate);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("L'appel HTTP a échoué.");
                }
            }

            return View(pate);
        }

        // GET: PatesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            var httpResponse = await _httpClient.DeleteAsync($"Pate/{id}");

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
