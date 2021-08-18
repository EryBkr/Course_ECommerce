using Microservices.Services.Catalog.Dtos;
using Microservices.Services.Catalog.Services.Abstract;
using Microservices.Shared.BaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }



        [HttpGet]
        [Route("/api/[controller]/GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }


        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{id}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateAsync(courseCreateDto);

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);

            //Base Controllerımızdan gelen dönüş tipini kullandım
            return CreateActionResultInstance(response);
        }

    }
}
