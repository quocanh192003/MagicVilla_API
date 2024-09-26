using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Model;
using MagicVilla_API.Model.dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public VillaAPIController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/VillaAPI
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult <IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villas = await _context.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villas));

        }




        // GET: api/VillaAPI/5
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }




        // POST: api/VillaAPI
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] villaCreateDTO createDTO)
        {
            if (await _context.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customerror", "Villa name already exists");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            Villa model = _mapper.Map<Villa>(createDTO);
           
            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }




        // DELETE: api/VillaAPI
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();

            }
            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // PUT: api/VillaAPI
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<villaUpdateDTO>> UpdateVilla(int id, [FromBody] villaUpdateDTO updateDTO)
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
          
            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();

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
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

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
            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 
