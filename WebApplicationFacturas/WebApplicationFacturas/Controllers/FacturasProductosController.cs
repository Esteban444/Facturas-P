﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.DTO;
using WebApplicationFacturas.DTO.Requests;
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
        public async Task<ActionResult<FacturasProductosRequestDTO>> Get()
        {
           var facturasproducto = await context.FacturasProductos.ToListAsync();
            var facturasproductos = mapper.Map<List<FacturasProductosRequestDTO>>(facturasproducto);

            return Ok(facturasproductos);
        }
        // GET api/facturasproductos/1
        [HttpGet("{id}", Name = "ObtenerFacturasProductos")]
        public async Task<ActionResult<FacturasProductosBase>> Get(int id)
        {
            var facturasproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.Id == id);

            if (facturasproductos == null)
            {
                return NotFound("El detalle de factura no existe");
            }
            var facturasproductosb = mapper.Map<FacturasProductosBase>(facturasproductos);
            return facturasproductosb;

        }
        // POST api/facturaproductos
        [HttpPost]
        public async Task<FacturasProductosBase> Post([FromBody] FacturasProductosBase facturasP)
        {
            var facturasproductosbd = mapper.Map<FacturasProductos>(facturasP);
            context.Add(facturasproductosbd);
            await context.SaveChangesAsync();
            return facturasP;
        }

        // PUT api/facturaproductos/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasProductosUpdateRequestDTO facturasProductos)
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
        public async Task<ActionResult> Patch(int id, [FromBody] FacturasProductosBase facturasProducto)
        {
           
            var properties = new UpdateMapperProperties<FacturasProductos, FacturasProductosBase>();
            var facturasproductos = context.FacturasProductos.Find(id);
            var result = await properties.MapperUpdate(facturasproductos, facturasProducto);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/facturaproductos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturasProductosBase>> Delete(int id)
        {
            var facturaproductos = await context.FacturasProductos.FirstOrDefaultAsync(x => x.Id == id);

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
