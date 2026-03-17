using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Application.Interfaces.ApiPUI
{
    public interface IApiPuiService
    {
        Task<LoginPuiResponseDto> Login(string institucion_id, string clave);
    }
}
