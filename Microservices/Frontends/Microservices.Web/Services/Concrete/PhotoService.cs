using Microservices.Web.Models;
using Microservices.Web.Models.Photo;
using Microservices.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Concrete
{
    //Photo mikroservisim ile iletişime geçecek servisim
    public class PhotoService : IPhotoService
    {
        private readonly HttpClient _httpClient;

        public PhotoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            //Silinecek Resmin ismini Servise gönderiyorum
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<Response<PhotoViewModel>> UploadPhoto(IFormFile photo)
        {
            //Fotoğraf boş mu diye kontrol ediyoruz
            if (photo == null || photo.Length <= 0)
                return new Response<PhotoViewModel> { IsSuccessful = false, Errors = new List<string> { "Fotoğraf boş olamaz" } };


            //Fotoğraf ismi belirledik
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            //Resmimizi Byte a çevirip göndereceğiz
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);

                var byteArray = memoryStream.ToArray();

                var multipartContent = new MultipartFormDataContent();

                //İkinci parametredeki photo string i aslında PhotoService in Post Action unda bizden beklenen parametre ismini temsil ediyor
                multipartContent.Add(new ByteArrayContent(byteArray), "photo", randomFileName);

                //ilk parametre controller ismidir
                var response = await _httpClient.PostAsync("photos", multipartContent);

                if (!response.IsSuccessStatusCode)
                    return new Response<PhotoViewModel> { IsSuccessful = false, Errors = new List<string> { "Sunucu taraflı bir hata meydana geldi" } };

                //Json dan Obje ye  çevirip dönüyoruz
               return await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            }
        }
    }
}
