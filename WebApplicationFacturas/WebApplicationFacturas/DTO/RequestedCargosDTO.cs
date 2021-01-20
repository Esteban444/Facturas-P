using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class RequestedCargosDTO
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public int EmpleadoId { get; set; }
    }
}
