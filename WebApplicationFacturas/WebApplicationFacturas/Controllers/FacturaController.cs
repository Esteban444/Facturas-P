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
        public async Task<ActionResult<IEnumerable<FacturasDTO>>> Get()
        {
            var facturas = await context.Facturas.Include("FacturasProductos").ToListAsync();
            var facturasDTO = mapper.Map<List<FacturasDTO>>(facturas);
            return facturasDTO;

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
        public async Task<ActionResult> Post([FromBody] Facturas facturas)
        {
            context.Add(facturas);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerFactura", new { id = facturas.Id }, facturas);
        }

        // PUT api/factura/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Facturas facturas)
        {
            if (facturas.Id != id)
            {
                return BadRequest();
            }

            context.Entry(facturas).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("La factura fue modificado");
        }

        // PATCH api/factura/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Facturas> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var facturas = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);
            if (facturas == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(facturas, ModelState);

            var isValid = TryValidateModel(facturas);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/factura/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Facturas>> Delete(int id)
        {
            var facturas = await context.Facturas.FirstOrDefaultAsync(x => x.Id == id);

            if (facturas == null)
            {
                return NotFound("La factura no existe");
            }
            context.Remove(facturas);
            await context.SaveChangesAsync();
            return Ok(facturas);
        }

    }
}
