using System;
using System.Collections.Generic;
using System.Text;
using PUI.Domain.Exceptions;

namespace PUI.Domain.ValueObjects
{
    public record CorreoElectronico
    {

        public string Valor { get; } = null!;

        private CorreoElectronico()
        {

        }

        public CorreoElectronico(string correoElectronico)
        {


            if (string.IsNullOrEmpty(correoElectronico))
            {
                throw new ExcepcionReglaNegocio($"El '{nameof(correoElectronico)}' es requerido.");
            }

            if (!correoElectronico.Contains("@"))
            {
                throw new ExcepcionReglaNegocio($"El '{nameof(correoElectronico)}' no es válido.");
            }

            Valor = correoElectronico;

        }

    }
}
