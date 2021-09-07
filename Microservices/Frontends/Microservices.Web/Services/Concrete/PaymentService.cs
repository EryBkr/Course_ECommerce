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
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInfo model)
        {
            var response = await _httpClient.PostAsJsonAsync("FakePayment", model);

            return response.IsSuccessStatusCode;
        }
    }
}
