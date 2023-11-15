using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
namespace Entity.Repository.Respositories
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
	}
	public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(EntityDbContext entityDbContext) : base(entityDbContext)
		{
		}
	}
}
