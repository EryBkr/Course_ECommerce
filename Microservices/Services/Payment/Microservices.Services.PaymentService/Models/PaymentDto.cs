using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.PaymentService.Models
{
    public class PaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }

        //Order ile asenkron olarak iletişim kuracağımız için gönderilecek modele ihtiyacımız var
        public OrderDto OrderDto { get; set; }
    }
}
