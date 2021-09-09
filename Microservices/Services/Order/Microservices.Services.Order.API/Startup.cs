using MassTransit;
using MassTransit.MultiBus;
using MediatR;
using Microservices.Services.Order.API.Settings;
using Microservices.Services.Order.Application.Subscriber;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Microservices.Services.Order.API
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

            //Handler s�n�flar�m�zdan birinin Assembly sini almam yeterli.Solution �zelinde kontrol ediyor zaten
            services.AddMediatR(typeof(Application.Handlers.CreateOrderCommandHandler).Assembly);



            //DbContext Yolumu veriyorum
            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    //DbContext yolumu belirledim
                    configure.MigrationsAssembly("Microservices.Services.Order.Infrastructure");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.Order.API", Version = "v1" });
            });

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini ald�m
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //HTTP Context �zerinden Token a eri�ebilmek i�in ekledim
            services.AddHttpContextAccessor();

            //Bizim i�in kullan�c�n�n Id de�erini alacak (Shared i�erisinden ald�k)
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();

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

            //RabbitMQ ile haberle�ebilmek i�in ekledik
            services.AddMassTransit(x =>
            {
                //Gelen mesaj� dinleyecek olan Subscriber �m�z� burada belirledik
                x.AddConsumer<CreateOrderMessageCommandSubscriber>();

                //Catalog taraf�ndan g�nderilen Event i dinleyecek s�n�f�m
                x.AddConsumer<CourseNameChangedEventSubscriber>();

                //ServiceBus ayarlar�n� ger�ekle�tiriyoruz
                x.UsingRabbitMq((context, cfg) =>
                {
                    //Default olara 5672 portundan aya�a kalkt��� i�in belirtmedik
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    //Bu Subscriber hangi kuyru�u dinleyecek
                    cfg.ReceiveEndpoint("create-order", e => 
                    {
                        e.ConfigureConsumer<CreateOrderMessageCommandSubscriber>(context);
                    });


                    //Bu Subscriber belirli bir kuyru�u dinlemeyecek.Catalog servisinde masstransit in olu�turdu�u exchange yi kendimizin burada olu�turdu�u kuyruk ile dinleyecek.Sonu�ta burada bir event i�lemi yap�l�yor.MassTransit in hangi Exchange yi olu�turaca�� tipe g�re karar veriliyor
                    cfg.ReceiveEndpoint("course-name-changed-event-order", e =>
                    {
                        e.ConfigureConsumer<CourseNameChangedEventSubscriber>(context);
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.Order.API v1"));
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
