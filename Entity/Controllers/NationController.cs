using Entity.Models;
using Entity.Services;
using Microsoft.AspNetCore.Mvc;
using Entity.Services.Interface;
using Entity.Services.ViewModels;
using Entity.Constants;

namespace Entity.Controllers
{

    public class NationController : Controller
    {
        INationService _nationService;

        public NationController(INationService nationService)
        {
            _nationService = nationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _nationService.GetAllNation();
            return View(result.Content);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(NationViewModel model)
        {
            var result = await _nationService.InsertNation(model);
            if (result.StatusCode == StatusCodeConstants.OK)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("");
            }
            TempData["Error"]=result.Message;
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _nationService.GetSingleNation(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _nationService.GetSingleNation(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NationViewModel model)
        {
            var result = await _nationService.UpdateNation(model);
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
            var result = await _nationService.GetSingleNation(id);
            if (result.StatusCode == StatusCodeConstants.NOT_FOUND)
            {
                TempData["Error"] = "Not found";
                return RedirectToAction("");
            }
            return View(result.Content);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var result = await _nationService.DeleteNation(id);
            if (result.StatusCode ==StatusCodeConstants.OK)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("");
            }
            TempData["Error"] = result.Message;
            return View(result);
        }

        [HttpGet("GetAllNation")]
        public async Task<IActionResult> GetAllNation()
        {
            return await _nationService.GetAllNation();
        }

        [HttpGet("GetNationById")]
        public async Task<IActionResult> GetNationById(int id)
        {
            return await _nationService.GetSingleNationById(id);
        }
    }
}
