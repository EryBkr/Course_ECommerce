using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Messages
{
    //MassTransit Command kullanımında şayet tek bir servise mesaj göndereceksek Command'ı tercih etmemiz söylenir. Command a Send işlemi uygulanır
    public class CreateOrderMessageCommand
    {
        public CreateOrderMessageCommand()
        {
            //Instance oluşturuyorum
            OrderItems = new List<OrderItem>();
        }
        //Sipariş Oluşturulurken Bize Ne Lazım


        public string BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        //Adres Sütunlarını Obje olarak eklemedim
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
