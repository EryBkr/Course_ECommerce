using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Messages
{
    //Event kullanımı birden fazla servisin değişiklikten etkileneceği zaman kullanılır.
    //Şayet Kurs ismini Catalog tan değiştireceksek bu değişikliğin Order tarafında da uygulanması gerekir
    //Data tutarlığı sağladığımız yöntemin adı Eventual Consistency dir
    public class CourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string UpdatedName { get; set; }
    }
}
