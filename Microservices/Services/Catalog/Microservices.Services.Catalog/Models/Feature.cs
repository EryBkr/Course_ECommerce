using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Models
{
    //Kurslara ait olan ek özellikleri belirteceğim.Bundan dolayı Id ye ihtiyacı yok çünkü one to one ilişki olacak içerisinde yer alacak
    public class Feature
    {
        public int Duration { get; set; }
    }
}
