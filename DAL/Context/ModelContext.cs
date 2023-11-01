using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

namespace DAL.Context
{
    public class ModelContext : DbContext
    {
        private readonly string _connectionString;

        public ModelContext(string connectionString) : base()
        {
            this._connectionString = connectionString;
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
            _connectionString = string.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseNpgsql(_connectionString);
#if DEBUG
            optionsBuilder.LogTo(x => System.Diagnostics.Debug.Write(x));
#endif
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
                throw new ApplicationException("Error clearing context.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddContext();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Event> EventSet { get; set; }
        public DbSet<Image> ImageSet { get; set; }
        public DbSet<EventSignUp> EventSignUpSet { get; set; }
    }
}
