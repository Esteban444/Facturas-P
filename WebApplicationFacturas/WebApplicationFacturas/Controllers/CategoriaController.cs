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
    public class CategoriaController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public CategoriaController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriasRequestDTO>>> Get()
        {
            var categoria = await context.Categorias.ToListAsync();
            var categoriasDTO = mapper.Map<List<CategoriasRequestDTO>>(categoria);
            return categoriasDTO;

        }
        // GET api/categoria/1
        [HttpGet("{id}", Name = "ObtenerCategoria")]
        public async Task<ActionResult<CategoriasBase>> Get(int id)
        {
            var categoriaBd = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaBd == null)
            {
                return BadRequest("La categoria no existe en la empresa");
            }
            var categoria = mapper.Map<CategoriasBase>(categoriaBd);
            return categoria;

        }
        // POST api/categoria
        [HttpPost]
        public async Task<CategoriasBase> Post([FromBody] CategoriasBase categoriasBase)
        {
            var categoria = mapper.Map<Categorias>(categoriasBase);
            context.Add(categoria);
            await context.SaveChangesAsync();
            return categoriasBase;
        }

        // PUT api/categorias/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriasUpdateRequestDTO categorias)
        {
            try
            {
                var categoriaBd = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
                if (categoriaBd != null)
                {
                    mapper.Map(categorias, categoriaBd);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new SystemException();
                }
            }
            catch (SystemException)
            {
                return BadRequest("La categoria no exixte");
            }
            
            return Ok(categorias);
        }

        // PATCH api/categorias/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] CategoriasBase categoriasb)
        {
            
            var properties = new UpdateMapperProperties<Categorias, CategoriasBase>();
            var categoria = context.Categorias.Find(id);
            var result = await properties.MapperUpdate(categoria, categoriasb);
            await context.SaveChangesAsync();

            return Ok(result);
        }
        // DELETE api/categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriasBase>> Delete(int id)
        {
            var categoriaBd = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaBd != null)
            {
                context.Categorias.Remove(categoriaBd);
                await context.SaveChangesAsync();
                return Ok(categoriaBd);
            }
            else
            {
                return BadRequest("La categoria no existe en la base de datos");
            }
        }

    }
}
