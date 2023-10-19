using Npgsql;

namespace DAL.Common
{
    public static class ContextHelper
    {
        public static string BuildConnectionString(string host, int port, string database, string username, string password)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Database = database,
                Username = username,
                Password = password
            };
            return builder.ToString();
        }
    }
}
