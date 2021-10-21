using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Producto
{
    public class OutputProductoDTO
    {
        public int ProductoCodigo { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioEstandar { get; set; }
        public decimal Tamanio { get; set; }
        public decimal Peso { get; set; }
        public decimal Descuento { get; set; }
        public decimal Categoria { get; set; }
        public DateTime FechaInicioVenta { get; set; }
        public DateTime FechaFinVenta { get; set; }
        public string Empresa { get; set; }
    }
}
