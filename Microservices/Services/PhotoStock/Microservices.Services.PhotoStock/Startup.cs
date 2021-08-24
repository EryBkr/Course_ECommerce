using Microservices.Services.PhotoStock.AuthSettings;
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

namespace Microservices.Services.PhotoStock
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

            //OptionPattern i kullanarak appsettings.json daki tokenOptions bilgilerini aldým
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //Yapýcý metotlarda artýk IOptions tanýmlamayla uðraþmama gerek yok interface üzerinden deðerlere direkt ulaþabileceðim (Geliþmiþ option pattern)
            services.AddSingleton<TokenOptions>(sp =>
            {
                return sp.GetRequiredService<IOptions<TokenOptions>>().Value;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices.Services.PhotoStock", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.PhotoStock v1"));
            }

            app.UseRouting();

            //Resim deposu olarak kullanacaðým için ekledim.Resimlerin dýþarýsý açýlmasý gerekiyor
            app.UseStaticFiles();

            app.UseAuthentication(); //Kimlik
            app.UseAuthorization(); //Yetkilendirme

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
