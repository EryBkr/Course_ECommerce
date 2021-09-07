using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Catalog
{
    public class CourseCreateViewModel
    {
        public string UserId { get; set; }

      
        public string CategoryId { get; set; }

     
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public IFormFile PhotoFormFile { get; set; }

        public FeatureViewModel Feature { get; set; } //Bu şekilde eklemesekte olurdu örnek olsun diye ekledik
        public string Description { get; set; }
    }
}
