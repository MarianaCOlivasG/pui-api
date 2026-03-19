using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PUI.Application.Security;

namespace PUI.Api.Filters
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string HEADER_NAME = "X-API-KEY";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IApiKeyValidator>();

            if (validator == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(HEADER_NAME, out var extractedKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var key = extractedKey.ToString().Trim().Replace("\"", "");

            if (!validator.EsValida(key))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            context.HttpContext.Items["ApiKey"] = key;

            await next();
        }
    }
}