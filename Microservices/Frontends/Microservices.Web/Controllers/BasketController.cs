using Microservices.Web.Models.Basket;
using Microservices.Web.Models.Discount;
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
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;

        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }


        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.Get();
            return View(basket);
        }


        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseId(courseId);
            var basketItem = new BasketItemViewModel { CourseId = courseId, CourseName = course.Name, Price = course.Price};
            await _basketService.AddBasketItem(basketItem);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);
            return RedirectToAction("Index");
        }

        //İndirim kuponunu uygula
        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

            if (!ModelState.IsValid)
            {
                //Oluşan hatayı handle ediyoruz
                TempData["discountError"] = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).First();

                return RedirectToAction("Index");
            }

            //Kod uygulandı mı
            TempData["discountStatus"] = discountStatus;

            return RedirectToAction("Index");
        }


        //Uygulanmış kuponu iptal et
        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelApplyDiscount();
            return RedirectToAction("Index");
        }
    }
}
