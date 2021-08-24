using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Entities;
using Microservices.Services.AuthServer.Services.Abstract;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Concrete
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<ApplicationAccess> _applications;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _uOw;
        private readonly IGenericRepository<UserRefreshToken> _genericService;

        public AuthenticationService(IOptions<List<ApplicationAccess>> applications, ITokenService tokenService, UserManager<User> userManager, IUnitOfWork uOw, IGenericRepository<UserRefreshToken> genericService)
        {
            _applications = applications.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _uOw = uOw;
            _genericService = genericService;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentException(nameof(loginDto));

            //Email e ait kayıt var mı
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Response<TokenDto>.Fail("Email veya Password Yanlış", 400);

            //Şifre yanlış ise
            if (!(await _userManager.CheckPasswordAsync(user, loginDto.Password)))
                return Response<TokenDto>.Fail("Email veya Password Yanlış", 400);

            //Tokeni oluşturduk
            var token = _tokenService.CreateToken(user);

            //Refresh token var mı
            var userRefreshToken = await _genericService.Where(i => i.UserId == user.Id).SingleOrDefaultAsync();

            //Yoksa oluşturuyorum
            if (userRefreshToken == null)
            {
                await _genericService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, ExpireDate = token.RefreshTokenExpireDate });
            }
            else
            {
                //Varsa güncelliyorum
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.ExpireDate = token.RefreshTokenExpireDate;
            }

            await _uOw.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ApplicationTokenDto> CreateTokenByApplication(ApplicationLoginDto applicationLoginDto)
        {
            if (applicationLoginDto == null) throw new ArgumentException(nameof(applicationLoginDto));

            //appsettings e kaydettiğimiz Aplikasyonların içerisinden bize istek yapan uygulamayı alıyoruz ve ona özel token oluşturuyoruz
            var application = _applications.SingleOrDefault(x => x.Id == applicationLoginDto.ApplicationId && x.Secret == applicationLoginDto.ApplicationSecret);

            if (application == null) return Response<ApplicationTokenDto>.Fail("Application Not Found", 400);

            var token = _tokenService.CreateTokenByClient(application);

            return Response<ApplicationTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            //Database de bana ait refresh token var mı?
            var myRefreshToken = await _genericService.Where(i => i.Code == refreshToken).SingleOrDefaultAsync();

            if (myRefreshToken == null) return Response<TokenDto>.Fail("Refresh token  not found", 404);

            //Refresh token tablomuzda ki o tokene ait kullanıcıyı aldık
            var user = await _userManager.FindByIdAsync(myRefreshToken.UserId);

            if (user == null) return Response<TokenDto>.Fail("User  not found", 404);

            //Kullanıcıya ait token i oluşturuyorum.Refresh token gönderdiği için giriş yapmasına gerek kalmadı 
            var token = _tokenService.CreateToken(user);
            myRefreshToken.Code = token.RefreshToken;
            myRefreshToken.ExpireDate = token.RefreshTokenExpireDate;

            await _uOw.CommitAsync();

            return Response<TokenDto>.Success(token, 200);
        }

        //Refresh token silinmek istenirse
        public async Task<Response<NoContent>> RevokeRefreshToken(string refreshToken)
        {
            //Db de refresh token var mı
            var myRefreshToken = await _genericService.Where(i => i.Code == refreshToken).SingleOrDefaultAsync();

            if (myRefreshToken == null) Response<NoContent>.Fail("Refresh Token Yok", 404);

            //Refreh tokeni db den siliyorum
            _genericService.Remove(myRefreshToken);

            await _uOw.CommitAsync();

            return Response<NoContent>.Success(200);
        }
    }
}
