using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PUI.Api.DTOs.Auth;
using PUI.Api.DTOs.Reportes;
using PUI.Application.UseCases.Auth.Commands.LoginUsuario;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.UseCases.Reportes.Commands.ActivarReporte;
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginPuiDto dto)
        {
            var command = new LoginUsuarioCommand
            {
                Credenciales = new CredencialesUsuarioDto
                {
                    UserName = dto.Usuario,
                    Password = dto.Pass
                }
            };

            var result = await mediator.Send(command);

            return Ok(result);
        }



        [Authorize]
        [HttpPost("activar-reporte-prueba")]
        public async Task<IActionResult> ActivarReportePrueba([FromBody] ActivarReportePUIDto dto)
        {
            return Ok();
        }


        [Authorize]
        [HttpPost("activar-reporte")]
        public async Task<IActionResult> ActivarReporte([FromBody] ActivarReportePUIDto dto)
        {
            var command = new ActivarReporteCommand
            {
                Id = dto.Id,
                Curp = dto.Curp,
                LugarNacimiento = dto.LugarNacimiento,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                FechaNacimiento = dto.FechaNacimiento,
                FechaDesaparicion = dto.FechaDesaparicion,
                SexoAsignado = dto.SexoAsignado,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Direccion = dto.Direccion,
                Calle = dto.Calle,
                Numero = dto.Numero,
                Colonia = dto.Colonia,
                CodigoPostal = dto.CodigoPostal,
                MunicipioOAlcaldia = dto.MunicipioOAlcaldia,
                EntidadFederativa = dto.EntidadFederativa
            };

            var id = await mediator.Send(command);

            return Ok(new { id });
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
