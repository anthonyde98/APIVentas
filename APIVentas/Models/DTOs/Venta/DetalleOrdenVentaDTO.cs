using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta
{
    public class DetalleOrdenVentaDTO
    {
        public string ProductoCodigo { get; set; }
        public int ProductoCantidad { get; set; }
    }
}
