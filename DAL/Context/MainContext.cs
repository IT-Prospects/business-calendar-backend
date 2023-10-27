using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL.Context
{
    public static class MainContext
    {
        public static ModelBuilder AddContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("eventset");
                entity.HasKey(e => e.Id).HasName("pk_eventset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.Title).HasColumnName("title").HasColumnType("text").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text").IsRequired();
                entity.Property(e => e.Address).HasColumnName("address").HasColumnType("text").IsRequired();
                entity.Property(e => e.EventDate).HasColumnName("eventdate").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.EventDuration).HasColumnName("eventduration").HasColumnType("interval").IsRequired();

                entity
                    .HasMany(d => d.Images)
                    .WithOne(d => d.Event)
                    .HasForeignKey(d => d.Event_Id)
                    .HasConstraintName("fk_event_image")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("imageset");
                entity.HasKey(e => e.Id).HasName("pk_imageset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasColumnType("text").IsRequired();
                entity.Property(e => e.IsMain).HasColumnName("ismain").HasColumnType("boolean").IsRequired();

                entity.Property(e => e.Event_Id).HasColumnName("event_id").HasColumnType("bigint");

                entity.HasIndex(i => i.Name).IsUnique();
            });

            return modelBuilder;
        }
    }
}
