using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Entities
{
    //Identity Stores aracılığıyla User sınıfımı oluşturdum
    //Kullanıcı düzeyinde yetkilendirme modelim
    public class User:IdentityUser
    {
        //Extra Property
        public string City { get; set; }
    }
}
