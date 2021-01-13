using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.Models;
using WebApplicationFacturas.DTO;

namespace WebApplicationFacturas.Controllers
{
    [Route("api/[Controller]")]
     [ApiController]
    public class CargoController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public CargoController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        // GET api/cargo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargosDTO>>> Get()
        {
            var cargos = await context.Cargos.ToListAsync();
            var cargosDTO = mapper.Map<List<CargosDTO>>(cargos);
            return cargosDTO;

        }
        // GET api/cargo/1
        [HttpGet("{id}", Name = "ObtenerCargo")]
        public async Task<ActionResult<CargosDTO>> Get(int id)
        {
            var cargo = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);

            if (cargo == null)
            {
                return NotFound("El cargo no existe en la empresa");
            }
            var cargoDTO = mapper.Map<CargosDTO>(cargo);
            return cargoDTO;

        }
        // POST api/cargo
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Cargos cargos)
        {
            context.Add(cargos);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerCargo", new { id = cargos.Id }, cargos);
        }

        // PUT api/cargos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Cargos cargos)
        {
            if (cargos.Id != id)
            {
                return BadRequest();
            }

            context.Entry(cargos).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        // PATCH api/cargos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Cargos> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var cargo = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);
            if (cargo == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(cargo, ModelState);

            var isValid = TryValidateModel(cargo);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/cargos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleados>> Delete(int id)
        {
            var cargo = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);

            if (cargo == null)
            {
                return NotFound("El cargo no existe en la base de datos");
            }
            context.Remove(cargo);
            await context.SaveChangesAsync();
            return Ok(cargo);
        }

    }
}
