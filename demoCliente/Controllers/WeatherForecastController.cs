using demoCliente.Data;
using demoCliente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace demoCliente.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("clientes", Name = "GetClientes")]
        public IEnumerable<DemoCliente> GetClientes()
        {
            //var clientesList = new List<DemoCliente>();
            //using (var db = new AppDbContext())
            //{
            //    clientesList = db.Clientes.ToList();
            //}
            //return clientesList;
            return _dbContext.Clientes.ToList();
        }

        [HttpGet("direcciones", Name = "GetDirecciones")]
        public IEnumerable<DemoDireccion> GetDirecciones()
        {
            return _dbContext.Direcciones.ToList();
        }

        [HttpPost("alta", Name = "AltaCliente")]
        public async Task<IActionResult> AltaCliente([FromBody] DtoAltaCliente nuevoCliente)
        {
            if (nuevoCliente == null)
                return BadRequest("Cliente no puede ser nulo.");

            string resultado = await _dbContext.AltaClienteAsync(
                nuevoCliente.Nombre,
                nuevoCliente.ApellidoPaterno,
                nuevoCliente.ApellidoMaterno,
                nuevoCliente.Calle,
                nuevoCliente.NumeroInterior,
                nuevoCliente.Colonia,
                nuevoCliente.CodigoPostal,
                nuevoCliente.Municipio
            );

            return Ok(new { Mensaje = resultado });
        }

        [HttpPost("Eliminar", Name = "EliminarCliente")]
        public async Task<IActionResult> EliminarCliente([FromBody] int clienteId)
        {
            var mensaje = new SqlParameter
            {
                ParameterName = "@Mensaje",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 50,
                Direction = System.Data.ParameterDirection.Output
            };

            await _dbContext.Database
                .ExecuteSqlRawAsync("EXEC EliminarCliente @ClienteId, @Mensaje OUTPUT", new SqlParameter("@ClienteId", clienteId), mensaje);

            string resultado = (string)mensaje.Value;
            if (resultado.StartsWith("Fail"))
                return NotFound(resultado);

            return Ok(resultado);
        }
    }
}
