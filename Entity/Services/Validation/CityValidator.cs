using Entity.Models;
using Entity.Services.ViewModels;
using FluentValidation;
namespace Entity.Services.Validation
{
    public class CityValidator : AbstractValidator<CityViewModel>
    {
        public CityValidator()
        {
            RuleFor(x => x.CityName).NotEmpty().WithMessage("First name is required")
                .MaximumLength(200).WithMessage("First name can not over 200 characters");
        }
    }
}
