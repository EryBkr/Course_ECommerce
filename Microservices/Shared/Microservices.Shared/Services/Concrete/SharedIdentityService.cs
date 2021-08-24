using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Services.Concrete
{
    //Token içerisinden Id değerini bizim için alacak sınıfımız
    public class SharedIdentityService : ISharedIdentityService
    {
        //Token a erişebilmek için aldık
        private IHttpContextAccessor _httpContext;

        public SharedIdentityService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        //Token içerisinden kullanıcı Id bilgisine ulaştık
        public string GetUserId => _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
