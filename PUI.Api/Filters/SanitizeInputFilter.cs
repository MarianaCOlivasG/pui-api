using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.Reflection;

namespace PUI.Api.Filters
{
    public class SanitizeInputFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null) continue;

                SanitizeObject(arg);
            }
        }

        private void SanitizeObject(object obj)
        {
            if (obj == null) return;

            var type = obj.GetType();

            if (type.IsPrimitive || type.IsEnum || type == typeof(string))
                return;

            if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item != null)
                        SanitizeObject(item);
                }
                return;
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (prop.GetCustomAttribute<Security.SkipSanitizationAttribute>() != null)
                    continue;

                var value = prop.GetValue(obj);
                if (value == null) continue;

                if (prop.PropertyType == typeof(string))
                {
                    var clean = Security.Sanitizer.Clean((string)value);
                    prop.SetValue(obj, clean);
                }
                else if (!prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum)
                {
                    SanitizeObject(value);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}