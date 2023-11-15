using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
namespace Entity.Respository.Respositories
{
	public interface IDistrictRespository : IRepository<District>
	{
	}
	public class DistrictRepository : RepositoryBase<District>, IDistrictRespository
	{
		public DistrictRepository(EntityDbContext enityDbContext)
			: base(enityDbContext)
		{
		}
	}
}
