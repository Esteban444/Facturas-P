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
        public async Task<ActionResult<IEnumerable<ProductosRequestDTO>>> Get()
        {
            var productos = await context.Productos.Include ("Categorias").ToListAsync();
            var productosDTO = mapper.Map<List<ProductosRequestDTO>>(productos);
            return productosDTO;

        }
        // GET api/producto/1
        [HttpGet("{id}", Name = "ObtenerProducto")]
        public async Task<ActionResult<ProductosBase>> Get(int id)
        {
            var productoBd = await context.Productos.FirstOrDefaultAsync(x => x.Id == id);

            if (productoBd == null)
            {
                return NotFound("El producto no existe");
            }
            var productob = mapper.Map<ProductosBase>(productoBd);
            return productob;

        }
        // POST api/producto
        [HttpPost]
        public async Task<ProductosBase> Post([FromBody] ProductosBase Producto)
        {
            var productos = mapper.Map<Productos>(Producto);
            context.Add(productos);
            await context.SaveChangesAsync();
            return Producto;
        }

        // PUT api/producto/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductosUpdateRequestDTO producto)
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
        public async Task<ActionResult> Patch(int id,[FromBody] ProductosBase productosb )
        {
            var properties = new UpdateMapperProperties<Productos, ProductosBase>();
            var producto = context.Productos.Find(id);

            var result = await properties.MapperUpdate(producto, productosb);
            producto.Categorias = context.Categorias.Find(producto.CategoriaId);
            await context.SaveChangesAsync();
            return Ok(result);
        }
        // DELETE api/producto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductosBase>> Delete(int id)
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
