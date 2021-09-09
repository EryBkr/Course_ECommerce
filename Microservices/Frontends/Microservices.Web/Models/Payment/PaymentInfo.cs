using Microservices.Web.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Payment
{
    public class PaymentInfo
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }

        //Payment Servis Aracılığıyla (MQ Message ile) Sipariş oluştarabilmek için gerekli dataları da gönderiyoruz
        public CreateOrderInput OrderDto { get; set; }
    }
}
