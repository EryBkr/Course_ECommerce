using FluentValidation;
using Microservices.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Validations
{
    public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateViewModel>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(i => i.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(i => i.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(i => i.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre Alanı boş olamaz");

            //Virgülden önce 4 karakter virgülden sonra 2 karakter kullanabiliriz
            //2500.99 gibi
            RuleFor(i => i.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı ücret formatı");


        }
    }
}
