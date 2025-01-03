using System.Net;
using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{

    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaServices;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaServices = villaService;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));


            }

            return View(list);
        }


        //Create numberVilla
        public async Task<IActionResult> CreateNumberVilla()
        {
            VillaNumberCreateVM VillaNumber = new VillaNumberCreateVM();
            var response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                VillaNumber.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n => new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString()
                });
            }
            return View(VillaNumber);    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNumberVilla(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());

                    }
                }
            }
			var res = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (res != null && res.IsSuccess)
			{
				model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result)).Select(n => new SelectListItem
				{
					Text = n.Name,
					Value = n.Id.ToString()
				});
			}

			return View(model);
        }


        //Update villaNumber
        public async Task<IActionResult> UpdateNumberVilla (int villaNo)
        {
            VillaNumberUpdateVM vm = new VillaNumberUpdateVM();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                vm.VillaNumber =  _mapper.Map<VillaNumberUpdateDTO>(model);
            }
			response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n => new SelectListItem
				{
					Text = n.Name,
					Value = n.Id.ToString()
				});
                return View(vm);
			}
			return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNumberVilla(VillaNumberUpdateVM model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
                var res = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
                if(res != null && res.IsSuccess)
                {
					model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result)).Select(n => new SelectListItem
					{
						Text = n.Name,
						Value = n.Id.ToString()
					});
				}
            }
			return View(model);

		}


        //Delete VillaNumber
        public async Task<IActionResult> DeleteNumberVilla (int villaNo)
        {
            VillaNumberDeleteVM vm = new VillaNumberDeleteVM();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                vm.VillaNumber = model;
            }
			response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(n => new SelectListItem
				{
					Text = n.Name,
					Value = n.Id.ToString()
				});
                return View(vm);
			}
			return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNumberVilla(VillaNumberDeleteVM model)
        {
           var respone = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            } 
            return View(model);
        }
	}
}
