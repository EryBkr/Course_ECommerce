using Microservices.Services.AuthServer.DatabaseConfigurations;
using Microservices.Services.AuthServer.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //Auto Migration Ýþlemi
            using (var scope=host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var applicationDbContext = serviceProvider.GetRequiredService<AppDbContext>();

                //Veritabaný yok ise oluþturacak.Yapýlmamýþ Migration varsa uygulayacak
                applicationDbContext.Database.Migrate();

                //Usermanager ile kullanýcý ekleyeceðim
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                //Kullanýcý yok ise
                if (!userManager.Users.Any())
                {
                    //Uygulama ayaða kalkýtýðýnda otomatik data eklemiþ olduk
                    userManager.CreateAsync(new User { UserName = "Blackerback", Email = "crazyeray94@gmail.com", City = "Istanbul" }, "Password123*").Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
