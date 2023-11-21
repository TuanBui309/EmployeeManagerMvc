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
        Task<IEnumerable<EmployeeViewExport>> GetAllEmployeeByKeyWord(string keyWord, int? pageNumber = null);
        Task<IEnumerable<EmployeeViewExport>> ExportData(string keyWord);
        Task<IEnumerable<DegreeView>> GetAllDegreeByKeyWord(string keyWord, int? pageNumber = null);
        Task<IEnumerable<DistrictView>> GetAllDistrictByKeyWord(string keyWord, int? pageNumber = null);
        Task<IEnumerable<WardViewModel>> GetAllWardByKeyWord(string keyWord, int? pageNumber = null);
        byte[] ExporttoExcel<T>(IEnumerable<T> table, string filename);
        IDbTransaction BeginTransaction();
    }
}
