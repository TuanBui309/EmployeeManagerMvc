using Entity.Data_Access;
using Entity.Models;
using Entity.Repository.Infranstructure;
using Entity.Repository.Interface;
using System.Data.Common;

namespace Entity.Repository.Repositories
{
    public interface IDegreeRepository : IRepository<Degree>
    {
    }

    public class DegreeRepository : RepositoryBase<Degree>, IDegreeRepository
    {
        public DegreeRepository(EntityDbContext entityDbContext) : base(entityDbContext)
        {
        }

    }

}
