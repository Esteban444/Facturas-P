using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class Productos
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int PrecioCompra { get; set; }
        public int PrecioVenta { get; set; }
        public int PrecioActual { get; set; }
        public int PrecioMinimo { get; set; }
        public int PrecioMaximo { get; set; }
        public DateTime  FechaVencimiento { get; set; }
        public int CategoriaId { get; set; }

        public Categorias Categorias { get; set; }
        public ICollection<FacturasProductos> FacturasProductos { get; set; }
    }
}
