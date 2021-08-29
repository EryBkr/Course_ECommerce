using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.Core
{
    //DDD içerisinde Yapılar kendi içlerinde ayrılır.Bu ayrımı yapmak için classlarımıızı imzalayacağız
    //Aggregate kavramı Tablolar arası ilişkilerde yapılacak temel CRUD işlemlerinde kullanılacak entityleri temsil etmektedir
    public interface IAggregateRoot
    {
    }
}
