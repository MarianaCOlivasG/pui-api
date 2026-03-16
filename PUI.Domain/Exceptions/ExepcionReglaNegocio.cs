using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Domain.Exceptions
{

        public class ExcepcionReglaNegocio : Exception
        {

            public ExcepcionReglaNegocio(string mensaje) : base(mensaje)
            {

            }

        }

}