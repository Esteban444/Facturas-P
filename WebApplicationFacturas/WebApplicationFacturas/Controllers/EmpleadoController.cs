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
using WebApplicationFacturas.DTO.Requests;

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
        public async Task<ActionResult<IEnumerable<EmpleadosRequestDTO>>> Get()
        {
            var empleados = await context.Empleados.Include("Cargos").ToListAsync();
            var peticionempleados = mapper.Map<List<EmpleadosRequestDTO>>(empleados);
            return peticionempleados;
            
        }
        // GET api/empleado/1
        [HttpGet("{id}", Name = "ObtenerEmpleado")]
        public async Task<ActionResult<EmpleadosBase>> Get(int id)
        {
            var empleado = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleado == null)
            {
                return NotFound("El empleado no esta en la base de datos");
            }
            var empleadob = mapper.Map<EmpleadosBase>(empleado);
            return empleadob;

        }
        // POST api/empleado
        [HttpPost]
        public async Task<EmpleadosBase> Post([FromBody] EmpleadosBase empleados)
        {
            var empleado = mapper.Map<Empleados>(empleados);
            context.Add(empleado);
            await context.SaveChangesAsync();
            return empleados;
        }

        // PUT api/empleado/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmpleadosUpdateRequestDTO empleado)
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
        public async Task<ActionResult> Patch(int id, [FromBody] EmpleadosBase empleados)
        {
           
            var properties = new UpdateMapperProperties<Empleados, EmpleadosBase>();
            var empleado = context.Empleados.Find(id);
            var result = await properties.MapperUpdate(empleado, empleados);
            await context.SaveChangesAsync();
            return Ok(result);
        }

        // DELETE api/empleado/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmpleadosBase>> Delete(int id)
        {
            var empleadobd = await context.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleadobd != null)
            {
                context.Facturas.Where(f => f.EmpleadoId == id).SingleOrDefault(a => a.EmpleadoId == id);

                context.Remove(empleadobd);
                await context.SaveChangesAsync();
                return Ok(empleadobd);
            }
            else
            {
                return NotFound("El empleado no existe en la base de datos");
            }
            
        }

    }
}
