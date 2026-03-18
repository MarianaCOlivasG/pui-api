using Microsoft.Extensions.Options;
using PUI.Application.Interfaces.ApiPUI;
using PUI.Infrastructure.Infrastructure.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

            if (string.IsNullOrEmpty(_settings.BaseUrl))
                throw new Exception("ApiPui:BaseUrl no está configurada");

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }
        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }

            string mensaje = "Error en API PUI";
            string? codigo = null;
            object? errores = null;

            try
            {
                var json = JsonSerializer.Deserialize<JsonElement>(content);

                if (json.TryGetProperty("mensaje", out var msg))
                    mensaje = msg.GetString() ?? mensaje;

                if (json.TryGetProperty("error", out var err))
                    mensaje = err.GetString() ?? mensaje;

                if (json.TryGetProperty("codigo", out var cod))
                    codigo = cod.GetString();

                if (json.TryGetProperty("errores", out var errs))
                    errores = errs;
            }
            catch
            {
                mensaje = content;
            }

            throw new ApiPuiException(
                mensaje,
                (int)response.StatusCode,
                codigo,
                errores
            );
        }

        private async Task EnsureAuth()
        {
            if (string.IsNullOrEmpty(_token))
                await Login();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }
  
        public async Task<LoginPuiResponseDto?> Login()
        {
            var request = new
            {
                institucion_id = _settings.Credenciales.InstitucionId ?? ":)",
                clave = _settings.Credenciales.Clave ?? ":)"
            };

            var response = await _httpClient.PostAsJsonAsync(_settings.Endpoints.Login, request);

            var loginResponse = await HandleResponse<LoginPuiResponseDto>(response);

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
            await EnsureAuth();

            var response = await _httpClient.GetAsync(_settings.Endpoints.ListarReportes);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Login();
                response = await _httpClient.GetAsync(_settings.Endpoints.ListarReportes);
            }

            return await HandleResponse<List<dynamic>>(response);
        }
     
        public async Task<NotificarCoincidenciaResponseDto> NotificarCoincidencia(NotificarCoincidenciaRequestDto request)
        {
            await EnsureAuth();

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Endpoints.NotificarCoincidencia,
                request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Login();
                response = await _httpClient.PostAsJsonAsync(
                    _settings.Endpoints.NotificarCoincidencia,
                    request);
            }

            return await HandleResponse<NotificarCoincidenciaResponseDto>(response);
        }
        
        public async Task<BusquedaFinalizadaResponseDto> BusquedaFinalizada(BusquedaFinalizadaRequestDto request)
        {
            await EnsureAuth();

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Endpoints.BusquedaFinalizada,
                request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Login();
                response = await _httpClient.PostAsJsonAsync(
                    _settings.Endpoints.BusquedaFinalizada,
                    request);
            }

            return await HandleResponse<BusquedaFinalizadaResponseDto>(response);
        }
 
    }
}