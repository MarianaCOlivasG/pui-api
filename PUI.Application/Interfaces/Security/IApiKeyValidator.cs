namespace PUI.Application.Interfaces.Security
{
    public interface IApiKeyValidator
    {
        bool EsValida(string apiKey);
    }
}
