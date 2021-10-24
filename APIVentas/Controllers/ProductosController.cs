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
    public class ProductosController : ControllerBase
    {
        public readonly Services.Productos.IProducto ProductoManager;
        public readonly Services.Productos.ProductoCategorias.IProductoCategoria ProductoCategoriaManager;

        public ProductosController(Services.Productos.IProducto productoManager,
            Services.Productos.ProductoCategorias.IProductoCategoria productoCategoriaManager)
        {
            ProductoManager = productoManager;
            ProductoCategoriaManager = productoCategoriaManager;
        }

        // GET: api/<ProductosController>
        [HttpGet]                            //---------------------Paginacion-----------------------//
        public async Task<IActionResult> Get([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Productos = await ProductoManager.Buscar(cantidad, pagina);

                return Ok(Productos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ProductosController>/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(string codigo)
        {
            try
            {
                var respuesta = await ProductoManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Producto = await ProductoManager.Buscar(codigo);

                return Ok(Producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ProductosController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.DTOs.Producto.InputProductoDTO nuevoPrducto)
        {
            try
            {
                var Producto = await ProductoManager.Crear(nuevoPrducto);

                return Created("Producto", Producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ProductosController>/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> Put(string codigo, [FromBody] Models.DTOs.Producto.InputProductoDTO nuevaInfoProducto)
        {
            try
            {
                var respuesta = await ProductoManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Producto = await ProductoManager.Editar(codigo, nuevaInfoProducto);

                return Ok(Producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            try
            {
                var respuesta = await ProductoManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var codigoProducto = await ProductoManager.Eliminar(codigo);

                return Ok(new { message = "El producto " + codigoProducto + " fue eliminado con exito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //-------------------------------------------PRODUCTO CATEGORIA-------------------------------------------//
        //--------------------------------------------------------------------------------------------------------//
        [HttpGet]
        [Route("Categorias/")]                            //---------------------Paginacion-----------------------//
        public async Task<IActionResult> GetAllCategorias([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Categorias = await ProductoCategoriaManager.Buscar(cantidad, pagina);

                return Ok(Categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Categorias/{codigo}")]
        public async Task<IActionResult> GetCategoria(string codigo)
        {
            try
            {
                var respuesta = await ProductoCategoriaManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Categoria = await ProductoCategoriaManager.Buscar(codigo);

                return Ok(Categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Categorias/")]
        public async Task<IActionResult> PostCategoria([FromBody] Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaCategoria)
        {
            try
            {
                var Categoria = await ProductoCategoriaManager.Crear(nuevaCategoria);

                return Created("Categoria", Categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Categorias/{codigo}")]
        public async Task<IActionResult> PutCategoria(string codigo, [FromBody] Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaInfoCategoria)
        {
            try
            {
                var respuesta = await ProductoCategoriaManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Categoria = await ProductoCategoriaManager.Editar(codigo, nuevaInfoCategoria);

                return Ok(Categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Categorias/{codigo}")]
        public async Task<IActionResult> DeleteCategoria(string codigo)
        {
            try
            {
                var respuesta = await ProductoCategoriaManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var codigoCategoria = await ProductoCategoriaManager.Eliminar(codigo);

                return Ok(new { message = "La categoria " + codigoCategoria + " fue eliminado con exito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Categorias/DropDown/")]
        public async Task<IActionResult> GetDropDown()
        {
            try
            {
                var Categorias = await ProductoCategoriaManager.BuscarDropDown();

                return Ok(Categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
