using System;
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
using System.Net;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Cinema.Controllers
{
    public class ItemController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemController(CatalogService catalogService, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<CatalogController>();
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index(int id)
        {
            //var manddy = _context.Film.Include(c => c.Access).Include(c => c.Dabing).Include(c => c.Price).Include(c => c.Time).Include(c => c.FilmDim).ThenInclude(c => c.Dimension).Include(c => c.Film_type).ThenInclude(c => c.Type).ToList();

            Stopwatch dapper = new Stopwatch();
            dapper.Start();
            var oneFilm = _catalogService.GetOneFilm(id);
            dapper.Stop();
            var dapperCheck = dapper.Elapsed;

            Cookie tmp = new Cookie();
            var cook = tmp.Value;

            //read cookie from IHttpContextAccessor  
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["key"];
            //read cookie from Request object  
            string cookieValueFromReq = Request.Cookies["Key"];
            //set the key value in Cookie  
            Set("kay", "Hello from cookie", 10);
            //Delete the cookie object  


            return View(oneFilm);
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }

        public string Get(string key)
        {
            return Request.Cookies["Key"];
        }
    }
}
