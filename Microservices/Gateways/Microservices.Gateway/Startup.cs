using Microservices.Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Gateway özelliði için Ocelot kullandýk
            services.AddOcelot();

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini aldým
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication(); //Kimlik
            app.UseAuthorization(); //Yetkilendirme

            //Ocelot u kullanacaðýmýzý belirtiyoruz
            await app.UseOcelot();
        }
    }
}
