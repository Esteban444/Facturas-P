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
using WebApplicationFacturas.DTO.Requests;
using System;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<CargosRequestDTO>>> Get()
        {
            var cargos = await context.Cargos.ToListAsync();
            var cargosDTO = mapper.Map<List<CargosRequestDTO>>(cargos);
            return cargosDTO;

        }
        // GET api/cargo/1
        [HttpGet("{id}", Name = "ObtenerCargo")]
        public async Task<ActionResult<CargosBase>> Get(int id)
        {
            var cargobd = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);

            if (cargobd == null)
            {
                return NotFound("El cargo no existe en la empresa");
            }
            var cargoDTO = mapper.Map<CargosBase>(cargobd);
            return cargoDTO;

        }
        // POST api/cargo
        [HttpPost]
        public async Task<CargosBase> Post([FromBody] CargosBase cargosBase)
        {
            
            var cargo = mapper.Map<Cargos>(cargosBase);
            context.Add(cargo);
            await context.SaveChangesAsync();
            return cargosBase;
        }

        // PUT api/cargos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CargosUpdateRequestDTO cargos)
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
        public async Task<ActionResult> Patch(int id, [FromBody] CargosBase cargosb)
        {
           
            var properties = new UpdateMapperProperties<Cargos, CargosBase>();
            var cargo = context.Cargos.Find(id);
            var result = await properties.MapperUpdate(cargo, cargosb);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/cargos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CargosBase>> Delete(int id)
        {
            var cargo = await context.Cargos.FirstOrDefaultAsync(x => x.Id == id);

            if (cargo != null)
            {
                var cargob = context.Cargos.Where(c => c.EmpleadoId == id).SingleOrDefault(a => a.EmpleadoId == id);
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
