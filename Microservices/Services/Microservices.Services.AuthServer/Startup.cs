using Microservices.Services.AuthServer.DatabaseConfigurations;
using Microservices.Services.AuthServer.Entities;
using Microservices.Services.AuthServer.Models;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Services.AuthServer.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.AuthServer", Version = "v1" });
            });

            //Automapper Eklendi
            services.AddAutoMapper(typeof(Startup));//Dependency Injection kullanabilmek i�in tan�mlad�k

            //Options Pattern
            //Token ayarlar�n� DI ile modele bind edecek ayarlar�m� olu�turdum
            services.Configure<CustomTokenOptions>(Configuration.GetSection("TokenOptions"));
            services.Configure<List<ApplicationAccess>>(Configuration.GetSection("Applications"));

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini ald�m
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();


            //Scoped Ayn� Requestte bir defa olu�ur
            //Transiet her seferinde yeni bir instance olu�turur
            //Singleton ya�am d�ng�s� boyunca ayn� instance ile �al���r
            //Context s�n�f�m� ekledim
            //DI i�lemlerini hallettim
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            //Generic halleriyle entity den ba��ms�z Inject Ettim
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //iki adet Generic Class ald���m�z i�in virg�lle belirttik
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Identity �zelliklerini de belirledim
            services.AddIdentity<User, IdentityRole>(Opt =>
            {
                Opt.User.RequireUniqueEmail = true;
                Opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            //JWT Bearer � ekliyorum
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //�ema ismi belirlendi
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                //Jwt �zelliklerini belirliyorum
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Key i verdik
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],//Buna istek yapabilmesi i�in buna ait auidence bilgisinin tokene verilmi� olmas� gerekir.Sadece buna ait olan audience bilgisinin verilmi� olmas� yeterli oldugu i�in diger audience bilgilerini vermedik hepsinide ekleyebilirdik
                    ValidateIssuerSigningKey = true,//Imza dogrulans�n m�
                    ValidateAudience = true,//Audience kontrol edilsin mi
                    ValidateIssuer = true,//Issuer  kontrol edilsin mi
                    ValidateLifetime = true,//Yasam �mr� de kontrol edilsin
                    ClockSkew = TimeSpan.Zero //Sunucu Time Zone farkl�l���ndan dolay� default olarak 5 dk ekliyor son zamana.Bunu iptal ediyoruz
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.AuthServer v1"));
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
