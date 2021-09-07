using Microservices.Web.Models;
using Microservices.Web.Models.Auth;
using Microservices.Web.Services.Abstract;
using Microservices.Web.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    //Uygulamalar için oluşturulan token mekanizmasına istek yapacak sınıfımız
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        //Mikroservis Uri bilgilerine ulaşabilmek için ekledim
        private readonly ServiceApiSettings _serviceApiSettings;

        //Auth Server dan auth bilgilerini appsettings ten alabilmek için ekledim 
        private readonly ClientSettings _clientSettings;

        //HTTP istekleri için ekliyorum
        private readonly HttpClient _httpClient;

        //Cache kullanımı için ekledik
        private readonly IMemoryCache _memoryCache;


        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _memoryCache = memoryCache;
            _httpClient = httpClient;

            //Base Address i burada eziyorum
            _httpClient.BaseAddress = new Uri(_serviceApiSettings.AuthUri);
        }

        //In Memory de olan Client Token i bize dönderecek metodumuz
        public async Task<string> GetToken()
        {
            //Memory den Client Token bilgisine erişiyoruz
            var clientToken = _memoryCache.Get<string>("client-token");

            //Client a ait bilgi var ise geri dönüyoruz
            if (!string.IsNullOrEmpty(clientToken))
            {
                return clientToken;
            }
            else //Client Credential Token Mevcut Değilse 
            {
                //Gönderilecek Model
                var sendedModel = new ApplicationInInput { ApplicationId = _clientSettings.Id, ApplicationSecret = _clientSettings.Secret };
                var response = await _httpClient.PostAsJsonAsync<ApplicationInInput>("/api/auth/CreateTokenForClient", sendedModel);

                if (response.IsSuccessStatusCode)
                {
                    var clientTokenResponse = JsonConvert.DeserializeObject<Response<ApplicationTokenDto>>(await response.Content.ReadAsStringAsync());

                    //Memory ayarlarını tanımlayacağımız sınıfımız
                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                    //Oluşturulan Cache nin ne kadar süre sonra sona ereceğini kararlaştıran yapıdır
                    options.AbsoluteExpiration = DateTime.Now.AddMinutes(4);


                    //20 dk içerisinde data ya ulaşırsak sayaç sıfırlanacaktır.
                    //20 dk içinde erişmezsek data silinecektir.
                    //Absolute ile kullanılması tavsiye edilir.
                    options.SlidingExpiration = TimeSpan.FromMinutes(4);

                    //Dataları önem sırasına göre etiketleyebiliyoruz.
                    //Net Core bellek taşmasını önlemek için silme işlemi yapacağı dataya buraya göre karar verecektir.Mümkün mertebe NeverRemove kullanmamaya çalışmak bizler için iyi olur.
                    options.Priority = CacheItemPriority.High;

                    //Bizden 4 parametreli delege bekliyor,metodu bu şekilde de ekleyebilirim
                    //Cache in neden silindiği bilgisine ulaşmamızı sağlar
                    options.RegisterPostEvictionCallback((key, value, reason, state) =>
                    {
                        _memoryCache.Set<string>("callback", $"{key}-{value}-{reason}-{state}");
                    });

                    //Generic olarak içerisinde tutacağımız datayı belirliyoruz ve ona erişebilmek için key ataması yapıyoruz.
                    //Cache te data tutmak demek sunucunun ram belleğinde data tutmak demektir.
                    //Cache süresini options olarak belirtip parametre olarak tanımladık
                    //Var olan aynı key ile data set edersek üzerine yazacaktır
                    _memoryCache.Set<string>("client-token", clientTokenResponse.Data.AccessToken, options);

                    return clientTokenResponse.Data.AccessToken;
                }

                else
                    return null;
            }
        }
    }
}
