using System;

namespace PUI.Api.Security
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipSanitizationAttribute : Attribute
    {
    }
}