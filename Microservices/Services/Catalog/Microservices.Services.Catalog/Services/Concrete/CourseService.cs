using AutoMapper;
using Mass=MassTransit; //Bu kullanım NameSpace ismini değiştirmek için uygulandı.Response dönüş tipi masstransit içerisinde de olduğundan dolayı hata alıyorduk
using Microservices.Services.Catalog.Dtos;
using Microservices.Services.Catalog.Models;
using Microservices.Services.Catalog.Services.Abstract;
using Microservices.Services.Catalog.Settings;
using Microservices.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.Messages;

namespace Microservices.Services.Catalog.Services.Concrete
{
    //Mongodb Kurs Tablosu işlemleri
    public class CourseService : ICourseService
    {
        //Mongo db de bulunan kurs tabloma bağlandım
        private readonly IMongoCollection<Course> _course;

        //Mongo db de bulunan category tabloma bağlandım
        private readonly IMongoCollection<Category> _category;

        private readonly IMapper _mapper;

        //RabbitMQ ile haberleşebilmek için ekliyoruz
        //Publish ibaresi genellikle Event lar için kullanılır.Birden fazla serviste değişiklik yapmak istiyorsak uygun bir yöntemdir.Her bir servisin sorumluluğu farklıdır
        private readonly Mass.IPublishEndpoint _publishEndPoint;

        public CourseService(IMapper mapper, IDatabaseSettings dbSettings, Mass.IPublishEndpoint publishEndPoint)
        {
            //MongoDb Bağlantısını oluşturdum
            var client = new MongoClient(dbSettings.ConnectionString);

            //Databaseye eriştim
            var database = client.GetDatabase(dbSettings.DatabaseName);

            //Kurs Tablosuna eriştim
            _course = database.GetCollection<Course>(dbSettings.CourseCollectionName);

            //Kategori Tablosuna eriştim
            _category = database.GetCollection<Category>(dbSettings.CategoryCollectionName);

            _mapper = mapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            //Tüm kursları aldım
            var courses = await _course.Find(course => true).ToListAsync();

            //Kayıt var ise
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    //Kursa ait kategoriyi aldım.RDMS mantığında ki Join e karşılık gelir
                    course.Category = await _category.Find(i => i.Id == course.CategoryId).FirstAsync();
                }
            }
            else
                courses = new List<Course>();

            //Kursları mapledim
            var mappedCourses = _mapper.Map<List<CourseDto>>(courses);

            return Response<List<CourseDto>>.Success(mappedCourses, 200);

        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            //Kursu alıyorum
            var course = await _course.Find(course => course.Id==id).FirstOrDefaultAsync();

            //Kayıt yok ise
            if (course==null)
                return Response<CourseDto>.Fail("Course Not Found", 404);


            //Kursa ait kategoriyi aldım.RDMS mantığında ki Join e karşılık gelir
            course.Category = await _category.Find(i => i.Id == course.CategoryId).FirstAsync();

            //Kursları mapledim
            var mappedCourse = _mapper.Map<CourseDto>(course);

            return Response<CourseDto>.Success(mappedCourse, 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            //Kullanıcıya ait Bütün kursları aldım
            var courses = await _course.Find(course => course.UserId==userId).ToListAsync();

            //Kayıt var ise
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    //Kursa ait kategoriyi aldım.RDMS mantığında ki Join e karşılık gelir
                    course.Category = await _category.Find(i => i.Id == course.CategoryId).FirstAsync();
                }
            }
            else
                courses = new List<Course>();

            //Kursları mapledim
            var mappedCourses = _mapper.Map<List<CourseDto>>(courses);

            return Response<List<CourseDto>>.Success(mappedCourses, 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _course.InsertOneAsync(newCourse);

            var mappedCourse = _mapper.Map<CourseDto>(newCourse);

            return Response<CourseDto>.Success(mappedCourse,200);
        }

        //Update işleminin neticesinde geriye data dönmeme gerek yok
        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updatedCourse = _mapper.Map<Course>(courseUpdateDto);
            
            //Güncelleme işlemini yapıyoruz
            var result= await _course.FindOneAndReplaceAsync(x=>x.Id==courseUpdateDto.Id,updatedCourse);

            //Güncelleme işlemi yapılamadıysa
            if (result==null)
                return Response<NoContent>.Fail("Will Update Course Not Found", 404);

            //Payment servisinde kullandığımız Send ten  farklı olarak kuyruk ismi belirlemedik .Send ve Publish birbirinden farklı kullanımlara sahiptir
            //Şayet kurs isminde bir değişiklik yaparsak bu değişikliğin order servisine de yansıması için rabbitMQ ve massTransit ile datayı publish ediyoruz,order tarafında ki subscriber bu değişikliği alıp uyguluyor
            await _publishEndPoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent 
            {
                CourseId=updatedCourse.Id,
                UpdatedName=updatedCourse.Name
            });

            return Response<NoContent>.Success(204);
        }

        //Delete işleminin neticesinde geriye data dönmeme gerek yok
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _course.DeleteOneAsync(i => i.Id == id);

            //Silme işlemi yapılamadıysa
            if (result.DeletedCount>0)
                return Response<NoContent>.Success(204);
            else
                return Response<NoContent>.Fail("Course Not Found",404);
        }

    }
}
