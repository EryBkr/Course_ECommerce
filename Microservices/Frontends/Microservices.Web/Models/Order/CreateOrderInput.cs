using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Order
{
    //Sipariş servisimize post edilecek model
    public class CreateOrderInput
    {
        //Listemizi yapıcıda oluşturuyoruz
        public CreateOrderInput()
        {
            OrderItemDtos = new List<OrderItemViewModel>();
        }

        public string BuyerId { get; set; } //UserId
        public List<OrderItemViewModel> OrderItemDtos { get; set; }
        public AddressCreateInput AddressDto { get; set; }
    }
}
