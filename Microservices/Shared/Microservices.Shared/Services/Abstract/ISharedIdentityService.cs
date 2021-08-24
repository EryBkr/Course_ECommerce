using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Services.Abstract
{
    //Token içerisinden Id değerini bizim için alacak soyut yapı
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}
