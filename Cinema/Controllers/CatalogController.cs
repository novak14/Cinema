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
            //var count = courses.Count();
            var manyToMany = _context.Film_dim.Include(c => c.Dimension).ToList();

            var filmType = _context.Film_type.Include(c => c.Type).ToList();
            //var countMany = manyToMany.Count();

            var many = _context.Film.Include(c => c.Access).Include(c => c.Dabing).Include(c => c.Price).Include(c => c.Time).Include(c => c.Film_dim).Include(c => c.Film_type).ToList();
            
            foreach (var item in many)
            {
                //var dim = item.Film_dim.ToList()[0].Dimension.DimensionType;
                //var tmp = item.Film_type.ToList()[0].Type.Genre;
                var pom = item.Film_dim.ToList();
            }

            //foreach (var item in filmType)
            //{
            //    var pom = item.Film.Name;
            //    var pom1 = item.Type.Genre;
            //    var pom2 = item.Film.Film_dim;
            //}

            //foreach (var item in courses)
            //{
            //    var pom = item.Access.Age;
            //}

            

            //foreach (var item in manyToMany)
            //{
            //    var pom = item.Film.Access.Age;
            //    var pom1 = item.Film.Dabing.Name;
            //    var pom2 = item.Film.Price.OverallPrice;
            //    var pom3 = item.Film.Time.OverallTime;
            //    foreach (var tmp in item.Film.Film_type)
            //    {
            //        var pom4 = tmp.Type.Genre;
            //    }
            //    var kkk = item.Dimension.DimensionType;
            //}

            var film = _catalogService.GetFilm(1); 
            return View(many);
        }

    }
}
