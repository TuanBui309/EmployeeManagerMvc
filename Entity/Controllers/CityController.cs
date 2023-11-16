using Entity.Constants;
using Entity.Models;
using Entity.Services;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using X.PagedList;

namespace Entity.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IValidator<CityViewModel> _validator;
        public CityController(ICityService cityService, IValidator<CityViewModel> validator)
        {
            _cityService = cityService;
            _validator = validator;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var result = await _cityService.GetListCity(currentPage);
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _cityService.GetSingleCity(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CityViewModel city)
        {
            ValidationResult result = await _validator.ValidateAsync(city);
            if (result.IsValid)
            {
                var employee = await _cityService.InsertCity(city);
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
                return View(city);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _cityService.GetSingleCity(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CityViewModel model, int id)
        {
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var employee = await _cityService.UpdateCity(id, model);
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
            var result = await _cityService.GetSingleCity(id);
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
            var result = await _cityService.DeleteCity(id);
            if (result.StatusCode == 200)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("");
            }
            TempData["Error"] = result.Message;
            return RedirectToAction("");
        }

        [HttpGet("GetAllCity")]
        public async Task<IActionResult> GetALLCity()
        {
            var result = await _cityService.GetAllCity();
            return result;
        }

        [HttpGet("GetCityById")]
        public async Task<IActionResult> GetCityById(int CityId)
        {
            return await _cityService.GetSingleCityById(CityId);
        }

        [HttpGet("GetCityByCondition")]
        public async Task<IActionResult> GetCityByCondition(int CityId, string name)
        {
            return await _cityService.GetAllCityByCondition(CityId, name);
        }


    }
}
