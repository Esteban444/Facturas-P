﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class EmpresasCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Propietario { get; set; }
        public string Email { get; set; }
        public string NIT { get; set; }
    }
}