using Entity.Models;
using Entity.Services;
using Microsoft.AspNetCore.Mvc;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Entity.Constants;
using X.PagedList;

namespace Entity.Controllers
{
	public class WardController : Controller
	{
		private readonly IWardService _wardService;
		IValidator<WardViewModel> _wardValidator;

		public WardController(IWardService WardService, IValidator<WardViewModel> validator)
		{
			_wardService = WardService;
			_wardValidator = validator;
		}

		public async Task<IActionResult> Index(string keyWord = "", int currentPage = 1)
		{
			var result = await _wardService.GetListWard(keyWord, currentPage);
			return PartialView(result);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(WardViewModel model)
		{
			ValidationResult result = await _wardValidator.ValidateAsync(model);
			if (result.IsValid)
			{
				var employee = await _wardService.InsertWard(model);
				if (employee.StatusCode == StatusCodeConstants.OK)
				{
					TempData["Success"] = employee.Message;
					return RedirectToAction("");
				}
				TempData["Error"] = employee.Message;
				return View();
			}
			else
			{
				foreach (var fail in result.Errors)
				{
					ModelState.AddModelError(fail.PropertyName, fail.ErrorMessage);
				}
				return View(model);
			}
		}

		public async Task<IActionResult> Details(int id)
		{
			var result = await _wardService.GetSingleWard(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var result = await _wardService.GetSingleWard(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(WardViewModel model)
		{
			ValidationResult result = await _wardValidator.ValidateAsync(model);
			if (result.IsValid)
			{
				var employee = await _wardService.UpdateWard(model);
				if (employee.StatusCode == 200)
				{
					TempData["Success"] = employee.Message;
					return RedirectToAction("");
				}
				TempData["Error"] = employee.Message;
				return View();
			}
			else
			{
				foreach (var fail in result.Errors)
				{
					ModelState.AddModelError(fail.PropertyName, fail.ErrorMessage);
				}
				return View(model);
			}
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = await _wardService.GetSingleWard(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var result = await _wardService.DeleteWard(id);
			if (result.StatusCode == 200)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("");
			}
			TempData["Error"] = result.Message;
			return View(result);
		}

		[HttpGet("GetAllWard")]
		public async Task<IActionResult> GetAllWard()
		{
			return await _wardService.GetAllWard();
		}

		[HttpGet("GetSingleWardById")]
		public async Task<IActionResult> GetSingleWardById(int id)
		{
			return await _wardService.GetSingleWardById(id);
		}

		[HttpGet("GetMultiWardByCondition")]
		public async Task<IActionResult> GetMultiWardByCondition(int districtId)
		{
			return await _wardService.GetMultiWardByCondition(districtId);
		}
	}
}
