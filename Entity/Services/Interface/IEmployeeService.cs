using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Services.ViewModels;
using System.Threading.Tasks;

namespace Entity.Services.Interface
{
	public interface IEmployeeService
	{
		Task<ResponseEntity> GetAllEmployee(string keyWord = "");
		Task<ResponseEntity> GetEmployeeById(int id);
		Task<ResponseEntity> InsertEmployee(EmployeeViewModel model);
		Task<ResponseEntity> UpdateEmployee(EmployeeViewModel model);
		Task<ResponseEntity> DeleteEmployee(int id);
		Task<byte[]> DownloadReport(string keyWord = "");
		List<EmployeeViewModel> ReadEmployeeFromExcel(string fullPath);
		Task<IEnumerable<EmployeeViewExport>> GetListEmployee(string keyWord = "", int? pageNumber = null);
		Task<ResponseEntity> GetSingleEmployee(int id);
		Task<ResponseEntity> GetTime();
	}
}
