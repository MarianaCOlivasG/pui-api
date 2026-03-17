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
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;

        public AuthService(
            UserManager<Usuario> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<UsuarioAutenticadoDto> RegisterAsync(CredencialesUsuarioDto dto)
        {
            var usuario = new Usuario
            {
                UserName = dto.UserName,
                NormalizedUserName = dto.UserName.ToUpper(),
                Email = dto.UserName,
                NormalizedEmail = dto.UserName.ToUpper(),
                FechaCreacion = DateTime.UtcNow
            };

            var resultado = await userManager.CreateAsync(usuario, dto.Password!);

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
            var usuario = await userManager.FindByNameAsync(dto.UserName);

            if (usuario == null)
                throw new IdentityValidationException(new[] { "Credenciales incorrectas" });

            // usuario desactivado
            if (!usuario.Activo)
                throw new IdentityValidationException(new[] { "La cuenta está desactivada." });

            // usuario bloqueado
            if (usuario.BloqueoHasta != null && usuario.BloqueoHasta > DateTime.UtcNow)
                throw new IdentityValidationException(new[] { "La cuenta está temporalmente bloqueada." });

            var validPassword = await userManager.CheckPasswordAsync(usuario, dto.Password!);

            if (!validPassword)
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 5)
                {
                    usuario.BloqueoHasta = DateTime.UtcNow.AddMinutes(15);
                    usuario.IntentosFallidos = 0;
                }

                await userManager.UpdateAsync(usuario);

                throw new IdentityValidationException(new[] { "Credenciales incorrectas" });
            }

            // reset intentos
            usuario.IntentosFallidos = 0;
            usuario.UltimoLogin = DateTime.UtcNow;

            await userManager.UpdateAsync(usuario);

            return ConstruirToken(usuario);
        }

        private UsuarioAutenticadoDto ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName!),
                new Claim("usuario_id", usuario.Id.ToString())
            };

            var llave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
            );

            var credenciales = new SigningCredentials(
                llave,
                SecurityAlgorithms.HmacSha256
            );

            var minutos = int.Parse(configuration["Jwt:ExpireMinutes"]!);
            var expiracion = DateTime.UtcNow.AddMinutes(minutos);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
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