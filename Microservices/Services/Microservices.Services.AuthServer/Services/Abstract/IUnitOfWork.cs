using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    public interface IUnitOfWork
    {
        //Asenkron metotlar için SaveChanges
        Task CommitAsync();

        //Asenkron olmayan metotlar için SaveChanges
        void Commit();
    }
}
