using Microservices.Services.BasketServer.Dtos;
using Microservices.Services.BasketServer.Services.Abstract;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        //Redis işlemleri
        private readonly IBasketService _basketService;

        //Token dan userId depğerini alma
        private readonly ISharedIdentityService _identityService;

        public BasketController(IBasketService basketService, ISharedIdentityService identityService)
        {
            _basketService = basketService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            //Giriş yapmış kullanıcıya ait Alışveriş Sepetini aldık
            return CreateActionResultInstance(await _basketService.GetBasket(_identityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            //Header daki User Id yi direkt atadık
            basketDto.UserId = _identityService.GetUserId;


            //Sepete ürün ekleme işlemi
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            //Kullanıcıya ait alışveriş sepetini siliyoruz
            return CreateActionResultInstance(await _basketService.Delete(_identityService.GetUserId));
        }

    }
}
