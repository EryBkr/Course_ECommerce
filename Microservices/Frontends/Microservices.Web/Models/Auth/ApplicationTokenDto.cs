using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Auth
{
    //Client Credential için gelen Token bilgisini handle edeceğim model
    public class ApplicationTokenDto
    {
        //Endpointlere istek yapacak olan token tipi
        public string AccessToken { get; set; }
        //Access Tokenin Expire Zamanı
        public DateTime AccessTokenExpireDate { get; set; }
    }
}
