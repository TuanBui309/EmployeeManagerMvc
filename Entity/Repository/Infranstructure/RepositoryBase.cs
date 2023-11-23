using Entity.Data_Access;
using Entity.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Entity.Services.ViewModels;
using Entity.Services.Utilities;
using Entity.Pagination;

namespace Entity.Repository.Infranstructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly EntityDbContext _toDoContext;
        private readonly DbSet<T> _table;

        public RepositoryBase(EntityDbContext toDoContext)
        {
            _toDoContext = toDoContext;
            _table = _toDoContext.Set<T>();
        }

        public async Task<T> DeleteAsync(T entity)
        {
            var result = _table.Remove(entity);
            await _toDoContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var results = _table;
                return await results.ToListAsync();
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public async Task<T?> GetSingleByIdAsync(Expression<Func<T, bool>> match)
        {
            var result = await _table.SingleOrDefaultAsync(match);
            return result;
        }

        public async Task<T> InsertAsync(T entity)
        {

            var result = await _table.AddAsync(entity);
            await _toDoContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T> UpdateAsync(T existing, T entity)
        {
            _toDoContext.Entry(existing).CurrentValues.SetValues(entity);
            await _toDoContext.SaveChangesAsync();
            return existing;
        }

        public async Task<IEnumerable<T>> GetMultiBycondition(Expression<Func<T, bool>> match)
        {
            return await _table.Where(match).ToListAsync();
        }

        public byte[] ExportToExcel<T>(IEnumerable<T> _table, string filename)
        {
            using ExcelPackage pack = new();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(_table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }

        public System.Data.IDbTransaction BeginTransaction()
        {
            var transaction = _toDoContext.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
        public async Task<IEnumerable<EmployeeViewExport>> ExportData(string keyWord)
        {
            return await (from e in _toDoContext.Employees
                          join j in _toDoContext.Jobs on e.JobId equals j.Id
                          join n in _toDoContext.Nations on e.NationId equals n.Id
                          join c in _toDoContext.Cities on e.CityId equals c.Id
                          join d in _toDoContext.Districts on e.DistrictId equals d.Id
                          join w in _toDoContext.Warns on e.WardId equals w.Id
                          select new EmployeeViewExport
                          {
                              Id = e.Id,
                              Name = e.Name,
                              DateOfBirth = FuncUtilities.ConvertDateToString(e.DateOfBirth),
                              Age = e.Age,
                              JobName = j.JobName,
                              NationName = n.NationName,
                              IdentityCardNumber = e.IdentityCardNumber,
                              PhoneNumber = e.PhoneNumber,
                              CityName = c.CityName,
                              DistrictName = d.DistictName,
                              WardName = w.WardName,
                          }).Where(x => x.Name.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                          x.JobName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                          x.NationName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                          x.CityName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                          x.DistrictName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                          x.WardName.Trim().ToLower().Contains(keyWord.Trim().ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeViewExport>> GetAllEmployeeByKeyWord(string keyWord, int? pageNumber = null)
        {
            var employee = (from e in _toDoContext.Employees
                            join j in _toDoContext.Jobs on e.JobId equals j.Id
                            join n in _toDoContext.Nations on e.NationId equals n.Id
                            join c in _toDoContext.Cities on e.CityId equals c.Id
                            join d in _toDoContext.Districts on e.DistrictId equals d.Id
                            join w in _toDoContext.Warns on e.WardId equals w.Id
                            select new EmployeeViewExport
                            {
                                Id = e.Id,
                                Name = e.Name,
                                DateOfBirth = FuncUtilities.ConvertDateToString(e.DateOfBirth),
                                Age = e.Age,
                                JobName = j.JobName,
                                NationName = n.NationName,
                                IdentityCardNumber = e.IdentityCardNumber,
                                PhoneNumber = e.PhoneNumber,
                                CityName = c.CityName,
                                DistrictName = d.DistictName,
                                WardName = w.WardName,
                            }).Where(x => x.Name.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                            x.JobName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                            x.NationName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                            x.CityName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                            x.DistrictName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                            x.WardName.Trim().ToLower().Contains(keyWord.Trim().ToLower()));
            return await GetPagination.GetPage(employee, pageNumber);
        }

        public async Task<IEnumerable<DegreeView>> GetAllDegreeByKeyWord(string keyWord, int? pageNumber = null)
        {
            var degees = (from degree in _toDoContext.Degrees
                          join e in _toDoContext.Employees on degree.EmployeeId equals e.Id
                          select new DegreeView
                          {
                              Id = degree.Id,
                              EmployeeName = e.Name,
                              DegreeName = degree.DegreeName,
                              DateRange = FuncUtilities.ConvertDateToString(degree.DateRange),
                              IssuedBy = degree.IssuedBy,
                              DateOfExpiry = FuncUtilities.ConvertDateToString(degree.DateOfExpiry)
                          }).Where(x => x.EmployeeName.ToLower().Trim().Contains(keyWord.ToLower()) ||
                          x.DegreeName.Trim().ToLower().Contains(keyWord.ToLower()) ||
                          x.IssuedBy.Trim().ToLower().Contains(keyWord.ToLower()));
            return await GetPagination.GetPage(degees, pageNumber);
        }

        public async Task<IEnumerable<DistrictView>> GetAllDistrictByKeyWord(string keyWord, int? pageNumber = null)
        {
            var districts = (from district in _toDoContext.Districts
                             join c in _toDoContext.Cities on district.CityId equals c.Id
                             select new DistrictView
                             {
                                 Id = district.Id,
                                 CityName = c.CityName,
                                 DistictName = district.DistictName,
                             }).Where(x => x.DistictName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                             x.CityName.Trim().ToLower().Contains(keyWord.Trim().ToLower()));
            return await GetPagination.GetPage(districts, pageNumber); 
        }

        public async Task<IEnumerable<WardViewModel>> GetAllWardByKeyWord(string keyWord, int? pageNumber)
        {
            var wards = (from ward in _toDoContext.Warns
                         join d in _toDoContext.Districts on ward.DistrictId equals d.Id
                         select new WardViewModel
                         {
                             Id = ward.Id,
                             DistrictId = ward.DistrictId,
                             DistrictName = d.DistictName,
                             WardName = ward.WardName,
                         }).Where(x => x.DistrictName.Trim().ToLower().Contains(keyWord.Trim().ToLower()) ||
                         x.WardName.Trim().ToLower().Contains(keyWord.Trim().ToLower()));
            return await GetPagination.GetPage(wards, pageNumber);
        }


    }
}
