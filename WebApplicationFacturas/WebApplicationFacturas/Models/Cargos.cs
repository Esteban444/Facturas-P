using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class Cargos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
       public int? EmpleadoId { get; set; }
        public virtual Empleados Empleado { get; set; }
    }
}
