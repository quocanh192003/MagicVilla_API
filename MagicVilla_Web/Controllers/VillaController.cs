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
    

    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public VillaController(IVillaService villaService, IMapper mapper, APIResponse response)
        {
            _villaService = villaService;
            _mapper = mapper;
            _response = response;
        }

        [HttpGet]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            
            
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }


        //Create Villa
         public async Task<IActionResult> CreateVilla()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(villaCreateDTO createDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(createDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa created successfully!";
                    return RedirectToAction(nameof(IndexVilla));
                }

            }
            TempData["error"] = "Error encountered!";

            return View(createDTO);
        }

        //Update villa
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<villaUpdateDTO>(model));
            }
            return NotFound();
         }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(villaUpdateDTO model){
            
            if(ModelState.IsValid)
            {
                TempData["success"] = "Villa update successfully!";

                var response = await _villaService.UpdateAsync<APIResponse>(model);
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered!";

            return View(model);
        }
        //Delete villa

        public async Task<IActionResult> DeleteVilla(int VillaId)
        {
            var villa = await _villaService.GetAsync<APIResponse>(VillaId);
            if (villa != null && villa.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(villa.Result));

                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
            if(response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully!";

                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error encountered!";

            return View(model);
        }
    }
}
