using Entity.Constants;
using Entity.Models;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;

namespace Entity.Services
{

	public class DistrictService : IDistrictService
	{
		private readonly IDistrictRespository _districtRepository;
		
		public DistrictService(IDistrictRespository districtRespository) : base()
		{
			_districtRepository = districtRespository;
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

		public async Task<IEnumerable<DistrictView>> GetListDistrict(string keyWord = "", int? pageNumber = null)
		{
			var districts = await _districtRepository.GetAllDistrictByKeyWord(keyWord,pageNumber);
			return districts;
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
				CityId = district.CityId,
				DistictName = district.DistictName,
			};
			return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);
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

		public async Task<ResponseEntity> InsertDistrict(DistrictViewModel model)
		{
			using var transaction = _districtRepository.BeginTransaction();
			try
			{
                District districts = new()
                {
                    CityId = model.CityId,
                    DistictName = model.DistictName
                };
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
