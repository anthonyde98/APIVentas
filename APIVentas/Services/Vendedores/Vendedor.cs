using APIVentas.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Vendedores
{
    public class Vendedor : IVendedor
    {
        public readonly VentasContext DbContext;

        public Vendedor(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Vendedor.OutputVendedorDTO>> Buscar(int cantidad, int pagina)
        {
            pagina = (pagina <= 0) ? 1 : pagina;
            cantidad = (cantidad <= 0) ? 10 : cantidad;


            var totalRecords = await DbContext.Vendedores.CountAsync();
            var totalPages = Math.Ceiling((double)totalRecords / cantidad);

            var skip = (pagina - 1) * cantidad;

            var Vendedores = DbContext.Vendedores.Skip(skip).Take(cantidad).Select(vendedores => new Models.DTOs.Vendedor.OutputVendedorDTO()
                            {
                                VendedorCodigo = vendedores.VendedorCodigo,
                                NombreCompleto = vendedores.Nombres + " " + vendedores.Apellidos,
                                Correo = vendedores.Correo,
                                NumeroTelefono = vendedores.NumeroTelefono,
                                Direccion = vendedores.Direccion,
                                Departamento = vendedores.Departamento
                            }).OrderBy(o => o.NombreCompleto);

            return await Vendedores.ToListAsync();
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.Vendedores.AnyAsync(v => v.VendedorCodigo == codigo);

            return respuesta;
        }

        public async Task<Models.DTOs.Vendedor.OutputVendedorDTO> Buscar(string codigo)
        {
            var Vendedor = from vendedor in DbContext.Vendedores
                           where vendedor.VendedorCodigo == codigo
                           select new Models.DTOs.Vendedor.OutputVendedorDTO()
                           {
                               NombreCompleto = vendedor.Nombres + " " + vendedor.Apellidos,
                               Correo = vendedor.Correo,
                               NumeroTelefono = vendedor.NumeroTelefono,
                               Direccion = vendedor.Direccion,
                               Departamento = vendedor.Departamento
                           };

            return await Vendedor.SingleOrDefaultAsync();
        }

        public async Task<string> CrearCodigo(string nombre, string apellido)
        {
            int longitud = 5;
            string codigo = nombre[0].ToString() + apellido[0].ToString();
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

                existeCodigo = await DbContext.Vendedores.AnyAsync(v => v.VendedorCodigo == codigo);

            } while (existeCodigo);

            return codigo;
        }

        public async Task<Models.DTOs.Vendedor.InputVendedorDTO> Crear(Models.DTOs.Vendedor.InputVendedorDTO nuevoVendedor)
        {
            var existeCedula = await DbContext.Vendedores.AnyAsync(c => c.Cedula == nuevoVendedor.Cedula);
            string vendedorCodigo = await CrearCodigo(nuevoVendedor.Nombres, nuevoVendedor.Apellidos);

            if(existeCedula)
                throw new Exception($"La cedula ingresada es parte de otro vendedor.");

            DbContext.Vendedores.Add(new Models.Data.Vendedor()
            {
                VendedorCodigo = vendedorCodigo,
                Nombres = nuevoVendedor.Nombres,
                Apellidos = nuevoVendedor.Apellidos,
                Cedula = nuevoVendedor.Cedula,
                Correo = nuevoVendedor.Correo,
                NumeroTelefono = nuevoVendedor.NumeroTelefono,
                Direccion = nuevoVendedor.Direccion,
                Departamento = nuevoVendedor.Departamento,
                SalarioBruto = nuevoVendedor.SalarioBruto,
                DeduccionTotal = nuevoVendedor.DeduccionTotal,
                SalarioNeto = nuevoVendedor.SalarioBruto - nuevoVendedor.DeduccionTotal,
                SeguroCodigo = nuevoVendedor.SeguroCodigo,
                AfpCodigo = nuevoVendedor.AfpCodigo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now

            });

            await DbContext.SaveChangesAsync();

            return nuevoVendedor;
        }

        public async Task<Models.DTOs.Vendedor.OutputVendedorDTO> Editar(string codigo, Models.DTOs.Vendedor.InputVendedorDTO nuevoInfoVendedor)
        {
            var existeCedula = await DbContext.Vendedores.AnyAsync(v => v.Cedula == nuevoInfoVendedor.Cedula);
            var vendedorActual = await DbContext.Vendedores.FindAsync(codigo);

            if(existeCedula && vendedorActual.Cedula != nuevoInfoVendedor.Cedula)
                throw new Exception($"La cedula ingresada es parte de otro vendedor.");

            vendedorActual.VendedorCodigo = codigo;
            vendedorActual.Nombres = nuevoInfoVendedor.Nombres == null ? vendedorActual.Nombres : nuevoInfoVendedor.Nombres;
            vendedorActual.Apellidos = nuevoInfoVendedor.Apellidos == null ? vendedorActual.Apellidos : nuevoInfoVendedor.Apellidos;
            vendedorActual.Cedula = nuevoInfoVendedor.Cedula == null ? vendedorActual.Cedula : nuevoInfoVendedor.Cedula;
            vendedorActual.Correo = nuevoInfoVendedor.Correo == null ? vendedorActual.Correo : nuevoInfoVendedor.Correo;
            vendedorActual.NumeroTelefono = nuevoInfoVendedor.NumeroTelefono == null ? vendedorActual.NumeroTelefono : nuevoInfoVendedor.NumeroTelefono;
            vendedorActual.Direccion = nuevoInfoVendedor.Direccion == null ? vendedorActual.Direccion : nuevoInfoVendedor.Direccion;
            vendedorActual.Departamento = nuevoInfoVendedor.Departamento == null ? vendedorActual.Departamento : nuevoInfoVendedor.Departamento;
            vendedorActual.SalarioBruto = nuevoInfoVendedor.SalarioBruto <= 0 ? vendedorActual.SalarioBruto : nuevoInfoVendedor.SalarioBruto;
            vendedorActual.DeduccionTotal = nuevoInfoVendedor.DeduccionTotal <= 0 ? vendedorActual.DeduccionTotal : nuevoInfoVendedor.DeduccionTotal;
            vendedorActual.SalarioNeto = vendedorActual.SalarioBruto - vendedorActual.DeduccionTotal;
            vendedorActual.SeguroCodigo = nuevoInfoVendedor.SeguroCodigo == null ? vendedorActual.SeguroCodigo : nuevoInfoVendedor.SeguroCodigo;
            vendedorActual.AfpCodigo = nuevoInfoVendedor.AfpCodigo == null ? vendedorActual.AfpCodigo : nuevoInfoVendedor.AfpCodigo;
            vendedorActual.FechaModificacion = DateTime.Now;

            DbContext.Update(vendedorActual);

            await DbContext.SaveChangesAsync();

            return await Buscar(codigo);
        }

        public async Task<string> Eliminar(string codigo)
        {
            var vendedor = await DbContext.Vendedores.FindAsync(codigo);

            DbContext.Remove(vendedor);
            await DbContext.SaveChangesAsync();

            return codigo;
        }

    }
}
