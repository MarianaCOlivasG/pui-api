
namespace PUI.Infrastructure.ApiPUI
{
    public class ApiPuiSettings
    {
        public string BaseUrl { get; set; } = null!;
        public ApiPuiEndpoints Endpoints { get; set; } = new();
        public ApiPuiCredenciales Credenciales { get; set; } = new();


    }
}
