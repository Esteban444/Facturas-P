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
    public class ClienteController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ClienteController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDTO>>> Get()
        {
            var clientes = await context.Clientes.Include("TipoClientes.Facturas").ToListAsync();
            var clientesDTO = mapper.Map<List<ClientesDTO>>(clientes);
            return clientesDTO;

        }
        // GET api/cliente/1
        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClientesDTO>> Get(int id)
        {
            var cliente = await context.Clientes.Include("TipoClientes.Facturas").FirstOrDefaultAsync(x => x.Id == id);

            if (cliente == null)
            {
                return NotFound("El cliente no existe");
            }
            var clienteDTO = mapper.Map<ClientesDTO>(cliente);
            return clienteDTO;

        }
        // POST api/cliente
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Clientes clientes)
        {
            context.Add(clientes);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerCliente", new { id = clientes.Id }, clientes);
        }

        // PUT api/cliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Clientes clientes)
        {
            if (clientes.Id != id)
            {
                return BadRequest();
            }

            context.Entry(clientes).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("Cliente modificado");
        }

        // PATCH api/cliente/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Clientes> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var clientes = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(clientes, ModelState);

            var isValid = TryValidateModel(clientes);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/cliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Clientes>> Delete(int id)
        {
            var clientes = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clientes == null)
            {
                return NotFound("El cliente no existe");
            }
            context.Remove(clientes);
            await context.SaveChangesAsync();
            return Ok(clientes);
        }
    }
}
