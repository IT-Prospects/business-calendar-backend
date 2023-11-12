using Microsoft.Extensions.Configuration;

namespace DAL.Common
{
    public static class ConfigurationHelper
    {
        private static IConfiguration? _configuration;
        private static readonly string _configNotInitialized = "The configuration is not initialized";
        private static readonly string _notFountMessage = "The \"{0}\" value was not found in the application configuration";
        private static readonly string _invalidFormatMessage = "An error occurred while trying to convert the value \"{0}\"";
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static void CheckConfiguration()
        {
            if (_configuration == null)
                throw new ApplicationException(_configNotInitialized);
        }

        public static string GetString(string key)
        {
            CheckConfiguration();

            var value = _configuration![key];
            return value ?? throw new ArgumentException(string.Format(_notFountMessage, key));
        }

        public static long GetLong(string key)
        {
            CheckConfiguration();

            var value = _configuration![key] ?? throw new ArgumentException(string.Format(_notFountMessage, key));
            if (!long.TryParse(value, out long parsedValue))
                throw new FormatException(string.Format(_invalidFormatMessage, key));

            return parsedValue;
        }

        public static int GetInt(string key)
        {
            CheckConfiguration();

            var value = _configuration![key] ?? throw new ArgumentException(string.Format(_notFountMessage, key));
            if (!int.TryParse(value, out int parsedValue))
                throw new FormatException(string.Format(_invalidFormatMessage, key));

            return parsedValue;
        }
    }
}
