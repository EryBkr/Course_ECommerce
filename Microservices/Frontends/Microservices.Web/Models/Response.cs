using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models
{
    public class Response<T>
    {
        //Properties
        //private set; sadece yapıcıda değer atabileceğimizi gösterir
        public T Data { get;  set; }
        public int StatusCode { get;  set; }
        public bool IsSuccessful { get;  set; }
        public List<string> Errors { get; set; }
    }
}
