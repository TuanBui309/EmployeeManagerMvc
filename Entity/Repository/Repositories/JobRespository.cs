using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
namespace Entity.Repository.Repositories
{
	public interface IJobRepository : IRepository<Job>
	{
	}

	public class JobRepository : RepositoryBase<Job>, IJobRepository
	{

		public JobRepository(EntityDbContext entityDbContext) : base(entityDbContext)
		{
		}
	}
}
