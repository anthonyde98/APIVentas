using APIVentas.Models.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Ventas
{
    public class Venta :IVenta
    {
        public readonly VentasContext DbContext;

        public Venta(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> Buscar(int cantidad, int pagina)
        {

            pagina = (pagina <= 0) ? 1 : pagina;
            cantidad = (cantidad <= 0) ? 10 : cantidad;


            var totalRecords = await DbContext.OrdenVenta.CountAsync();
            var totalPages = Math.Ceiling((double)totalRecords / cantidad);

            var skip = (pagina - 1) * cantidad;

            var Ventas = DbContext.OrdenVenta.Skip(skip).Take(cantidad).Select(ventas => new Models.DTOs.Venta.OutputOrdenVentaDTO()
                         {
                             OrdenVentaCodigo = ventas.OrdenVentaCodigo,
                             OrdenFecha = ventas.OrdenFecha,
                             ClienteNombre = DbContext.Clientes.Where(c => c.ClienteCodigo == ventas.ClienteCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                             DbContext.Clientes.Where(c => c.ClienteCodigo == ventas.ClienteCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                             VendedorNombre = DbContext.Vendedores.Where(v => v.VendedorCodigo == ventas.VendedorCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                              DbContext.Vendedores.Where(v => v.VendedorCodigo == ventas.VendedorCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                             ValorCantidadInicial = ventas.ValorCantidadBruto,
                             ValorCantidadFinal = ventas.ValorCantidadNeto,
                             DescuentoTotalOrden = ventas.DescuentoTotalOrden,
                             ValorPagado = ventas.ValorPagado,
                             ValorDevuelto = ventas.ValorDevuelto,
                             Productos = (from detalle in DbContext.DetalleOrdenVenta
                                         where detalle.OrdenVentaCodigo == ventas.OrdenVentaCodigo
                                         select new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
                                         {
                                             ProductoCodigo = detalle.ProductoCodigo,
                                             ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalle.ProductoCodigo)
                                                                                 .Select(x => x.Nombre).SingleOrDefault(),
                                             ProductoCantidad = detalle.ProductoCantidad,
                                             ValorFinal = detalle.ValorNeto,
                                             ValorInicial = detalle.ValorBruto,
                                             DescuentoTotal = detalle.DescuentoTotal

                                         }).ToList()
                           }).OrderBy(o => o.VendedorNombre);

            return await Ventas.ToListAsync();
        }

        public async Task<Models.DTOs.Venta.OutputOrdenVentaDTO> Buscar(string codigo)
        {
            var Venta = from venta in DbContext.OrdenVenta
                         where venta.OrdenVentaCodigo == codigo
                         select new Models.DTOs.Venta.OutputOrdenVentaDTO()
                         {
                             OrdenVentaCodigo = venta.OrdenVentaCodigo,
                             OrdenFecha = venta.OrdenFecha,
                             ClienteNombre = DbContext.Clientes.Where(c => c.ClienteCodigo == venta.ClienteCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                             DbContext.Clientes.Where(c => c.ClienteCodigo == venta.ClienteCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                             VendedorNombre = DbContext.Vendedores.Where(v => v.VendedorCodigo == venta.VendedorCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                              DbContext.Vendedores.Where(v => v.VendedorCodigo == venta.VendedorCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                             ValorCantidadInicial = venta.ValorCantidadBruto,
                             ValorCantidadFinal = venta.ValorCantidadNeto,
                             DescuentoTotalOrden = venta.DescuentoTotalOrden,
                             ValorPagado = venta.ValorPagado,
                             ValorDevuelto = venta.ValorDevuelto,
                             Productos = (from detalle in DbContext.DetalleOrdenVenta
                                          where detalle.OrdenVentaCodigo == venta.OrdenVentaCodigo
                                          select new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
                                          {
                                              ProductoCodigo = detalle.ProductoCodigo,
                                              ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalle.ProductoCodigo)
                                                                                  .Select(x => x.Nombre).SingleOrDefault(),
                                              ProductoCantidad = detalle.ProductoCantidad,
                                              ValorFinal = detalle.ValorNeto,
                                              ValorInicial = detalle.ValorBruto,
                                              DescuentoTotal = detalle.DescuentoTotal

                                          }).ToList()
                         };

            return await Venta.SingleOrDefaultAsync();
        }

        public async Task<string> CrearCodigo(char c)
        {
            int longitud = 7;
            string codigo;
            bool existeCodigo;

            do
            {
                codigo = "";
                string caracteres = "1234567890";
                StringBuilder respuesta = new StringBuilder();
                Random rand = new Random();
                while (0 < longitud--)
                {
                    respuesta.Append(caracteres[rand.Next(caracteres.Length)]);
                }

                codigo = respuesta.ToString();

                if (c == 'o')
                    existeCodigo = await DbContext.OrdenVenta.AnyAsync(o => o.OrdenVentaCodigo == codigo);
                else
                    existeCodigo = await DbContext.DetalleOrdenVenta.AnyAsync(d => d.DetalleOrdenVentaCodigo == codigo);


            } while (existeCodigo);

            return codigo;
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.OrdenVenta.AnyAsync(o => o.OrdenVentaCodigo == codigo);

            return respuesta;
        }

        public async Task<Models.DTOs.Venta.InputOrdenVentaDTO> ProcesarOrden(Models.DTOs.Venta.InputOrdenVentaDTO orden)
        {
            var ordenCodigo = await CrearCodigo('o');

            decimal valorTotal = 0;
            decimal descuentoTotal = 0;
            decimal valorNoDescuento = 0;

            List<Models.Data.DetalleOrdenVenta> PD = new List<Models.Data.DetalleOrdenVenta>();

            foreach(var producto in orden.Productos)
            {
                /*var valorPorProducto = await DbContext.Productos.Where(p => p.ProductoCodigo == producto.ProductoCodigo).Select(x => x.PrecioEstandar)
                                                                  .SingleOrDefaultAsync();
                var descuentoPorProducto = await DbContext.Productos.Where(p => p.ProductoCodigo == producto.ProductoCodigo).Select(x => x.Descuento)
                                                                  .SingleOrDefaultAsync();*/
                var infoProducto = from p in DbContext.Productos
                                   where p.ProductoCodigo == producto.ProductoCodigo
                                   select new Models.DTOs.Venta.DetalleOrdenVenta.ProductoFacilitador()
                                   {
                                       PrecioEstandar = p.PrecioEstandar,
                                       Descuento = p.Descuento
                                   };
                var valores = await infoProducto.SingleOrDefaultAsync();

                var pd = new Models.Data.DetalleOrdenVenta()
                {
                    ProductoCodigo = producto.ProductoCodigo,
                    DescuentoTotal = valores.Descuento * producto.ProductoCantidad,
                    DetalleOrdenVentaCodigo = await CrearCodigo('c'),
                    OrdenVentaCodigo = ordenCodigo,
                    ValorBruto = (valores.PrecioEstandar + valores.Descuento) * producto.ProductoCantidad,
                    ValorNeto = valores.PrecioEstandar * producto.ProductoCantidad,
                    ProductoCantidad = producto.ProductoCantidad,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now

                };

                PD.Add(pd);

                valorTotal += valores.PrecioEstandar * producto.ProductoCantidad;
                descuentoTotal += valores.Descuento * producto.ProductoCantidad;
                valorNoDescuento += (valores.PrecioEstandar + valores.Descuento) * producto.ProductoCantidad;

            }

            if ((orden.ValorPagado - valorTotal) < 0)
                throw new Exception($"El valor pagado no es suficiente.");

            var OrdenVenta = new OrdenVenta()
            {
                OrdenVentaCodigo = ordenCodigo,
                OrdenFecha = DateTime.Now,
                ValorCantidadBruto = valorNoDescuento,
                ValorCantidadNeto = valorTotal,
                ValorPagado = orden.ValorPagado,
                ValorDevuelto = orden.ValorPagado - valorTotal,
                DescuentoTotalOrden = descuentoTotal,
                VendedorCodigo = orden.VendedorCodigo,
                ClienteCodigo = orden.ClienteCodigo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now

            };

            DbContext.OrdenVenta.Add(OrdenVenta);
            await DbContext.SaveChangesAsync();

            foreach(var detalle in PD)
            {
                DbContext.DetalleOrdenVenta.Add(detalle);
                await DbContext.SaveChangesAsync();
            }

            return orden;
        }

        public async Task<string> Eliminar(string codigo)
        {
            var OrdenVenta = await DbContext.OrdenVenta.FindAsync(codigo);

            DbContext.Remove(OrdenVenta);
            await DbContext.SaveChangesAsync();

            return codigo;

        }

        //-----------------------------------------------------------------------CLIENTE ORDEN DE VENTA---------------------------------------------//

        public async Task<bool> ExisteOrdenPorCliente(string codigo)
        {
            var respuesta = await DbContext.OrdenVenta.AnyAsync(o => o.ClienteCodigo == codigo);

            return respuesta;
        }

        public async Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> BuscarOrdenPorCliente(string codigo)
        {
            var Ordenes = from ordenes in DbContext.OrdenVenta
                          where ordenes.ClienteCodigo == codigo
                          select new Models.DTOs.Venta.OutputOrdenVentaDTO()
                          {
                              OrdenVentaCodigo = ordenes.OrdenVentaCodigo,
                              OrdenFecha = ordenes.OrdenFecha,
                              ClienteNombre = DbContext.Clientes.Where(c => c.ClienteCodigo == ordenes.ClienteCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                             DbContext.Clientes.Where(c => c.ClienteCodigo == ordenes.ClienteCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                              VendedorNombre = DbContext.Vendedores.Where(v => v.VendedorCodigo == ordenes.VendedorCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                              DbContext.Vendedores.Where(v => v.VendedorCodigo == ordenes.VendedorCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                              ValorCantidadInicial = ordenes.ValorCantidadBruto,
                              ValorCantidadFinal = ordenes.ValorCantidadNeto,
                              DescuentoTotalOrden = ordenes.DescuentoTotalOrden,
                              ValorPagado = ordenes.ValorPagado,
                              ValorDevuelto = ordenes.ValorDevuelto,
                              Productos = (from detalle in DbContext.DetalleOrdenVenta
                                           where detalle.OrdenVentaCodigo == ordenes.OrdenVentaCodigo
                                           select new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
                                           {
                                               ProductoCodigo = detalle.ProductoCodigo,
                                               ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalle.ProductoCodigo)
                                                                                   .Select(x => x.Nombre).SingleOrDefault(),
                                               ProductoCantidad = detalle.ProductoCantidad,
                                               ValorFinal = detalle.ValorNeto,
                                               ValorInicial = detalle.ValorBruto,
                                               DescuentoTotal = detalle.DescuentoTotal

                                           }).ToList()
                          };

            return await Ordenes.ToListAsync();
        }

        //-----------------------------------------------------------------------VENDEDOR ORDEN DE VENTA---------------------------------------------//

        public async Task<bool> ExisteOrdenPorVendedor(string codigo)
        {
            var respuesta = await DbContext.OrdenVenta.AnyAsync(o => o.VendedorCodigo == codigo);

            return respuesta;
        }

        public async Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> BuscarOrdenPorVendedor(string codigo)
        {
            var Ordenes = from ordenes in DbContext.OrdenVenta
                          where ordenes.VendedorCodigo == codigo
                          select new Models.DTOs.Venta.OutputOrdenVentaDTO()
                          {
                              OrdenVentaCodigo = ordenes.OrdenVentaCodigo,
                              OrdenFecha = ordenes.OrdenFecha,
                              ClienteNombre = DbContext.Clientes.Where(c => c.ClienteCodigo == ordenes.ClienteCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                             DbContext.Clientes.Where(c => c.ClienteCodigo == ordenes.ClienteCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                              VendedorNombre = DbContext.Vendedores.Where(v => v.VendedorCodigo == ordenes.VendedorCodigo).Select(x => x.Nombres).SingleOrDefault() + " " +
                                              DbContext.Vendedores.Where(v => v.VendedorCodigo == ordenes.VendedorCodigo).Select(x => x.Apellidos).SingleOrDefault(),
                              ValorCantidadInicial = ordenes.ValorCantidadBruto,
                              ValorCantidadFinal = ordenes.ValorCantidadNeto,
                              DescuentoTotalOrden = ordenes.DescuentoTotalOrden,
                              ValorPagado = ordenes.ValorPagado,
                              ValorDevuelto = ordenes.ValorDevuelto,
                              Productos = (from detalle in DbContext.DetalleOrdenVenta
                                           where detalle.OrdenVentaCodigo == ordenes.OrdenVentaCodigo
                                           select new Models.DTOs.Venta.DetalleOrdenVenta.OutputDetalleOrdenVentaDTO()
                                           {
                                               ProductoCodigo = detalle.ProductoCodigo,
                                               ProductoNombre = DbContext.Productos.Where(p => p.ProductoCodigo == detalle.ProductoCodigo)
                                                                                   .Select(x => x.Nombre).SingleOrDefault(),
                                               ProductoCantidad = detalle.ProductoCantidad,
                                               ValorFinal = detalle.ValorNeto,
                                               ValorInicial = detalle.ValorBruto,
                                               DescuentoTotal = detalle.DescuentoTotal

                                           }).ToList()
                          };

            return await Ordenes.ToListAsync();
        }
    }
}
