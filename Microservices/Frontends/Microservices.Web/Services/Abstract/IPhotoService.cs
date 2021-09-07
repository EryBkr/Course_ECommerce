using Microservices.Web.Models;
using Microservices.Web.Models.Photo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface IPhotoService
    {
        Task<Response<PhotoViewModel>> UploadPhoto(IFormFile photo);
        Task<bool> DeletePhoto(string photoUrl);
    }
}
