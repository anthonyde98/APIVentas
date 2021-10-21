using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Venta
{
    public class InputOrdenVentaDTO
    {
        public DateTime OrdenFecha { get; set; }
        public decimal ValorPagado { get; set; }
        public string VendedorCodigo { get; set; }
        public string ClienteCodigo { get; set; }
        public List<DetalleOrdenVentaDTO> Productos { get; set; }
    }
}
