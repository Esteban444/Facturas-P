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
        public async Task<ActionResult<FacturasBase>> CreacionDetalleFacturas(FacturasBase facturas)
        {
            var detallefactura = new FacturasBase();
            using (var transaccion = context.Database.BeginTransaction())
            {
                try
                {
                    var tablafactura = mapper.Map<Facturas>(facturas);
                    if (!context.Empleados.Any(x => x.Id == tablafactura.EmpleadoId)) return BadRequest("El empleado no existe");
                    if (!context.Clientes.Any(x => x.Id == tablafactura.ClienteId)) return BadRequest("El cliente no existe");

                    context.Facturas.Add(tablafactura);
                    context.SaveChanges();

                    foreach (var item in facturas.FacturasProductos)
                    {
                        var facturadetalle = mapper.Map<FacturasProductos>(item);
                        if (!context.Facturas.Any(x => x.Id == facturadetalle.FacturaId)) return BadRequest("la factura no existe");
                        if (!context.Productos.Any(x => x.Id == facturadetalle.ProductosId)) return BadRequest("El producto no existe");

                        facturadetalle.FacturaId = tablafactura.Id;

                        context.FacturasProductos.Add(facturadetalle);
                    }
                    context.SaveChanges();
                    transaccion.Commit();

                }
                catch (Exception ex)
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
        public async Task<FacturasBase> Post([FromBody] FacturasBase Facturas)
        {
            var factura = mapper.Map<Facturas>(Facturas);
            context.Add(factura);
            await context.SaveChangesAsync();
            return Facturas;
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
