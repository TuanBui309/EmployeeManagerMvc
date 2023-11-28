using Entity.Constants;
using FluentValidation;
using FluentValidation.Results;
using Entity.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Entity.Services.ViewModels;
using System.Text;
using System.Diagnostics;

namespace Entity.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<EmployeeViewModel> _validator;
        public EmployeeController(IEmployeeService employeeService, IValidator<EmployeeViewModel> validations)
        {
            _employeeService = employeeService;
            _validator = validations;
        }

        public async Task<ActionResult> Index(string keyWord = "", int? pageNumber = null)
        {
            var employees = await _employeeService.GetListEmployee(keyWord, pageNumber);
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
            var result = await _employeeService.GetSingleEmployee(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not Found";
                return RedirectToAction("");
            }
            return View(result.Content);
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
        public async Task<IActionResult> ExportToExcel(string keyWord = "")
        {
            var exportbytes = await _employeeService.DownloadReport(keyWord);
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
                    if (excelData.Count > 0)
                    {
                        for (int i = 0; i < excelData.Count; i++)
                        {
                            ValidationResult result = await _validator.ValidateAsync(excelData[i]);
                            if (result.IsValid)
                            {
                                await _employeeService.InsertListtEmployee(excelData[i]);
                                TempData["Success"] = $"Added {i + 1} item";
                            }
                            else
                            {
                                foreach (var fail in result.Errors)
                                {
                                    TempData["Error"] += $"Error in line {i + 1} : " + fail.PropertyName + " : " + fail.ErrorMessage + "\n" + "---- ";
                                }
                            }
                        }
                    }
                    TempData["Success"] = "Added data from the file successfully";
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
        public async Task<IActionResult> GetAllEmployee(string keyWord = "")
        {
            return await _employeeService.GetAllEmployee(keyWord);
        }


        [HttpGet("GetTime")]
        public async Task<IActionResult> GetTime()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            await Index();


            stopwatch.Stop();
            return new ResponseEntity(StatusCodeConstants.NOT_FOUND, stopwatch.Elapsed, MessageConstants.MESSAGE_ERROR_404);
        }

        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> GetEmplyeeById(int id)
        {
            return await _employeeService.GetEmployeeById(id);
        }
    }
}