using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Auth
{
    //Client Credentials SignIn  için Server a göndereceğim model oluşturdum
    public class ApplicationInInput
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
    }
}
