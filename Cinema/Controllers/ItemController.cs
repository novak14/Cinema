using System;
using System.Collections.Generic;
using Catalog.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Catalog.Dal.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Cinema.Controllers
{
    public class ItemController : Controller
    {
        private readonly CatalogService _catalogService;

        public ItemController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }


        public IActionResult Index(int id)
        {
            var oneFilm = _catalogService.GetOneFilm(id);

            return View(oneFilm);
        }
    }
}
