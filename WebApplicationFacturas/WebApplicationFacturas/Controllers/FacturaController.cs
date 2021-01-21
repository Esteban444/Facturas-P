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
    [Route("Api/[Controller]")]
    [ApiController]
    public class FacturaController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public FacturaController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestedFacturasDTO>>> Get()
        {
            var facturas = await context.Facturas.Include("FacturasProductos").ToListAsync();
            var pedidofacturasDTO = mapper.Map<List<RequestedFacturasDTO>>(facturas);
            return pedidofacturasDTO;

        }
        // GET api/factura/1
        [HttpGet("{id}", Name = "ObtenerFactura")]
        public async Task<ActionResult<FacturasDTO>> Get(int id)
        {
            var facturas = await context.Facturas.Include("FacturasProductos").FirstOrDefaultAsync(x => x.Id == id);

            if (facturas == null)
            {
                return NotFound("El factura no existe");
            }
            var facturasDTO = mapper.Map<FacturasDTO>(facturas);
            return facturasDTO;

        }
        // POST api/factura
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionFacturasDTO creacionFacturasDTO)
        {
            var factura = mapper.Map<Facturas>(creacionFacturasDTO);
            context.Add(factura);
            await context.SaveChangesAsync();
            var facturasDTO = mapper.Map<FacturasDTO>(factura);
            return new CreatedAtRouteResult("ObtenerFactura", new { id = factura.Id }, facturasDTO);
        }

        // PUT api/factura/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasDTO facturas)
        {
            try
            {
                var facturaBd = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);
                if (facturaBd != null)
                {
                    mapper.Map(facturas, facturaBd);
                    await context.SaveChangesAsync();
                    return Ok(facturas);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return BadRequest("La factura no existe");
            }
        }

        // PATCH api/factura/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] FacturasDTO facturasDTO)
        {
            
            var properties = new UpdateMapperProperties<Facturas, FacturasDTO>();
            var facturas = context.Facturas.Find(id);
            var result = await properties.MapperUpdate(facturas, facturasDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/factura/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Facturas>> Delete(int id)
        {
            var facturas = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);

            if (facturas != null)
            {
                context.Remove(facturas);
                await context.SaveChangesAsync();
                return Ok(facturas);
            }
            else
            {
                return NotFound("La factura no existe");
            }
            
        }

    }
}
