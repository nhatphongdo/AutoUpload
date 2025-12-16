using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AutoUpload.WebService.Models;

namespace AutoUpload.WebService.Migrations
{
    [DbContext(typeof(AutoUploadContext))]
    [Migration("20170304173444_Add Mockup Templates")]
    partial class AddMockupTemplates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutoUpload.WebService.Models.License", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LicenseKey")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<long?>("UserId");

                    b.Property<DateTime>("ValidFrom");

                    b.Property<string>("ValidPlatforms");

                    b.Property<DateTime>("ValidTo");

                    b.HasKey("Id");

                    b.HasIndex("LicenseKey")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasAnnotation("MaxLength", 2048);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<long?>("LicenseId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("LicenseId");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.MockupTemplate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("FilePath");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("ThumbnailPath");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("ValidPlatforms");

                    b.HasKey("Id");

                    b.ToTable("MockupTemplates");
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MachineId");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.License", b =>
                {
                    b.HasOne("AutoUpload.WebService.Models.User", "User")
                        .WithMany("Licenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.Log", b =>
                {
                    b.HasOne("AutoUpload.WebService.Models.License", "License")
                        .WithMany("Logs")
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("AutoUpload.WebService.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
        }
    }
}
