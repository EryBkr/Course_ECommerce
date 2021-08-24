using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Entities
{
    //Bizlere Refresh Token Dönecek Modelim
    public class UserRefreshToken
    {
        [Key]
        public string UserId { get; set; }
        public string Code { get; set; } //RefreshToken
        public DateTime ExpireDate { get; set; }
    }
}
