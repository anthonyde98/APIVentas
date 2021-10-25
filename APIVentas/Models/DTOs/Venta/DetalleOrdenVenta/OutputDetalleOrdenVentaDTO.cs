using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta.DetalleOrdenVenta
{
    public class OutputDetalleOrdenVentaDTO
    {
        public string DetalleOrdenVenta { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        public int ProductoCantidad { get; set; }
        public decimal ValorFinal { get; set; }
        public decimal ValorInicial{ get; set; }
        public decimal DescuentoTotal { get; set; }
    }
}
