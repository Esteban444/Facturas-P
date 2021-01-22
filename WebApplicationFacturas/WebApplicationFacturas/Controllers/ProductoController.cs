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
    public class ProductoController: ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ProductoController(ApplicationDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestedProductosDTO>>> Get()
        {
            var productos = await context.Productos.Include ("Categorias").ToListAsync();
            var pedidoproductosDTO = mapper.Map<List<RequestedProductosDTO>>(productos);
            return pedidoproductosDTO;

        }
        // GET api/producto/1
        [HttpGet("{id}", Name = "ObtenerProducto")]
        public async Task<ActionResult<ProductosDTO>> Get(int id)
        {
            var producto = await context.Productos.Include("Categorias").FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                return NotFound("El producto no existe");
            }
            var productoDTO = mapper.Map<ProductosDTO>(producto);
            return productoDTO;

        }
        // POST api/producto
        [HttpPost]
        public async Task<Productos> Post([FromBody] CreacionProductosDTO creacionProductosDTO)
        {
            var productos = mapper.Map<Productos>(creacionProductosDTO);
            context.Add(productos);
            await context.SaveChangesAsync();
            return productos;
        }

        // PUT api/producto/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductosDTO producto)
        {
           try
           {
                var productoBd = await context.Productos.FirstOrDefaultAsync(x => x.Id == id);
                if (productoBd != null)
                {
                     mapper.Map(producto,productoBd);
                    await context.SaveChangesAsync();

                }
                else
                {
                    throw new NullReferenceException();
                }
           }
           catch (NullReferenceException)
           {
              return BadRequest("El producto no existe en la base de datos");
           }
            return Ok(producto);
        }

        // PATCH api/producto/1
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id,[FromBody] ProductosDTO productosDTO )
        {
            var properties = new UpdateMapperProperties<Productos, ProductosDTO>();
            var producto = context.Productos.Find(id);

            var result = await properties.MapperUpdate(producto, productosDTO);
            producto.Categorias = context.Categorias.Find(producto.CategoriaId);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/producto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Productos>> Delete(int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.Id == id);

            if (producto != null)
            {
                context.Productos.Remove(producto);
                await context.SaveChangesAsync();
                return Ok(producto);
                
            }
            else
            {
                return BadRequest("El producto no existe");
            }
            
        }
    }
}
