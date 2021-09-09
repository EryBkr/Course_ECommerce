using MassTransit;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Subscriber
{
    //Course Update işleminden sonra Course entity sindeki değişikliği rabbitMQ dan dinleyip uygulayacak sınıfımız
    public class CourseNameChangedEventSubscriber : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventSubscriber(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            //Siparişlerim içerisinden ,Course servisi tarafından gönderilen ürünü alıyorum
            var orderItems = await _orderDbContext.OrderItems.Where(i => i.ProductId == context.Message.CourseId).ToListAsync();

            //Contexten gelen datalarım ile güncelleme işlemi yapıyorum
            orderItems.ForEach(i=> 
            {
                i.UpdateOrderItem(context.Message.UpdatedName, i.PictureUrl, i.Price);
            });

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
