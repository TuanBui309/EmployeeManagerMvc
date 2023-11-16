using Entity.Data_Access;
using Entity.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.Data.Common;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage;

namespace Entity.Repository.Infranstructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly EntityDbContext _toDoContext;
        private DbSet<T> table;

        public RepositoryBase(EntityDbContext toDoContext)
        {
            _toDoContext = toDoContext;
            table = _toDoContext.Set<T>();
        }

        public async Task<T> DeleteAsync(T entity)
        {
            var result = table.Remove(entity);
            await _toDoContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var results = table;
                return await results.ToListAsync();
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public async Task<T?> GetSingleByIdAsync(Expression<Func<T, bool>> match)
        {
            var result = await table.SingleOrDefaultAsync(match);
            return result;
        }

        public async Task<T> InsertAsync(T entity)
        {

            var result = await table.AddAsync(entity);
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
            return await table.Where(match).ToListAsync();
        }

        public byte[] ExporttoExcel<T1>(IEnumerable<T1> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }

        public System.Data.IDbTransaction BeginTransaction()
        {
            var transaction = _toDoContext.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
    }
}
