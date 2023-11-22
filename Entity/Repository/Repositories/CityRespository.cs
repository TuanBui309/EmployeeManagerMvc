using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
using System.Data;

namespace Entity.Respository.Respositories
{

    public interface ICityRepository : IRepository<City>
    {
    }
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(EntityDbContext enityDbContext)
            : base(enityDbContext)
        {
        }
    }
}

