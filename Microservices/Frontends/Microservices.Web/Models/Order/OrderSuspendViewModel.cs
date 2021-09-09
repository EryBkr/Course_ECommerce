using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Order
{
    //Ödeme ve sipariş işlemleri neticesinde bize dönecek olan sınıf
    public class OrderSuspendViewModel
    {
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
