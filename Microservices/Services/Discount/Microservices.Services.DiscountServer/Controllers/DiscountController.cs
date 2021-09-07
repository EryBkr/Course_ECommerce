using Microservices.Services.DiscountServer.Models;
using Microservices.Services.DiscountServer.Services.Abstract;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.DiscountServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        //Token dan userId değerini alma
        private readonly ISharedIdentityService _identityService;

        //Dapper ile Postgre bağlantısı
        private readonly IDiscountService _discountService;

        public DiscountController(ISharedIdentityService identityService, IDiscountService discountService)
        {
            _identityService = identityService;
            _discountService = discountService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        // /api/discount/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResultInstance(await _discountService.GetById(id));
        }


        //Kullanıcı Id si ve Code değeriyle eşleşen kod
        [Route("/api/[controller]/[action]/{code}")]
        [HttpGet]
        public async Task<IActionResult> GetByUserIdAndCode(string code)
        {
            var discountResponse = await _discountService.GetByCodeAndUserId(code, _identityService.GetUserId);
            return CreateActionResultInstance(discountResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Add(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.Delete(id));
        }
    }
}
