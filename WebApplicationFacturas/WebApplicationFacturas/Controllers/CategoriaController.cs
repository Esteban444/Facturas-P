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
        public async Task<ActionResult<IEnumerable<RequestedCategoriasDTO>>> Get()
        {
            var categoria = await context.Categorias.ToListAsync();
            var pedidocategoriasDTO = mapper.Map<List<RequestedCategoriasDTO>>(categoria);
            return pedidocategoriasDTO;

        }
        // GET api/categoria/1
        [HttpGet("{id}", Name = "ObtenerCategoria")]
        public async Task<ActionResult<CategoriasDTO>> Get(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return BadRequest("La categoria no existe en la empresa");
            }
            var categoriaDTO = mapper.Map<CategoriasDTO>(categoria);
            return categoriaDTO;

        }
        // POST api/categoria
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionCategoriasDTO creacionCategoriasDTO)
        {
            var categoria = mapper.Map<Categorias>(creacionCategoriasDTO);
            context.Add(categoria);
            await context.SaveChangesAsync();
            var categoriasDTO = mapper.Map<CategoriasDTO>(categoria);
            return new CreatedAtRouteResult("ObtenerCategoria", new { id = categoria.Id }, categoriasDTO);
        }

        // PUT api/categorias/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriasDTO categorias)
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
        public async Task<ActionResult> Patch(int id, [FromBody] CategoriasDTO categoriasDTO)
        {
            
            var properties = new UpdateMapperProperties<Categorias, CategoriasDTO>();
            var categoria = context.Categorias.Find(id);
            var result = await properties.MapperUpdate(categoria, categoriasDTO);
            await context.SaveChangesAsync();

            return Ok(result);
        }
        // DELETE api/categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categorias>> Delete(int id)
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
