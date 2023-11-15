using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
namespace Entity.Services
{

    public class DistrictService : IDistrictService
    {
        IDistrictRespository _districtRepository;
        ICityRepository _cityRepository;
        public DistrictService(IDistrictRespository districtRespository, ICityRepository cityRepository) : base()
        {
            _districtRepository = districtRespository;
            _cityRepository = cityRepository;
        }

        public async Task<ResponseEntity> DeleteDistrict(int id)
        {
            try
            {
                var district = await _districtRepository.GetSingleByIdAsync(d => d.Id == id);
                if (district == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _districtRepository.DeleteAsync(district);
                return new ResponseEntity(StatusCodeConstants.OK, district, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllDistrict()
        {
            try
            {
                var districts = await _districtRepository.GetAllAsync();
                return new ResponseEntity(StatusCodeConstants.OK, districts, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, ex.Message, MessageConstants.MESSAGE_ERROR_400);
            }
        }

        private async Task<IEnumerable<DistrictView>> GetDistrictViews()
        {
            var districts = await _districtRepository.GetAllAsync();
            var LstResult = new List<DistrictView>();
            foreach (var district in districts)
            {
                var view = new DistrictView
                {
                    Id = district.Id,
                    CityName = _cityRepository.GetSingleByIdAsync(x => x.Id == district.CityId).Result.CityName,
                    DistictName = district.DistictName,
                };
                LstResult.Add(view);
            }
            return LstResult;
        }

        public async Task<PaginationSet<DistrictView>> GetListDistrict(int currentPage = 1, int pageSize = 5)
        {
            var districts = await GetDistrictViews();
            var resutl = new PaginationSet<DistrictView>();
            resutl.CurrentPage = currentPage;
            resutl.TotalPages = (districts.Count() / pageSize) + 1;
            resutl.Items = districts.Skip((currentPage - 1) * pageSize).Take(pageSize);
            resutl.TotalCount = districts.Count();
            return resutl;
        }

        public async Task<ResponseEntity> GetMultiDistrictByCondition(int cityId)
        {
            try
            {
                var districts = await _districtRepository.GetMultiBycondition(c => c.CityId == cityId);
                return new ResponseEntity(StatusCodeConstants.OK, districts, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }

        public async Task<ResponseEntity> GetSingleDistirctById(int id)
        {
            var district = await _districtRepository.GetSingleByIdAsync(d => d.Id == id);
            return new ResponseEntity(StatusCodeConstants.OK, district, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetSingleDistrict(int id)
        {
            var district = await _districtRepository.GetSingleByIdAsync(x => x.Id == id);
            if (district == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, district, MessageConstants.MESSAGE_ERROR_404);
            }
            var result = new DistrictViewModel
            {
                CityId = district.Id,
                DistictName = district.DistictName,
            };
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);

        }

        public async Task<ResponseEntity> InsertDistrict(DistrictViewModel model)
        {
            using var transaction = _districtRepository.BeginTransaction();
            try
            {
                District districts = new District();
                districts.CityId = model.CityId;
                districts.DistictName = model.DistictName;
                await _districtRepository.InsertAsync(districts);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, districts, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateDistrict(DistrictViewModel model)
        {
            using var transaction = _districtRepository.BeginTransaction();
            try
            {
                var district = await _districtRepository.GetSingleByIdAsync(d => d.Id == model.Id);
                if (district == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                district.CityId = model.CityId;
                district.DistictName = model.DistictName;
                await _districtRepository.UpdateAsync(district, district);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, district, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
            }
        }
    }
}
