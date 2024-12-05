using System.Net;
using AutoMapper;
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
        private readonly APIResponse _apiResponse;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService, APIResponse apiResponse)
        {
            _villaNumberService = villaNumberService;
            _villaServices = villaService;
            _mapper = mapper;
            _apiResponse = apiResponse;
        }

        [HttpGet]
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();
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
            var response = await _villaServices.GetAllAsync<APIResponse>();
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
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                } 
            }
            return View(model);
        }
    }
}
