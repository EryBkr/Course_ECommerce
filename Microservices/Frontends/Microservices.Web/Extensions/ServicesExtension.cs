using Microservices.Web.Handler;
using Microservices.Web.Services.Abstract;
using Microservices.Web.Services.Concrete;
using Microservices.Web.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Extensions
{
    //Startup çok dolu olmasın diye servis DI larını burada tanımlıyorum
    public static class ServicesExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services,IConfiguration Configuration )
        {
            //Uri bilgilerinin ServiceApiSettings modelim ile kullanacağım (Option Pattern)
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));

            //Uygulama Giriş Kimlik Bilgilerini modelime bind ediyorum (Option Pattern)
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            //OptionPattern i kullanarak appsettings.json daki api uri bilgilerini aldım
            var apiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            //Handle ımızı service olarak ekledik.HTTP isteklerinde token yönetimini sağlayacak
            services.AddScoped<UserTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();


            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            //Client Token işlemlerinden sorumlu servis (Client==Application)
            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IAuthService, AuthService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                opt.BaseAddress = new Uri(apiSettings.AuthUri);
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;


            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                opt.BaseAddress = new Uri(apiSettings.AuthUri);
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;;

            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;

            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IPhotoService, PhotoService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;


            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.Basket.Path}");
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;


            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.Discount.Path}");
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;

            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.Payment.Path}");
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;

            //Hem DI ile interface i doldurduk hemde HttpClient nesnemizi ayrı olarak belirtmemize gerek kalmadı
            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                //Sürekli Base Url i girmemek için burada tanımladım
                //BaseUrl api gateway i temsil ediyor
                opt.BaseAddress = new Uri($"{apiSettings.BaseUri}/{apiSettings.Order.Path}");
            }).AddHttpMessageHandler<UserTokenHandler>(); //Token bilgisini her HTTP isteğinde header a ekleyecek şekilde ayarladık;
        }
    }
}
