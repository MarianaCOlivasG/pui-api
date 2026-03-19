using PUI.Application.UseCases.NotificacionesPam.Dtos;
using PUI.Application.Utils.Mediator;
using PUI.Application.Utils.Paginacion;

namespace PUI.Application.UseCases.NotificacionesPam.Queries.ObtenerListado
{
    public class ObtenerListadoNotificacionesPamQuery 
        : IRequest<ResultadoPaginadoDto<NotificacionPamListadoDto>>
    {
        public string? Curp { get; set; }

        public int NumeroPagina { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
    }
}