using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Discount
{
    public class DiscountViewModel
    {
        public string UserId { get; set; } //Kupon hangi kullanıcı için oluşturuldu
        public int Rate { get; set; } //İndirim oranı
        public string Code { get; set; } //Kuponun kendisi
    }
}
