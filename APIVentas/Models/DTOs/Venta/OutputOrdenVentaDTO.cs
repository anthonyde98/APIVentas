using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta
{
    public class OutputOrdenVentaDTO
    {
        public string OrdenVentaCodigo { get; set; }
        public DateTime OrdenFecha { get; set; }
        public decimal ValorCantidadNeto { get; set; }
        public decimal ValorCantidadBruto { get; set; }
        public decimal ValorPagado { get; set; }
        public decimal ValorDevuelto { get; set; }
        public decimal DescuentoTotalOrden { get; set; }
        public string VendedorCodigo { get; set; }
        public string ClienteCodigo { get; set; }
        public List<DetalleOrdenVentaDTO> Productos { get; set; }
    }
}
