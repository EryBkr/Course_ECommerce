using Microservices.Services.AuthServer.Dtos;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Abstract
{
    public interface IAuthenticationService
    {
        //User lara  token ları  dönecek metot
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

        //Gönderdiğimiz Refresh Token aracılığıyla bizlere access token dönecek metot
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        //Refresh token i sil
        Task<Response<NoContent>> RevokeRefreshToken(string refreshToken);

        //Aplikasyonların apilere iletişimi için
        Response<ApplicationTokenDto> CreateTokenByApplication(ApplicationLoginDto applicationLoginDto);
    }
}
