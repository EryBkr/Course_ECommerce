using Microservices.Web.Exceptions;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Filters
{
    //Uygulama kısmında hata oluştuğu zaman devreye girecek merkezi yapıdır
    public class UserTokenHandlerExceptionFilter : IExceptionFilter
    {
        //UnAuth hatası alındığında çıkış işlemini gerçekleştirebilmek için ekledik
        private readonly IAuthService _authService;

        public UserTokenHandlerExceptionFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public async void OnException(ExceptionContext context)
        {
            //Hata kontrolünü biz devralıyoruz
            context.ExceptionHandled = true;


            //Bizlere gelen exception türü UnAuthorizeException ise
            switch (context.Exception)
            {
                //Token süresi dolmuş ise alacağımız hata aslında
                case UnAuthorizeException:
                    //Token süresi dolmuş ise çıkış işlemi yaptırıyoruz
                    await _authService.LogOut();
                    //Çıkış işleminden sonra LogOut işlemi yaparak kullanıcıyı login sayfasına gönderiyoruz
                    context.HttpContext.Response.Redirect("/Auth/LogOut");
                    break;
                default:
                    break;
            }
        }
    }
}
