using Microservices.Web.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface IOrderService
    {
        //Senkron iletişim
        Task<OrderStatusViewModel> CreateOrder(Checkout checkout);

        //Asenkron İletişim (RabbitMQ)
        Task SuspendOrder(Checkout checkout);

        //Sipariş Geçmişi
        Task<List<OrderViewModel>> GetOrder();
    }
}
