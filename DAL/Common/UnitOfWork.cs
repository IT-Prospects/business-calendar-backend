using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL.Common
{
    public class UnitOfWork
    {
        private ModelContext _context;

        public UnitOfWork() : this(
            ConfigurationHelper.GetString("serverName"),
            ConfigurationHelper.GetInt("serverPort"),
            ConfigurationHelper.GetString("databaseName"),
            ConfigurationHelper.GetString("dbAdminLogin"),
            ConfigurationHelper.GetString("dbAdminPassword")
            ) 
        { }

        public UnitOfWork(string connectionString)
        {
            _context = GetMainContext(connectionString);
        }

        public UnitOfWork(string connectionString, int commandTimeout)
        {
            _context = GetMainContext(connectionString, commandTimeout);
        }

        public UnitOfWork(string host, int port, string database, string username, string password)
        {
            var connectionString = ContextHelper.BuildConnectionString(host, port, database, username, password);
            _context = GetMainContext(connectionString);
        }

        private ModelContext GetMainContext(string connectionString, int commandTimeout = 60)
        {
            var context = new ModelContext(connectionString, commandTimeout);
            return context;
        }

        public ModelContext Context() => _context;

        public DbSet<T> DbSet<T>() where T : DomainObject
        {
            return _context.Set<T>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.ClearEntities();
                _context.Dispose();
                _context = null;
            }
        }
    }
}
