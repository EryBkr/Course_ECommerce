using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
