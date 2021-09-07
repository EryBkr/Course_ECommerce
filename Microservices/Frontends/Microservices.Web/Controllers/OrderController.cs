using Microservices.Web.Models.Order;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.Basket = basket.Data;

            return View(new Checkout());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Checkout model)
        {
            var orderStatus = await _orderService.CreateOrder(model);

            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.Basket = basket.Data;

                ViewBag.Error = orderStatus.Error;
                return View();
            }

            //Başarılı sayfasına gönderiyoruz
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
