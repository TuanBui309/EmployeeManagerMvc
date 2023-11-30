using Entity.Constants;
using Entity.Models;
using Entity.Repository.Repositories;
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
				var job = await _jobRepository.GetSingleByIdAsync(c => c.Id == id);
				if (job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				await _jobRepository.DeleteAsync(job);
				return new ResponseEntity(StatusCodeConstants.OK, job, MessageConstants.DELETE_SUCCESS);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.DELETE_ERROR);
			}
		}

		public async Task<ResponseEntity> GetAllJob()
		{
			var jobs = await _jobRepository.GetAllAsync();
			return new ResponseEntity(StatusCodeConstants.OK, jobs, MessageConstants.MESSAGE_SUCCESS_200);
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
				var job = await _jobRepository.GetSingleByIdAsync(c => c.Id == id);
				if (job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				return new ResponseEntity(StatusCodeConstants.OK, job, MessageConstants.MESSAGE_SUCCESS_200);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.MESSAGE_ERROR_404);
			}
		}

		public async Task<ResponseEntity> InsertJob(JobViewModel model)
		{
			try
			{
                Job jobs = new()
                {
                    JobName = model.JobName
                };
                await _jobRepository.InsertAsync(jobs);
				return new ResponseEntity(StatusCodeConstants.OK, jobs, MessageConstants.INSERT_SUCCESS);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.INSERT_ERROR);
			}
		}

		public async Task<ResponseEntity> UpdateJob(JobViewModel model)
		{
			try
			{
				var job = await _jobRepository.GetSingleByIdAsync(c => c.Id == model.Id);
				if (job == null)
				{
					return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "", MessageConstants.MESSAGE_ERROR_404);
				}
				job.Id = model.Id;
				job.JobName = model.JobName;
				await _jobRepository.UpdateAsync(job, job);
				return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.MESSAGE_SUCCESS_200);
			}
			catch (Exception ex)
			{
				return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.UPDATE_ERROR);
			}
		}
	}
}
