using Microsoft.AspNetCore.Mvc;
using PUI.Application.UseCases.Reportes.Dtos;
using PUI.Application.UseCases.Reportes.Queries.ObtenerListado;
using PUI.Application.Utils.Mediator;

namespace PUI.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PuiController : ControllerBase
    {

        private readonly IMediator mediator;

        public PuiController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<List<ReporteListadoDto>>> Get()
        {
            var query = new ObtenerListadoReportesQuery();

            var result = await mediator.Send(query);

            return result;

        }




    }
}
