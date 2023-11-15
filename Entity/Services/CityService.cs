﻿using Entity.Constants;
using Entity.Models;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using System.Text.RegularExpressions;
namespace Entity.Services
{

    public class CityService : ICityService
    {
        ICityRepository _cityRepository;
        IWardRepository _wardRepository;
        IDistrictRespository _districtRepository;
        public CityService(ICityRepository cityRespository, IDistrictRespository districtRespository, IWardRepository wardRepository) : base()
        {
            _cityRepository = cityRespository;
            _wardRepository = wardRepository;
            _districtRepository = districtRespository;
        }

        public async Task<ResponseEntity> GetAllCity()
        {
            var cities = await _cityRepository.GetAllAsync();

            return new ResponseEntity(StatusCodeConstants.OK, cities, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetSingleCityById(int id)
        {
            var city = await _cityRepository.GetSingleByIdAsync(c => c.Id == id);
            if (city == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
            }
            return new ResponseEntity(StatusCodeConstants.OK, city, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> InsertCity(CityViewModel model)
        {
            using var transaction = _cityRepository.BeginTransaction();
            try
            {
                City Cities = new City();
                Cities.CityName = model.CityName;
                await _cityRepository.InsertAsync(Cities);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, Cities, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateCity(int id, CityViewModel model)
        {
            using var transaction = _cityRepository.BeginTransaction();
            try
            {
                var singleCity = await _cityRepository.GetSingleByIdAsync(c => c.Id == id);
                if (singleCity == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "aaa", MessageConstants.MESSAGE_ERROR_400);
                }
                singleCity.CityName = model.CityName;
                transaction.Commit();
                await _cityRepository.UpdateAsync(singleCity, singleCity);
                return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
            }
        }

        public async Task<ResponseEntity> DeleteCity(int id)
        {
            try
            {
                var singleCity = await _cityRepository.GetSingleByIdAsync(c => c.Id == id);
                if (singleCity == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_400);
                }
                await _cityRepository.DeleteAsync(singleCity);
                return new ResponseEntity(StatusCodeConstants.OK, singleCity, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllCityByCondition(int id, string name)
        {
            try
            {
                var cities = await _cityRepository.GetMultiBycondition(c => c.Id == id && c.CityName == name);
                return new ResponseEntity(StatusCodeConstants.OK, cities, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, ex.Message, MessageConstants.MESSAGE_ERROR_400);
            }
        }

        public async Task<ResponseEntity> GetSingleCity(int id)
        {
            var city = await _cityRepository.GetSingleByIdAsync(x => x.Id == id);
            if (city == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
            }
            var result = new CityViewModel
            {
                id = city.Id,
                CityName = city.CityName,
            };
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);

        }

        public Task<IEnumerable<City>> GetListCity()
        {
            var cities = _cityRepository.GetAllAsync();
            return cities;
        }
    }
}