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
                entity.Property(e => e.EventDate).HasColumnName("eventdate").HasColumnType("timestamp without time zone").IsRequired();
                entity.Property(e => e.EventDuration).HasColumnName("eventduration").HasColumnType("interval").IsRequired();
                entity.Property(e => e.Image_Id).HasColumnName("image_id").HasColumnType("bigint").IsRequired();

                entity
                    .HasOne(d => d.Image)
                    .WithOne()
                    .HasForeignKey<Event>(d => d.Image_Id)
                    .HasConstraintName("fk_event_image");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("imageset");
                entity.HasKey(e => e.Id).HasName("pk_imageset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasColumnType("text").IsRequired();

                entity.HasIndex(i => i.Name).IsUnique();
            });

            return modelBuilder;
        }
    }
}
