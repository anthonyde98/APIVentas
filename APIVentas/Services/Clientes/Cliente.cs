using APIVentas.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIVentas.Services.Clientes
{
    public class Cliente : ICliente
    {
        public readonly VentasContext DbContext;

        public Cliente(VentasContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Models.DTOs.Cliente.OutputClienteDTO>> Buscar()
        {
            var Clientes = from cliente in DbContext.Clientes
                           select new Models.DTOs.Cliente.OutputClienteDTO()
                           {
                               ClienteCodigo = cliente.ClienteCodigo,
                               NombreCompleto = cliente.Nombres + " " + cliente.Apellidos,
                               Correo = cliente.Correo,
                               NumeroTelefono = cliente.NumeroTelefono,
                               Direccion = cliente.Direccion,
                               MembresiaCodigo = cliente.MembresiaCodigo
                           };

            return await Clientes.ToListAsync();
        }

        public async Task<Models.DTOs.Cliente.OutputClienteDTO> Buscar(string codigo)
        {
            var Cliente = from cliente in DbContext.Clientes
                           where cliente.ClienteCodigo == codigo
                           select new Models.DTOs.Cliente.OutputClienteDTO()
                           {
                               NombreCompleto = cliente.Nombres + " " + cliente.Apellidos,
                               Correo = cliente.Correo,
                               NumeroTelefono = cliente.NumeroTelefono,
                               Direccion = cliente.Direccion,
                               MembresiaCodigo = cliente.MembresiaCodigo
                           };

            return await Cliente.SingleOrDefaultAsync();
        }

        public async Task<bool> Existe(string codigo)
        {
            var respuesta = await DbContext.Clientes.AnyAsync(c => c.ClienteCodigo == codigo);
  
            return respuesta;
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

                existeCodigo = await DbContext.Clientes.AnyAsync(c => c.ClienteCodigo == codigo);

            } while (existeCodigo);

            return codigo;
        }

        public async Task<string> CrearMembresiaCodigo()
        {
            int longitud = 10;

            bool existeMembresia;

            string membresia;

            do
            {
                string caracteres = "1234567890";
                StringBuilder respuesta = new StringBuilder();
                Random rand = new Random();
                while (0 < longitud--)
                {
                    respuesta.Append(caracteres[rand.Next(caracteres.Length)]);
                }

                membresia = respuesta.ToString();

                existeMembresia = await DbContext.Clientes.AnyAsync(c => c.MembresiaCodigo == membresia);

            } while (existeMembresia);

            return membresia;
        }

        public async Task<Models.DTOs.Cliente.InputClienteDTO> Crear(Models.DTOs.Cliente.InputClienteDTO nuevoCliente)
        {
            string clienteCodigo = await CrearCodigo(nuevoCliente.Nombres, nuevoCliente.Apellidos);
            string membresiaCodigo = await CrearMembresiaCodigo();

            DbContext.Clientes.Add(new Models.Data.Cliente()
            {
                ClienteCodigo = clienteCodigo,
                Nombres = nuevoCliente.Nombres,
                Apellidos = nuevoCliente.Apellidos,
                Cedula = nuevoCliente.Cedula,
                Correo = nuevoCliente.Correo,
                NumeroTelefono = nuevoCliente.NumeroTelefono,
                Direccion = nuevoCliente.Direccion,
                MembresiaCodigo = membresiaCodigo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now

            });

            await DbContext.SaveChangesAsync();

            return nuevoCliente;
        }

        public async Task<Models.DTOs.Cliente.InputClienteDTO> Editar(string codigo, Models.DTOs.Cliente.InputClienteDTO nuevoInfoCliente)
        {
            var clienteActual = await DbContext.Clientes.FindAsync(codigo);

            clienteActual.ClienteCodigo = codigo;
            clienteActual.Nombres = nuevoInfoCliente.Nombres;
            clienteActual.Apellidos = nuevoInfoCliente.Apellidos;
            clienteActual.Cedula = nuevoInfoCliente.Cedula;
            clienteActual.Correo = nuevoInfoCliente.Correo;
            clienteActual.NumeroTelefono = nuevoInfoCliente.NumeroTelefono;
            clienteActual.Direccion = nuevoInfoCliente.Direccion;
            clienteActual.FechaModificacion = DateTime.Now;

            DbContext.Update(clienteActual);

            await DbContext.SaveChangesAsync();

            return nuevoInfoCliente;
        }

        public async Task<string> Eliminar(string codigo)
        {
            var cliente = await DbContext.Clientes.FindAsync(codigo);

            DbContext.Remove(cliente);
            await DbContext.SaveChangesAsync();

            return codigo;
        }
    }
}
