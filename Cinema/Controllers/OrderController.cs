﻿using System;
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
using System.Globalization;

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

            //return OrderContinue(id.Value, date);

            var plac = new PlaceViewModel(id.Value, date);
            plac.plac = _orderService.GetSeats(date, id.Value);

            return View(plac);
        }

        [HttpGet]
        public IActionResult OrderContinue(int? id, DateTime date)
        {
            var plac = new PlaceViewModel(id.Value, date);
              plac.plac = _orderService.GetSeats(date, id.Value);

            return View("Order", plac);
        }

        /// <summary>
        /// Pro dokonceni objednavky, presmerovani z kosiku
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OrderFinish()
        {
            var user = await _userManager.GetUserAsync(User);

            var pom = _orderService.GetSummary(user.Id);

            var summary = new TestSummary();
            decimal price = 0;
            foreach (var item in pom)
            {
                var sum = new Summary(item.Film.Price.OverallPrice, item.IdTime, item.IdDate, item.Film.Name, item.CartPlaces, item.IdCartFilm);
                price += item.Film.Price.OverallPrice;
                sum.ChoosePaymentMethod = _orderService.GetAllMethod();
                sum.ChooseDeliveryType = _orderService.GetAllDelivery();
                summary.Summ.Add(sum);
            }

            if (pom.Count() == 0)
            {
                return RedirectToAction(nameof(CatalogController.Index), "Catalog");
            }

            summary.OverallPrice = price;
            return View("Summary", summary);
        }

        /// <summary>
        /// Pro dokonceni objednavky ihned po vyberu mista
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Order(PlaceViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var dfs = model.Date.ToString("MM-dd-yyyy h:mm tt");
            DateTime dt = DateTime.Parse(dfs);

            _orderService.FindChooseSeats(model.plac, user.Id, model.IdFilm, dt);

            return await OrderFinish();

            //var pom = _orderService.GetSummary(user.Id);

            //var summary = new TestSummary();
            //decimal price = 0;
            //foreach (var item in pom)
            //{
            //    var sum = new Summary(item.Film.Price.OverallPrice, item.IdTime, item.IdDate, item.Film.Name, item.CartPlaces, item.IdCartFilm);
            //    price += item.Film.Price.OverallPrice;
            //    sum.ChoosePaymentMethod = _orderService.GetAllMethod();
            //    sum.ChooseDeliveryType = _orderService.GetAllDelivery();
            //    summary.Summ.Add(sum);
            //}

            //summary.OverallPrice = price;
            //return View("Summary", summary);
        }

        /// <summary>
        /// Dokonceni objednavky, ulozeni polozek do databaze
        /// </summary>
        /// <param name="IdPayment"></param>
        /// <param name="DeliveryType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Zobrazuje polozky v kosiku
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);

            var cart = _orderService.GetUserCartForShow(user.Id);
            
            return View(cart);
        }

        /// <summary>
        /// Vymaze film v kosiku
        /// </summary>
        /// <param name="IdCartFilm"></param>
        /// <returns></returns>
        public IActionResult DeleteFilm(int? IdCartFilm)
        {
            string tmp = Request.Headers["Referer"].ToString();
            _orderService.DeleteFilm(IdCartFilm.Value);
            return RedirectToLocal(tmp);
        }

        /// <summary>
        /// Vymaze film tesne pred dokoncenim objednavky
        /// </summary>
        /// <param name="IdCartFilm"></param>
        /// <returns></returns>
        public IActionResult DeleteFilmOrder(int? IdCartFilm)
        {
            _orderService.DeleteFilm(IdCartFilm.Value);
            return RedirectToAction("OrderFinish");
        }




        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}
