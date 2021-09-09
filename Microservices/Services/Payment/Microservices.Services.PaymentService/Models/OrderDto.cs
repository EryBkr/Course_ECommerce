using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.PaymentService.Models
{
    //FakePayment Order Servisiyle Asenkron olarak iletişim kuracak.Gönderilecek Modeli Burada oluşturuyorum
    public class OrderDto
    {
        //Listemizi yapıcıda oluşturuyoruz
        public OrderDto()
        {
            OrderItemDtos = new List<OrderItem>();
        }

        public string BuyerId { get; set; } //UserId
        public List<OrderItem> OrderItemDtos { get; set; }
        public AddressCreateInput AddressDto { get; set; }
    }

    public class AddressCreateInput
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; } //CourseId
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
