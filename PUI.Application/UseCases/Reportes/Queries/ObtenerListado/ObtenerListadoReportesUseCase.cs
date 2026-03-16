



using PUI.Application.Interfaces.Repositories;
using PUI.Application.UseCases.Reportes.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Application.UseCases.Reportes.Queries.ObtenerListado
{

    public class ObtenerListadoReportesUseCase : IRequestHandler<ObtenerListadoReportesQuery, List<ReporteListadoDto>>
    {
        private readonly IReportesRepository reportesRepository;

        public ObtenerListadoReportesUseCase(IReportesRepository reportesRepository)
        {
            this.reportesRepository = reportesRepository;
        }


        public async Task<List<ReporteListadoDto>> Handle(ObtenerListadoReportesQuery request)
        {
            
            var reportes = await reportesRepository.ObtenerTodos();

            var listadoReportesDto = reportes.Select(reporte => reporte.ToListadoDto()).ToList();

            return listadoReportesDto;

        }


    }

}