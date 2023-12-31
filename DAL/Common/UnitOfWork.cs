﻿using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL.Common
{
    public class UnitOfWork
    {
        private ModelContext _context;

        public UnitOfWork() : this(
            ConfigurationHelper.GetString("dbServerHost"),
            ConfigurationHelper.GetInt("dbServerPort"),
            ConfigurationHelper.GetString("dbName"),
            ConfigurationHelper.GetString("dbAdminLogin"),
            ConfigurationHelper.GetString("dbAdminPassword")
            ) 
        { }

        public UnitOfWork(string connectionString)
        {
            _context = GetMainContext(connectionString);
        }

        public UnitOfWork(string host, int port, string database, string username, string password)
        {
            var connectionString = ContextHelper.BuildConnectionString(host, port, database, username, password);
            _context = GetMainContext(connectionString);
        }

        private ModelContext GetMainContext(string connectionString)
        {
            var context = new ModelContext(connectionString);
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
            }
        }
    }
}
