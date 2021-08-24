using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer.Services.Connection
{
    //Redis bağlantısını üstlenen sınıfımız
    public class RedisConService
    {
        private readonly string _host;
        private readonly int _port;

        private ConnectionMultiplexer _connectionMultiplexer;

        public RedisConService(string host,int port)
        {
            _host = host;
            _port = port;
        }

        //Redis bağlantısını sağladık
        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        //Rediste birden çok Database vardır.Hangisine bağlanacağımızı seçiyoruz
        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);
    }
}
