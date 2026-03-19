using PUI.Domain.Entities;

namespace PUI.Application.UseCases.NotificacionesPam.Dtos
{
    public static class MapperExtensions
    {
        public static NotificacionPamListadoDto ToListadoDto(this NotificacionPam reporte)
        {
            return new NotificacionPamListadoDto
            {
                Id = reporte.Id,
                Curp = reporte.Curp?.Valor ?? string.Empty,
                Nombre = reporte.Nombre,
                PrimerApellido = reporte.PrimerApellido,
                SegundoApellido = reporte.SegundoApellido,
                TipoEvento = reporte.TipoEvento,
                FechaEvento = reporte.FechaEvento,
                DescripcionLugarEvento = reporte.DescripcionLugarEvento 
            };
        }
    }
}