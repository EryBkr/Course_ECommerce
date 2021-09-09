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

        [HttpGet]
        public async Task<IActionResult> CheckoutHistory()
        {
            //Geçmiş Siparişlerimi alıyorum
            var checkouts =await _orderService.GetOrder();

            return View(checkouts);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Checkout model)
        {
            //Senkron iletişim örneği
            //var orderStatus = await _orderService.CreateOrder(model);

            //Asenkron iletişim örneği (RabbitMQ)
            var orderSuspend = await _orderService.SuspendOrder(model);

            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.Basket = basket.Data;

                ViewBag.Error = orderSuspend.Error;
                return View();
            }

            //Başarılı sayfasına gönderiyoruz (Senkron İletişim)
            //return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });

            //Asenkron iletişim (RabbitMQ)
            //Temsili olarak OrderId oluşturduk.Asenkron iletişim noktasında bizler için gerekli değil
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = Guid.NewGuid().ToString()});
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
