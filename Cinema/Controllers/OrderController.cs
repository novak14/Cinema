using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Business;
using Catalog.Dal.Entities;
using Cinema.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Order.Business;
using System.Data.SqlClient;
using Dapper;
using Order.Dal.Entities;

namespace Cinema.Controllers
{
    public class OrderController : Controller
    {
        private readonly CatalogService _catalogService;

        private readonly OrderService _orderService;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrderController(
            CatalogService catalogService,
            OrderService orderService,
            ILoggerFactory loggerFactory, 
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _catalogService = catalogService;
            _orderService = orderService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<OrderController>();
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Order(int? id, DateTime date)
        {
            var filmList = _catalogService.GetFilm(id.Value);

            var user = await _userManager.GetUserAsync(User);


            var conString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sql = @"insert CartFilm(IdUser,IdFilm,IdTime,IdDate) values(@IdUser,@IdFilm,@IdTime,@IdDate);";


            using (var connection = new SqlConnection(conString))
            {
               // var dom = connection.Execute(sql, new { IdUser = user.Id, IdFilm = id.Value, IdTime = filmList.Time.OverallTime, IdDate = date });

                var d = connection.Query<Places>("Select * From Places").ToList();
                return View(d);

            }



        }

        [HttpPost]
        public IActionResult Order(List<Places> model)
        {

            for (int i = 0; i < 10; i++)
            {

                if (model[i].checkboxAnswer == true)
                {
                    //_catalogService.FilterOneFirm(model, model.Firms[i].id_fir, tmpModel);
                    //isCheckFirm = 1; // nastavuji na hodnotu 1 abych vedel ze se nasel alespon jeden
                }

            }

            if (_signInManager.IsSignedIn(User))
            {

            }
            return View("Order");
        }


    }
}
