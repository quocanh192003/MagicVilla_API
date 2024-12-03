using System.Net;
using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{

    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, APIResponse apiResponse)
        {
            _villaNumberService = villaNumberService;
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
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNumberVilla(VillaNumberCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                } 
            }
            return View(model);
        }
    }
}
