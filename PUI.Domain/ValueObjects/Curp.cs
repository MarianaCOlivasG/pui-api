using PUI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Domain.ValueObjects
{
    public record Curp
    {
        public string Valor { get; } = null!;

        private Curp()
        {

        }

        public Curp(string curp)
        {
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
            {
                throw new ExcepcionReglaNegocio($"El '{nameof(curp)}' debe tener exactamente 18 caracteres.");
            }
            Valor = curp.ToUpper();
        }
    }
}