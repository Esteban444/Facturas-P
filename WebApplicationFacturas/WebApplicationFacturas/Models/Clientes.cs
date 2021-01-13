using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public string Direccion {get; set;}
        public int Telefono { get; set; }
        public string Email { get; set; }

        public  ICollection<Facturas> Facturas { get; set; }
        public  ICollection<TipoClientes> TipoClientes { get; set; }
    }
}
