﻿using Entity.Constants;
using Entity.Repository.Repositories;
using Entity.Services.Utilities;
using Entity.Services.ViewModels;
using FluentValidation;

namespace Entity.Services.Validation
{
	public class DegreeValidetor : AbstractValidator<DegreeViewModel>
	{
		private readonly IDegreeRepository _degreeRepository;
		public DegreeValidetor(IDegreeRepository degreeRepository)
		{
			_degreeRepository = degreeRepository;

			RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("EmloyeeId is required")
				.MustAsync((model, EmployeeId, CancellationToken) => IsValidEmployeeId(model)).WithMessage("This person has a maximum of 3 unexpired degrees!");
			RuleFor(x => x.DegreeName).NotEmpty().WithMessage("DegreeName is required")
				.MaximumLength(Validations.MaxLenghtNameDegree).WithMessage("Name can not over 250 characters");
			RuleFor(x => x.IssuedBy).NotEmpty().WithMessage("IssuedBy is required")
				.MaximumLength(Validations.MaxLeghtIssuedBy).WithMessage("IssuedBy can not over 550 characters");
			RuleFor(x => x.DateRange).NotEmpty().WithMessage("DateRange is required")
				.Must(FuncUtilities.BeAValidDate).WithMessage("Invaild date (MM/dd/yyyy)!")
				.Must((model, DateRage, CancellationToken) => IsValidDate(model)).WithMessage("The issue date cannot be greater than the expiration date !");
			RuleFor(x => x.DateOfExpiry).NotEmpty().WithMessage("DateOfExpiry is required")
				.Must(FuncUtilities.BeAValidDateOfExpiry).WithMessage("Invaild date (MM/dd/yyyy)!")
				.Must((model, DateRange, CancellationToken) => IsValidDate(model)).WithMessage("Expiration date cannot be less than issue date");
		}
		protected async Task<bool> IsValidEmployeeId(DegreeViewModel model)
		{
			var degree = await _degreeRepository.GetMultiBycondition(x => x.EmployeeId == model.EmployeeId && FuncUtilities.ConvertStringToDate(model.DateOfExpiry) > DateTime.Now && x.Id != model.Id);
			return degree.Count() < Validations.MaximumNumberOfDegrees;
		}

		protected bool IsValidDate(DegreeViewModel model)
		{
			return FuncUtilities.ConvertStringToDate(model.DateOfExpiry) > FuncUtilities.ConvertStringToDate(model.DateRange);
		}
	}
}
