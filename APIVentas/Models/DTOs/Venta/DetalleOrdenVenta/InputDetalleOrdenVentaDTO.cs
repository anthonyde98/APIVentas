using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta.DetalleOrdenVenta
{
    public class InputDetalleOrdenVentaDTO
    {
        public string ProductoCodigo { get; set; }
        public int ProductoCantidad { get; set; }
    }
}
