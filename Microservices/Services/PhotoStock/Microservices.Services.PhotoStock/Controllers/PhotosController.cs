using Microservices.Services.PhotoStock.Dtos;
using Microservices.Shared.BaseClasses;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController //Extension Dönüş tipimiz için ekledik
    {
        //Fotoğraf kaydetme işlemi
        [HttpPost] //CancellationToken işlemin sonlandırılabilmesi için gerekli
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            //Fotoğraf dolu mu 
            if (photo != null && photo.Length > 0)
            {
                //Fotoğraf ismiyle beraber path oluşturuyorum
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                //Kaydetme işlemini gerçekleştiriyoruz
                using (var stream = new FileStream(path, FileMode.Create))
                    await photo.CopyToAsync(stream, cancellationToken);

                //Dosyanın path ini dönüyorum (domain bağımsız)
                var returnPath = "photos/" + photo.FileName;

                //Dönüş modelimiz oluşturuldu
                var photoDto = new PhotoDto { Url = returnPath };

                //Custom dönüş sağladık
                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto,200));
            }

            //Custom dönüş sağladık
            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is empty", 400));
        }

        //Fotoğraf silme işlemi
        [HttpGet]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            //Path (Resim) mevcut mu
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return CreateActionResultInstance(Response<NoContent>.Success(204));
            }

            return CreateActionResultInstance(Response<NoContent>.Fail("Photo Not Found", 404));
        }
    }
}
