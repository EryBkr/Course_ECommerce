using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Queries;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController //Dönüş tiplerimiz için ekledik 
    {
        //Gelen tipe göre event çalıştaracak interface
        private readonly IMediator _mediator;

        //JWT içerisinden ID yi alabilmek için ekledik
        private readonly ISharedIdentityService _sharedService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedService)
        {
            _mediator = mediator;
            _sharedService = sharedService;
        }

        [HttpGet] //Kişiye ait siparişleri getirecek
        public async Task<IActionResult> GetOrders()
        {
            //Giriş yapmış kullanıcının Id sini aldık.Bu Action a giriş yapmamış kullanıcının ulaşabilme şansı yok zaten
            var userId = _sharedService.GetUserId;

            //MediatR aracılığıyla bu model e ait execute handler çalışacaktır
            //Dönüş tipimiz IRequestHandler tarafında belirlenmişti
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId });

            return CreateActionResultInstance(response);
        }

        [HttpPost] //Kişiye ait siparişler kayıt edilecek
        public async Task<IActionResult> SaveOrder(CreateOrderCommand model)
        {
            //Gelen Tip e göre de farklı Handler çalışacaktır
            var response = await _mediator.Send(model);

            return CreateActionResultInstance(response);
        }

    }
}
