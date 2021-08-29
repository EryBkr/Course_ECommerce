using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Mapping
{
    //DI kullanmadan Mapper kullanabilmek için ekledik
    public static class ObjectMapper
    {
        //Lazy kullanında sadece çağrıldığı zaman instance oluşur
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<CustomMapping>(); });

            return config.CreateMapper();
        });

        //Çağrıldığı zaman lazy devreye girecektir
        public static IMapper Mapper => lazy.Value;
    }
}
