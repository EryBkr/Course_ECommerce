using Microservices.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<CourseViewModel> GetByCourseId(string courseId);

        Task<bool> AddCourseAsync(CourseCreateViewModel model);
        Task<bool> UpdateCourseAsync(CourseUpdateViewModel model);
        Task<bool> DeleteCourseAsync(string courseId);
    }
}
