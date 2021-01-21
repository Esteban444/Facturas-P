using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.DTO;
using WebApplicationFacturas.Helpers;
using WebApplicationFacturas.Models;

namespace WebApplicationFacturas.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FacturasProductosController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public FacturasProductosController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/facturasproductos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestedFacturasProductosDTO>>> Get()
        {
           var facturasproductos = await context.FacturasProductos.ToListAsync();
            var pedidofacturasproductos = mapper.Map<List<RequestedFacturasProductosDTO>>(facturasproductos);
            return Ok(pedidofacturasproductos);
        }
        // GET api/facturasproductos/1
        [HttpGet("{id}", Name = "ObtenerFacturasProductos")]
        public async Task<ActionResult<FacturasProductosDTO>> Get(int id)
        {
            var facturasproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.Id == id);

            if (facturasproductos == null)
            {
                return NotFound("El detalle de factura no existe");
            }
            var facturasproductosDTO = mapper.Map<FacturasProductosDTO>(facturasproductos);
            return facturasproductosDTO;

        }
        // POST api/facturaproductos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreacionFacturasProductosDTO facturasProductosdto)
        {
            var facturasproductos = mapper.Map<FacturasProductos>(facturasProductosdto);
            context.Add(facturasproductos);
            await context.SaveChangesAsync();
            var facturasproductosDTO = mapper.Map<FacturasProductosDTO>(facturasproductos);
            return new CreatedAtRouteResult("ObtenerFacturasProductos", new { id = facturasproductos.Id }, facturasproductosDTO);
        }

        // PUT api/facturaproductos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasProductosDTO facturasProductos)
        {
            try
            {
                var facturasproductosBd = await context.FacturasProductos.FirstOrDefaultAsync(x => x.Id == id);
                if (facturasproductosBd != null)
                {
                    mapper.Map(facturasProductos, facturasproductosBd);
                    await context.SaveChangesAsync();
                    return Ok(facturasProductos);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                return BadRequest("El detalle de factura no existe");
            }
        }

        // PATCH api/facturaproductos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] FacturasProductosDTO facturasProductosDTO)
        {
           
            var properties = new UpdateMapperProperties<FacturasProductos, FacturasProductosDTO>();
            var facturasproductos = context.FacturasProductos.Find(id);
            var result = await properties.MapperUpdate(facturasproductos, facturasProductosDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/facturaproductos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturasProductos>> Delete(int id)
        {
            var facturaproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.FacturaId == id);

            if (facturaproductos != null)
            {
                context.Remove(facturaproductos);
                await context.SaveChangesAsync();
                return Ok(facturaproductos);
                
            }
            else
            {
                return NotFound("El detalle de factura no existe");
            }
            
        }
    }
}
