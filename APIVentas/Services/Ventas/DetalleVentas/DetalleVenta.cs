using System;
using APIVentas.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Ventas.DetalleVentas
{
    public class DetalleVenta : IDetalleVenta
    {
        public readonly VentasContext DbContext;

        public DetalleVenta(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO>> Buscar(int cantidad, int pagina)
        {
            pagina = (pagina <= 0) ? 1 : pagina;
            cantidad = (cantidad <= 0) ? 10 : cantidad;


            var totalRecords = await DbContext.DetalleOrdenVenta.CountAsync();
            var totalPages = Math.Ceiling((double)totalRecords / cantidad);

            var skip = (pagina - 1) * cantidad;

            var Detalles = DbContext.DetalleOrdenVenta.Skip(skip).Take(cantidad).Select(detalles => new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
            {
                DetalleOrdenVenta = detalles.DetalleOrdenVentaCodigo,
                ProductoCodigo = detalles.ProductoCodigo,
                ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalles.ProductoCodigo)
                                                                    .Select(x => x.Nombre).SingleOrDefault(),
                ProductoCantidad = detalles.ProductoCantidad,
                ValorInicial = detalles.ValorBruto,
                DescuentoTotal = detalles.DescuentoTotal,
                ValorFinal = detalles.ValorNeto

            }).OrderBy(o => o.ProductoNombre);

            return await Detalles.ToListAsync();         
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.DetalleOrdenVenta.AnyAsync(o => o.DetalleOrdenVentaCodigo == codigo);

            return respuesta;
        }

        public async Task<Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO> Buscar(string codigo)
        {
            var Detalle = from detalle in DbContext.DetalleOrdenVenta
                          where detalle.DetalleOrdenVentaCodigo == codigo
                          select new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
                          {
                              ProductoCodigo = detalle.ProductoCodigo,
                              ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalle.ProductoCodigo)
                                                                  .Select(x => x.Nombre).SingleOrDefault(),
                              ProductoCantidad = detalle.ProductoCantidad,
                              ValorInicial = detalle.ValorBruto,
                              DescuentoTotal = detalle.DescuentoTotal,
                              ValorFinal = detalle.ValorNeto
                          };

            return await Detalle.SingleOrDefaultAsync();
        }
    }
}
