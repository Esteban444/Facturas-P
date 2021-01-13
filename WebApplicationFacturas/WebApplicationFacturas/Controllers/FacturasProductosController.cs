using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.Models;

namespace WebApplicationFacturas.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FacturasProductosController: ControllerBase
    {
        private readonly ApplicationDBContext context;

        public FacturasProductosController(ApplicationDBContext context)
        {
            this.context = context;
        }

        // GET api/facturasproductos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturasProductos>>> Get()
        {
            return await context.FacturasProductos.ToListAsync();

        }
        // GET api/facturasproductos/1
        [HttpGet("{id}", Name = "ObtenerFacturasProductos")]
        public async Task<ActionResult<FacturasProductos>> Get(int id)
        {
            var facturasproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.FacturaId == id);

            if (facturasproductos == null)
            {
                return NotFound();
            }
            return facturasproductos;

        }
        // POST api/facturaproductos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FacturasProductos facturasProductos)
        {
            context.Add(facturasProductos);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerFacturasProductos", new { id = facturasProductos.FacturaId }, facturasProductos);
        }

        // PUT api/facturaproductos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasProductos facturasProductos)
        {
            if (facturasProductos.FacturaId != id)
            {
                return BadRequest();
            }

            context.Entry(facturasProductos).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("");
        }

        // PATCH api/facturaproductos/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<FacturasProductos> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var facturasProductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.FacturaId == id);
            if (facturasProductos == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(facturasProductos, ModelState);

            var isValid = TryValidateModel(facturasProductos);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE api/facturaproductos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturasProductos>> Delete(int id)
        {
            var facturaproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.FacturaId == id);

            if (facturaproductos == null)
            {
                return NotFound("");
            }
            context.Remove(facturaproductos);
            await context.SaveChangesAsync();
            return Ok(facturaproductos);
        }
    }
}
