using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Cliente
{
    public class OutputClienteDTO
    {
        public string ClienteCodigo { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefono { get; set; }
        public string Direccion { get; set; }
        public string MembresiaCodigo { get; set; }
    }
}
