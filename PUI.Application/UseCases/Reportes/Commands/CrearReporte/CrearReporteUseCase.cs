


using PUI.Application.Interfaces.Persistence;
using PUI.Application.Interfaces.Repositories;
using PUI.Application.Utils.Mediator;
using PUI.Domain.Entities;
using PUI.Domain.ValueObjects;

namespace PUI.Application.UseCases.Reportes.Commands.CrearReporte
{
    
    public class CrearReporteUseCase: IRequestHandler<CrearReporteCommand, Guid>
    {
        private readonly IReportesRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CrearReporteUseCase(
            IReportesRepository repository,
            IUnitOfWork unitOfWork
        )
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        public async Task<Guid> Handle(CrearReporteCommand request )
        {

            return new Guid();
        }

    }

}