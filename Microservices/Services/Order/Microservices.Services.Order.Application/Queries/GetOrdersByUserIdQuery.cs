using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Queries
{
    //MediatR CQRS Pattern içerisinde Common ve Query Nesnelerine göre eventler oluşturulmaktadır.
    //Gelen Tip e göre Create,Update,Delete (Commands) ya da Read (Queries) işlemi yapılmaktadır
    //IRequest içerisinde ki Generic ifade bizim dönüş tipimiz olmaktadır
    //GetOrdersByUserIdQuery tipinde gelen istekler burası aracılığıyla birer event e dönüşeceklerdir
    public class GetOrdersByUserIdQuery:IRequest<Response<List<OrderDto>>>
    {
        //Gerekli değeri buradan okuyacağız
        public string UserId { get; set; }
    }
}
