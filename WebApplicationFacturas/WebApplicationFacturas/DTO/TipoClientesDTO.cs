using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class TipoClientesDTO
    {

        public int Id { get; set; }
        [Required]
        public string TipoCliente { get; set; }
        public int ClienteId { get; set; }

        public ClientesDTO Cliente { get; set; }
    }
}
