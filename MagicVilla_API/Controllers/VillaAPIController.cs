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
        public VillaAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult <IEnumerable<VillaDTO>>> GetVillas()
        {
            return Ok(await _context.Villas.ToListAsync());

        }
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
            return Ok(villa);
        }


        // POST: api/VillaAPI
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<villaCreateDTO>> CreateVilla([FromBody] villaCreateDTO villaDTO)
        {
            if (await _context.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customerror", "Villa name already exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                
                Occupancy = villaDTO.Occupancy,
                CreateDate = villaDTO.CreateDate,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                UpdateDate = villaDTO.UpdateDate,
                Sqft = villaDTO.Sqft
                
            };
            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }


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
        public async Task<ActionResult<villaUpdateDTO>> UpdateVilla(int id, [FromBody] villaUpdateDTO villaDTO)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                Occupancy = villaDTO.Occupancy,
                CreateDate = villaDTO.CreateDate,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                UpdateDate = villaDTO.UpdateDate,
                Sqft = villaDTO.Sqft

            };
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
            villaUpdateDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Name = villa.Name,
                Details = villa.Details,
                Id = villa.Id,
                Occupancy = villa.Occupancy,
                CreateDate = villa.CreateDate,
                ImageUrl = villa.ImageUrl,
                Rate = villa.Rate,
                UpdateDate = villa.UpdateDate,
                Sqft = villa.Sqft

            };
            if (villaDTO == null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                Occupancy = villaDTO.Occupancy,
                CreateDate = villaDTO.CreateDate,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                UpdateDate = villaDTO.UpdateDate,
                Sqft = villaDTO.Sqft

            };
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
