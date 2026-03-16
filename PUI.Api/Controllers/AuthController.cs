using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PUI.Api.DTOs.Auth;
using PUI.Application.Interfaces.Auth;
using PUI.Application.UseCases.Auth.Commands.LoginUsuario;
using PUI.Application.UseCases.Auth.Commands.RegistrarUsuario;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Application.Utils.Mediator;

namespace PUI.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserService currentUser;

        public AuthController(IMediator mediator, ICurrentUserService currentUser)
        {
            this.mediator = mediator;
            this.currentUser = currentUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CredencialesUsuarioDto dto)
        {
            var command = new RegistrarUsuarioCommand
            {
                Credenciales = dto
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CredencialesUsuarioDto dto)
        {
            var command = new LoginUsuarioCommand
            {
                Credenciales = dto
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("renew")]
        public IActionResult Renew()
        {
            var userId = currentUser.UserId;

            return Ok(new
            {
                usuarioId = userId
            });
        }
    }
}