using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    //Hem Entity hemde Agregate rolünü üstlendiği için iki imzayı da tanımladım
    public class Order:Entity,IAggregateRoot
    {
        public DateTime CreatedDate { get; set; }

        //Value Object ekledik
        public Address Address { get; set; }

        //Kim satın aldı
        public string BuyerId { get; set; }

        //Field (Backing Field)
        //Kontrollü bir şekilde OrderItems ile işlem yapabilmek için field olarak ekledik
        //Agregate Root un kullandığı bu entity i başka bir Entity nin kullanmaması gerekiyor
        private readonly List<OrderItem> _orderItems;

        //Sadece okunabilecek
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        //Default  Yapıcı Migration İçin Gerekli Sonuçta burası Entity(Lazy Loading için gerekebilir diye istiyor)
        public Order(){}

        //Data Set işlemi burada yapılıyor
        public Order(Address address, string buyerId)
        {
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
            _orderItems = new List<OrderItem>();
        }

        //Order Item Ekleme işlemi (kontrollü)
        public void AddOrderItem(string productId,string productName,decimal price,string pictureUrl)
        {
            //Gelen productId ye sahip bir ürün var mı
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                //Gelen ürün daha önce eklenmemişse şayet ekleme işlemini gerçekleştiriyoruz
                var newOrderItem = new OrderItem(productId,productName,pictureUrl,price);
                _orderItems.Add(newOrderItem);
            }
        }

        //Toplam Fiyat
        public decimal GetTotalPrice => _orderItems.Sum(i => i.Price);
    }
}
