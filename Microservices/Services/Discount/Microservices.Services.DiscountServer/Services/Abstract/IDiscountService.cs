using Microservices.Services.DiscountServer.Models;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.DiscountServer.Services.Abstract
{
    public interface IDiscountService
    {
        Task<Response<List<Discount>>> GetAll();
        Task<Response<Discount>> GetById(int id);
        Task<Response<NoContent>> Add(Discount discount);
        Task<Response<NoContent>> Update(Discount discount);
        Task<Response<NoContent>> Delete(int id);

        //Code ve UserId eşleşme kontrolü
        Task<Response<Discount>> GetByCodeAndUserId(string code,string userId);
    }
}
