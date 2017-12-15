using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Catalog.Business;
using Microsoft.Extensions.Logging;
using Cinema.Models.HomeViewModels;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly ILogger _logger;

        public HomeController(CatalogService catalogService, ILoggerFactory loggerFactory)
        {
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        public IActionResult Index()
        {
            var pictures = _catalogService.HomePage();
            var day = _catalogService.GetHomePage();

            var front = new HomePageViewModel(pictures, day);
            return View(front);
        }

        public IActionResult About()
        {

            var product = 
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
