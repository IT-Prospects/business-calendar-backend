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
                entity.Property(e => e.ArchivePassword).HasColumnName("archivepassword").HasColumnType("text").IsRequired();
                entity.Property(e => e.Image_Id).HasColumnName("image_id").HasColumnType("bigint").IsRequired();

                entity
                    .HasOne(d => d.Image)
                    .WithOne()
                    .HasForeignKey<Event>(d => d.Image_Id)
                    .HasConstraintName("fk_event_image")
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasMany(d => d.SubImages)
                    .WithOne(d => d.Event)
                    .HasForeignKey(d => d.Event_Id)
                    .HasConstraintName("fk_image_event")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("imageset");
                entity.HasKey(e => e.Id).HasName("pk_imageset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.URL).HasColumnName("url").HasColumnType("text").IsRequired();
                entity.Property(e => e.Event_Id).HasColumnName("event_id").HasColumnType("bigint");

                entity.HasIndex(i => i.URL).IsUnique();
            });

            modelBuilder.Entity<EventSignUp>(entity =>
            {
                entity.ToTable("eventsignupset");
                entity.HasKey(e => e.Id).HasName("pk_eventsignupset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).HasColumnName("firstname").HasColumnType("text").IsRequired();
                entity.Property(e => e.LastName).HasColumnName("lastname").HasColumnType("text").IsRequired();
                entity.Property(e => e.Patronymic).HasColumnName("patronymic").HasColumnType("text");
                entity.Property(e => e.Email).HasColumnName("email").HasColumnType("text").IsRequired();
                entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber").HasColumnType("text").IsRequired();

                entity.Property(e => e.Event_Id).HasColumnName("event_id").HasColumnType("bigint");

                entity
                    .HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.Event_Id)
                    .HasConstraintName("fk_eventsignup_event")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("userset");
                entity.HasKey(e => e.Id).HasName("pk_userset");
                entity.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).HasColumnName("firstname").HasColumnType("text").IsRequired();
                entity.Property(e => e.LastName).HasColumnName("lastname").HasColumnType("text").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasColumnType("text").IsRequired();
                entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber").HasColumnType("text").IsRequired();
                entity.Property(e => e.Password).HasColumnName("password").HasColumnType("text").IsRequired();
                entity.Property(e => e.RefreshToken).HasColumnName("refreshtoken").HasColumnType("text").IsRequired(false);
            });

            return modelBuilder;
        }
    }
}
