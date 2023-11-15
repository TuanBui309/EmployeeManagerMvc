using Entity.Constants;
using Entity.Models;
using Entity.Pagination;
using Entity.Repository.Repositories;
using Entity.Repository.Respositories;
using Entity.Services.Interface;
using Entity.Services.Utilities;
using Entity.Services.ViewModels;

namespace Entity.Services
{
    public class DegreeService : IDegreeService
    {
        IDegreeRepository _degreeRepository;
        IEmployeeRepository _employeeRepository;
        public DegreeService(IDegreeRepository degreeRepository, IEmployeeRepository employeeRepository) : base()
        {
            _degreeRepository = degreeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<ResponseEntity> DeleteDegree(int id)
        {
            try
            {
                var degree = await _degreeRepository.GetSingleByIdAsync(x => x.Id == id);
                if (degree == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                await _degreeRepository.DeleteAsync(degree);
                return new ResponseEntity(StatusCodeConstants.OK, degree, MessageConstants.DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
            }
        }

        public async Task<ResponseEntity> GetAllDegree()
        {
            var degrees = await _degreeRepository.GetAllAsync();
            return new ResponseEntity(StatusCodeConstants.OK, degrees, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> GetDegreeById(int id)
        {
            try
            {
                var degree = await _degreeRepository.GetSingleByIdAsync(x => x.Id == id);
                if (degree == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }

                return new ResponseEntity(StatusCodeConstants.OK, degree, MessageConstants.MESSAGE_SUCCESS_200);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, ex.Message, MessageConstants.MESSAGE_ERROR_404);
            }
        }

        private async Task<IEnumerable<DegreeView>> GetListDegreeBykeyWord(string keyWord = "")
        {
            IEnumerable<DegreeView> entity = await GetDegrees();
            if (entity.Count() != 0)
            {
                if (keyWord != null)
                {
                    List<DegreeView> LstGetByName = entity.Where(x => x.employeeName.ToLower().Trim().Contains(keyWord.ToLower())).ToList();
                    List<DegreeView> LstGetByDateRange = entity.Where(x => x.DateRange.Trim().Contains(keyWord.ToLower())).ToList();
                    List<DegreeView> LstGetByDateOfExpiry = entity.Where(x => x.DateOfExpiry.Trim().Contains(keyWord.ToLower())).ToList();
                    IEnumerable<DegreeView> result = new List<DegreeView>();
                    result = result.Union(LstGetByName);
                    result = result.Union(LstGetByDateOfExpiry);
                    result = result.Union(LstGetByDateRange);
                    return result;
                }
                return entity;
            }
            return entity;

        }

        private async Task<IEnumerable<DegreeView>> GetDegrees()
        {
            var ListDegree = await _degreeRepository.GetAllAsync();
            var Listresult = new List<DegreeView>();
            foreach (var degree in ListDegree)
            {
                var result = new DegreeView
                {
                    Id = degree.Id,
                    employeeName = _employeeRepository.GetSingleByIdAsync(x => x.Id == degree.EmployeeId).Result.Name,
                    DegreeName = degree.DegreeName,
                    DateRange = FuncUtilities.ConvertDateToString(degree.DateRange),
                    IssuedBy = degree.IssuedBy,
                    DateOfExpiry = FuncUtilities.ConvertDateToString(degree.DateOfExpiry)
                };
                Listresult.Add(result);
            }
            return Listresult;

        }

        public async Task<ResponseEntity> GetSingleDegree(int id)
        {
            var degree = await _degreeRepository.GetSingleByIdAsync(x => x.Id == id);
            if (degree == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
            }
            var result = new DegreeViewModel
            {
                EmployeeId = degree.EmployeeId,
                DegreeName = degree.DegreeName,
                DateRange = FuncUtilities.ConvertDateToString(degree.DateRange),
                IssuedBy = degree.IssuedBy,
                DateOfExpiry = FuncUtilities.ConvertDateToString(degree.DateOfExpiry)

            };
            return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);
        }

        public async Task<ResponseEntity> InsertDegree(DegreeViewModel model)
        {
            using var transaction = _degreeRepository.BeginTransaction();
            try
            {
                Degree degrees = new Degree();
                degrees.EmployeeId = model.EmployeeId;
                degrees.DegreeName = model.DegreeName;
                degrees.DateRange = FuncUtilities.ConvertStringToDate(model.DateRange);
                degrees.IssuedBy = model.IssuedBy;
                degrees.DateOfExpiry = FuncUtilities.ConvertStringToDate(model.DateOfExpiry);
                await _degreeRepository.InsertAsync(degrees);
                transaction.Commit();
                return new ResponseEntity(StatusCodeConstants.OK, degrees, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateDegree(DegreeViewModel model)
        {
            using var transaction = _degreeRepository.BeginTransaction();
            try
            {
                var degree = await _degreeRepository.GetSingleByIdAsync(x => x.Id == model.id);
                if (degree == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
                }
                degree.EmployeeId = model.EmployeeId;
                degree.DegreeName = model.DegreeName;
                degree.DateRange = FuncUtilities.ConvertStringToDate(model.DateRange);
                degree.IssuedBy = model.IssuedBy;
                degree.DateOfExpiry = FuncUtilities.ConvertStringToDate(model.DateOfExpiry);
                transaction.Commit();
                await _degreeRepository.UpdateAsync(degree, degree);
                return new ResponseEntity(StatusCodeConstants.OK, degree, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
            }
        }

        public async Task<PaginationSet<DegreeView>> GetListDegree(string keyWord = "",int currenrPage = 1,int pageSize=5)
        {
            var degree = await GetListDegreeBykeyWord(keyWord);
            PaginationSet<DegreeView> result = new PaginationSet<DegreeView>();
            result.CurrentPage = currenrPage;
            result.TotalPages = (degree.Count() / pageSize) + 1;
            result.Items = degree.Skip((currenrPage - 1) * pageSize).Take(pageSize);
            result.TotalCount = degree.Count();
            return result;
        }
    }
}
