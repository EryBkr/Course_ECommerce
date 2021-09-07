using Microservices.Web.Helpers;
using Microservices.Web.Models;
using Microservices.Web.Models.Catalog;
using Microservices.Web.Services.Abstract;
using Microservices.Web.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    //Catalog Microservice ile iletişime geçecek sınıfımız
    public class CatalogService : ICatalogService
    {
        //Http isteklerimiz için ekledik
        private readonly HttpClient _httpClient;

        //Photo Servisi ile iletişime geçecek Interface imiz
        private readonly IPhotoService _photoService;

        //Fotoğraf Url leri oluşturacak Helper
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoService photoService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoService = photoService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> AddCourseAsync(CourseCreateViewModel model)
        {
            //Fotoğraf gönderme işlemi
            var responsePhoto = await _photoService.UploadPhoto(model.PhotoFormFile);
            if (responsePhoto.IsSuccessful)
            {
                //Resmin ismini alıyorum
                model.Picture = responsePhoto.Data.Url;
            }

            //Direkt Json a çevirip gönderecek.3 satırda yaptığımız işi tek satırda yapıyor
            var response = await _httpClient.PostAsJsonAsync<CourseCreateViewModel>("course", model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            //http://localhost:5000/services/catalog/course/5
            var response = await _httpClient.DeleteAsync($"course/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            //http://localhost:5000/services/catalog/category
            var response = await _httpClient.GetAsync("category");

            //Başarısız ise
            if (!response.IsSuccessStatusCode)
                return null;

            //Gelen datayı direkt json a çevirip atama işlemini gerçekleştirdik
            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseData.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            //http://localhost:5000/services/catalog/course
            var response = await _httpClient.GetAsync("course");

            //Başarısız ise
            if (!response.IsSuccessStatusCode)
                return null;

            //Gelen datayı direkt json a çevirip atama işlemini gerçekleştirdik
            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            //Stock Picture Url property sine Resmin Service URL ini de ekliyoruz
            responseData.Data.ForEach(x => { x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture); });

            return responseData.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            //http://localhost:5000/services/catalog/course/GetAllByUserId/5
            var response = await _httpClient.GetAsync($"course/GetAllByUserId/{userId}");

            //Başarısız ise
            if (!response.IsSuccessStatusCode)
                return null;

            //Gelene datayı direkt json a çevirip atama işlemini gerçekleştirdik
            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            //Stock Picture Url property sine Resmin Service URL ini de ekliyoruz
            responseData.Data.ForEach(x => { x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture); });

            return responseData.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            //http://localhost:5000/services/catalog/course//GetById/5
            var response = await _httpClient.GetAsync($"course/GetById/{courseId}");

            //Başarısız ise
            if (!response.IsSuccessStatusCode)
                return null;

            //Gelene datayı direkt json a çevirip atama işlemini gerçekleştirdik
            var responseData = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            //Resmi sayfada gösterebilmek için farklı bir property e url ile birleştirilmiş halini veriyorum
            responseData.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseData.Data.Picture);
            return responseData.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateViewModel model)
        {
            //Fotoğraf gönderme işlemi
            var responsePhoto = await _photoService.UploadPhoto(model.PhotoFormFile);
            if (responsePhoto.IsSuccessful)
            {
                //Yeni Bir Resim yüklendiği için eski resmi silmem gerekiyor
                await _photoService.DeletePhoto(model.Picture);

                //Resmin ismini alıyorum
                model.Picture = responsePhoto.Data.Url;
            }


            //Direkt Json a çevirip gönderecek.3 satırda yaptığımız işi tek satırda yapıyor
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateViewModel>("course", model);

            return response.IsSuccessStatusCode;
        }
    }
}
