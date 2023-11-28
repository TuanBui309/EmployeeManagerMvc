using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
namespace Entity.Respository.Respositories
{
	public interface IWardRepository:IRepository<Ward>
	{
	}

	public class WardRespository : RepositoryBase<Ward>, IWardRepository
	{

		public WardRespository(EntityDbContext entityDbContext) : base(entityDbContext)
		{
		}
	}
}
