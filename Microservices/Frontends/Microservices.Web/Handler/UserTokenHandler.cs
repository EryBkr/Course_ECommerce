using Microservices.Web.Exceptions;
using Microservices.Web.Models;
using Microservices.Web.Models.Auth;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Web.Handler
{
    //Kullanıcı isteklerini kontrol edecek
    //HTTP isteklerinde araya girerek otomatik olarak Access Token ve Refresh Token isteklerini kontrol ederek ekleyecek
    public class UserTokenHandler:DelegatingHandler //HTTP İsteklerinde araya girmemizi sağlayacak
    {
        //Session ve cookie işlemleri için ekledik
        private readonly IHttpContextAccessor _httpAccessor;

        //Http istekleri atabilmek için ekledik
        private readonly HttpClient _httpClient;

        public UserTokenHandler(IHttpContextAccessor httpAccessor, HttpClient httpClient)
        {
            _httpAccessor = httpAccessor;
            _httpClient = httpClient;
        }

        //Her istek başlatıldığında araya girecek
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Sessiondan token bilgisini alıyoruz
            var accessToken = _httpAccessor.HttpContext.Session.GetString("access_token");

            //İsteğin Header ına access token i ekliyoruz
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //İsteği gönderiyorum
            var response = await base.SendAsync(request, cancellationToken);

            //İstek yetkisiz ise (401 ya da 403)
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                //Refresh Token aracılığıyla access token alıyorum
                var tokenResponse = await GetAccessByTokenByRefreshToken();

                //Refresh token ile birlikte başarılı bir şekilde token alabilmiş isek
                if (tokenResponse.IsSuccessful)
                {
                    //İsteğin Header ına yeni aldığımız access token i ekliyoruz
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Data.AccessToken);

                    //Tekrar istek atıyorum
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            //Yine 401 ya da 403 aldıysak (yeni response için kontrol ediyorum)
            if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                //Kendi hata sınıfımızı fırlatıyoruz
                throw new UnAuthorizeException();
            }
            return response;
        }

        //Access Token ile giriş işlemi başarısız ise Refresh Token ile Access Token almayı deniyoruz
        private async Task<Response<TokenDto>> GetAccessByTokenByRefreshToken()
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

            return tokensResponse;
        }
    }
}
