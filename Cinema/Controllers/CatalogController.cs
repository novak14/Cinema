using Catalog.Business;
using Catalog.Dal.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dal.Entities;
using Microsoft.EntityFrameworkCore;

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
            var courses = _context.Film.Include(c => c.Access).ToList();
            var count = courses.Count();
            var manyToMany = _context.Film_dim.Include(c => c.Film.Access).Include(c => c.Film).Include(c => c.Dimension).ToList();
            var countMany = manyToMany.Count();


            foreach (var item in courses)
            {
                var pom = item.Access.Age;
            }


            foreach (var item in manyToMany)
            {
                var pom = item.Film.Access.Age;
                var dim = item.Dimension.DimensionType;
            }

            var film = _catalogService.GetFilm(1); 
            return View(manyToMany);
        }

    }
}
