using PUI.Domain.Entities;

namespace PUI.Application.UseCases.Reportes.Dtos
{
    public static class MapperExtensions
    {
        public static ReporteDto ToDto(this Reporte reporte)
        {
            return new ReporteDto
            {
                Id = reporte.Id,
                FolioPui = reporte.FolioPui,
                Curp = reporte.Curp?.Valor ?? string.Empty,
                Nombre = reporte.Nombre,
                PrimerApellido = reporte.PrimerApellido,
                SegundoApellido = reporte.SegundoApellido,
                FechaNacimiento = reporte.FechaNacimiento,
                FechaDesaparicion = reporte.FechaDesaparicion,
                Telefono = reporte.Telefono,
                Correo = reporte.Correo?.Valor,
                FechaActivacion = reporte.FechaActivacion,
                FechaDesactivacion = reporte.FechaDesactivacion
            };
        }

        public static ReporteListadoDto ToListadoDto(this Reporte reporte)
        {
            return new ReporteListadoDto
            {
                Id = reporte.Id,
                FolioPui = reporte.FolioPui,
                Curp = reporte.Curp?.Valor ?? string.Empty,
                NombreCompleto = $"{reporte.Nombre} {reporte.PrimerApellido}",
                FechaActivacion = reporte.FechaActivacion,
                Estatus = reporte.Estatus.ToString()
            };
        }
    }
}