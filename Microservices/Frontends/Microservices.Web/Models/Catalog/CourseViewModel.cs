using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Catalog
{
    public class CourseViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public FeatureViewModel Feature { get; set; } //Bu şekilde eklemesekte olurdu örnek olsun diye ekledik
        public string Description { get; set; }

        //Description ı kısalttık (for Home Page)
        public string ShortDescription { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description; }

        //Bu property e Resmin Url ile birleştirilmiş halini vereceğim
        public string StockPictureUrl { get; set; }
    }
}
