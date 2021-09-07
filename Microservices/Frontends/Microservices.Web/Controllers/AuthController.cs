using Microservices.Web.Models.Auth;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Controllers
{
    public class AuthController : Controller
    {
        //Giriş işlemlerini yönetecek servisimizin soyut hali
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //Giriş Sayfasını açacak
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        //Giriş işlemi gerçekleşecek
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(model);

                //Giriş Başarılı ise
                if (response.IsSuccessful)
                    return RedirectToAction("Index", "Home");


                //Servis tarafından gelen hataları da ekledim bu hatalar div kısmında görünecektir
                response.Errors.ForEach(error => 
                { 
                    ModelState.AddModelError(string.Empty, error); 
                });
                return View(model);
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {

            await _authService.LogOut();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
