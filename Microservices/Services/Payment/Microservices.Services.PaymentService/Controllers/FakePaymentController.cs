using Microservices.Services.PaymentService.Models;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        //Deneme amaçlı oluşturulmuştur.Sanal Pos Entegrasyonu yapılmamıştır
        [HttpPost]
        public IActionResult ReceivePayment(PaymentDto model)
        {
            return CreateActionResultInstance<NoContent>(Response<NoContent>.Success(200));
        }
    }
}
