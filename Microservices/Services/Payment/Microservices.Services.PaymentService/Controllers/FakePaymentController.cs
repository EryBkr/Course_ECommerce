using MassTransit;
using Microservices.Services.PaymentService.Models;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Dtos;
using Microservices.Shared.Messages;
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
        //MassTransit i kullanmak için ekledik
        //Send ibaresi genellikle Command kullanılacağı zaman seçilir.Tek bir servis erişecekse Send , birden çok servis erişecekse Publish ibaresi kullanılır
        private readonly ISendEndpointProvider _sendProvider;

        public FakePaymentController(ISendEndpointProvider sendProvider)
        {
            _sendProvider = sendProvider;
        }

        //Deneme amaçlı oluşturulmuştur.Sanal Pos Entegrasyonu yapılmamıştır
        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto model)
        {
            //Kuyruk ismini belirttim
            var sendEndPoint =await _sendProvider.GetSendEndpoint(new Uri("queue:create-order"));

            //Shared katmanında ki modelimizi oluşturuyorum
            var createOrderMessageCommand = new CreateOrderMessageCommand();

            //Modelimi gelen datalarla dolduruyorum
            createOrderMessageCommand.BuyerId = model.OrderDto.BuyerId;
            createOrderMessageCommand.Province = model.OrderDto.AddressDto.Province;
            createOrderMessageCommand.District = model.OrderDto.AddressDto.District;
            createOrderMessageCommand.Street = model.OrderDto.AddressDto.Street;
            createOrderMessageCommand.Line = model.OrderDto.AddressDto.Line;
            createOrderMessageCommand.ZipCode = model.OrderDto.AddressDto.ZipCode;

            //Order Items Create
            model.OrderDto.OrderItemDtos.ForEach(i => 
            {
                createOrderMessageCommand.OrderItems.Add(new Shared.Messages.OrderItem 
                {
                    PictureUrl=i.PictureUrl,
                    Price=i.Price,
                    ProductId=i.ProductId,
                    ProductName=i.ProductName
                });
            });

            //Oluşturulan mesajı kuyruğa gönderiyorum
            await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance<NoContent>(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
