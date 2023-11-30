using Entity.Models;
using Entity.Services.ViewModels;
using System.Data;
using System.Linq.Expressions;
namespace Entity.Repository.Interface
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T?> GetSingleByIdAsync(Expression<Func<T, bool>> match);
		Task<T> InsertAsync(T entity);
		Task<T> UpdateAsync(T existing, T entity);
		Task<T> DeleteAsync(T entity);
		Task<IEnumerable<T>> GetMultiBycondition(Expression<Func<T, bool>> match);
		Task<IEnumerable<EmployeeViewExport>> GetAllEmployeeByKeyWord(string keyWord, int? pageNumber = null, int currentPage = 1, int pageSize = 5);
		Task<IEnumerable<EmployeeViewExport>> ExportData(string keyWord);
		Task<IEnumerable<DegreeView>> GetAllDegreeByKeyWord(string keyWord, int? pageNumber = null, int currentPage = 1, int pageSize = 5);
		Task<IEnumerable<DistrictView>> GetAllDistrictByKeyWord(string keyWord, int? pageNumber = null, int currentPage = 1, int pageSize = 5);
		Task<IEnumerable<WardViewModel>> GetAllWardByKeyWord(string keyWord, int? pageNumber = null, int currentPage = 1, int pageSize = 5);
		Task<IEnumerable<City>> GetAllCityByKeWord(string keyWord, int? pageNumber = null, int currentPage = 1, int pageSize = 5);
		byte[] ExportToExcel<T>(IEnumerable<T> table, string filename);
	}
}
