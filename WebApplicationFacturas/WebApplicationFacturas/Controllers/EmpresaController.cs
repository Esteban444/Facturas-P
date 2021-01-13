
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
    public class EmpresaController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public EmpresaController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        // Get para obtener listado de empresas
        // GET api/empresa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresasDTO>>> Get()
        {
            var empresas = await context.Empresas.Include("Empleados").ToListAsync();
            var empresasDTO = mapper.Map<List<EmpresasDTO>>(empresas);
            return empresasDTO;
        }

        // get para obtener una empresa por su id
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
        // peticion para crear empresa
        // POST api/empresa
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmpresasDTO empresa)
        {
            var empresas = mapper.Map<Empresas>(empresa);
            context.Add(empresas);
            await context.SaveChangesAsync();
            var empresaDTO = mapper.Map<EmpresasDTO>(empresas);
            return new CreatedAtRouteResult("ObtenerEmpresa", new { id = empresas.Id }, empresaDTO);
        }
        // peticion para actualizar empresa
        // PUT api/empresa/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmpresasDTO empresas)
        {
          
            var empresa = mapper.Map<Empresas>(empresas);
            empresa.Id = id;
            context.Entry(empresa).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }
        // para actualizar determinado campo
        // PATCH api/cargos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<EmpresasDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var empresaDB = await context.Empresas.FirstOrDefaultAsync(x => x.Id == id);
            if (empresaDB == null)
            {
                return NotFound();
            }
            var empresaDTO = mapper.Map<EmpresasDTO>(empresaDB);

            patchDocument.ApplyTo(empresaDTO, ModelState);

            mapper.Map(empresaDTO, empresaDB);

            var isValid = TryValidateModel(empresaDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // peticion Http para eliminar una empresa
        // DELETE api/empresa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empresas>> Delete(int id)
        {
            var empresa = await context.Empresas.FirstOrDefaultAsync(x => x .Id == id);

            if (empresa == null)
            {
                return NotFound("La empresa no existe en la base de datos");
            }
            context.Remove(empresa);
            await context.SaveChangesAsync();
            return Ok(empresa);
        }
      
    }
}

