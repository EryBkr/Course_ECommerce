using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Models
{
    public class Category
    {
        [BsonId] //MongoDb nin Primary Key olarak algılaması için ekledik
        [BsonRepresentation(BsonType.ObjectId)] //Tip dönüşümü için ekledik
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
