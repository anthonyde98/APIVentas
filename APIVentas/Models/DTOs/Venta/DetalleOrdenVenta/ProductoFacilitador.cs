using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta.DetalleOrdenVenta
{
    public class ProductoFacilitador
    {
        public decimal PrecioEstandar { get; set; }
        public decimal Descuento { get; set; }
    }
}
