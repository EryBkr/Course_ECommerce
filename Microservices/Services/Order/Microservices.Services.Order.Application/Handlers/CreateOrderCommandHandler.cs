using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Domain.OrderAggregate;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Handlers
{
    //CreateOrderCommand tipinde gelen istekleri event a dönüştürecek sınıfımız
    //İkinci Generic Parametre Handler in dönüş tipini belirtmektedir
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //Yapıcı metot aracılığıyla AddressDto aracılığıyla Address imizi oluşturuyoruz
            var newAddress = new Address(request.AddressDto.Province, request.AddressDto.District, request.AddressDto.Street, request.AddressDto.ZipCode, request.AddressDto.Line);

            //Order ı oluşturuyoruz
            Domain.OrderAggregate.Order newOrder = new(newAddress, request.BuyerId);

            //Order Item oluşturuyoruz
            request.OrderItemDtos.ForEach(x =>
            {
                //Request ten gelen OrderItem aracılığıyla Order nesnemiz içerisinde ki OrderItem listesini dolduruyoruz
                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });

            //Order nesnemizin içerisi doldurulup Veritabanına ekleniyor.
            //Onunla ilişkili nesneler Order aracılığıyla ekleniyor
            await _context.Orders.AddAsync(newOrder);

            await _context.SaveChangesAsync();

            //Oluşan Order in Id sini geri dönüyoruz
            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 204);
        }
    }
}
