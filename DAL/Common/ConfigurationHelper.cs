using Microsoft.Extensions.Configuration;

namespace DAL.Common
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetString(string key)
        {
            return _configuration[key].ToString();
        }

        public static long GetLong(string key)
        {
            return long.Parse(_configuration[key]);
        }

        public static int GetInt(string key)
        {
            return int.Parse(_configuration[key]);
        }
    }
}
