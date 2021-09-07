using Microservices.Web.Models;
using Microservices.Web.Models.Discount;
using Microservices.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Kod a göre kullanıcı ID si de alınarak bizlere code gelecek
        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"Discount/GetByUserIdAndCode/{discountCode}");

            if (!response.IsSuccessStatusCode)
                return null;

            //İndirim kodunu alıyoruz
            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

            return discount.Data;
        }
    }
}
