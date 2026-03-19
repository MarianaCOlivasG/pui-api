using Microsoft.AspNetCore.Mvc;
using PUI.Api.DTOs.Auth;
using PUI.Api.Filters;
using PUI.Application.Interfaces.Auth;
using PUI.Application.UseCases.Auth.Commands.RegistrarUsuario;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {

        private readonly IMediator mediator;

        public EventosController(IMediator mediator, ICurrentUserService currentUser)
        {
            this.mediator = mediator;
        }

        [HttpGet("notificacion")]
        [ApiKey]
        public async Task<IActionResult> NuevaNotificacion()
        {
            var apiKey = HttpContext.Items["ApiKey"]?.ToString();

            return Ok(new { mensaje = "ok", apiKey });
        }


       
    }
}
