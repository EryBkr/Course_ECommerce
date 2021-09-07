using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Basket
{
    public class BasketItemViewModel
    {
        //Kurs ürünü birden fazla alınamayacağı için otomatik 1 değerini verdik
        public int Quantity { get; } = 1;
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }

        //İndirimli Fiyat
        private decimal? DiscountedPrice;

        //Fiyata İndirim Uygulandıysa indirimli fiyat değilse normal fiyat gelecek
        public decimal GetCurrentPrice { get => DiscountedPrice != null ? DiscountedPrice.Value : Price; }

        //İndirimli Fiyat oluşturuldu
        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountedPrice = discountPrice;
        }
    }
}
