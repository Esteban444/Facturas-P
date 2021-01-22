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
        public async Task<ActionResult<IEnumerable<RequestedClientesDTO>>> Get()
        {
            var clientes = await context.Clientes.Include("TipoClientes").ToListAsync();
            var pedidoclientesDTO = mapper.Map<List<RequestedClientesDTO>>(clientes);
            return pedidoclientesDTO;

        }
        // GET api/cliente/1
        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClientesDTO>> Get(int id)
        {
            var clienteBd = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clienteBd == null)
            {
                return NotFound("El cliente no existe");
            }
            var clienteDTO = mapper.Map<ClientesDTO>(clienteBd);
            return clienteDTO;

        }
        // POST api/cliente
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionClientesDTO creacionClientesDTO)
        {
            var cliente = mapper.Map<Clientes>(creacionClientesDTO);
            context.Add(cliente);
            await context.SaveChangesAsync();
            var clienteDTO = mapper.Map<ClientesDTO>(cliente);
            return new CreatedAtRouteResult("ObtenerCliente", new { id = cliente.Id }, clienteDTO);
        }

        // PUT api/cliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ClientesDTO clientes)
        {
            try
            {
                var clienteBd = await context.Clientes.Include("TipoClientes").FirstOrDefaultAsync(x => x.Id == id);
                if (clienteBd != null)
                {
                    mapper.Map(clientes, clienteBd);
                    await context.SaveChangesAsync();
                    
                }
                else
                {
                    throw new SystemException();
                }

            }
            catch (SystemException)
            {
                return BadRequest("El cliente no existe");
            }
            return Ok(clientes);
        }

        // PATCH api/cliente/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] ClientesDTO clientesDTO)
        {
            
            var properties = new UpdateMapperProperties<Clientes, ClientesDTO>();
            var cliente = context.Clientes.Find(id);
            var result = await properties.MapperUpdate(cliente, clientesDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/cliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Clientes>> Delete(int id)
        {
            var clientesBd = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clientesBd != null)
            {
                context.Clientes.Remove(clientesBd);
                await context.SaveChangesAsync();
                return Ok(clientesBd);
            }
            else
            {
                return NotFound("El cliente no existe");
            }
            
        }
    }
}
