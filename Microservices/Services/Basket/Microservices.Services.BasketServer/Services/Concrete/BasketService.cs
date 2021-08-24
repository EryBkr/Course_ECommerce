using Microservices.Services.BasketServer.Dtos;
using Microservices.Services.BasketServer.Services.Abstract;
using Microservices.Services.BasketServer.Services.Connection;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.Services.BasketServer.Services.Concrete
{
    //Redis ile iletişimimizi sağlayacak sınıfımız
    public class BasketService : IBasketService
    {
        //Startup ta yapığımız configuration dan kaynaklı otomatik bağlantı sağlayacaktır
        private readonly RedisConService _redisCon;

        public BasketService(RedisConService redisCon)
        {
            _redisCon = redisCon;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            //userId ye göre sepetteki datayı silme işlemi yapıyoruz
            var status = await _redisCon.GetDb().KeyDeleteAsync(userId);

            return status ? Response<bool>.Success(200) : Response<bool>.Fail("Basket not found", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisCon.GetDb().StringGetAsync(userId);

            //Böyle bir alışveriş sepeti yok ise yok ise
            if (string.IsNullOrEmpty(existBasket))
                return Response<BasketDto>.Fail("Basket not found",404);

            //Sepet içeriğini Json dan BasketDto a çevirdim
            var basketData = JsonSerializer.Deserialize<BasketDto>(existBasket);

            return Response<BasketDto>.Success(basketData, 200);
            
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            //Sepet içeriğini userId ye göre oluşturuyorum
            var status = await _redisCon.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status ? Response<bool>.Success(200) : Response<bool>.Fail("Basket could not update or save", 500);
        }
    }
}
