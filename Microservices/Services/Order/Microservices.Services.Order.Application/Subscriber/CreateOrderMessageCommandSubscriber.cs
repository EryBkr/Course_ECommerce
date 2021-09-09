using MassTransit;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Subscriber
{
    //Payment serviste yayınladığımız datayı (MassTransit aracılığıyla) burada handle edeceğiz.
    public class CreateOrderMessageCommandSubscriber : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _dbContext;

        public CreateOrderMessageCommandSubscriber(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //Payment servisinin gönderdiği RabbitMQ daki kuyruktan gelen mesajı burada okuyorum
        //CreateOrderMessageCommand sınıfımız Shared katmanımız da tanımlıdır.Hem Publisher ın hemde Subscriber ın kullandığı bu sınıfın ortak bir katmanda buluması gerekmektedir
        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            //Context üzerinden gelen mesajdan addres bilgisini ve Order bilgisini oluşturduk
            var newAddress = new Domain.OrderAggregate.Address(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);

            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(newAddress, context.Message.BuyerId);

            //Order Items bilgisini de Order nesneme ekledim
            context.Message.OrderItems.ForEach(i=> 
            {
                order.AddOrderItem(i.ProductId, i.ProductName, i.Price, i.PictureUrl);
            });

            //Kuyruktan aldığımız Order ve Adres bilgisini kayıt ediyoruz
            await _dbContext.Orders.AddAsync(order);

            await _dbContext.SaveChangesAsync();
        }
    }
}
