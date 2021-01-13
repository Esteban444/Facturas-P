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
    public class TipoClienteController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public TipoClienteController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/tipocliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoClientesDTO>>> Get()
        {
            var tipocliente = await context.TipoClientes.ToListAsync();
            var tipoclienteDTO = mapper.Map<List<TipoClientesDTO>>(tipocliente);
            return tipoclienteDTO;

        }
        // GET api/tipocliente/1
        [HttpGet("{id}", Name = "ObtenerTipoCliente")]
        public async Task<ActionResult<TipoClientesDTO>> Get(int id)
        {
            var tipoclientes = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoclientes == null)
            {
                return NotFound("El tipo cliente no existe");
            }
            var tipoclientesDTO = mapper.Map<TipoClientesDTO>(tipoclientes);
            return tipoclientesDTO;

        }
        // POST api/tipocliente
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoClientes tipoclientes)
        {
            context.Add(tipoclientes);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerTipoCliente", new { id = tipoclientes.Id }, tipoclientes);
        }

        // PUT api/tipocliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TipoClientes tipoclientes)
        {
            if (tipoclientes.Id != id)
            {
                return BadRequest();
            }

            context.Entry(tipoclientes).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("El tipo de Cliente fue modificado");
        }

        // PATCH api/tipocliente/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<TipoClientes> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var tipoclientes = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);
            if (tipoclientes == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(tipoclientes, ModelState);

            var isValid = TryValidateModel(tipoclientes);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/tipocliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoClientes>> Delete(int id)
        {
            var tipoclientes = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoclientes == null)
            {
                return NotFound("El tipo de cliente no existe");
            }
            context.Remove(tipoclientes);
            await context.SaveChangesAsync();
            return Ok(tipoclientes);
        }
    }
}
