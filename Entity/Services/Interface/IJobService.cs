using Entity.Constants;
using Entity.Models;
using Entity.Services.ViewModels;
namespace Entity.Services.Interface
{
    public interface IJobService
    {
        Task<ResponseEntity> GetAllJob();
        Task<ResponseEntity> GetSingleJobById(int id);
        Task<ResponseEntity> InsertJob(JobViewModel model);
        Task<ResponseEntity> UpdateJob(JobViewModel model);
        Task<ResponseEntity> DeleteJob(int id);
        Task<ResponseEntity> GetSingleJob(int id);
    }
}
