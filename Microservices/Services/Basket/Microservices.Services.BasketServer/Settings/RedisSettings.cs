using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer.Settings
{
    //Redis bağlantı ayarları
    public class RedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
