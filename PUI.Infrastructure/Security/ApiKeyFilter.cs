using Microsoft.Extensions.Configuration;
using PUI.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUI.Infrastructure.Security
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _config;

        public ApiKeyValidator(IConfiguration config)
        {
            _config = config;
        }

        public bool EsValida(string apiKey)
        {
            var keys = _config.GetSection("SistemasAutorizados:ApiKeys").Get<List<string>>();

            return keys != null && keys.Contains(apiKey);
        }
    }
}
