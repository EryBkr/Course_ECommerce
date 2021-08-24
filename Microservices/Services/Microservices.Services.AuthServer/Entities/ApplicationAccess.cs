using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Entities
{
    //Uygulama düzeyinde Yetkilendirme Modelim
    public class ApplicationAccess
    {
        //Aplikasyon kimlik bilgileri
        public string Id { get; set; }
        public string Secret { get; set; }

        //Hangi Apilere erişim olacak
        public List<string> Audiences { get; set; }
    }
}
