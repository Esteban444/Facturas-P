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
        public async Task<ActionResult<IEnumerable<CategoriasDTO>>> Get()
        {
            var categorias = await context.Categorias.ToListAsync();
            var categoriasDTO = mapper.Map<List<CategoriasDTO>>(categorias);
            return categoriasDTO;

        }
        // GET api/categoria/1
        [HttpGet("{id}", Name = "ObtenerCategoria")]
        public async Task<ActionResult<CategoriasDTO>> Get(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound("La categoria no existe en la empresa");
            }
            var categoriaDTO = mapper.Map<CategoriasDTO>(categoria);
            return categoriaDTO;

        }
        // POST api/categoria
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Categorias categorias)
        {
            context.Add(categorias);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerCategoria", new { id = categorias.Id }, categorias);
        }

        // PUT api/categorias/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categorias categorias)
        {
            if (categorias.Id != id)
            {
                return BadRequest();
            }

            context.Entry(categorias).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        // PATCH api/categorias/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Categorias> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(categoria, ModelState);

            var isValid = TryValidateModel(categoria);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categorias>> Delete(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound("La categoria no existe en la base de datos");
            }
            context.Remove(categoria);
            await context.SaveChangesAsync();
            return Ok(categoria);
        }

    }
}
