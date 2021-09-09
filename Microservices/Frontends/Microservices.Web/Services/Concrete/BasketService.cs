using Microservices.Web.Models;
using Microservices.Web.Models.Basket;
using Microservices.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    public class BasketService : IBasketService
    {
        //Servisimize Http istekleri yapabilmek için ekledik
        private readonly HttpClient _httpClient;

        //İndirim kuponu işlemleri için ekledik
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel model)
        {
            //Servisten sepeti alıyorum
            var basket = await Get();

          
            if (basket != null)
            {
                //Ekleyeceğimiz ürün sepette mevcut mu?
                if (!basket.Data.BasketItems.Any(x => x.CourseId == model.CourseId))
                {
                    basket.Data.BasketItems.Add(model);
                }
            }
            else
            {
                //Henüz sepet oluşmamış ise yeni bir sepet oluşturuyorum
                basket.Data = new BasketViewModel();
                basket.Data.BasketItems.Add(model);
            }


            await SaveOrUpdate(basket.Data);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            //Öncelikle mevcut kupon kodunu iptal ediyoruz
            await CancelApplyDiscount();
            var basket = await Get();

            if (basket.Data==null)
                return false;

            //Kullanıcıya ait verilen kod mevcut mu
            var hasDiscount = await _discountService.GetDiscount(discountCode);

            if (hasDiscount == null)
                return false;

            //Code ve Rate değerlerini modele atıyoruz
            basket.Data.ApplyDiscount(hasDiscount.Code,hasDiscount.Rate);

            //Yapılan değişiklikleri uyguluyorum
            await SaveOrUpdate(basket.Data);
            return true;
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await Get();

            if (basket == null || basket.Data.DiscountCode==null)
                return false;

            //indirim kodu ve rate iptal edildi
            basket.Data.CancelDiscount();

            //Kupon iptali uygulanıyor
            await SaveOrUpdate(basket.Data);

            return true;
        }

        public async Task<Response<bool>> Delete()
        {
            //Sepet ID değerleri kullanıcı Id değerine göre tanımlı.Header içerisinde kullanıcı Id sini gönderdiğimiz için silme işlemi için ekstra bir parametre göndermemize gerek yok
            var result = await _httpClient.DeleteAsync("basket");

            return new Response<bool> { IsSuccessful = result.IsSuccessStatusCode };
        }

        public async Task<Response<BasketViewModel>> Get()
        {
            var response = await _httpClient.GetAsync("basket");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();


            return new Response<BasketViewModel> { Errors = new List<string> { "Hata Aldık" }, IsSuccessful = false };
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            //Class içerisinde ki Get ile basketi alıyoruz
            var basket = await Get();

            //Sepeti başarıyla alabildik mi
            if (!basket.IsSuccessful)
                return false;

            //Silinecek Ürünü sepetten seçtik ve sildik
            var removedBasketItem = basket.Data.BasketItems.FirstOrDefault(x => x.CourseId == courseId);

            //Silinecek ürün bulunamadıysa
            if (removedBasketItem == null)
                return false;

            var deleteResult = basket.Data.BasketItems.Remove(removedBasketItem);

            if (!deleteResult)
                return false;

            //Sepet temizlendikten sonra İndirim kuponunu iptal ettik
            if (!basket.Data.BasketItems.Any())
                basket.Data.DiscountCode = null;

            //Yapılan son değişiklikleri servise göndererek kaydettik
            var response = await SaveOrUpdate(basket.Data);
            return response.IsSuccessful;
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("basket", model);

            //Gelen datayı direkt json a çevirip atama işlemini gerçekleştirdik
            var responseData = await response.Content.ReadFromJsonAsync<Response<bool>>();
            return responseData;
        }
    }
}
