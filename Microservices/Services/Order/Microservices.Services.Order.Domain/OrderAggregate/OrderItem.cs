using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    //Tabloya karşılık geldiği için Entity den miras aldık
    //İlişkisel durumlarda crud işlemlerini sadece agregateroot aracılığıyla yapacağım için navigation propery eklemiyorum
    public class OrderItem:Entity
    {
        //Properylere dışarıdan değer atanamaz
        public string ProductId { get;private set; } //CourseId
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public Decimal Price { get; private set; }

        //OrderId eklemesi yapmasak bile Order ile OrderItem arasında bir ilişki oluduğu için Database tarafında OrderId eklemesi otomatik olarak yapılır.Buna Shadow property denir.Biz AggregateRoot olarak Order ı belirlediğimiz için burada navigation property ve foreign key tanımlaması yapmadık

        //Değer atamasını yapıcı metot ile yapıyoruz
        //Bussiness kodu buralarda uygulanabilir
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        //Default  Yapıcı Migration İçin Gerekli Sonuçta burası Entity(Lazy Loading için gerekebilir diye istiyor)
        public OrderItem(){}


        public void UpdateOrderItem(string productName,string pictureUrl,decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
    }
}
