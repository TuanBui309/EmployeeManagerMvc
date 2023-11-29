using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Services.ViewModels;
namespace Entity.Services.Interface
{
    public interface IDistrictService
    {
        Task<ResponseEntity> GetAllDistrict();
        Task<ResponseEntity> GetSingleDistirctById(int id);
        Task<ResponseEntity> InsertDistrict(DistrictViewModel model);
        Task<ResponseEntity> UpdateDistrict(DistrictViewModel model);
        Task<ResponseEntity> DeleteDistrict(int id);
        Task<ResponseEntity> GetSingleDistrict(int id);
        Task<IEnumerable<DistrictView>> GetListDistrict(string keyWord = "", int? pageNumber = null);
        Task<ResponseEntity> GetMultiDistrictByCondition(int cityId);



	}
}
