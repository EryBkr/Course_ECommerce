using Microservices.Shared.Services.Abstract;
using Microservices.Web.Helpers;
using Microservices.Web.Models.Catalog;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        //Catalog Microservice ile iletişime geçecek servisim
        private readonly ICatalogService _catalogService;

        //Shared katmanında tanımlamış olduğum, kullanıcının id değerini claim içerisinden bize getirecek servisim
        private readonly ISharedIdentityService _sharedIdentityService;

        public CatalogController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        //Kullanıcıya ait kursları listeliyoruz
        public async Task<IActionResult> Index()
        {
            var userId = _sharedIdentityService.GetUserId;
            var coursesByUserId =await _catalogService.GetAllCourseByUserIdAsync(userId);
            return View(coursesByUserId);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Select Box içerisinde Kategorilerin dolu gelmesi için 
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
           
            //Validation Error var ise
            if (!ModelState.IsValid)
            {
                //Select Box içerisinde Kategorilerin dolu gelmesi için (Hata Durumunda)
                var categories = await _catalogService.GetAllCategoryAsync();
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            //Kullanıcı Id bilgisini verdik
            model.UserId = _sharedIdentityService.GetUserId;
            await _catalogService.AddCourseAsync(model);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            //Select Box içerisinde Kategorilerin dolu gelmesi için 
            var categories = await _catalogService.GetAllCategoryAsync();
         

            //Güncellenecek Kurs u alıyoruz
            var course = await _catalogService.GetByCourseId(id);

            //Kurs boş ise (Yüksek ihtimal yanlış Id değeri verilmiştir)
            if (course==null)
            {
                ModelState.AddModelError("","Böyle Bir kurs bulunamadı");
                return View("Index");
            }

          
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name",course.CategoryId);

            //MongoDb den gelen kurs u sayfaya gönderiyorum
            CourseUpdateViewModel model = new CourseUpdateViewModel 
            {
                Id=course.Id,
                Name=course.Name,
                Picture=course.Picture,
                CategoryId=course.CategoryId,
                UserId=course.UserId,
                Description=course.Description,
                Price=course.Price,
                Feature = course.Feature
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //Select Box içerisinde Kategorilerin dolu gelmesi için 
                var categories = await _catalogService.GetAllCategoryAsync();
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name",model.CategoryId);

                return View(model);
            }

            await _catalogService.UpdateCourseAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction("Index");
        }
    }
}
