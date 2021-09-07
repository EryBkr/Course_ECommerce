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
    public class UserController : Controller
    {
        //Kullanıcı bilgilerini alacağımız servisimiz
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserInfo();
            return View(user);
        }
    }
}
