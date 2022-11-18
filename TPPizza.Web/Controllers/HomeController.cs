using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TPPizza.Business;
using TPPizza.Web.Models;

namespace TPPizza.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PizzaService _pizzaService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, PizzaService pizzaService)
        {
            _logger = logger;
            _pizzaService = pizzaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}