using APIVentas.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIVentas.Services.Productos
{
    public class Producto : IProducto
    {
        public readonly VentasContext DbContext;

        public Producto(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Producto.OutputProductoDTO>> Buscar(int cantidad, int pagina)
        {
            pagina = (pagina <= 0) ? 1 : pagina;
            cantidad = (cantidad <= 0) ? 10 : cantidad;


            var totalRecords = await DbContext.ProductoCategoria.CountAsync();
            var totalPages = Math.Ceiling((double)totalRecords / cantidad);

            var skip = (pagina - 1) * cantidad;

            var Productos = (from producto in DbContext.Productos
                             from categoria in DbContext.ProductoCategoria
                             where producto.CategoriaId == categoria.CategoriaId
                             select new Models.DTOs.Producto.OutputProductoDTO()
                             {
                                 ProductoCodigo = producto.ProductoCodigo,
                                 Nombre = producto.Nombre,
                                 PrecioEstandar = producto.PrecioEstandar,
                                 Descuento = producto.Descuento,
                                 Tamanio = producto.Tamanio,
                                 Peso = producto.Peso,
                                 Categoria = categoria.Nombre,
                                 FechaFinVenta = Convert.ToString(producto.FechaFinVenta),
                                 FechaInicioVenta = Convert.ToString(producto.FechaInicioVenta),
                                 Empresa = producto.Empresa

                             }).Skip(skip).Take(cantidad).OrderBy(o => o.Nombre);


            return await Productos.ToListAsync();
        }

        public async Task<Models.DTOs.Producto.OutputProductoDTO> Buscar(string codigo)
        {

            var Producto = from producto in DbContext.Productos
                            from categoria in DbContext.ProductoCategoria
                            where producto.CategoriaId == categoria.CategoriaId && producto.ProductoCodigo == codigo
                            select new Models.DTOs.Producto.OutputProductoDTO()
                            {
                                Nombre = producto.Nombre,
                                PrecioEstandar = producto.PrecioEstandar,
                                Descuento = producto.Descuento,
                                Tamanio = producto.Tamanio,
                                Peso = producto.Peso,
                                Categoria = categoria.Nombre,
                                FechaFinVenta = Convert.ToString(producto.FechaFinVenta),
                                FechaInicioVenta = Convert.ToString(producto.FechaInicioVenta),
                                Empresa = producto.Empresa

                            };

            return await Producto.SingleOrDefaultAsync();
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.Productos.AnyAsync(p => p.ProductoCodigo == codigo);

            return respuesta;
        }

        public async Task<string> CrearCodigo(string nombre)
        {
            int longitud = 6;
            string codigo = nombre[0].ToString();
            bool existeCodigo;
            string numeros;

            do
            {
                numeros = "";
                string caracteres = "1234567890";
                StringBuilder respuesta = new StringBuilder();
                Random rand = new Random();
                while (0 < longitud--)
                {
                    respuesta.Append(caracteres[rand.Next(caracteres.Length)]);
                }

                numeros = respuesta.ToString();
                codigo += numeros;

                existeCodigo = await DbContext.Productos.AnyAsync(p => p.ProductoCodigo == codigo);

            } while (existeCodigo);

            return codigo;
        }

        public async Task<decimal> BuscarImpuesto(int id)
        {
            var impuesto = await DbContext.ProductoCategoria.Where(c => c.CategoriaId == id).Select(i => i.Impuesto).SingleAsync();

            return impuesto;
        }

        public async Task<Models.DTOs.Producto.InputProductoDTO> Crear(Models.DTOs.Producto.InputProductoDTO nuevoProducto)
        {
            string productoCodigo = await CrearCodigo(nuevoProducto.Nombre);
            decimal impuestoCategoria = await BuscarImpuesto(nuevoProducto.CategoriaId);

            DbContext.Productos.Add(new Models.Data.Producto()
            {
                ProductoCodigo = productoCodigo,
                Nombre = nuevoProducto.Nombre,
                PrecioLista = nuevoProducto.PrecioLista,
                PrecioEstandar = nuevoProducto.PrecioLista + (nuevoProducto.PrecioLista * (impuestoCategoria / 100)) + nuevoProducto.Ganancia - nuevoProducto.Descuento,
                Descuento = nuevoProducto.Descuento,
                Ganancia = nuevoProducto.Ganancia,
                Tamanio = nuevoProducto.Tamanio,
                Peso = nuevoProducto.Peso,
                CategoriaId = nuevoProducto.CategoriaId,
                FechaFinVenta = Convert.ToDateTime(nuevoProducto.FechaFinVenta),
                FechaInicioVenta = Convert.ToDateTime(nuevoProducto.FechaInicioVenta),
                Empresa = nuevoProducto.Empresa,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now

            });

            await DbContext.SaveChangesAsync();

            return nuevoProducto;
        }

        public async Task<Models.DTOs.Producto.OutputProductoDTO> Editar(string codigo, Models.DTOs.Producto.InputProductoDTO nuevaInfoProducto)
        {
            var productoActual = await DbContext.Productos.FindAsync(codigo);
            

            productoActual.CategoriaId = nuevaInfoProducto.CategoriaId <= 0 ? productoActual.CategoriaId : nuevaInfoProducto.CategoriaId;

            var impuestoCategoria = await BuscarImpuesto(productoActual.CategoriaId);

            productoActual.ProductoCodigo = codigo;
            productoActual.Nombre = nuevaInfoProducto.Nombre == null ? productoActual.Nombre : nuevaInfoProducto.Nombre;
            productoActual.PrecioLista = nuevaInfoProducto.PrecioLista <= 0 ? productoActual.PrecioLista : nuevaInfoProducto.PrecioLista;

            if (nuevaInfoProducto.Descuento == -1)
                productoActual.Descuento = 0;
            else
                productoActual.Descuento = nuevaInfoProducto.Descuento <= 0 ? productoActual.Descuento : nuevaInfoProducto.Descuento;

            productoActual.Ganancia = nuevaInfoProducto.Ganancia <= 0 ? productoActual.Ganancia : nuevaInfoProducto.Ganancia;
            productoActual.Tamanio = nuevaInfoProducto.Tamanio <= 0 ? productoActual.Tamanio : nuevaInfoProducto.Tamanio;
            productoActual.Peso = nuevaInfoProducto.Peso <= 0 ? productoActual.Peso : nuevaInfoProducto.Peso;
            productoActual.PrecioEstandar = productoActual.PrecioLista + (productoActual.PrecioLista * (impuestoCategoria / 100)) + productoActual.Ganancia - productoActual.Descuento;
            productoActual.FechaFinVenta = nuevaInfoProducto.FechaFinVenta == null ? productoActual.FechaFinVenta : Convert.ToDateTime(nuevaInfoProducto.FechaFinVenta);
            productoActual.FechaInicioVenta = nuevaInfoProducto.FechaInicioVenta == null ? productoActual.FechaInicioVenta : Convert.ToDateTime(nuevaInfoProducto.FechaInicioVenta);
            productoActual.Empresa = nuevaInfoProducto.Empresa == null ? productoActual.Empresa : nuevaInfoProducto.Empresa;
            productoActual.FechaModificacion = DateTime.Now;

            DbContext.Update(productoActual);

            await DbContext.SaveChangesAsync();

            return await Buscar(codigo);
        }

        public async Task<string> Eliminar(string codigo)
        {
            var Producto = await DbContext.Productos.FindAsync(codigo);

            DbContext.Remove(Producto);
            await DbContext.SaveChangesAsync();

            return codigo;
        }
    }
}
