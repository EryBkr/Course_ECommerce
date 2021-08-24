using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Shared.BaseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        //Kullanıcı oluşturuyoruz
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return CreateActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }


        [HttpGet]
        [Authorize]
        //Giriş yapmış Kullanıcı ismine göre kullanıcı bilgilerini alıyoruz
        //Giriş yapmamış kullanıcılar haliyle ulaşamayacaklar
        public async Task<IActionResult> GetUser()
        {
            //Gelen token içerisinden NameClaim değerini bu şekilde alabiliyoruz
            //Token Header içerisinde geliyor
            var userName = HttpContext.User.Identity.Name;

            return CreateActionResultInstance(await _userService.GetUserByNameAsync(userName));
        }

    }
}
