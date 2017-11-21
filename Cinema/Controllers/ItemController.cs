using System.Collections.Generic;
using Catalog.Business;
using Catalog.Dal.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Catalog.Dal.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class ItemController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly ILogger _logger;

        public ItemController(CatalogService catalogService, ILoggerFactory loggerFactory)
        {
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<CatalogController>();
        }


        public IActionResult Index(int id)
        {
            //var manddy = _context.Film.Include(c => c.Access).Include(c => c.Dabing).Include(c => c.Price).Include(c => c.Time).Include(c => c.FilmDim).ThenInclude(c => c.Dimension).Include(c => c.Film_type).ThenInclude(c => c.Type).ToList();

            Stopwatch dapper = new Stopwatch();
            dapper.Start();
            var oneFilm = _catalogService.GetOneFilm(id);
            dapper.Stop();
            var dapperCheck = dapper.Elapsed;
            return View(oneFilm);
        }
    }
}
