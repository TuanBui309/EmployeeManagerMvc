using Entity.Constants;
using Entity.Models;
using Entity.Services;
using Entity.Services.Validation;
using FluentValidation;
using FluentValidation.Results;
using Entity.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Entity.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using X.PagedList;
using OfficeOpenXml.DataValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation.AspNetCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Entity.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private IValidator<EmployeeViewModel> _validator;
        public EmployeeController(IEmployeeService employeeService, IValidator<EmployeeViewModel> validations)
        {
            _employeeService = employeeService;
            _validator = validations;
        }

        public async Task<ActionResult> Index(string keyWord = "", int pageNumber = 1)
        {
            var employees = await _employeeService.GetlistEmployee(keyWord, pageNumber);
            return PartialView(employees);
        }

        public async Task<ActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not Found";
                return RedirectToAction("");
            }
            return View(employee.Content);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetSingleEmployee(id);
            if (employee.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not Found";
                return RedirectToAction("");
            }
            return View(employee.Content);
        }

        public IActionResult Create() => View();

        [HttpPost("Employee/Create")]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {

            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var employee = await _employeeService.InsertEmployee(model);
                if (employee.StatusCode == StatusCodeConstants.OK)
                {
                    TempData["Success"] = employee.Message;
                    return RedirectToAction("");
                }
                TempData["Error"] = employee.Message;
                return View();
            }
            foreach (var fail in result.Errors)
            {
                ModelState.AddModelError(fail.PropertyName, fail.ErrorMessage);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var employee = await _employeeService.UpdateEmployee(model);
                if (employee.StatusCode == StatusCodeConstants.OK)
                {
                    TempData["Success"] = employee.Message;
                    return RedirectToAction("");
                }
                TempData["Error"] = employee.Message;
                return View(model);
            }
            foreach (var fail in result.Errors)
            {
                ModelState.AddModelError(fail.PropertyName, fail.ErrorMessage);
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
			var employee = await _employeeService.GetEmployeeById(id);
			if (employee.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not Found";
				return RedirectToAction("");
			}
			return View(employee.Content);
		}

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            if (result.StatusCode == StatusCodeConstants.OK)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("");
            }
            TempData["Error"] = result.Message;
            return RedirectToAction("");

        }

        [HttpGet, ActionName("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(string keyWord)
        {
            var exportbytes = await _employeeService.DownloadReport(keyWord);
            if (exportbytes.Count() == 0)
            {
                TempData["Error"] = "Can't read data";
            }
            return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employee.xlsx");
        }

        public IActionResult ImportData() => View();

        [HttpPost]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var filePath = Path.Combine(uploadsFolder, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var excelData = _employeeService.ReadEmployeeFromExcel(filePath);
                    Console.WriteLine(excelData);
                    if (excelData.Count() > 0)
                    {

                        for (int i = 0; i < excelData.Count; i++)
                        {
                            ValidationResult result = await _validator.ValidateAsync(excelData[i]);
                            if (result.IsValid)
                            {
                                await _employeeService.InsertEmployee(excelData[i]);
                                TempData["Success"] = $"Added {i + 1} item";
                            }
                            else
                            {
                                TempData["ErrorLine"] = $"error in line {i + 1} ";
                                foreach (var fail in result.Errors)
                                {
                                    TempData["Error"] += fail.PropertyName + " : " + fail.ErrorMessage + "\n" + "----";
                                }
                                return RedirectToAction("");
                            }
                        }
                    }
                    //return new ResponseEntity(StatusCodeConstants.OK, "Added all data from the file successfully", MessageConstants.INSERT_SUCCESS);
                    TempData["Success"] = "Added all data from the file successfully";
                    return RedirectToAction("");
                }
                TempData["Error"] = "file is requied";
                return View(file);
            }
            catch (Exception)
            {
                TempData["Error"] = "Can't import data";
                return View(file);
            }
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee(string keyWord = "", int soTrang = 1, int soPhanTuTrenTrang = 10)
        {
            return await _employeeService.GetAllEmployee(keyWord, soTrang, soPhanTuTrenTrang);
        }

        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> GetEmplyeeById(int id)
        {
            return await _employeeService.GetEmployeeById(id);
        }
    }
}