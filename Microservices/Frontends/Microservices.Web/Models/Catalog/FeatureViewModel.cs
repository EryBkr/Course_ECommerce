using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Catalog
{
    public class FeatureViewModel
    {
        [Required(ErrorMessage = "Kurs Süresi boş olamaz")]
        public int Duration { get; set; }
    }
}
