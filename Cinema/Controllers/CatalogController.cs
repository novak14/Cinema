using Catalog.Business;
using Catalog.Dal.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly CatalogDbContext _context;
        private readonly ILogger _logger;


        public CatalogController(CatalogDbContext context, CatalogService catalogService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<CatalogController>();

        }

        public IActionResult Index()
        {
            var film = _catalogService.GetFilm(1); // toto nefunguje
            //var film = _catalogservice.Film.Where(s => (s.id_film == 1)).FirstOrDefault();
            return View(film);
        }

    }
}
