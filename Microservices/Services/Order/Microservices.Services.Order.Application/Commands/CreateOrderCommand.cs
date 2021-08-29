using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Commands
{
    //MediatR CQRS Pattern içerisinde Common ve Query Nesnelerine göre eventler oluşturulmaktadır.
    //Gelen Tip e göre Create,Update,Delete (Commands) ya da Read (Queries) işlemi yapılmaktadır
    //IRequest içerisinde ki Generic ifade bizim dönüş tipimiz olmaktadır
    //CreateOrderCommand tipinde gelen istekler burası aracılığıyla birer event e dönüşeceklerdir
    public class CreateOrderCommand:IRequest<Response<CreatedOrderDto>>
    {
        //Bu Model içerisinde bu propertyler başarıyla geldiği zaman gerekli event oluşacaktır
        public string BuyerId { get; set; } //UserId
        public List<OrderItemDto> OrderItemDtos { get; set; }
        public AddressDto AddressDto { get; set; }
    }
}
