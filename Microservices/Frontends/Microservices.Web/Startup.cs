using FluentValidation.AspNetCore;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microservices.Web.Extensions;
using Microservices.Web.Handler;
using Microservices.Web.Helpers;
using Microservices.Web.Services.Abstract;
using Microservices.Web.Services.Concrete;
using Microservices.Web.Settings;
using Microservices.Web.Validations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Fluent Validation Eklendi
            services.AddControllersWithViews().AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());

            //In memory cache kullanımı için ekledik
            services.AddMemoryCache();


            //Session u ekledik
            services.AddSession();

            //Token i session a controller dışında ki bir classtan atayabilmek için ekledik
            services.AddHttpContextAccessor();


            //Auth Cookie Ekledik (Client uygulamamız içerisinde Auth mekanizmasını daha efektif kullanabileceğiz)
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.HttpOnly = true;//Cookie erisimine tarayıcı üzerinden erisilmesin diye bu sekilde tanimladik
                opt.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Auth/SignIn");//Giriş yapılmamış ise logine yönlendiriyoruz
                opt.Cookie.Name = "MicroserviceCookie";
                opt.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;//Cookie kullanımı diğer web sayfaları için kapalı
                opt.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;//HTTP ya da HTTPS ler kendi arasında iletisim kurar
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(50); //Cookie yasam süresi (Refresh Tokendan daha az süre verildi)
                opt.SlidingExpiration = false; //Tekrar dan giriş yaparsak cookie süresi uzasın mı
            });

            //Bizim için kullanıcının Id değerini alacak (Shared içerisinden aldık)
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

            //Fotoğraf Url leri oluşturacak Helper
            services.AddSingleton<PhotoHelper>();

            //HTTP DI larını Extension Class içerisinde tanımladık
            services.AddHttpClientServices(Configuration);

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Hata aldığımız zaman buraya gönderiyoruz.Burada da gerekli hata sınıflarına göre yönlendirme yapabiliriz
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            //Session u kullanıma hazır hale getirdik
            app.UseSession();

            app.UseAuthentication(); //Üyelik Sistemi
            app.UseAuthorization(); //Yetkilendirme Sistemi

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
