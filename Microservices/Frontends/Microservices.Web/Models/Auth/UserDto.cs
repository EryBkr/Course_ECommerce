using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Auth
{
    //login işleminden sonra kullanıcı bilgilerini alacağımız model
    public class UserDto
    {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string City { get; set; }   
    }
}
