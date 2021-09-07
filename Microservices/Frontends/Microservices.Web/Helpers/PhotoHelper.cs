using Microservices.Web.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Helpers
{
    //Catalog Servisimizden Gelen Fotoğraf ismimizi Photo Server daki Url ile dinamik olarak birleştirecek helper
    public class PhotoHelper
    {
        //Microservice Uri's
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
        }

        //http://localhost:5012/photos/photoName
        public string GetPhotoStockUrl(string photoUrl)
        {
            return $"{_serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
        }
    }
}
