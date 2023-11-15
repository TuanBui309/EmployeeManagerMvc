using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Services.ViewModels;

namespace Entity.Services.Interface
{
    public interface IDegreeService
    {
        Task<ResponseEntity> GetAllDegree();
        Task<ResponseEntity> GetDegreeById(int id);
        Task<ResponseEntity> InsertDegree(DegreeViewModel model);
        Task<ResponseEntity> UpdateDegree(DegreeViewModel model);
        Task<ResponseEntity> DeleteDegree(int id);
        Task<PaginationSet<DegreeView>> GetListDegree(string keyWord = "",int currenrPage=1,int pageSize=5);
        Task<ResponseEntity> GetSingleDegree(int id);
    }
}
