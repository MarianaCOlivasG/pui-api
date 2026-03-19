using Microsoft.AspNetCore.Mvc;
using PUI.Api.Dtos.Eventos;
using PUI.Api.Filters;
using PUI.Application.UseCases.NotificacionesPam.Commands.AgregarNotificacionPam;
using PUI.Application.UseCases.NotificacionesPam.Dtos;
using PUI.Application.UseCases.NotificacionesPam.Queries.ObtenerListado;
using PUI.Application.Utils.Mediator;
using PUI.Application.Utils.Paginacion;

namespace PUI.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {

        private readonly IMediator mediator;

        public EventosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("notificacion")]
        [ApiKey]
        public async Task<IActionResult> NuevaNotificacion([FromBody] NuevaNotificacionPAMDto dto)
        {
            //var apiKey = HttpContext.Items["ApiKey"]?.ToString();

            var command = new AgregarNotificacionPamCommand
            {
                Curp = dto.Curp,
                Nombre = dto.Nombre,    
                PrimerApellido = dto.PrimerApellido,   
                SegundoApellido = dto.SegundoApellido,
                TipoEvento = dto.TipoEvento,
                FechaEvento = dto.FechaEvento,
                DescripcionLugarEvento = dto.DescripcionLugarEvento 
            };

            var result = await mediator.Send(command);

            return Ok( result );
        }



        [HttpGet("listado")]
        public async Task<ActionResult<ResultadoPaginadoDto<NotificacionPamListadoDto>>> ObtenerListadoFiltrado(
            [FromQuery] string? curp,
            [FromQuery] int numeroPagina = 1,
            [FromQuery] int registrosPorPagina = 10)
            {
                var query = new ObtenerListadoNotificacionesPamQuery
                {
                    Curp = curp,
                    NumeroPagina = numeroPagina,
                    RegistrosPorPagina = registrosPorPagina
                };

                var result = await mediator.Send(query);

                return Ok(result);
            }



    }
}
