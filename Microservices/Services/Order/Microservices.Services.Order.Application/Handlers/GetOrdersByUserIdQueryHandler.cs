using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Application.Mapping;
using Microservices.Services.Order.Application.Queries;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Handlers
{
    //GetOrdersByUserIdQuery tipinde gelen istekleri event a dönüştürecek sınıfımız
    //İkinci Generic Parametre Handler in dönüş tipini belirtmektedir
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        //Kullanıcı Id sine göre Siperişleri sırayalacağız
        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(i => i.OrderItems).Where(i => i.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);

            //Çevrim işlemi yapıldı
            var orderDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            //Siparişlerimizi dönüyoruz
            return Response<List<OrderDto>>.Success(orderDto, 200);
        }
    }
}
