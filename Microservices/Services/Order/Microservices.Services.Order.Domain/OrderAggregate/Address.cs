using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    //ValueObject tipinde bir model.Database ile ilişkiili kimliksiz bir yapısı vardır
    public class Address : ValueObject
    {
        //Properylere dışarıdan değer atanamaz
        public string Province { get; private set; }
        public string District { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string Line { get; private set; }

        //Değer atamasını yapıcı metot ile yapıyoruz
        //Bussiness kodu buralarda uygulanabilir
        public Address(string province, string district, string street, string zipCode, string line)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        //Harici Set işlemi
        //Örnek açısından ekledik kullanmayacağız
        public void SetZipCode(string zipCode)
        {
            //Bussiness Logic
            ZipCode = zipCode;
        }


        //İmplementasyon sonucu ezmemiz gerekti
        protected override IEnumerable<object> GetEqualityComponents()
        {
            //Iterator mantığıyla yield ibaresi  görüldükçe tekrar execute işlemi yapılabiliyor
            //Bundan kaynaklı çoklu return kullanabiliyoruz
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
