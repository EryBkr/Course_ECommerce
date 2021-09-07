using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microservices.Web.Exceptions
{
    //Kendi Exception Sınıfımızı yazıyoruz
    public class UnAuthorizeException : Exception
    {
        public UnAuthorizeException()
        {
        }

        public UnAuthorizeException(string message) : base(message)
        {
        }

        public UnAuthorizeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
