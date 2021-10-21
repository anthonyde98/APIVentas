using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Producto
{
    public class InputProductoCategoriaDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Impuesto { get; set; }
    }
}
