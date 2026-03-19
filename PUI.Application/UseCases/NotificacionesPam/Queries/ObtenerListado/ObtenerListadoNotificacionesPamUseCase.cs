



using PUI.Application.Interfaces.Repositories;
using PUI.Application.UseCases.NotificacionesPam.Dtos;
using PUI.Application.UseCases.Reportes.Dtos;
using PUI.Application.Utils.Mediator;
using PUI.Application.Utils.Paginacion;


namespace PUI.Application.UseCases.NotificacionesPam.Queries.ObtenerListado
    {
        public class ObtenerListadoNotificacionesPamUseCase
            : IRequestHandler<ObtenerListadoNotificacionesPamQuery, ResultadoPaginadoDto<NotificacionPamListadoDto>>
        {
            private readonly INotificacionesPamRepository notificacionesPamRepository;

            public ObtenerListadoNotificacionesPamUseCase(
                INotificacionesPamRepository notificacionesPamRepository)
            {
                this.notificacionesPamRepository = notificacionesPamRepository;
            }

            public async Task<ResultadoPaginadoDto<NotificacionPamListadoDto>> Handle(
                ObtenerListadoNotificacionesPamQuery request)
            {
                var paginacion = new PaginacionDto(
                    request.NumeroPagina,
                    request.RegistrosPorPagina
                );

                var query = notificacionesPamRepository.Query();

                if (!string.IsNullOrWhiteSpace(request.Curp))
                {
                    query = query.Where(n => n.Curp.Valor == request.Curp);
                }

                var resultado = await notificacionesPamRepository
                    .ObtenerPaginado(query, paginacion);

                return new ResultadoPaginadoDto<NotificacionPamListadoDto>
                {
                    TotalRegistros = resultado.TotalRegistros,
                    NumeroPagina = resultado.NumeroPagina,
                    RegistrosPorPagina = resultado.RegistrosPorPagina,
                    Registros = resultado.Registros
                        .Select(n => n.ToListadoDto())
                        .ToList()
                };
            }
        }



}