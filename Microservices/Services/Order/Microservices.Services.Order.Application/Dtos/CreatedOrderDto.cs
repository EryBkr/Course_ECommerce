using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Dtos
{
    //Sipariş oluştuktan sonra Id sini döneceğimiz modeli oluşturduk
    public class CreatedOrderDto
    {
        public int OrderId { get; set; }
    }
}
