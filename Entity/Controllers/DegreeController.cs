using Entity.Constants;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Entity.Controllers
{
    public class DegreeController : Controller
    {
        private readonly IDegreeService _degreeService;
        private readonly IValidator<DegreeViewModel> _validator;
        public DegreeController(IDegreeService degreeService, IValidator<DegreeViewModel> validator)
        {
            _validator = validator;
            _degreeService = degreeService;
        }

        public async Task<IActionResult> Index(string keyWord = "", int? pageNumber = null)
        
        {
            var result = await _degreeService.GetListDegree(keyWord, pageNumber);
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
            var results = await _degreeService.GetSingleDegree(id);
            if (results.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(results.Content);
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
