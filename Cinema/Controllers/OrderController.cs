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

        public IActionResult Order(int? id, DateTime date)
        {
            var filmList = _catalogService.GetFilm(id.Value);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart([FromBody] Film viewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                
            }
            return View("Order");
        }


    }
}
