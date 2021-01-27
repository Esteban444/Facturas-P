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
        public async Task<ActionResult<IEnumerable<TipoClientesRequestDTO>>> Get()
        {
            var tipocliente = await context.TipoClientes.ToListAsync();
            var tipoclienteDTO = mapper.Map<List<TipoClientesRequestDTO>>(tipocliente);
            return tipoclienteDTO;

        }
        // GET api/tipocliente/1
        [HttpGet("{id}", Name = "ObtenerTipoCliente")]
        public async Task<ActionResult<TipoClientesBase>> Get(int id)
        {
            var tipoclientesBd = await context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);

            if (tipoclientesBd == null)
            {
                return NotFound("El tipo cliente no existe");
            }
            var tipoclientesb = mapper.Map<TipoClientesBase>(tipoclientesBd);
            return tipoclientesb;

        }
        // POST api/tipocliente
        [HttpPost]
        public async Task<TipoClientesBase> Post([FromBody] TipoClientesBase tipoclientes)
        {
            var tipocliente = mapper.Map<TipoClientes>(tipoclientes);
            context.Add(tipocliente);
            await context.SaveChangesAsync();
            return tipoclientes;
        }

        // PUT api/tipocliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TipoClientesUpdateRequestDTO tipoclientes)
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
        public async Task<ActionResult> Patch(int id, [FromBody] TipoClientesBase tipoClientesDTO)
        {
            
            var properties = new UpdateMapperProperties<TipoClientes, TipoClientesBase>();
            var tipocliente = context.TipoClientes.Find(id);
            var result = await properties.MapperUpdate(tipocliente, tipoClientesDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/tipocliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoClientesBase>> Delete(int id)
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
