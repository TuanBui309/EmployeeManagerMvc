using Entity.Services.ViewModels;
using FluentValidation;

namespace Entity.Services.Validation
{
    public class WardValidator : AbstractValidator<WardViewModel>
    {
        public WardValidator()
        {
            RuleFor(x => x.WardName).NotEmpty().WithMessage("Ward name is requied");
        }
    }
}
