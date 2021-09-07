using Microservices.Web.Models;
using Microservices.Web.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface IBasketService
    {
        Task<Response<bool>> SaveOrUpdate(BasketViewModel model);
        Task<Response<BasketViewModel>> Get();
        Task<Response<bool>> Delete();
        Task AddBasketItem(BasketItemViewModel model);
        Task<bool> RemoveBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}
