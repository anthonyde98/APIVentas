using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Producto
{
    public class OutputProductoDTO
    {
        public string ProductoCodigo { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioEstandar { get; set; }
        public decimal Tamanio { get; set; }
        public decimal Peso { get; set; }
        public decimal Descuento { get; set; }
        public string Categoria { get; set; }
        public string FechaInicioVenta { get; set; }
        public string FechaFinVenta { get; set; }
        public string Empresa { get; set; }
    }
}
