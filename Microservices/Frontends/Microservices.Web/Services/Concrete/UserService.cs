using Microservices.Web.Models;
using Microservices.Web.Models.Auth;
using Microservices.Web.Services.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    public class UserService : IUserService
    {
        //Rest istekleri için ekledik
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //gerekli end point içerisinden kullanıcı bilgisini alıyoruz
        public async Task<Response<UserDto>> GetUserInfo()
        {
            //Header a Access Token bilgisi Startup ta tanımladığımız Handler aracılığıyla eklenecektir
            //Get isteğini yapıyoruz
            var response = await _httpClient.GetAsync("/api/user");

            //İstek başarılı ise kullanıcıyı oluşturup geri dönüyoruz
            if (response.IsSuccessStatusCode)
            {
                //İstek Başarılı ise Response<UserDto> Yanıtını alıyoruz 
                var userResponse = JsonConvert.DeserializeObject<Response<UserDto>>(await response.Content.ReadAsStringAsync());

                //Girişin başarılı olup olmadığı bilgisi bize auth server dan geliyor
                return userResponse;
            }

            return new Response<UserDto> { Errors = new List<string> { "İstek sırasında bir hata oluştu" }, IsSuccessful = false };
        }
    }
}
