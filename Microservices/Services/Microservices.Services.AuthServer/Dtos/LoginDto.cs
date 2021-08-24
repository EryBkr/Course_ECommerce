using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Dtos
{
    //Kullanıcı Login İşlemi için model
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
