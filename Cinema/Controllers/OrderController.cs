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
using Cinema.Models.OrderViewModels;

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
            var user = await _userManager.GetUserAsync(User);

            var filmList = _catalogService.GetFilm(id.Value);
            _orderService.Add(user.Id, id.Value, filmList.Time.OverallTime, date);

            List<Places> plac = new List<Places>();
            plac = _orderService.GetSeats(date, id.Value);

            return View(plac);
        }

        [HttpPost]
        public async Task<IActionResult> Order(List<Places> model)
        {
            var user = await _userManager.GetUserAsync(User);

            _orderService.FindChooseSeats(model, user.Id);

            var pom = _orderService.GetSummary(user.Id);

            var summary = new TestSummary();
            decimal price = 0;
            foreach ( var item in pom)
            {
                var sum = new Summary(item.Film.Price.OverallPrice, item.IdTime, item.IdDate, item.Film.Name, item.CartPlaces);
                price += item.Film.Price.OverallPrice;
                sum.ChoosePaymentMethod = _orderService.GetAllMethod();
                sum.ChooseDeliveryType = _orderService.GetAllDelivery();
                summary.Summ.Add(sum);
            }

            summary.OverallPrice = price;
            return View("Summary",summary);
        }

        [HttpPost]
        public async Task<IActionResult> FinishOrder(int? IdPayment, int? DeliveryType, TestSummary model)
        {
            var user = await _userManager.GetUserAsync(User);

            var payment = _orderService.AddGetPayment(IdPayment.Value, model.OverallPrice);

            var newOrder = _orderService.AddOrder(user.Id, payment.IdPayment, DeliveryType.Value);

            
            var pom = _orderService.GetUserCart(user.Id);

            foreach (var item in pom)
            {
                _orderService.AddOrderFilm(newOrder.IdOrder, item.Film.IdFilm, item.Amount.Value, item.IdTime, item.IdDate, item.IdCartFilm);
                _orderService.DeleteCartFilm(item.IdCartFilm);
            }
            return View();
        }
    }
}
