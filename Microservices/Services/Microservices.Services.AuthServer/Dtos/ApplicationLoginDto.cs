using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Dtos
{
    //Uygulamaların erişim bilgilerini paketleyecek sınıfım
    public class ApplicationLoginDto
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
    }
}
