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
            //B�t�n Controller a Authorize attribute 'ni eklemi� oldum
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
            });

            //JWT olu�tururken baz� alanlara farkl� key de�eri versek bile kendi ClaimType'na uygun olarak de�i�iklik yapabiliyor.Bunu �nlemek i�in eklenebilir
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //Option Pattern
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini ald�m
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //HTTP Context �zerinden Token a eri�ebilmek i�in ekledim
            services.AddHttpContextAccessor();

            //Bizim i�in kullan�c�n�n Id de�erini alacak (Shared i�erisinden ald�k)
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

            //Redis i�lemleri i�in ekledik
            services.AddScoped<IBasketService, BasketService>();

            //appsettings i�erisinden ald���m�z datalar� connection s�n�f�m�za veriyoruz
            //Otomatik olarak ba�lant�y� bizim i�in olu�turacakt�r
            services.AddSingleton<RedisConService>(sp=> 
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                //Yap�c�ya gerekli parametreleri veriyorum
                var redis = new RedisConService(redisSettings.Host,redisSettings.Port);

                //Ba�lant�y� sa�l�yorum
                redis.Connect();

                //Geriye bekleneni d�n�yorum
                return redis;
            });

            //JWT Bearer � ekliyorum
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //�ema �smi belirlendi
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //JWT �emas� ile ba�lad�m.Onu a�a��da tan�mlad�m
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                //Jwt �zelliklerini belirliyorum
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Key i verdik
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,//Buna istek yapabilmesi i�in buna ait auidence bilgisinin tokene verilmi� olmas� gerekir.
                    ValidateIssuerSigningKey = true,//�mza do�rulanmal�
                    ValidateAudience = true,//Audience kontrol edilsin
                    ValidateIssuer = true,//Issuer  i de do�rula
                    ValidateLifetime = true,//Ya�am �mr� de kontrol edilsin
                    ClockSkew = TimeSpan.Zero //Sunucu Time Zone farkl�l���ndan dolay� default olarak 5 dk ekliyor son zamana.Bunu iptal ediyoruz
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
