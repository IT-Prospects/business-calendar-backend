﻿// <auto-generated />
using System;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _2DAL.Migrations
{
    [DbContext(typeof(ModelContext))]
    partial class ModelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Model.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<string>("ArchivePassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("archivepassword");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("eventdate");

                    b.Property<TimeSpan>("EventDuration")
                        .HasColumnType("interval")
                        .HasColumnName("eventduration");

                    b.Property<long>("Image_Id")
                        .HasColumnType("bigint")
                        .HasColumnName("image_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_eventset");

                    b.HasIndex("Image_Id")
                        .IsUnique();

                    b.ToTable("eventset", (string)null);
                });

            modelBuilder.Entity("Model.EventSignUp", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<long>("Event_Id")
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text")
                        .HasColumnName("patronymic");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.HasKey("Id")
                        .HasName("pk_eventsignupset");

                    b.HasIndex("Event_Id");

                    b.ToTable("eventsignupset", (string)null);
                });

            modelBuilder.Entity("Model.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("Event_Id")
                        .HasColumnType("bigint")
                        .HasColumnName("event_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_imageset");

                    b.HasIndex("Event_Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("imageset", (string)null);
                });

            modelBuilder.Entity("Model.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refreshtoken");

                    b.HasKey("Id")
                        .HasName("pk_userset");

                    b.ToTable("userset", (string)null);
                });

            modelBuilder.Entity("Model.Event", b =>
                {
                    b.HasOne("Model.Image", "Image")
                        .WithOne()
                        .HasForeignKey("Model.Event", "Image_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_event_image");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Model.EventSignUp", b =>
                {
                    b.HasOne("Model.Event", "Event")
                        .WithMany()
                        .HasForeignKey("Event_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_eventsignup_event");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Model.Image", b =>
                {
                    b.HasOne("Model.Event", "Event")
                        .WithMany("SubImages")
                        .HasForeignKey("Event_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_image_event");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Model.Event", b =>
                {
                    b.Navigation("SubImages");
                });
#pragma warning restore 612, 618
        }
    }
}
