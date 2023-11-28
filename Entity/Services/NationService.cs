using Entity.Constants;
using Entity.Models;
using Entity.Repository.Repositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
namespace Entity.Services
{

    public class NationService : INationService
    {
        private readonly INationRepository _nationRepository;
        public NationService(INationRepository nationRepository) : base()
        {
            _nationRepository = nationRepository;
        }

        public async Task<ResponseEntity> DeleteNation(int id)
        {
            try
            {
                var nation = await _nationRepository.GetSingleByIdAsync(c => c.Id == id);
                if (nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _nationRepository.DeleteAsync(nation);
                return new ResponseEntity(StatusCodeConstants.OK, nation, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllNation()
        {
            var nations = await _nationRepository.GetAllAsync();
            return new ResponseEntity(StatusCodeConstants.OK, nations, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetSingleNation(int id)
        {
            var nation = await _nationRepository.GetSingleByIdAsync(x => x.Id == id);
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
                var nation = await _nationRepository.GetSingleByIdAsync(c => c.Id == id);
                if (nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                return new ResponseEntity(StatusCodeConstants.OK, nation, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }

        public async Task<ResponseEntity> InsertNation(NationViewModel model)
        {
            using var transaction = _nationRepository.BeginTransaction();
            try
            {
                Nation nations = new()
                {
                    NationName = model.NationName
                };
                await _nationRepository.InsertAsync(nations);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, nations, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateNation(NationViewModel model)
        {
            using var transaction = _nationRepository.BeginTransaction();
            try
            {
                var nation = await _nationRepository.GetSingleByIdAsync(c => c.Id == model.Id);
                if (nation == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
				nation.Id = model.Id;
				nation.NationName = model.NationName;
                await _nationRepository.UpdateAsync(nation, nation);
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
