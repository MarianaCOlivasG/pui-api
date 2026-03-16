using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PUI.Application.Interfaces.Auth;
using PUI.Application.UseCases.Auth.Dtos;
using PUI.Identity.Models;
using PUI.Infrastructure.Identity.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PUI.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<Usuario> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UsuarioAutenticadoDto> RegisterAsync(CredencialesUsuarioDto dto)
        {
            var usuario = new Usuario
            {
                UserName = dto.UserName
            };

            var resultado = await _userManager.CreateAsync(usuario, dto.Password!);

            if (!resultado.Succeeded)
            {
                var errores = resultado.Errors.Select(e =>
                {
                    if (e.Code == "DuplicateUserName")
                        return "Ya existe una cuenta con ese username.";

                    if (e.Code == "PasswordTooShort")
                        return "La contraseña es demasiado corta.";

                    if (e.Code == "PasswordRequiresUpper")
                        return "La contraseña debe tener una mayúscula.";

                    return e.Description;
                }).ToArray();

                throw new IdentityValidationException(errores);
            }

            return ConstruirToken(usuario);
        }

        public async Task<UsuarioAutenticadoDto> LoginAsync(CredencialesUsuarioDto dto)
        {
            var usuario = await _userManager.FindByNameAsync(dto.UserName);

            if (usuario == null)
                throw new IdentityValidationException(new[] { "Credenciales incorrectas" });

            var validPassword = await _userManager.CheckPasswordAsync(usuario, dto.Password!);

            if (!validPassword)
                throw new IdentityValidationException(new[] { "Credenciales incorrectas" });

            return ConstruirToken(usuario);
        }

        private UsuarioAutenticadoDto ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Name, usuario.UserName!)
            };

            var llave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)
            );

            var credenciales = new SigningCredentials(
                llave,
                SecurityAlgorithms.HmacSha256
            );

            var expiracion = DateTime.UtcNow.AddDays(4);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            return new UsuarioAutenticadoDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }
    }
}