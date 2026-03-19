

namespace PUI.Application.Security
{
    public interface IApiKeyValidator
    {
        bool EsValida(string apiKey);
    }
}
