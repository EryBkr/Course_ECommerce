using FluentValidation;
using Microservices.Web.Models.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Validations
{
    public class DiscountCreateValidator:AbstractValidator<DiscountApplyInput>
    {
        public DiscountCreateValidator()
        {
            RuleFor(i => i.Code).NotEmpty().WithMessage("Kupon Kodu Boş Olamaz");
        }
    }
}
