using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Settings
{
    //Uygulamanın Güvenliği geçebilmesi için oluşturduk.
    public class ClientSettings
    {
        public string Id { get; set; }
        public string Secret { get; set; }
    }
}
