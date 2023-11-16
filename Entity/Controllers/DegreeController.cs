using Entity.Constants;
using Entity.Models;
using Entity.Services;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using X.PagedList;
namespace Entity.Controllers
{
    public class DegreeController : Controller
    {
        private readonly IDegreeService _degreeService;
        private IValidator<DegreeViewModel> _validator;
        public DegreeController(IDegreeService degreeService, IValidator<DegreeViewModel> validator)
        {
            _validator = validator;
            _degreeService = degreeService;
        }

        public async Task<IActionResult> Index(string keyWord, int? page,int currentPage)
        {
            var result = await _degreeService.GetListDegree(keyWord,currentPage);
            return PartialView(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _degreeService.GetSingleDegree(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DegreeViewModel model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var employee = await _degreeService.InsertDegree(model);
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

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _degreeService.GetSingleDegree(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DegreeViewModel model)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var employee = await _degreeService.UpdateDegree(model);
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
                foreach (var fail in validationResult.Errors)
                {
                    ModelState.AddModelError(fail.PropertyName, fail.ErrorMessage);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _degreeService.GetSingleDegree(id);
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
            var result = await _degreeService.DeleteDegree(id);
            if (result.StatusCode == StatusCodeConstants.OK)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("");
            }
            TempData["Error"] = result.Message;
            return RedirectToAction("");
        }
    }
}
