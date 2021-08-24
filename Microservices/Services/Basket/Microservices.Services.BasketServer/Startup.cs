using Microservices.Services.BasketServer.Services.Abstract;
using Microservices.Services.BasketServer.Services.Concrete;
using Microservices.Services.BasketServer.Services.Connection;
using Microservices.Services.BasketServer.Settings;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer
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
            //Bütün Controller a Authorize attribute 'ni eklemiþ oldum
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
            });

            //JWT oluþtururken bazý alanlara farklý key deðeri versek bile kendi ClaimType'na uygun olarak deðiþiklik yapabiliyor.Bunu önlemek için eklenebilir
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //Option Pattern
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini aldým
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //HTTP Context üzerinden Token a eriþebilmek için ekledim
            services.AddHttpContextAccessor();

            //Bizim için kullanýcýnýn Id deðerini alacak (Shared içerisinden aldýk)
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

            //Redis iþlemleri için ekledik
            services.AddScoped<IBasketService, BasketService>();

            //appsettings içerisinden aldýðýmýz datalarý connection sýnýfýmýza veriyoruz
            //Otomatik olarak baðlantýyý bizim için oluþturacaktýr
            services.AddSingleton<RedisConService>(sp=> 
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                //Yapýcýya gerekli parametreleri veriyorum
                var redis = new RedisConService(redisSettings.Host,redisSettings.Port);

                //Baðlantýyý saðlýyorum
                redis.Connect();

                //Geriye bekleneni dönüyorum
                return redis;
            });

            //JWT Bearer ý ekliyorum
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Þema Ýsmi belirlendi
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //JWT þemasý ile baðladým.Onu aþaðýda tanýmladým
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                //Jwt özelliklerini belirliyorum
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Key i verdik
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,//Buna istek yapabilmesi için buna ait auidence bilgisinin tokene verilmiþ olmasý gerekir.
                    ValidateIssuerSigningKey = true,//Ýmza doðrulanmalý
                    ValidateAudience = true,//Audience kontrol edilsin
                    ValidateIssuer = true,//Issuer  i de doðrula
                    ValidateLifetime = true,//Yaþam ömrü de kontrol edilsin
                    ClockSkew = TimeSpan.Zero //Sunucu Time Zone farklýlýðýndan dolayý default olarak 5 dk ekliyor son zamana.Bunu iptal ediyoruz
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.BasketServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.BasketServer v1"));
            }

            app.UseRouting();

            app.UseAuthentication(); //Kimlik
            app.UseAuthorization(); //Yetkilendirme

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
