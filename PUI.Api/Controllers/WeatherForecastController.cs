using Microsoft.AspNetCore.Mvc;
using PUI.Api.DTOs.Reportes;
using PUI.Application.UseCases.Reportes.Commands.ActivarReporte;
using PUI.Application.UseCases.Reportes.Commands.CrearReporte;
using PUI.Application.UseCases.Reportes.Dtos;
using PUI.Application.UseCases.Reportes.Queries.ObtenerListado;
using PUI.Application.Utils.Mediator;

namespace PUI.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly IMediator mediator;

        public WeatherForecastController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<List<ReporteListadoDto>>> Get()
        {
            var query = new ObtenerListadoReportesQuery();

            var result = await mediator.Send(query);

            return result;

        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivarReportePUIDto dto)
        {
            var command = new ActivarReporteCommand
            {
                Id = dto.Id,
                Curp = dto.Curp,
                LugarNacimiento = dto.LugarNacimiento,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                FechaNacimiento = dto.FechaNacimiento,
                FechaDesaparicion = dto.FechaDesaparicion,
                SexoAsignado = dto.SexoAsignado,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
            };

            
            var id = await mediator.Send(command);

            return Ok(new { id });
        
        }


    }
}
