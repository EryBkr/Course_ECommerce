using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Settings
{
    //Database ayarları için Option Pattern Uygulayacağım
    //IOptions tanımlamasıyla uğraşmamak için soyutlama gereği duydum
    public interface IDatabaseSettings
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
