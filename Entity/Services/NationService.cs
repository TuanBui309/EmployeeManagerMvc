using Entity.Constants;
using Entity.Models;
using Entity.Repository.Repositories;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
namespace Entity.Services
{

    public class NationService : INationService
    {
        INationRepository _NationRepository;
        public NationService(INationRepository NationRepository) : base()
        {
            _NationRepository = NationRepository;
        }

        public async Task<ResponseEntity> DeleteNation(int id)
        {
            try
            {
                var Nation = await _NationRepository.GetSingleByIdAsync(c => c.Id == id);
                if (Nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _NationRepository.DeleteAsync(Nation);
                return new ResponseEntity(StatusCodeConstants.OK, Nation, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllNation()
        {
            var Nations = await _NationRepository.GetAllAsync();
            return new ResponseEntity(StatusCodeConstants.OK, Nations, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetSingleNation(int id)
        {
            var nation = await _NationRepository.GetSingleByIdAsync(x => x.Id == id);
            if (nation == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
            }
            var result = new NationViewModel { Id = nation.Id, NationName = nation.NationName };
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetSingleNationById(int id)
        {
            try
            {
                var Nation = await _NationRepository.GetSingleByIdAsync(c => c.Id == id);
                if (Nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                return new ResponseEntity(StatusCodeConstants.OK, Nation, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }
        public async Task<ResponseEntity> InsertNation(NationViewModel model)
        {
            using var transaction = _NationRepository.BeginTransaction();
            try
            {
                Nation Nations = new Nation();
                Nations.NationName = model.NationName;
                await _NationRepository.InsertAsync(Nations);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, Nations, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }
        public async Task<ResponseEntity> UpdateNation(NationViewModel model)
        {
            using var transaction = _NationRepository.BeginTransaction();
            try
            {
                var Nation = await _NationRepository.GetSingleByIdAsync(c => c.Id == model.Id);
                if (Nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                Nation.Id = model.Id;
                Nation.NationName = model.NationName;
                await _NationRepository.UpdateAsync(Nation, Nation);
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
