using Entity.Models;
using Entity.Repository.Repositories;
using Entity.Repository.Respositories;
using Entity.Respository.Respositories;
using Entity.Services.Utilities;
using Entity.Services.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Entity.Services.Validation
{
	public class EmployeeValidator : AbstractValidator<EmployeeViewModel>
	{
		private readonly IJobRepository _jobRepository;
		private readonly INationRepository _nationRepository;
		private readonly IDistrictRespository _districtRespository;
		private readonly IWardRepository _wardRepository;
		private readonly ICityRepository _cityRepository;
		private readonly IEmployeeRepository _employeeRepository;

		public EmployeeValidator(IDistrictRespository districtRespository, IWardRepository wardRepository, ICityRepository cityRepository, IEmployeeRepository employeeRepository, IJobRepository jobRepository, INationRepository nationRepository)
		{
			_cityRepository = cityRepository;
			_districtRespository = districtRespository;
			_wardRepository = wardRepository;
			_employeeRepository = employeeRepository;
			_jobRepository = jobRepository;
			_nationRepository = nationRepository;

			RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
				.MaximumLength(250).WithMessage("Name can not over 250 characters");
			RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required").Must(FuncUtilities.BeAValidDate).WithMessage("Invaild date (dd/MM/yyyy)!");
			RuleFor(x => x.Age).NotEmpty().WithMessage("Age is required")
				.GreaterThan(0).WithMessage("Age must be greater than 0")
				.LessThan(150).WithMessage("Age must be less than 150");
			RuleFor(x => x.JobId).NotEmpty().WithMessage("JobId is required")
				.MustAsync((model, JobId, CancellationToken) => IsValidJobId(model)).WithMessage("JobId does not exist");
			RuleFor(x => x.NationId).NotEmpty().WithMessage("NationId is required")
				.MustAsync((model, NationId, CancellationToken) => IsValidNationId(model)).WithMessage("NationId does not exist");
			RuleFor(x => x.PhoneNumber)/*.NotEmpty().WithMessage("Phone number í required")*/
				.Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$").WithMessage("Invaild phone number");
			RuleFor(x => x.IdentityCardNumber)/*.NotEmpty().WithMessage("IdentityCardNumber is required")*/
				.MustAsync((model, IdentityCarNumber, CancellationToken) => IsValidIdentityCardNumber(model)).WithMessage($"IdentityCardNumber is already")
				.Matches(@"^\d{12}$").WithMessage("IdentityCardNumber is required");
			RuleFor(model => model.CityId).NotEmpty()
				.MustAsync((model, CityId, CancellationToken) => IsValidCityId(model)).WithMessage("Cityid does not exist");
			RuleFor(model => model.DistrictId)
				.MustAsync((model, DistrictId, CancellationToken) => IsValidDistrictId(model, DistrictId)).WithMessage("The district must belong to the province.");
			RuleFor(model => model.WardId)
				.MustAsync((model, ward, CancellationToken) => IsValidWardId(model, ward))
				.WithMessage("The commune must belong to the district.");
		}

		protected async Task<bool> IsValidCityId(EmployeeViewModel model)
		{
			var city = await _cityRepository.GetSingleByIdAsync(c => c.Id == model.CityId);
			return city != null;
		}

		protected async Task<bool> IsValidJobId(EmployeeViewModel model)
		{
			var job = await _jobRepository.GetSingleByIdAsync(c => c.Id == model.JobId);
			return job != null;
		}

		protected async Task<bool> IsValidNationId(EmployeeViewModel model)
		{
			var nation = await _nationRepository.GetSingleByIdAsync(c => c.Id == model.NationId);
			return nation != null;
		}

		protected async Task<bool> IsValidDistrictId(EmployeeViewModel model, int districtId)
		{
			var district = await _districtRespository.GetSingleByIdAsync(c => c.Id == districtId);
			if (district != null)
			{
				return model.CityId == district.CityId;
			}
			return false;
		}

		protected async Task<bool> IsValidWardId(EmployeeViewModel model, int wardId)
		{
			var ward = await _wardRepository.GetSingleByIdAsync(c => c.Id == wardId);
			if (ward != null)
			{
				return model.DistrictId == ward.DistrictId;
			}
			return false;
		}

		protected async Task<bool> IsValidIdentityCardNumber(EmployeeViewModel model)
		{
			if (model.IdentityCardNumber != null)
			{
				var identityCardNumber = await _employeeRepository.GetSingleByIdAsync(x => x.Id != model.Id && x.IdentityCardNumber == model.IdentityCardNumber);
				return identityCardNumber == null;
			}
			return true;
		}
	}
}
