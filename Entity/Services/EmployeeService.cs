using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Repository.Respositories;
using Entity.Services.Interface;
using Entity.Services.Utilities;
using Entity.Services.ViewModels;
using OfficeOpenXml;
using System.Diagnostics;

namespace Entity.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) : base()
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ResponseEntity> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleByIdAsync(c => c.Id == id);
                if (employee == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _employeeRepository.DeleteAsync(employee);
                return new ResponseEntity(StatusCodeConstants.OK, employee, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<byte[]> DownloadReport(string keyWord = "")
        {
            var reportName = $"User_Wise_{Guid.NewGuid():N}.xlsx";
            var entity = await _employeeRepository.ExportData(keyWord);
            var exportBytes = _employeeRepository.ExportToExcel<EmployeeViewExport>(entity, reportName);
            return exportBytes;
        }
        public async Task<IEnumerable<EmployeeViewExport>> GetListEmployee(string keyWord = "", int? pageNumber = null)
        {
            var employees = await _employeeRepository.GetAllEmployeeByKeyWord(keyWord, pageNumber);
            return employees;
        }

        public async Task<ResponseEntity> GetAllEmployee(string keyWord = "")
        {
            var employees = await _employeeRepository.ExportData(keyWord);
            return new ResponseEntity(StatusCodeConstants.OK, employees, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleByIdAsync(c => c.Id == id);
                if (employee == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                return new ResponseEntity(StatusCodeConstants.OK, employee, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            } 
        }

        public async Task<ResponseEntity> InsertEmployee(EmployeeViewModel model)
        {
            using var transaction = _employeeRepository.BeginTransaction();
            try
            {
                Employee employee = new()
                {
                    Name = model.Name,
                    DateOfBirth = FuncUtilities.ConvertStringToDate(model.DateOfBirth),
                    Age = model.Age,
                    JobId = model.JobId,
                    NationId = model.NationId,
                    PhoneNumber = model.PhoneNumber,
                    IdentityCardNumber = model.IdentityCardNumber,
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    WardId = model.WardId
                };
                employee = await _employeeRepository.InsertAsync(employee);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, employee, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> InsertListtEmployee(EmployeeViewModel model)
        {
            try
            {
                Employee employee = new()
                {
                    Name = model.Name,
                    DateOfBirth = FuncUtilities.ConvertStringToDate(model.DateOfBirth),
                    Age = model.Age,
                    JobId = model.JobId,
                    NationId = model.NationId,
                    PhoneNumber = model.PhoneNumber,
                    IdentityCardNumber = model.IdentityCardNumber,
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    WardId = model.WardId
                };
                employee = await _employeeRepository.InsertAsync(employee);
                return new ResponseEntity(StatusCodeConstants.OK, employee, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateEmployee(EmployeeViewModel model)
        {
            using var transaction = _employeeRepository.BeginTransaction();
            try
            {
                var employee = await _employeeRepository.GetSingleByIdAsync(c => c.Id == model.Id);
                if (employee == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                employee.Name = model.Name;
                employee.DateOfBirth = FuncUtilities.ConvertStringToDate(model.DateOfBirth);
                employee.Age = model.Age;
                employee.JobId = model.JobId;
                employee.NationId = model.NationId;
                employee.PhoneNumber = model.PhoneNumber;
                employee.IdentityCardNumber = model.IdentityCardNumber;
                employee.CityId = model.CityId;
                employee.DistrictId = model.DistrictId;
                employee.WardId = model.WardId;
                await _employeeRepository.UpdateAsync(employee, employee);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, employee, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public List<EmployeeViewModel> ReadEmployeeFromExcel(string fullPath)
        {
                using var package = new ExcelPackage(new FileInfo(fullPath));
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                List<EmployeeViewModel> listEmployee = new();
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    _ = int.TryParse(workSheet.Cells[i, 4].Value?.ToString(), out int age);
                    _ = int.TryParse(workSheet.Cells[i, 5].Value?.ToString(), out int jobId);
                    _ = int.TryParse(workSheet.Cells[i, 6].Value?.ToString(), out int nationId);
                    _ = int.TryParse(workSheet.Cells[i, 9].Value?.ToString(), out int cityId);
                    _ = int.TryParse(workSheet.Cells[i, 10].Value?.ToString(), out int districtId);
                    _ = int.TryParse(workSheet.Cells[i, 11].Value?.ToString(), out int wardId);
                    EmployeeViewModel employeeView = new()
                    {
                        Name = workSheet.Cells[i, 2].Value?.ToString() ?? "",
                        DateOfBirth = workSheet.Cells[i, 3].Value?.ToString() ?? "",
                        Age = age,
                        JobId = jobId,
                        NationId = nationId,
                        IdentityCardNumber = workSheet.Cells[i, 7].Value?.ToString(),
                        PhoneNumber = workSheet.Cells[i, 8]?.Value?.ToString(),
                        CityId = cityId,
                        DistrictId = districtId,
                        WardId = wardId,
                    };
                    listEmployee.Add(employeeView);
                }
                return listEmployee;
        }

        public async Task<ResponseEntity> GetSingleEmployee(int id)
        {
            var employee = await _employeeRepository.GetSingleByIdAsync(x => x.Id == id);
            if (employee != null)
            {
                var result = new EmployeeViewModel
                {
                    Name = employee.Name,
                    DateOfBirth = FuncUtilities.ConvertDateToString(employee.DateOfBirth),
                    Age = employee.Age,
                    JobId = employee.JobId,
                    NationId = employee.NationId,
                    IdentityCardNumber = employee.IdentityCardNumber,
                    PhoneNumber = employee.PhoneNumber,
                    CityId = employee.CityId,
                    DistrictId = employee.DistrictId,
                    WardId = employee.WardId
                };
                return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);
            }
            return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
        }

        public async Task<ResponseEntity> GetTime()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            await GetListEmployee();
            stopwatch.Stop();
            return new ResponseEntity(StatusCodeConstants.NOT_FOUND, stopwatch.Elapsed, MessageConstants.MESSAGE_ERROR_404);

        }
    }
}

