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
        public async Task<ActionResult<IEnumerable<ClientesRequestDTO>>> Get()
        {
            var clientes = await context.Clientes.Include("TipoClientes").ToListAsync();
            var clientesb = mapper.Map<List<ClientesRequestDTO>>(clientes);
            return clientesb;

        }
        // GET api/cliente/1
        [HttpGet("{id}", Name = "ObtenerCliente")]
        public async Task<ActionResult<ClientesBase>> Get(int id)
        {
            var clienteBd = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clienteBd == null)
            {
                return NotFound("El cliente no existe");
            }
            var clienteb = mapper.Map<ClientesBase>(clienteBd);
            return clienteb;

        }
        // POST api/cliente
        [HttpPost]
        public async Task<ClientesBase> Post([FromBody] ClientesBase clientesb)
        {
            var cliente = mapper.Map<Clientes>(clientesb);
            context.Add(cliente);
            await context.SaveChangesAsync();
            return clientesb;
        }

        // PUT api/cliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ClientesUpdateRequestDTO clientes)
        {
            try
            {
                var clienteBd = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<ActionResult> Patch(int id, [FromBody] ClientesBase clientesb)
        {
            
            var properties = new UpdateMapperProperties<Clientes, ClientesBase>();
            var cliente = context.Clientes.Find(id);
            var result = await properties.MapperUpdate(cliente, clientesb);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/cliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientesBase>> Delete(int id)
        {
            var clientesBd = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (clientesBd != null)
            {
                var cliente = context.TipoClientes.Where(f => f.ClienteId == id).SingleOrDefault(a => a.ClienteId == id);
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
