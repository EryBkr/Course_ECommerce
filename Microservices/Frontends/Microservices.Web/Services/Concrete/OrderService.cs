using Microservices.Shared.Services.Abstract;
using Microservices.Web.Models;
using Microservices.Web.Models.Order;
using Microservices.Web.Models.Payment;
using Microservices.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        //Ödeme alındıktan sonra sipariş oluşturulacak
        private readonly IPaymentService _paymentService;

        //Sepetteki ürünleri kullanacağız
        private readonly IBasketService _basketService;

        //Kullanıcının Id değerişin alacağız
        private readonly ISharedIdentityService _identityService;



        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService identityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _identityService = identityService;
        }

        //Senkron iletişim
        public async Task<OrderStatusViewModel> CreateOrder(Checkout checkout)
        {
            var basket = await _basketService.Get();//Sepetteki dataları alıyoruz

            //Ödeme servisine gidecek modelimizi oluşturuyoruz
            var payment = new PaymentInfo()
            {
                CardName = checkout.CardName,
                CardNumber = checkout.CardNumber,
                CVV = checkout.CVV,
                Expiration = checkout.Expiration,
                TotalPrice = basket.Data.TotalPrice
            };

            //Ödeme işlemi yapılıyor
            var responsePayment = await _paymentService.ReceivePayment(payment);

            //Ödeme başarısız ise
            if (!responsePayment)
                return new OrderStatusViewModel { Error = "Ödeme alınamadı", IsSuccessful = false };

            var orderCreateInput = new CreateOrderInput
            {
                //Kullanıcı Id sini atadık
                BuyerId = _identityService.GetUserId,
                AddressDto = new AddressCreateInput { Province = checkout.Province, District = checkout.District, Line = checkout.Line, Street = checkout.Street, ZipCode = checkout.ZipCode }
            };

            //Sepet içerisinde ki Ürünleri sipariş modelimize ekliyoruz
            basket.Data.BasketItems.ForEach(i =>
            {
                orderCreateInput.OrderItemDtos.Add(new OrderItemViewModel { ProductId = i.CourseId, Price = i.GetCurrentPrice, PictureUrl = "", ProductName = i.CourseName });
            });

            //Order modelimizi gönderiyoruz
            var response = await _httpClient.PostAsJsonAsync<CreateOrderInput>("Order", orderCreateInput);
            var responseContent = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
                return new OrderStatusViewModel { Error = "Sipariş oluşturulamadı", IsSuccessful = false };

            var orderCreated = await response.Content.ReadFromJsonAsync<Response<OrderStatusViewModel>>();

            //Bu değer servis tarafından bize gelmiyor.Burasını biz dolduruyoruz
            orderCreated.Data.IsSuccessful = true;

            //Sepeti temizledik
            await _basketService.Delete();

            return orderCreated.Data;
        }

        //Sipariş Geçmişini alacağız
        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("Order");
            return response.Data;
        }

        //Asenkron İletişim (RabbitMQ)
        public Task SuspendOrder(Checkout checkout)
        {
            throw new NotImplementedException();
        }
    }
}
