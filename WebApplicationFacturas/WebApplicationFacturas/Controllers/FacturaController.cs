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
using System.Reflection.Emit;

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

        [HttpPost("Creacion-Detalle-Factura")]
        public async Task<ActionResult<FacturasBase>> CreacionDetalleFacturas(FacturasBase basef)
        {
            var detallefactura = new FacturasBase();
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    var tablafactura = new Facturas();

                    tablafactura.EmpleadoId = basef.EmpleadoId;
                    tablafactura.ClienteId = basef.ClienteId;
                    tablafactura.NombreCliente = basef.NombreCliente;
                    tablafactura.Estado = basef.Estado;
                    tablafactura.Total = basef.Total;
                    context.Facturas.Add(tablafactura);
                    context.SaveChanges();

                    foreach (var item in basef.FacturasProductos)
                    {
                        var facturadetalle = new FacturasProductos();

                        facturadetalle.FacturaId = tablafactura.Id;   
                        facturadetalle.ProductosId = item.ProductosId;
                        facturadetalle.NombreProducto = item.NombreProducto;
                        facturadetalle.Cantidad = item.Cantidad;
                        facturadetalle.Precio = item.Precio;
                        context.FacturasProductos.Add(facturadetalle);
                    }
                    context.SaveChanges();
                    transaccion.Commit();

                }
                catch (Exception)
                {

                }
            }
            return detallefactura;
        }
        // GET api/factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturasRequestsDTO>>> Get()
        {
            var facturas = await context.Facturas.ToListAsync();
            var peticionesfacturas = mapper.Map<List<FacturasRequestsDTO>>(facturas);
            return peticionesfacturas;

        }
        // GET api/factura/1
        [HttpGet("{id}", Name = "ObtenerFactura")]
        public async Task<ActionResult<FacturasBase>> Get(int id)
        {
            var facturas = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);

            if (facturas == null)
            {
                return NotFound("El factura no existe");
            }
            var facturasb = mapper.Map<FacturasBase>(facturas);
            return facturasb;

        }
        // POST api/factura
        [HttpPost]
        public async Task<FacturasBase> Post([FromBody] FacturasBase facturas)
        {
            var factura = mapper.Map<Facturas>(facturas);
            context.Add(factura);
            await context.SaveChangesAsync();
            return facturas;
        }

        // PUT api/factura/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasUpdateRequestDTO facturas)
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
        public async Task<ActionResult> Patch(int id, [FromBody] FacturasBase facturasDTO)
        {
            
            var properties = new UpdateMapperProperties<Facturas, FacturasBase>();
            var facturas = context.Facturas.Find(id);
            var result = await properties.MapperUpdate(facturas, facturasDTO);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/factura/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturasBase>> Delete(int id)
        {
            var facturas = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);

            if (facturas != null)
            {
                var empleadobd = context.Facturas.SingleOrDefault(a => a.EmpleadoId == id);
                var clientebd = context.Facturas.SingleOrDefault(a => a.ClienteId == id);
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
