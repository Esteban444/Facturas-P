
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
    public class EmpresaController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public EmpresaController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/empresa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestedEmpresasDTO>>> Get()
        {
            var empresas = await context.Empresas.Include("Empleados").ToListAsync();
            var pedidoempresasDTO = mapper.Map<List<RequestedEmpresasDTO>>(empresas);
            return pedidoempresasDTO;
        }

        // GET api/empresa/1
        [HttpGet("{id}", Name = "ObtenerEmpresa")]
        public async Task<ActionResult<EmpresasDTO>> Get(int id)
        {
            var empresa = await context.Empresas.Include("Empleados").FirstOrDefaultAsync(x => x.Id == id);

            if (empresa == null)
            {
                return NotFound("La empresa no existe en la base de datos");
            }

            var empresaDTO = mapper.Map<EmpresasDTO>(empresa);
            return empresaDTO ;

        }
       
        // POST api/empresa
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmpresasCreacionDTO empresascreacionDTO)
        {
            var empresa = mapper.Map<Empresas>(empresascreacionDTO);
            context.Add(empresa);
            await context.SaveChangesAsync();
            var empresaDTO = mapper.Map<EmpresasDTO>(empresa);
            return new CreatedAtRouteResult("ObtenerEmpresa", new { id = empresa.Id }, empresaDTO);
        }
        
        // PUT api/empresa/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmpresasDTO empresas)
        {
            
            try
            {
                var empresaBd = await context.Empresas.FirstOrDefaultAsync(x => x.Id == id);
                if(empresaBd != null)
                {
                    mapper.Map(empresas, empresaBd);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch(NullReferenceException)
            {
                return BadRequest("la empresa no esta en la base de datos");
            }
            return Ok(empresas);
        }
        
        // PATCH api/cargos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] EmpresasDTO empresasDTO)
        {

            var properties = new UpdateMapperProperties<Empresas, EmpresasDTO>();
            var empresa = context.Empresas.Find(id);
            var result = await properties.MapperUpdate(empresa, empresasDTO);
            await context.SaveChangesAsync();
            
            return Ok(result);
        }
        
        // DELETE api/empresa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empresas>> Delete(int id)
        {
            var empresa = await context.Empresas.FirstOrDefaultAsync(x => x .Id == id);

            if (empresa != null)
            {
                context.Empresas.Remove(empresa);
                await context.SaveChangesAsync();
                return Ok(empresa);
            }
            else
            {
                return BadRequest("La empresa no existe en la base de datos");
            }
            
        }
      
    }
}

