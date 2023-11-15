using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Services.ViewModels;

namespace Entity.Services.Interface
{
    public interface IWardService
    {
        Task<ResponseEntity> GetAllWard();
        Task<ResponseEntity> GetSingleWardById(int id);
        Task<ResponseEntity> GetMultiWardByCondition(int DistrictId);
        Task<ResponseEntity> InsertWard(WardViewModel model);
        Task<ResponseEntity> UpdateWard(WardViewModel model);
        Task<ResponseEntity> DeleteWard(int id);
        Task<ResponseEntity> GetSingleWard(int id);
        Task<PaginationSet<WardViewModel>> GetListWard(int currentPage = 1, int pageSize = 5);
    }
}
