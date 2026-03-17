using PUI.Application.Interfaces.ApiPUI;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PUI.Infrastructure.ApiPUI
{
    public class ApiPuiService : IApiPuiService
    {
        private readonly HttpClient _httpClient;

        public ApiPuiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginPuiResponseDto> Login(string institucion_id, string clave)
        {
            var request = new
            {
                institucion_id,
                clave
            };

            var response = await _httpClient.PostAsJsonAsync("api/login", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginPuiResponseDto>()!;
        }
        /*_httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);*/

    }
}
