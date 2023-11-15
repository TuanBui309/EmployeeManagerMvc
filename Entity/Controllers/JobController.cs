using Entity.Models;
using Entity.Services;
using Microsoft.AspNetCore.Mvc;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using Entity.Constants;

namespace Entity.Controllers
{

	public class JobController : Controller
	{
		private readonly IJobService _jobService;
		public JobController(IJobService jobService)
		{
			_jobService = jobService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _jobService.GetAllJob();
			return View(result.Content);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(JobViewModel model)
		{
			var result = await _jobService.InsertJob(model);
			if (result.StatusCode == StatusCodeConstants.OK)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("");
			}
			TempData["Error"] = result.Message;
			return View(result);
		}

		public async Task<IActionResult> Details(int id)
		{
			var result = await _jobService.GetSingleJob(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var result = await _jobService.GetSingleJob(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(JobViewModel model)
		{
			var result = await _jobService.UpdateJob(model);
			if (result.StatusCode == StatusCodeConstants.OK)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("");
			}
			TempData["Error"] = result.Message;
			return View(result.Content);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = await _jobService.GetSingleJob(id);
			if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
			{
				TempData["Error"] = "Not found";
				return RedirectToAction("");
			}
			return View(result.Content);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var result = await _jobService.DeleteJob(id);
			if (result.StatusCode == StatusCodeConstants.OK)
			{
				TempData["Success"] = result.Message;
				return RedirectToAction("");
			}
			TempData["Error"] = result.Message;
			return RedirectToAction("");
		}

		[HttpGet("GetAllJob")]
		public async Task<IActionResult> GetAllJob()
		{
			return await _jobService.GetAllJob();
		}

		[HttpGet("GetJobById")]
		public async Task<IActionResult> GetJobById(int id)
		{
			return await _jobService.GetSingleJobById(id);
		}


	}
}
