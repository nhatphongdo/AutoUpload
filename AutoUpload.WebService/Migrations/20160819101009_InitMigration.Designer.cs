using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AutoUpload.WebService.Models;

namespace AutoUpload.WebService.Migrations
{
    [DbContext(typeof(AutoUploadContext))]
    [Migration("20160819101009_InitMigration")]
    partial class InitMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
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

                    b.HasIndex("UserId");

                    b.ToTable("Licenses");
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

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AutoUpload.WebService.Models.License", b =>
                {
                    b.HasOne("AutoUpload.WebService.Models.User", "User")
                        .WithMany("Licenses")
                        .HasForeignKey("UserId");
                });
        }
    }
}
