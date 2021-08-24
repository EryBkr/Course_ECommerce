using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    //Token İşlemleri için Interface
    public interface ITokenService
    {
        //Kullanıcıya özgü access ve refresh token oluşturacak
        TokenDto CreateToken(User user);

        //Apilerle iletişime geçecek olan aplikasyonlar için token oluşturacak
        ApplicationTokenDto CreateTokenByClient(ApplicationAccess applicationAccess);
    }
}
