using Microsoft.AspNetCore.Mvc;
using PUI.Api.DTOs.Reportes;
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



    }
}
