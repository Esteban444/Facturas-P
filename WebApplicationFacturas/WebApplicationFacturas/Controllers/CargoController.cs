using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.Models;
using WebApplicationFacturas.DTO;
using WebApplicationFacturas.Helpers;

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
        public async Task<ActionResult<IEnumerable<RequestedCargosDTO>>> Get()
        {
            var cargos = await context.Cargos.ToListAsync();
            var pedidocargosDTO = mapper.Map<List<RequestedCargosDTO>>(cargos);
            return pedidocargosDTO;

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
        public async Task<ActionResult> Post([FromBody] CreacioncargosDTO creacioncargosDTO)
        {
            var cargo = mapper.Map<Cargos> (creacioncargosDTO);
            context.Add(cargo);
            await context.SaveChangesAsync();
            var cargoDTO = mapper.Map<CargosDTO>(cargo);
            return new CreatedAtRouteResult("ObtenerCargo", new { id = cargo.Id }, cargoDTO);
        }

        // PUT api/cargos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CargosDTO cargos)
        {
            try
            {
                var cargoBd = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);
                if (cargoBd != null)
                {
                    mapper.Map(cargos, cargoBd);
                    await context.SaveChangesAsync();
                    
                }
                else
                {
                    throw new System.Exception();
                }

                
            }
            catch (System.Exception)
            {
                return BadRequest("El cargo no exixte");
            }
            return Ok(cargos);
        }

        // PATCH api/cargos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] CargosDTO cargosDTO)
        {
           
            var properties = new UpdateMapperProperties<Cargos, CargosDTO>();
            var cargo = context.Cargos.Find(id);
            var result = await properties.MapperUpdate(cargo, cargosDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/cargos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleados>> Delete(int id)
        {
            var cargo = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);

            if (cargo != null)
            {
                context.Remove(cargo);
                await context.SaveChangesAsync();
                return Ok(cargo);
            }
            else
            {
                return NotFound("El cargo no existe en la base de datos");
            }
            
        }

    }
}
