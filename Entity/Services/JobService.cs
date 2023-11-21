using Entity.Constants;
using Entity.Models;
using Entity.Repository.Repositories;
using Entity.Respository.Respositories;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
namespace Entity.Services
{

	public class JobService : IJobService
	{
		private readonly IJobRepository _jobRepository;
		public JobService(IJobRepository jobRepository) : base()
		{
			_jobRepository = jobRepository;
		}

		public async Task<ResponseEntity> DeleteJob(int id)
		{
			try
			{
				var Job = await _jobRepository.GetSingleByIdAsync(c => c.Id == id);
				if (Job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				await _jobRepository.DeleteAsync(Job);
				return new ResponseEntity(StatusCodeConstants.OK, Job, MessageConstants.DELETE_SUCCESS);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
			}
		}

		public async Task<ResponseEntity> GetAllJob()
		{
			var Jobs = await _jobRepository.GetAllAsync();
			return new ResponseEntity(StatusCodeConstants.OK, Jobs, MessageConstants.MESSAGE_SUCCESS_200);
		}

		public async Task<ResponseEntity> GetSingleJob(int id)
		{
			var job = await _jobRepository.GetSingleByIdAsync(x => x.Id == id);
			if (job == null)
			{
				return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
			}
			var result = new JobViewModel { Id = job.Id, JobName = job.JobName };
			return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.MESSAGE_SUCCESS_200);
		}

		public async Task<ResponseEntity> GetSingleJobById(int id)
		{
			try
			{
				var Job = await _jobRepository.GetSingleByIdAsync(c => c.Id == id);
				if (Job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				return new ResponseEntity(StatusCodeConstants.OK, Job, MessageConstants.MESSAGE_SUCCESS_200);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
			}
		}

		public async Task<ResponseEntity> InsertJob(JobViewModel model)
		{
			using var transaction = _jobRepository.BeginTransaction();
			try
			{
				Job Jobs = new Job();
				Jobs.JobName = model.JobName;
				await _jobRepository.InsertAsync(Jobs);
				transaction.Commit();
				return new ResponseEntity(StatusCodeConstants.OK, Jobs, MessageConstants.INSERT_SUCCESS);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
			}
		}

		public async Task<ResponseEntity> UpdateJob(JobViewModel model)
		{
			using var transaction = _jobRepository.BeginTransaction();
			try
			{

				var Job = await _jobRepository.GetSingleByIdAsync(c => c.Id == model.Id);
				if (Job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				Job.Id = model.Id;
				Job.JobName = model.JobName;
				await _jobRepository.UpdateAsync(Job, Job);
				transaction.Commit();
				return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.MESSAGE_SUCCESS_200);
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
			}
		}
	}
}
