using AutoMapper;
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

namespace Microservices.Services.Catalog.Services.Concrete
{
    public class CategoryService: ICategoryService
    {
        //Mongo db de bulunan category tabloma bağlandım
        private readonly IMongoCollection<Category> _category;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings dbSettings)
        {
            //MongoDb Bağlantısını oluşturdum
            var client = new MongoClient(dbSettings.ConnectionString);
            //Databaseye eriştim
            var database = client.GetDatabase(dbSettings.DatabaseName);
            //Tabloya eriştim
            _category = database.GetCollection<Category>(dbSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        //Kategorileri Response 'a paketleyip döneceğiz
        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            //Tüm kategorileri aldık
            var categories = await _category.Find(category => true).ToListAsync();
            //Map işlemi yaptık
            var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);

            //Kategorileri Response ile dönüyoruz
            return Response<List<CategoryDto>>.Success(mappedCategories, 200);
        }

        //Kategori ekleyip response ile döneceğiz
        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto category)
        {
            //Map işlemi yaptık
            var mappedCategory = _mapper.Map<Category>(category);

            //Collection a yani tabloya category ekledik
            await _category.InsertOneAsync(mappedCategory);

            //Referans tip olduğu için id eklenmiş olacaktır
            //Kategorileri Response ile dönüyoruz
            return Response<CategoryDto>.Success(category, 200);
        }

        //Id ye göre kategori döneceğim
        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            //Kategoriyi aldık
            var category = await _category.Find(i => i.Id == id).FirstOrDefaultAsync();

            //Kategori boş ise
            if (category==null)
                return Response<CategoryDto>.Fail("Category Not Found", 404);

            //Map işlemi yaptık
            var mappedCategory = _mapper.Map<CategoryDto>(category);

            //Kategoriyi Response ile dönüyoruz
            return Response<CategoryDto>.Success(mappedCategory, 200);
        }
    }
}
