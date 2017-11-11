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
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly CatalogDbContext _context;
        private readonly ILogger _logger;

        //private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);



        public CatalogController(CatalogDbContext context, CatalogService catalogService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<CatalogController>();
        }


        public IActionResult Index()
        {
            //var manddy = _context.Film.Include(c => c.Access).Include(c => c.Dabing).Include(c => c.Price).Include(c => c.Time).Include(c => c.FilmDim).ThenInclude(c => c.Dimension).Include(c => c.Film_type).ThenInclude(c => c.Type).ToList();


            Stopwatch dapper = new Stopwatch();
            dapper.Start();
            var allFilms = _catalogService.GetAllFilms();
            dapper.Stop();
            var dapperCheck = dapper.Elapsed;
            return View(allFilms);
        }

    }
}
