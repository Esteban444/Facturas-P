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
            var tipoclientesBd = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoclientesBd == null)
            {
                return NotFound("El tipo cliente no existe");
            }
            var tipoclientesDTO = mapper.Map<TipoClientesDTO>(tipoclientesBd);
            return tipoclientesDTO;

        }
        // POST api/tipocliente
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreaciontipoclientesDTO creaciontipoclientesDTO)
        {
            var tipocliente = mapper.Map<TipoClientes>(creaciontipoclientesDTO);
            context.Add(tipocliente);
            await context.SaveChangesAsync();
            var tipoclienteDTO = mapper.Map<TipoClientesDTO>(tipocliente);
            return new CreatedAtRouteResult("ObtenerTipoCliente", new { id = tipocliente.Id }, tipoclienteDTO);
        }

        // PUT api/tipocliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TipoClientesDTO tipoclientes)
        {
            try
            {
                var tipoclienteBd = await context.TipoClientes.FirstOrDefaultAsync(X => X.Id == id);
                if (tipoclienteBd != null)
                {
                    mapper.Map(tipoclientes, tipoclienteBd);
                    await context.SaveChangesAsync();

                }
                else
                {
                    throw new SystemException();
                }
            }
            catch (SystemException)
            {
                return BadRequest("El tipo de cliente no exixte");
            }
            return Ok(tipoclientes);
        }

        // PATCH api/tipocliente/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] TipoClientesDTO tipoClientesDTO)
        {
            
            var properties = new UpdateMapperProperties<TipoClientes, TipoClientesDTO>();
            var tipocliente = context.TipoClientes.Find(id);
            var result = await properties.MapperUpdate(tipocliente, tipoClientesDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/tipocliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoClientes>> Delete(int id)
        {
            var tipoclienteBd = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoclienteBd != null)
            {
                context.TipoClientes.Remove(tipoclienteBd);
                await context.SaveChangesAsync();
                return Ok(tipoclienteBd);

            }
            else
            {
                return BadRequest("El tipo de cliente no existe");
            }
            
        }
    }
}
