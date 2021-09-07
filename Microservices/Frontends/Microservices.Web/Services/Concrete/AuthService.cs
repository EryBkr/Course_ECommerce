using Microservices.Web.Models;
using Microservices.Web.Models.Auth;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    public class AuthService : IAuthService
    {
        //Session ve cookie işlemleri için ekledik
        private readonly IHttpContextAccessor _httpAccessor;

        //Get User İşlemi için ekledim
        private readonly IUserService _userService;

        //Rest istekleri için ekledik
        private readonly HttpClient _httpClient;

        //Base Url  (localhost:5001) Startup da tanımlı olduğu için burada option pattern e ihtiyaç duymadık
        public AuthService(IHttpContextAccessor httpAccessor, HttpClient httpClient, IUserService userService)
        {
            _httpAccessor = httpAccessor;
            _httpClient = httpClient;
            _userService = userService;
        }

        /// <summary>
        /// Login olma durumunda Access Token i ve Refresh Token i saklayacağız ardından Client tarafında Authentication işlemi için Cookie oluşturup kaydedeceğiz
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<UserDto>> Login(SignInInput model)
        {
            //Modelimizi Json a çeviriyoruz
            var postedJsonData = JsonConvert.SerializeObject(model);

            //Json a çevirdiğimiz datayı göndermeye hazır hale getiriyoruz
            var stringContent = new StringContent(postedJsonData, Encoding.UTF8, "application/json");

            //Oluşan json datayı Auth servis tarafında ki login end pointine gönderiyoruz
            var response = await _httpClient.PostAsync("/api/auth/createtoken", stringContent);

            //İstek başarılı ise
            if (response.IsSuccessStatusCode)
            {
                //Datayı handle ediyoruz
                var tokenResponse = JsonConvert.DeserializeObject<Response<TokenDto>>(await response.Content.ReadAsStringAsync());


                if (tokenResponse.IsSuccessful)  //Giriş Başarılı olmuş ise
                {
                    //Tekrar istek yapabilmek için token ları session a ekliyoruz
                    _httpAccessor.HttpContext.Session.SetString("access_token", tokenResponse.Data.AccessToken);
                    _httpAccessor.HttpContext.Session.SetString("refresh_token", tokenResponse.Data.RefreshToken);

                    //Tokenleri kaydedikten sonra Client içerisinde Cookie based auth için kullanıcı bilgilerini gerekli end pointten alıp cookiye ekleyeceğiz
                    var userResponse = await _userService.GetUserInfo();

                    //User Başarılı Gelmiş ise
                    if (userResponse.IsSuccessful && userResponse.Data != null)
                    {
                        //Giriş işlemi başarılı olmuş, kullanıcı bilgilerini Claims lere veriyoruz
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,userResponse.Data.Id),
                            new Claim(ClaimTypes.Name,userResponse.Data.UserName),
                            new Claim(ClaimTypes.Email,userResponse.Data.Email)
                        };


                        //COOKIE GİRİŞ İŞLEMLERİ
                        //Claimsleri ve Auth türünü oluşturup ClaimsIdentity e tanımladık
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Auth özelliklerini belirliyoruz
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.IsRemember //Kullanıcı beni hatırla derse cookie ayarlarında ki süre kadar cookie saklanacaktır
                        };
                        //Claims bilgileri,cookie şema bilgileri ve auth özelliklerinin belirlendiği bir login işlemi yapıldı
                        await _httpAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                        return userResponse;
                    }
                    else
                        return new Response<UserDto>
                        {
                            Errors = new List<string> { "Giriş İşlemi başarısız oldu" },
                            IsSuccessful = false
                        };
                }
                else  //Giriş başarısız olmuş ise
                    return new Response<UserDto>
                    {
                        Errors = new List<string> { "Giriş İşlemi başarısız oldu" },
                        IsSuccessful = false
                    };
            }

            //Server a bağlanırken hata aldık
            return new Response<UserDto>
            {
                Errors = new List<string> { "Server bağlantısında problem var" },
                IsSuccessful = false
            };

        }

        //çıkış işlemi
        public async Task<Response<NoContent>> LogOut()
        {
            //Session dan refresh token i alıyoruz
            var refreshToken = _httpAccessor.HttpContext.Session.GetString("refresh_token");

            //Return type oluşturduk
            var resultModel = new Response<NoContent>();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                //Refresh tokeni paketliyoruz
                RefreshTokenModel refreshTokenModel = new() { RefreshToken = refreshToken };

                //Refreh token modelimizi paketlemeye hazır hala getiriyoruz
                var postedJsonData = JsonConvert.SerializeObject(refreshTokenModel);
                var stringContent = new StringContent(postedJsonData, Encoding.UTF8, "application/json");

             

                //Refresh tokeni auth server da ki dbden silebilmek için istek yapıyoruz
                var response = await _httpClient.PostAsync("/api/auth/RevokeRefreshToken", stringContent);

                //isteğimiz başarılı ise gerekli işlemleri yapıyoruz
                if (response.IsSuccessStatusCode)
                {
                    //Token ları sessiondan da siliyoruz.
                    resultModel.IsSuccessful = true;

                    _httpAccessor.HttpContext.Session.Remove("refresh_token");
                    _httpAccessor.HttpContext.Session.Remove("access_token");
                }
                else
                {
                    //İşlem başarısız ise hata mesajını dönüyoruz
                    resultModel.IsSuccessful = false;
                    resultModel.Errors = new List<string> { "Log Out işlemi başarısız oldu" };
                }
            }
           

            //Cookie çıkış işleminide gerçekleştiriyoruz
            await _httpAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return resultModel;
        }

    
        /// <summary>
        /// Refresh Token sayesinde access token elde edeceğiz ve otomatik giriş yaptıracağız
        /// </summary>
        /// <returns></returns>
        public async Task<Response<TokenDto>> GetAccessByTokenByRefreshToken()
        {
            //Session dan refresh token i alıyoruz
            var refreshToken = _httpAccessor.HttpContext.Session.GetString("refresh_token");

            if (string.IsNullOrEmpty(refreshToken))
                return new Response<TokenDto> { IsSuccessful = false, Errors = new List<string> { "Refresh Token bulunamadı" } };

            //Refresh tokeni paketliyoruz
            RefreshTokenModel refreshTokenModel = new() { RefreshToken = refreshToken };

            //Refreh token modelimizi paketlemeye hazır hala getiriyoruz
            var postedJsonData = JsonConvert.SerializeObject(refreshTokenModel);
            var stringContent = new StringContent(postedJsonData, Encoding.UTF8, "application/json");

            //Refresh tokeni auth server a gönderip access token elde etmeye çalışıyoruz
            var response = await _httpClient.PostAsync("/api/auth/CreateTokenByRefreshToken", stringContent);

            //Auth Server ile iletişim esnasında bir problem olursa
            if (!response.IsSuccessStatusCode)
                return new Response<TokenDto> { IsSuccessful = false, Errors = new List<string> { "İstek esnasında bir hata oluştu" } };


            //İstek Başarılı ise Response<UserDto> Yanıtını alıyoruz 
            var tokensResponse = JsonConvert.DeserializeObject<Response<TokenDto>>(await response.Content.ReadAsStringAsync());

            if (!tokensResponse.IsSuccessful)
                return new Response<TokenDto> { IsSuccessful = false, Errors = new List<string> { "Üzgünüz bir hata ile karşılaştık" } };

            //Session daki token bilgilerini temizliyoruz
            _httpAccessor.HttpContext.Session.Remove("refresh_token");
            _httpAccessor.HttpContext.Session.Remove("access_token");

            //Tekrar istek yapabilmek için token ları session a ekliyoruz
            _httpAccessor.HttpContext.Session.SetString("access_token", tokensResponse.Data.AccessToken);
            _httpAccessor.HttpContext.Session.SetString("refresh_token", tokensResponse.Data.RefreshToken);

            //Tokenleri kaydedikten sonra Client içerisinde Cookie based auth için kullanıcı bilgilerini gerekli end pointten alıp cookiye ekleyeceğiz
            var userResponse = await _userService.GetUserInfo();

            //User Başarılı Gelmiş ise otomatik giriş yaptırabiliyoruz
            if (userResponse.IsSuccessful && userResponse.Data != null)
            {
                //Giriş işlemi başarılı olmuş, kullanıcı bilgilerini Claims lere veriyoruz
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,userResponse.Data.Id),
                            new Claim(ClaimTypes.Name,userResponse.Data.UserName),
                            new Claim(ClaimTypes.Email,userResponse.Data.Email)
                        };


                //COOKIE GİRİŞ İŞLEMLERİ
                //Claimsleri ve Auth türünü oluşturup ClaimsIdentity e tanımladık
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Auth özelliklerini belirliyoruz
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true //Kullanıcı beni hatırla derse cookie ayarlarında ki süre kadar cookie saklanacaktır
                };
                //Claims bilgileri,cookie şema bilgileri ve auth özelliklerinin belirlendiği bir login işlemi yapıldı
                await _httpAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                return tokensResponse;
            }
            else
                return new Response<TokenDto> { IsSuccessful = false, Errors = new List<string> { "Üzgünüz Refresh token ile giremedik" } };

        }

       
    }
}
