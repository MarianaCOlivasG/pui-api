using PUI.Application.UseCases.Reportes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Application.Interfaces.ApiPUI
{
    public interface IApiPuiService
    {
        Task<LoginPuiResponseDto?> Login();
        Task<List<dynamic>> ListarReportes();
        Task<NotificarCoincidenciaResponseDto> NotificarCoincidencia( NotificarCoincidenciaRequestDto request );
        Task<BusquedaFinalizadaResponseDto> BusquedaFinalizada( BusquedaFinalizadaRequestDto request );
    }
}
