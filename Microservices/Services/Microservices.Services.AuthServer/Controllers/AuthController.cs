using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Models;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Shared.BaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        //Token işlemlerini yürütecek olan servisimiz
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }


        //domain.com/api/auth/createtoken
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto model)
        {
            //İşlem başarılıysa kullanıcılar için token oluşacaktır
            var result = await _authService.CreateTokenAsync(model);
            //Dönüş tiplerimizi BaseControllerdan alıyoruz kod tekrarı yapmamış oluyorz
            return CreateActionResultInstance(result);
        }

        //domain.com/api/auth/CreateTokenForClient
        [HttpPost]
        public IActionResult CreateTokenForClient(ApplicationLoginDto model)
        {
            //İşlem başarılıysa aplikasyonlar için token oluşacaktır
            var result = _authService.CreateTokenByApplication(model);
            //Dönüş tiplerimizi BaseControllerdan alıyoruz kod tekrarı yapmamış oluyorz
            return CreateActionResultInstance(result);
        }

        //domain.com/api/auth/CreateTokenByRefreshToken
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenModel refreshTokenModel)
        {
            //İşlem başarılıysa Refresh token aracılığıyla Access Token alacağız
            var result = await _authService.CreateTokenByRefreshToken(refreshTokenModel.RefreshToken);

            //Dönüş tiplerimizi BaseControllerdan alıyoruz kod tekrarı yapmamış oluyorz
            return CreateActionResultInstance(result);
        }

        //domain.com/api/auth/RevokeRefreshToken
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenModel refreshTokenModel)
        {
            //İşlem başarılıysa refresh token silinecektir
            var result = await _authService.RevokeRefreshToken(refreshTokenModel.RefreshToken);

            //Dönüş tiplerimizi BaseControllerdan alıyoruz kod tekrarı yapmamış oluyorz
            return CreateActionResultInstance(result);
        }

    }
}
