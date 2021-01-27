using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class TipoClientes
    {
        public int Id { get; set;}
        [Required]
        public string TipoCliente { get; set; }
        public int? ClienteId { get; set; }

        public Clientes Cliente { get; set; }
    }
}
