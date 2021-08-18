using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Models
{
    public class Course
    {
        [BsonId] //MongoDb nin Primary Key olarak algılaması için ekledik
        [BsonRepresentation(BsonType.ObjectId)] //Tip dönüşümü için ekledik
        public string Id { get; set; }

        //UserId bizim için önemli
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)] //Category Id Object Id olduğu için ekledik
        public string CategoryId { get; set; }

        [BsonIgnore] //Navigation Property gibi kullanacağım.Database e field olarak eklenmemesi için bu tanımlamayı yaptım
        public Category Category { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }
        public Feature Feature { get; set; } //Bu şekilde eklemesekte olurdu örnek olsun diye ekledik
        public string Description { get; set; }
    }
}
