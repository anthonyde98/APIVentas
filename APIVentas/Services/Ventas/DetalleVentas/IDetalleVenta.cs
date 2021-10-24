using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Ventas.DetalleVentas
{
    public interface IDetalleVenta
    {
        Task<List<Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO>> Buscar(int cantidad, int pagina);
        Task<Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO> Buscar(string codigo);
        Task<bool> Existe(string codigo);
    }
}
