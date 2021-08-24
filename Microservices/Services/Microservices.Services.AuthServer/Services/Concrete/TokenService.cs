using Microservices.Services.AuthServer.Dtos;
using Microservices.Services.AuthServer.Entities;
using Microservices.Services.AuthServer.Models;
using Microservices.Services.AuthServer.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microservices.Services.AuthServer.Services.Concrete
{
    //Access Token ve RefreshToken oluşturacak Class
    public class TokenService : ITokenService
    {
        //Token Bilgilerini tuttuğum sınıfım
        private readonly CustomTokenOptions _tokenOptions;

        //IOptions sayesinde appsettings içerisinde ki datalarım modele bind edilecektir
        public TokenService(IOptions<CustomTokenOptions> options)
        {
            _tokenOptions = options.Value;
        }

        //Unique String bir ifade oluşturdum.Guid e nazaran daha Unique olma ihtimali fazladır
        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var random = RandomNumberGenerator.Create();

            random.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        //Payload için Claimler oluşturuldu
        private IEnumerable<Claim> GetClaims(User user, List<string> audiences)
        {
            //Token a eklenecek user claim bilgilerini oluşturuyorum 
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                //Token a random id veriyoruz belki lazım olur
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Claim listeme geçerli olan Audience bilgilerini tek tek ekledim
            //kimin için imzalanıyor, hangi apilere istek atabiliriz
            claimList.AddRange(audiences.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

            return claimList;
        }

        //Uygulamalar için Claim nesneleri oluşturuyorum
        private IEnumerable<Claim> GetClaimsByClient(ApplicationAccess applicationAccess)
        {
            //Token a eklenecek client(aplikasyon) claim bilgilerini oluşturuyorum 
            var claimList = new List<Claim>
            {
                //Bu Token kimin için oluşturuluyor
                new Claim(JwtRegisteredClaimNames.Sub,applicationAccess.Id.ToString()),
                //Token a random id veriyoruz belki lazım olur
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Claim listeme geçerli olan Audience bilgilerini tek tek ekledim
            //kimin için imzalanıyor, hangi apilere istek atabiliriz
            claimList.AddRange(applicationAccess.Audiences.Select(i => new Claim(JwtRegisteredClaimNames.Aud, i)));

            return claimList;
        }

        //Kullanıcı için oluşturduk
        public TokenDto CreateToken(User user)
        {
            //Access Token Yaşam süresi
            var accessTokenExpireTime = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            //Token i imzalayacak Key geldi
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            //Token i şifreleyecek key hazır
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    issuer: _tokenOptions.Issuer, //Kim imzalıyor
                    expires: accessTokenExpireTime, //Sona erme zamanı
                    notBefore: DateTime.Now, //Başlangıç zamanı
                    claims: GetClaims(user, _tokenOptions.Audience), //kimin için imzalanıyor, hangi apilere istek atabiliriz
                    signingCredentials: signingCredentials //imza
                );

            //Token oluşturuldu
            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(jwtSecurityToken);

            //Dönüş tipimizi oluşturuyorum
            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpireDate = accessTokenExpireTime,
                RefreshTokenExpireDate = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration)
            };

            return tokenDto;

        }

        //Aplikasyonlar için oluşturduk
        public ApplicationTokenDto CreateTokenByClient(ApplicationAccess applicationAccess)
        {
            //Access Token Yaşam süresi
            var accessTokenExpireTime = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            //Token i imzalayacak Key geldi
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            //Token i şifreleyecek key hazır
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    issuer: _tokenOptions.Issuer, //Kim imzalıyor
                    expires: accessTokenExpireTime, //Sona erme zamanı
                    notBefore: DateTime.Now, //Başlangıç zamanı
                    claims: GetClaimsByClient(applicationAccess), //kimin için imzalanıyor, hangi apilere istek atabiliriz
                    signingCredentials: signingCredentials //imza
                );

            //Token oluşturuldu
            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(jwtSecurityToken);

            //Dönüş tipimizi oluşturuyorum
            var applicationTokenDto = new ApplicationTokenDto
            {
                AccessToken = accessToken,
                AccessTokenExpireDate = accessTokenExpireTime,
            };

            return applicationTokenDto;
        }
    }
}
