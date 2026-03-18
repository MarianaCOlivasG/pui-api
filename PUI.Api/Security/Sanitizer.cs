using System.Text.RegularExpressions;

namespace PUI.Api.Security
{
    public static class Sanitizer
    {
        private static readonly Regex DangerousChars =
            new(@"[<>%""'/]", RegexOptions.Compiled);

        public static string Clean(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return DangerousChars.Replace(input, string.Empty);
        }
    }
}