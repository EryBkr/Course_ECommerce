using MassTransit;
using Microservices.Services.Catalog.AuthSettings;
using Microservices.Services.Catalog.Services.Abstract;
using Microservices.Services.Catalog.Services.Concrete;
using Microservices.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog
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
            //AutoMapper Eklendi
            //Startup a ba�l� Profile � miras alan classlar� ekledik
            services.AddAutoMapper(typeof(Startup));

            //Servislerimi DI ekliyorum
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();

            //B�t�n Controller a Authorize attribute 'ni eklemi� oldum
            services.AddControllers(opt=> 
            {
                opt.Filters.Add(new AuthorizeFilter());
            });

            //appsettings ayarlar�m� modele bind ediyorum
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini ald�m
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //Yap�c� metotlarda art�k IOptions tan�mlamayla u�ra�mama gerek yok interface �zerinden de�erlere direkt ula�abilece�im (Geli�mi� option pattern)
            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });
            services.AddSingleton<TokenOptions>(sp =>
            {
                return sp.GetRequiredService<IOptions<TokenOptions>>().Value;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.Catalog", Version = "v1" });
            });

            //RabbitMQ ile haberle�ebilmek i�in ekledik
            services.AddMassTransit(x =>
            {
                //ServiceBus ayarlar�n� ger�ekle�tiriyoruz
                x.UsingRabbitMq((context, cfg) =>
                {
                    //Default olara 5672 portundan aya�a kalkt��� i�in belirtmedik
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });

            //Configurasyonlar� uygulad�k
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.Catalog v1"));
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
