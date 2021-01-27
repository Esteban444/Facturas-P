
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
        public async Task<ActionResult<IEnumerable<EmpresasRequestDTO>>> Get()
        {
            var empresas = await context.Empresas.Include("Empleados").ToListAsync();
            var empresasDTO = mapper.Map<List<EmpresasRequestDTO>>(empresas);
            return empresasDTO;
        }

        // GET api/empresa/1
        [HttpGet("{id}", Name = "ObtenerEmpresa")]
        public async Task<ActionResult<EmpresasBase>> Get(int id)
        {
            var empresa = await context.Empresas.FirstOrDefaultAsync(x => x.Id == id);

            if (empresa == null)
            {
                return NotFound("La empresa no existe en la base de datos");
            }

            var empresab = mapper.Map<EmpresasBase>(empresa);
            return empresab ;

        }
       
        // POST api/empresa
        [HttpPost]
        public async Task<EmpresasBase> Post([FromBody] EmpresasBase empresasb)
        {
            var empresa = mapper.Map<Empresas>(empresasb);
            context.Add(empresa);
            await context.SaveChangesAsync();
            //var empresaDTO = mapper.Map<EmpresasDTO>(empresa);
            return empresasb;
        }
        
        // PUT api/empresa/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmpresasUpdateRequestDTO empresas)
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
        public async Task<ActionResult> Patch(int id, [FromBody] EmpresasBase empresasb)
        {

            var properties = new UpdateMapperProperties<Empresas, EmpresasBase>();
            var empresa = context.Empresas.Find(id);
            var result = await properties.MapperUpdate(empresa, empresasb);
            await context.SaveChangesAsync();
            
            return Ok(result);
        }
        
        // DELETE api/empresa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmpresasBase>> Delete(int id)
        {
            var empresa = await context.Empresas.FirstOrDefaultAsync(x => x .Id == id);

            if (empresa != null)
            {
                var empresas = context.Empleados.Where(c => c.EmpresaId == id).SingleOrDefault(a => a.EmpresaId == id);
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

