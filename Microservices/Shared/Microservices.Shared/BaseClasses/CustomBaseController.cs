using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.BaseClasses
{
    //Dönüş tipleri için Base Controller oluşturuyorum
    //Classs Library projesinde ControllerBase i kullanabilmek için Ayarlarda düzenleme yapmam gerekti
    public class CustomBaseController : ControllerBase
    {
        //Status Code ile birlikte dönebilmek için oluşturduk.Kalıtım alındığında tüm Controller 'larda kullanabileceğim
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
