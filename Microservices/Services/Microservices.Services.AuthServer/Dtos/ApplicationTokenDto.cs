using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Dtos
{
    //Uygulamaların Token ve Token süresi için oluşturduğum modelim
    public class ApplicationTokenDto
    {
        //Endpointlere istek yapacak olan token tipi
        public string AccessToken { get; set; }
        //Access Tokenin Expire Zamanı
        public DateTime AccessTokenExpireDate { get; set; }
    }
}
