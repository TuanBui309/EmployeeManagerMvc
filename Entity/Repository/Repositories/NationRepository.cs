using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
namespace Entity.Repository.Repositories
{
	public interface INationRepository : IRepository<Nation>
	{
	}

	public class NationRepositoty : RepositoryBase<Nation>, INationRepository
	{

		public NationRepositoty(EntityDbContext entityDbContext) 
			: base(entityDbContext)
		{
		}
	}
}
