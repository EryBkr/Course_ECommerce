using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Settings
{
    //Appsettings te tanımladığım URL bilgilerini option pattern ile alacağım
    public class ServiceApiSettings
    {
        public string BaseUri { get; set; }
        public string PhotoStockUri { get; set; }
        public string AuthUri { get; set; }

        //Ocelot tarafında belirlediğimiz URL bilgilerini Catalog için tutacak
        public ServicesUri Catalog { get; set; }
        public ServicesUri PhotoStock { get; set; }
        public ServicesUri Basket { get; set; }
        public ServicesUri Discount { get; set; }
        public ServicesUri Payment { get; set; }
        public ServicesUri Order { get; set; }
    }

    /// <summary>
    /// Ocelot tarafında belirlediğimiz URL leri burada paketleyeceğiz
    /// </summary>
    public class ServicesUri
    {
        public string Path { get; set; }
    }
}
