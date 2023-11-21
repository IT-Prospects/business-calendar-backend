using DAL.Common;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BusinessCalendar.Helpers
{
    public static class AuthHelper
    {
        public const string Issuer = "BusinessCalendarBackend";
        public const string Audience = "BusinessCalendarFrontend";
        private const string _key = "dNESzWzH9fEwDB2z7RroIrr7TyEkk6CInbmSgko66JAGjkBqFDl6dc8v9KunyA5k78jzjgcgUCYDOFhwMAxJU6gtjlxEZdpZ6HyNUJfene4P8IhKwLCmPDoD9TwHyUWX";
        public const int Lifetime = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        }

        public static void InitConfiguration()
        {
            _accessKey = ConfigurationHelper.GetString("YCAccessKey");
            _secretKey = ConfigurationHelper.GetString("YCSecretKey");
            _serviceURL = ConfigurationHelper.GetString("YCServiceURL");
            _bucketName = ConfigurationHelper.GetString("YCBucketName");
        }
    }
}
