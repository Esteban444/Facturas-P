using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.DTO.Requests;
using WebApplicationFacturas.Models;

namespace WebApplicationFacturas.DTO
{
    public class FacturasDTO
    {
        //public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }

       
        public  List<FacturasProductosRequestDTO> FacturasProductos { get; set; }
    }
}
