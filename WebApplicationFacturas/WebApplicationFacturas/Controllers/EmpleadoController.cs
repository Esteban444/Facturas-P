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
using WebApplicationFacturas.Helpers;

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
        public async Task<ActionResult<IEnumerable<RequestedEmpleadosDTO>>> Get()
        {
            var empleados = await context.Empleados.Include("Cargos").ToListAsync();
            var pedidoempleadosDTO = mapper.Map<List<RequestedEmpleadosDTO>>(empleados);
            return pedidoempleadosDTO;
            
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
        public async Task<ActionResult> Post([FromBody] CreacionEmpleadosDTO creacionEmpleadosDTO)
        {
            var empleado = mapper.Map<Empleados>(creacionEmpleadosDTO);
            context.Add(empleado);
            await context.SaveChangesAsync();
            var empleadoDTO = mapper.Map<EmpleadosDTO>(empleado);
            return new CreatedAtRouteResult("ObtenerEmpleado", new { id = empleado }, empleadoDTO);
        }

        // PUT api/empleado/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmpleadosDTO empleado)
        {
            try
            {
                var empleadoBd = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);
                if (empleadoBd != null)
                {
                    mapper.Map(empleado, empleadoBd);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(NullReferenceException)
            {
                return BadRequest("El empleado no se encuentra en la base de datos");
            }
            
            return Ok(empleado);
        }

        // PATCH
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] EmpleadosDTO empleadosDTO)
        {
           
            var properties = new UpdateMapperProperties<Empleados, EmpleadosDTO>();
            var empleado = context.Empleados.Find(id);
            var result = await properties.MapperUpdate(empleado, empleadosDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }

        // DELETE api/empleado/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleados>> Delete(int id)
        {
            var empleado = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleado != null)
            {
                context.Remove(empleado);
                await context.SaveChangesAsync();
                return Ok(empleado);
                
            }
            else
            {
                return NotFound("El empleado no existe en la base de datos");
            }
            
        }

    }
}
