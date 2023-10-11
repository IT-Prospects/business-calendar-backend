using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

namespace DAL.Context
{
    public class ModelContext : DbContext
    {
        protected string connectionString;
        protected int commandTimeout;

        public ModelContext(string connectionString, int commandTimeout) : base()
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            if (!OptionsBuilder.IsConfigured)
            {
                OptionsBuilder.UseNpgsql(connectionString,
                    x => x.CommandTimeout(commandTimeout));
#if DEBUG
                OptionsBuilder.LogTo(x => System.Diagnostics.Debug.Write(x));
#endif
            }
        }

        public IEnumerable<EntityEntry> GetAllObjectStateEntries()
        {
            return ChangeTracker.Entries().Where(s => s.State == EntityState.Added
             || s.State == EntityState.Deleted
             || s.State == EntityState.Modified
             || s.State == EntityState.Unchanged)
                 .ToList();
        }

        public void ClearEntities()
        {
            try
            {
                foreach (var objectStateEntry in GetAllObjectStateEntries())
                {
                    try
                    {

                        objectStateEntry.State = EntityState.Detached;
                    }
                    catch
                    {
                    }
                }
                ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка при очистке контекста.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddContext();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Event> EventSet { get; set; }
        public DbSet<Image> ImageSet { get; set; }
    }
}
