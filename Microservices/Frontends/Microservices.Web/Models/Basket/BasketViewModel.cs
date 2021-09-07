using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Basket
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();

        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; } //İndirim Oranı
        private List<BasketItemViewModel> _basketItems;

        //Ürün Fiyatlarının her birine verilen indirim oranı uygulanıyor
        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(
                        x =>
                        {
                            var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                            x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                        });

                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }

        //Toplam fiyatı model üzerinden aldık
        public decimal TotalPrice { get => BasketItems.Sum(i => i.GetCurrentPrice); }

        //İndirim kodu mevcut mu
        public bool HasDiscount { get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue; }

        //Code ve Rate değerlerini iptal ediyorum
        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        //Code ve Rate değerlerini modele atıyoruz
        public void ApplyDiscount(string code,int rate)
        {
            DiscountRate = rate;
            DiscountCode = code;
        }
    }
}
