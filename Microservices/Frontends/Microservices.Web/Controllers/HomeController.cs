using Microservices.Web.Exceptions;
using Microservices.Web.Models;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Controllers
{
    public class HomeController : Controller
    {
        //Course Listesini Servisten alacağım
        private readonly ICatalogService _catalogService;

        public HomeController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses =await _catalogService.GetAllCourseAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var course = await _catalogService.GetByCourseId(id);
            return View(course);
        }

        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            //Auth Hatası aldıysak LogOut a gönderiyoruz ki tekrar login olsun
            if (errorFeature!=null && errorFeature.Error is UnAuthorizeException)
            {
                RedirectToAction("LogOut", "Auth");
            }
            return View();
        }
    }
}
