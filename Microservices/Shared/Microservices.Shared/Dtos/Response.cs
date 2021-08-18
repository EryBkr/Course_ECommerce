using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microservices.Shared.Dtos
{
    //Dönüşlerimizi paketlemek için oluşturduk
    public class Response<T>
    {
        //Properties
        //private set; sadece yapıcıda değer atabileceğimizi gösterir
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public bool IsSuccessful { get; private set; }
        public List<string> Errors { get; set; }



        //STATIC FACTORY METHODS

        //Başarılı Durumda Data dahil dönüş
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessful = true //Success Dönüş tipi olduğu için true olarak direkt verdim
            };
        }

        //Başarılı Durumda Data dahil olmayan dönüş
        public static Response<T> Success(int statusCode)
        {
            return new Response<T>
            {
                Data = default(T),//T tipinde null data dönüyorum
                StatusCode = statusCode,
                IsSuccessful = true //Success Dönüş tipi olduğu için true olarak direkt verdim
            };
        }

        //Hatalı Durumda dönüş
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Data = default(T),//T tipinde null data dönüyorum
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false //Hatalı Dönüş tipi olduğu için false olarak direkt verdim
            };
        }

        //Hatalı Durumda dönüş (1 adet hata var ise)
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T>
            {
                Data = default(T),//T tipinde null data dönüyorum
                Errors = new List<string>() { error },
                StatusCode = statusCode,
                IsSuccessful = false //Hatalı Dönüş tipi olduğu için false olarak direkt verdim
            };
        }
    }
}
