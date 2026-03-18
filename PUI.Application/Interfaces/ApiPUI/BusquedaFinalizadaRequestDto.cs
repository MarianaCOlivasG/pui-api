using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Application.Interfaces.ApiPUI
{
    public class BusquedaFinalizadaRequestDto
    {
        public required string id { get; set; } = null!;
        public required string institucion_id { get; set; } = null!;
    }
}
