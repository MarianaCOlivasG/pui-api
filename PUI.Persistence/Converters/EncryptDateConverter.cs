using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace PUI.Persistence.Converters
{
    public class EncryptDateConverter : ValueConverter<DateTime?, string?>
    {
        public EncryptDateConverter()
            : base(
                v => EncryptDate(v),
                v => DecryptDate(v))
        {
        }

        private static string? EncryptDate(DateTime? value)
        {
            if (!value.HasValue)
                return null;

            var iso = value.Value.ToString("O", CultureInfo.InvariantCulture);

            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(iso));
        }

        private static DateTime? DecryptDate(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var iso = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(value));

            return DateTime.Parse(
                iso,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);
        }
    }
}