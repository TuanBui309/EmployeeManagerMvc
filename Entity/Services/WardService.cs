using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using System.Transactions;

namespace Entity.Services
{

    public class WardService : IWardService
    {
        private readonly IWardRepository _wardRepository;
        private readonly IDistrictRespository _districtRespository;
        public WardService(IWardRepository wardRepository, IDistrictRespository districtRespository) : base()
        {
            _wardRepository = wardRepository;
            _districtRespository = districtRespository;
        }

        public async Task<ResponseEntity> DeleteWard(int id)
        {
            try
            {
                var ward = await _wardRepository.GetSingleByIdAsync(c => c.Id == id);
                if (ward == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _wardRepository.DeleteAsync(ward);
                return new ResponseEntity(StatusCodeConstants.OK, ward, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllWard()
        {
            var wards = await _wardRepository.GetAllAsync();
            return new ResponseEntity(StatusCodeConstants.OK, wards, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<PaginationSet<WardViewModel>> GetListWard(string keyWord = "", int currentPage = 1, int pageSize = 5)
        {
            var wards = await _wardRepository.GetAllWardByKeyWord(keyWord);
            var resutl = new PaginationSet<WardViewModel>();
            resutl.CurrentPage = currentPage;
            resutl.TotalPages = (int)(Math.Ceiling((double)wards.Count() / pageSize));
            resutl.Items = wards.Skip((currentPage - 1) * pageSize).Take(pageSize);
            resutl.TotalCount = wards.Count();
            return resutl;
        }

        public async Task<ResponseEntity> GetMultiWardByCondition(int DistrictId)
        {
            try
            {
                var wards = await _wardRepository.GetMultiBycondition(c => c.DistrictId == DistrictId);
                if (wards == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                return new ResponseEntity(StatusCodeConstants.OK, wards, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }

        public async Task<ResponseEntity> GetSingleWard(int id)
        {
            var ward = await _wardRepository.GetSingleByIdAsync(x => x.Id == id);
            if (ward == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
            }
            var result = new WardViewModel
            {
                Id = ward.Id,
                DistrictId = ward.DistrictId,
                DistrictName = _districtRespository.GetSingleByIdAsync(x => x.Id == ward.DistrictId).Result.DistictName,
                WardName = ward.WardName,
            };
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);

        }

        public async Task<ResponseEntity> GetSingleWardById(int id)
        {
            try
            {
                var ward = await _wardRepository.GetSingleByIdAsync(c => c.Id == id);
                if (ward == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                return new ResponseEntity(StatusCodeConstants.OK, ward, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }

        public async Task<ResponseEntity> InsertWard(WardViewModel model)
        {
            using var transaction = _wardRepository.BeginTransaction();
            try
            {
                Ward wards = new Ward();
                wards.DistrictId = model.DistrictId;
                wards.WardName = model.WardName;
                await _wardRepository.InsertAsync(wards);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, wards, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }
        public async Task<ResponseEntity> UpdateWard(WardViewModel model)
        {
            using var transaction = _wardRepository.BeginTransaction();
            try
            {
                var ward = await _wardRepository.GetSingleByIdAsync(c => c.Id == model.Id);
                if (ward == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                ward.DistrictId = model.DistrictId;
                ward.WardName = model.WardName;
                await _wardRepository.UpdateAsync(ward, ward);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
            }
        }
    }
}
