using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Order
{
    //Sipariş Başarılı ise Oluşan siparişin Id si bize gelecek
    public class OrderStatusViewModel
    {
        public int OrderId { get; set; }
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
