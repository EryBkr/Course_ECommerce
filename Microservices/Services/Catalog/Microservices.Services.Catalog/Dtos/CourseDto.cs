using Microservices.Services.Catalog.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Dtos
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedTime { get; set; }
        public FeatureDto Feature { get; set; } //Bu şekilde eklemesekte olurdu örnek olsun diye ekledik
        public string Description { get; set; }
    }
}
