using Azure.Messaging;
using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Repository.Repositories;
using Entity.Repository.Respositories;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.Utilities;
using Entity.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Security.Cryptography.Xml;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Entity.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJobRepository _jobRepository;
        private readonly INationRepository _nationRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IDistrictRespository _districtRespository;
        private readonly IWardRepository _wardRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IJobRepository jobRepository, INationRepository nationRepository, ICityRepository cityRepository, IDistrictRespository districtRespository, IWardRepository wardRepository) : base()
        {
            _employeeRepository = employeeRepository;
            _jobRepository = jobRepository;
            _cityRepository = cityRepository;
            _districtRespository = districtRespository;
            _wardRepository = wardRepository;
            _nationRepository = nationRepository;
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

        public async Task<byte[]> DownloadReport(string keyWord)
        {
            string reportname = $"User_Wise_{Guid.NewGuid():N}.xlsx";
            var entity = await GetEmployeeByKeyWord(keyWord);
            var exportbytes = _employeeRepository.ExporttoExcel<EmployeeViewExport>(entity, reportname);
            return exportbytes;
        }

        public async Task<List<EmployeeViewExport>> GetEmloyee()
        {
            var lsteEmlpoyee = await _employeeRepository.GetAllAsync();
            var listResult = new List<EmployeeViewExport>();
            foreach (var n in lsteEmlpoyee)
            {
                var result = new EmployeeViewExport
                {
                    Id = n.Id,
                    Name = n.Name,
                    DateOfBirth = FuncUtilities.ConvertDateToString(n.DateOfBirth),
                    Age = n.Age,
                    JobName = _jobRepository.GetSingleByIdAsync(x => x.Id == n.JobId).Result.JobName,
                    NationName = _nationRepository.GetSingleByIdAsync(x => x.Id == n.NationId).Result.NationName,
                    IdentityCardNumber = n.IdentityCardNumber,
                    PhoneNumber = n.PhoneNumber,
                    CityName = _cityRepository.GetSingleByIdAsync(x => x.Id == n.CityId).Result.CityName,
                    DistrictName = _districtRespository.GetSingleByIdAsync(x => x.Id == n.DistrictId).Result.DistictName,
                    WardName = _wardRepository.GetSingleByIdAsync(x => x.Id == n.WardId).Result.WardName
                };
                listResult.Add(result);
            }
            return listResult;
        }

        public async Task<PaginationSet<EmployeeViewExport>> GetlistEmployee(string keyWord = "", int pageNumber = 1, int pageSize = 5)
        {
            var employees = await GetEmployeeByKeyWord(keyWord);
            PaginationSet<EmployeeViewExport> result = new PaginationSet<EmployeeViewExport>();
            result.CurrentPage = pageNumber;
            result.TotalPages = (int)(Math.Ceiling((double)employees.Count() / pageSize));
            result.Items = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            result.TotalCount = employees.Count();
            return result;
        }

        public async Task<ResponseEntity> GetAllEmployee(string keyWord = "", int soTrang = 1, int soPhanTuTrenTrang = 10)
        {
            var employees = await GetEmployeeByKeyWord(keyWord);
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
                Employee employee = new Employee();
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
                Employee employee = new Employee();
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
                var employee = await _employeeRepository.GetSingleByIdAsync(c => c.Id == model.id);
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
            try
            {
                using (var package = new ExcelPackage(new FileInfo(fullPath)))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    List<EmployeeViewModel> listEmployee = new List<EmployeeViewModel>();
                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        EmployeeViewModel employeeView = new EmployeeViewModel();
                        employeeView.Name = workSheet.Cells[i, 2].Value.ToString();
                        employeeView.DateOfBirth = workSheet.Cells[i, 3].Value.ToString();
                        int.TryParse(workSheet.Cells[i, 4].Value.ToString(), out int age);
                        employeeView.Age = age;
                        int.TryParse(workSheet.Cells[i, 5].Value.ToString(), out int jobId);
                        employeeView.JobId = jobId;
                        int.TryParse(workSheet.Cells[i, 6].Value.ToString(), out int nationId);
                        employeeView.NationId = nationId;
                        employeeView.IdentityCardNumber = workSheet.Cells[i, 7].Value?.ToString();
                        employeeView.PhoneNumber = workSheet.Cells[i, 8]?.Value?.ToString();
                        int.TryParse(workSheet.Cells[i, 9].Value.ToString(), out int cityId);
                        employeeView.CityId = cityId;
                        int.TryParse(workSheet.Cells[i, 10].Value.ToString(), out int districtId);
                        employeeView.DistrictId = districtId;
                        int.TryParse(workSheet.Cells[i, 11].Value.ToString(), out int wardId);
                        employeeView.WardId = wardId;
                        listEmployee.Add(employeeView);
                    }
                    return listEmployee;
                }
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        private async Task<IEnumerable<EmployeeViewExport>> GetEmployeeByKeyWord(string keyWord)
        {

            IEnumerable<EmployeeViewExport> entity = await GetEmloyee();

            if (entity.Any())
            {
                if (!string.IsNullOrEmpty(keyWord))
                {
                    List<EmployeeViewExport> lstGetByEmployeeName = entity.Where(n => n.Name.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    List<EmployeeViewExport> lstGetByNationName = entity.Where(n => n.NationName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    List<EmployeeViewExport> lstGetByJobName = entity.Where(n => n.JobName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    List<EmployeeViewExport> lstGetByCityName = entity.Where(n => n.CityName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    List<EmployeeViewExport> lstGetByDistrictName = entity.Where(n => n.DistrictName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    List<EmployeeViewExport> lstGetByWardName = entity.Where(n => n.WardName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToList();
                    IEnumerable<EmployeeViewExport> result = new List<EmployeeViewExport>();
                    result = result.Union(lstGetByEmployeeName);
                    result = result.Union(lstGetByNationName);
                    result = result.Union(lstGetByJobName);
                    result = result.Union(lstGetByCityName);
                    result = result.Union(lstGetByDistrictName);
                    result = result.Union(lstGetByWardName);
                    return result;
                }
                return entity;
            }
            return entity;
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
    }
}

