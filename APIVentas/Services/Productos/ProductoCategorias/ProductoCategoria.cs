using System;
using Microsoft.EntityFrameworkCore;
using APIVentas.Models.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIVentas.Services.Productos.ProductoCategorias
{
    public class ProductoCategoria : IProductoCategoria
    {
        public readonly VentasContext DbContext;

        public ProductoCategoria(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO>> Buscar()
        {
            var Categorias = from categoria in DbContext.ProductoCategoria
                             select new Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO()
                             {
                                 CategoriaCodigo = categoria.CategoriaCodigo,
                                 Nombre = categoria.Nombre,
                                 Descripcion = categoria.Descripcion,
                                 Impuesto = categoria.Impuesto
                             };

            return await Categorias.ToListAsync();
        }

        public async Task<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO> Buscar(string codigo)
        {
            var Categoria = from categoria in DbContext.ProductoCategoria
                            where categoria.CategoriaCodigo == codigo
                            select new Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO()
                            {
                                Nombre = categoria.Nombre,
                                Descripcion = categoria.Descripcion,
                                Impuesto = categoria.Impuesto
                            };

            return await Categoria.SingleOrDefaultAsync();
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.ProductoCategoria.AnyAsync(c => c.CategoriaCodigo == codigo);

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

                existeCodigo = await DbContext.ProductoCategoria.AnyAsync(c => c.CategoriaCodigo == codigo);

            } while (existeCodigo);

            return codigo;
        }

        public async Task<Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO> Crear(Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaCategoria)
        {
            string categoriaCodigo = await CrearCodigo(nuevaCategoria.Nombre);

            DbContext.ProductoCategoria.Add(new Models.Data.ProductoCategoria()
            {
                CategoriaCodigo = categoriaCodigo,
                Nombre = nuevaCategoria.Nombre,
                Descripcion = nuevaCategoria.Descripcion,
                Impuesto = nuevaCategoria.Impuesto,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now

            });

            await DbContext.SaveChangesAsync();

            return nuevaCategoria;
        }

        public async Task<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO> Editar(string codigo, Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaInfoCategoria)
        {
            var categoriaActual = await DbContext.ProductoCategoria.SingleOrDefaultAsync(c => c.CategoriaCodigo == codigo);

            categoriaActual.CategoriaCodigo = codigo;
            categoriaActual.Nombre = nuevaInfoCategoria.Nombre == null ? categoriaActual.Nombre : nuevaInfoCategoria.Nombre;
            categoriaActual.Descripcion = nuevaInfoCategoria.Descripcion == null ? categoriaActual.Descripcion : nuevaInfoCategoria.Descripcion;
            categoriaActual.Impuesto = nuevaInfoCategoria.Impuesto <= 0 ? categoriaActual.Impuesto : nuevaInfoCategoria.Impuesto;
            categoriaActual.FechaModificacion = DateTime.Now;

            DbContext.Update(categoriaActual);

            await DbContext.SaveChangesAsync();

            return await Buscar(codigo);
        }

        public async Task<string> Eliminar(string codigo)
        {
            var categoria = await DbContext.ProductoCategoria.SingleOrDefaultAsync(c => c.CategoriaCodigo == codigo);

            DbContext.Remove(categoria);
            await DbContext.SaveChangesAsync();

            return codigo;
        }

        public async Task<List<Models.DTOs.Producto.ProductoCategoria.DropDownCategoriaDTO>> BuscarDropDown()
        {
            var Categorias = from categoria in DbContext.ProductoCategoria
                             select new Models.DTOs.Producto.ProductoCategoria.DropDownCategoriaDTO()
                             {
                                 CategoriaId = categoria.CategoriaId,
                                 Nombre = categoria.Nombre,
                             };

            return await Categorias.ToListAsync();
        }
    }
}

