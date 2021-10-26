using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        // GET: api/<EstudiantesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                HttpClient cliente = new HttpClient();
                cliente.BaseAddress = new Uri("https://powerful-bayou-95420.herokuapp.com/api/v1/");

                var estudiantes = await cliente.GetAsync("students").Result.Content.ReadAsStringAsync();

                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
