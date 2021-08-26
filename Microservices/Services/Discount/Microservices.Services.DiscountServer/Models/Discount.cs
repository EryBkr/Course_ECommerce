using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.DiscountServer.Models
{
    [Dapper.Contrib.Extensions.Table("discount")] //PostgreSql küçük harflerle çalıştığı için böyle bir düzenleme yaptık
    public class Discount //İndirim tablosu
    {
        public int Id { get; set; }
        public string UserId { get; set; } //Kupon hangi kullanıcı için oluşturuldu
        public int Rate { get; set; } //İndirim oranı
        public string Code { get; set; } //Kuponun kendisi
        public DateTime CreatedTime { get; set; }
    }
}
