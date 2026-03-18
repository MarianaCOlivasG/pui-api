using Microsoft.Extensions.Options;
using PUI.Application.Interfaces.ApiPUI;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PUI.Infrastructure.ApiPUI
{
    public class ApiPuiService : IApiPuiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiPuiSettings _settings;
        private string? _token;

        public ApiPuiService(HttpClient httpClient, IOptions<ApiPuiSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;

            // BaseAddress desde settings
            if (string.IsNullOrEmpty(_settings.BaseUrl))
                throw new Exception("ApiPui:BaseUrl no está configurada");

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        // Login y guardar token
        public async Task<LoginPuiResponseDto?> Login()
        {
            var request = new
            {
                institucion_id = _settings.Credenciales.InstitucionId ?? ":)",
                clave = _settings.Credenciales.Clave ?? ":)"
            };

            var endpoint = _settings.Endpoints.Login;

            var response = await _httpClient.PostAsJsonAsync(endpoint, request);
            response.EnsureSuccessStatusCode();

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginPuiResponseDto>();

            if (loginResponse?.Token != null)
            {
                _token = loginResponse.Token;
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
            }

            return loginResponse;
        }

        public async Task<List<dynamic>> ListarReportes()
        {
            if (string.IsNullOrEmpty(_token)) throw new InvalidOperationException("Debe hacer login antes de llamar a este endpoint");

            var endpoint = _settings.Endpoints.ListarReportes;

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }

            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Login();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                response = await _httpClient.GetAsync(endpoint);
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<dynamic>>()!;
        }

        public async Task<NotificarCoincidenciaResponseDto> NotificarCoincidencia(NotificarCoincidenciaRequestDto request)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Debe hacer login antes de llamar a este endpoint");

            var endpoint = _settings.Endpoints.NotificarCoincidencia;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.PostAsJsonAsync(endpoint, request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Login();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                response = await _httpClient.PostAsJsonAsync(endpoint, request);
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NotificarCoincidenciaResponseDto>()!;
        }

        public async Task<BusquedaFinalizadaResponseDto> BusquedaFinalizada(BusquedaFinalizadaRequestDto request)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Debe hacer login antes de llamar a este endpoint");

            var endpoint = _settings.Endpoints.BusquedaFinalizada;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.PostAsJsonAsync(endpoint, request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await Login();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                response = await _httpClient.PostAsJsonAsync(endpoint, request);
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<BusquedaFinalizadaResponseDto>()!;
        }
    }
}