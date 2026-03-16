using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PUI.Persistence.Converters
{
    public class EncryptStringConverter : ValueConverter<string, string>
    {
        public EncryptStringConverter()
            : base(
                v => Encrypt(v),
                v => Decrypt(v))
        { }

        private static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(value));
        }

        private static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(value));
        }
    }
}
