using AutoMapper;
using Azure;
using MagicVilla_API.Data;
using MagicVilla_API.Model;
using MagicVilla_API.Model.dto;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/VillaNumberAPI")]
    [Controller]
    public class VillaNumberAPIController : Controller
    {
        protected APIReponse _reponse;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IMapper _mapper;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper){
            this._reponse = new();
            this._dbVillaNumber = dbVillaNumber;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIReponse>> getAllVillaNumbers(){
            try
            {
                IEnumerable<VillaNumber> villas = await _dbVillaNumber.getAllAsync();
                _reponse.Result = _mapper.Map<List<VillaNumberDTO>>(villas);
                _reponse.Status = HttpStatusCode.OK;
                return Ok(_reponse);

            }
            catch (Exception ex){
                _reponse.IsSuccess = false;
                _reponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _reponse;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIReponse>> GetVillaNumber(int id){
            try{
                if(id == 0){
                    _reponse.Status = HttpStatusCode.BadRequest;
                    return BadRequest(_reponse);
                }
                
                VillaNumber villaNumber =await _dbVillaNumber.getAsync(n => n.VillaNo == id);
                if(villaNumber == null){
                    _reponse.Status = HttpStatusCode.NotFound;
                    return NotFound (_reponse);
                }
                _reponse.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _reponse.Status = HttpStatusCode.OK;
                return Ok(_reponse);
            }
            catch (Exception ex){
                _reponse.IsSuccess = false;
                _reponse.ErrorMessages = new List<string>{ex.Message};
             }
             return _reponse;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIReponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumber){
            try{
                if(villaNumber == null){
                    _reponse.Status = HttpStatusCode.BadRequest;
                    return BadRequest(_reponse);
                }
                if(await _dbVillaNumber.getAsync(n => n.VillaNo == villaNumber.villaNo) != null){
                    ModelState.AddModelError("CustomError", "Villa Number already Exists");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(villaNumber);
                await _dbVillaNumber.CreateAsync(model);
                _reponse.Result = _mapper.Map<VillaNumberDTO>(model);
                _reponse.Status = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber",new {id = model.VillaNo}, _reponse);
            }
            catch(Exception ex){
                _reponse.IsSuccess = false;
                _reponse.ErrorMessages = new List<string>{ex.Message};
                
            }
            return _reponse;
        }

        [HttpDelete("{id:int}", Name ="DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIReponse>> DeleteVillaNumber(int id ){
            try
            {
                if(id == 0){
                    return BadRequest();
                }

                var deleVillaNumber =await _dbVillaNumber.getAsync(n => n.VillaNo == id);
                if(deleVillaNumber == null){
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(deleVillaNumber);
                _reponse.IsSuccess = true;
                _reponse.Status = HttpStatusCode.NoContent;
               return(_reponse);
            }
            catch (Exception ex)
            {
                _reponse.IsSuccess =false;
                _reponse.ErrorMessages = new List<string>{ex.Message};
                
            }
            return _reponse;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIReponse>> UpdateVillaNumber(int id  , [FromBody] VillaNumberUpdateDTO villaUpdateNumber)
        {
            try
            {
                if(id == 0){
                    return BadRequest();
                }
               
                if(villaUpdateNumber == null || id != villaUpdateNumber.villaNo){
                    return BadRequest();
                }
                VillaNumber model = _mapper.Map<VillaNumber>(villaUpdateNumber);

                await _dbVillaNumber.UpdateAsync(model);
                _reponse.IsSuccess =true;
                _reponse.Status = HttpStatusCode.NoContent;
                return Ok(_reponse);
            }
            catch (Exception ex)
            {
                _reponse.IsSuccess =false;
                _reponse.ErrorMessages = new List<string>{ex.Message};
            }
            return _reponse;
        }
    }
}
