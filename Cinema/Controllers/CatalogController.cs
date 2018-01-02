using System.Collections.Generic;
using Catalog.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Catalog.Dal.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Order.Dal.Entities;

namespace Cinema.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IActionResult Index()
        {
            var allFilms = _catalogService.GetDateFilms();
            return View(allFilms);
        }

    }
}
