using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.Models;
using WebApplicationFacturas.DTO;

namespace WebApplicationFacturas.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EmpleadoController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public EmpleadoController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        // GET api/empleado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadosDTO>>> Get()
        {
            var empleados = await context.Empleados.Include("Cargos").ToListAsync();
            var empleadosDTO = mapper.Map<List<EmpleadosDTO>>(empleados);
            return empleadosDTO;
            
        }
        // GET api/empleado/1
        [HttpGet("{id}", Name = "ObtenerEmpleado")]
        public async Task<ActionResult<EmpleadosDTO>> Get(int id)
        {
            var empleado = await context.Empleados.Include("Cargos").FirstOrDefaultAsync(x => x.Id == id);

            if (empleado == null)
            {
                return NotFound("El empleado no esta en la base de datos");
            }
            var empleadoDTO = mapper.Map<EmpleadosDTO>(empleado);
            return empleadoDTO;

        }
        // POST api/empleado
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Empleados empleado)
        {
            context.Add(empleado);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerEmpleado", new { id = empleado }, empleado);
        }

        // PUT api/empleado/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Empleados empleado)
        {
            if (empleado.Id != id)
            {
                return BadRequest();
            }

            context.Entry(empleado).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        // PATCH
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Empleados> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var empleado = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(empleado, ModelState);

            var isValid = TryValidateModel(empleado);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/empleado/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleados>> Delete(int id)
        {
            var empleado = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleado == null)
            {
                return NotFound("El empleado no existe en la base de datos");
            }
            context.Remove(empleado);
            await context.SaveChangesAsync();
            return Ok(empleado);
        }

    }
}
