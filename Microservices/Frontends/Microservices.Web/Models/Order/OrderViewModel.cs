using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Order
{
    //Kullanıcının Siparişlerini görüntüleyeceğiz
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; } //UserId
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
