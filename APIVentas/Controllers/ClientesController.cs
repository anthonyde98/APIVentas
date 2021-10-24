using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        public readonly Services.Clientes.ICliente ClienteManager;
        public readonly Services.Ventas.IVenta VentaManager;

        public ClientesController(Services.Clientes.ICliente clienteManager,
                                  Services.Ventas.IVenta ventaManager)
        {
            ClienteManager = clienteManager;
            VentaManager = ventaManager;
        }

        // GET: api/<ClientesController>
        [HttpGet]                            //---------------------Paginacion-----------------------//
        public async Task<IActionResult> Get([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Clientes = await ClienteManager.Buscar(cantidad, pagina);

                return Ok(Clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ClientesController>/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(string codigo)
        {
            try
            {
                var respuesta = await ClienteManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Cliente = await ClienteManager.Buscar(codigo);

                return Ok(Cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ClientesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.DTOs.Cliente.InputClienteDTO nuevoCliente)
        {
            try
            {
                var Cliente = await ClienteManager.Crear(nuevoCliente);

                return Created("Cliente", Cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> Put(string codigo, [FromBody] Models.DTOs.Cliente.InputClienteDTO nuevaInfoCliente)
        {
            try
            {
                var respuesta = await ClienteManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Cliente = await ClienteManager.Editar(codigo, nuevaInfoCliente);

                return Ok(Cliente);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            try
            {
                var respuesta = await ClienteManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var codigoCliente = await ClienteManager.Eliminar(codigo);

                return Ok(new { message = "El cliente " + codigoCliente + " fue eliminado con exito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //-------------------------------------------CLIENTE ORDEN DE VENTA-------------------------------------------//
        //------------------------------------------------------------------------------------------------------------//

        [HttpGet("Ordenes/")]
        public async Task<IActionResult> GetOrdenes([FromHeader] string codigo)
        {
            try
            {
                var respuesta = await VentaManager.ExisteOrdenPorCliente(codigo);

                if (!respuesta)
                    return NotFound();

                var Detalle = await VentaManager.BuscarOrdenPorCliente(codigo);

                return Ok(Detalle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
