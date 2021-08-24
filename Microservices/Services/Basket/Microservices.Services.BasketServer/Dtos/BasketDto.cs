using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer.Dtos
{
    //Ana sepet modeli
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }

        //Toplam fiyatı model üzerinden aldık
        public decimal TotalPrice { get => BasketItems.Sum(i => i.Price * i.Quantity); }
    }
}
