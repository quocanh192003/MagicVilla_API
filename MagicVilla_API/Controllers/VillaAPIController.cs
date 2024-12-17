using AutoMapper;
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
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIReponse _Reponse;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            this._Reponse = new();
            _dbVilla = dbVilla;
            _mapper = mapper;
        }


        // GET: api/VillaAPI
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIReponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villas = await _dbVilla.getAllAsync();
                _Reponse.Result = _mapper.Map<List<VillaDTO>>(villas);
                _Reponse.Status = HttpStatusCode.OK;
                return Ok(_Reponse);

            }
            catch (Exception ex)
            {
                _Reponse.IsSuccess = false;
                _Reponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _Reponse;
        }




        // GET: api/VillaAPI/5
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIReponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _Reponse.Status = HttpStatusCode.BadRequest;
                    return BadRequest(_Reponse);
                }
                var villa = await _dbVilla.getAsync(_dbVilla => _dbVilla.Id == id);
                if (villa == null)
                {
                    _Reponse.Status = HttpStatusCode.NotFound;
                    return NotFound(_Reponse);
                }
                _Reponse.Result = _mapper.Map<VillaDTO>(villa);
                _Reponse.Status = HttpStatusCode.OK;
                return Ok(_Reponse);

            }
            catch (Exception ex)
            {
                _Reponse.IsSuccess = false;
                _Reponse.ErrorMessages = new List<string> { ex.Message };
            }

            return _Reponse;
        }




        // POST: api/VillaAPI
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIReponse>> CreateVilla([FromBody] villaCreateDTO createDTO)
        {
            try
            {
                if (await _dbVilla.getAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa name already exists");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                Villa model = _mapper.Map<Villa>(createDTO);

                await _dbVilla.CreateAsync(model);
                _Reponse.Result = _mapper.Map<VillaDTO>(model);
                _Reponse.Status = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _Reponse);

            }
            catch (Exception ex)
            {
                _Reponse.IsSuccess = false;
                _Reponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _Reponse;
        }




        // DELETE: api/VillaAPI
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIReponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.getAsync(v => v.Id == id);

                if (villa == null)
                {
                    return NotFound();

                }
                await _dbVilla.RemoveAsync(villa);
                _Reponse.Status = HttpStatusCode.NoContent;
                _Reponse.IsSuccess = true;
                return Ok(_Reponse);
            }
            catch (Exception ex)
            {

                _Reponse.IsSuccess = false;
                _Reponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _Reponse;
        }


        // PUT: api/VillaAPI
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIReponse>> UpdateVilla(int id, [FromBody] villaUpdateDTO updateDTO)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest(updateDTO);
                }

                Villa model = _mapper.Map<Villa>(updateDTO);

                await _dbVilla.UpdateAsync(model);

                _Reponse.Status = HttpStatusCode.NoContent;
                _Reponse.IsSuccess = true;
                return Ok(_Reponse);
            }
            catch (Exception ex)
            {
                _Reponse.IsSuccess = false;
                _Reponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _Reponse;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVilla(int id, JsonPatchDocument<villaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.getAsync(v => v.Id == id, tracked: false);

            villaUpdateDTO villaDTO = _mapper.Map<villaUpdateDTO>(villa);

            if (villaDTO == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dbVilla.UpdateAsync(model);

            return NoContent();
        }
    }
}
