using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO.Requests
{
    public class ProductosBase
    {
        public string Nombre { get; set; }
        public int? Cantidad { get; set; }
        public int? PrecioCompra { get; set; }
        public int? PrecioVenta { get; set; }
        public int? PrecioActual { get; set; }
        public int? PrecioMinimo { get; set; }
        public int? PrecioMaximo { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? CategoriaId { get; set; }
    }
}
