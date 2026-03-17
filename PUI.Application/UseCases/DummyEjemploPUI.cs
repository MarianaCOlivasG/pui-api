

using PUI.Application.Interfaces.ApiPUI;

namespace PUI.Application.UseCases
{
    public class MiUseCase
    {
        private readonly IApiPuiService _api;

        public MiUseCase(IApiPuiService api)
        {
            _api = api;
        }

        public async Task Ejecutar()
        {
            var data = await _api.Login("usuario", "pwd");
            // lógica de negocio aquí
        }
    }
}
