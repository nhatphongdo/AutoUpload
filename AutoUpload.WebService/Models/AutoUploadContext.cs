using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoUpload.WebService.Models
{
    public class AutoUploadContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<License> Licenses { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<MockupTemplate> MockupTemplates { get; set; }

        public AutoUploadContext(DbContextOptions<AutoUploadContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<License>()
                        .HasOne(l => l.User)
                        .WithMany(u => u.Licenses)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Log>()
                        .HasOne(l => l.User)
                        .WithMany(u => u.Logs)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Log>()
                        .HasOne(l => l.License)
                        .WithMany(l => l.Logs)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                        .HasIndex(b => b.Email)
                        .IsUnique();

            modelBuilder.Entity<License>()
                        .HasIndex(b => b.LicenseKey)
                        .IsUnique();

            modelBuilder.Entity<License>()
                        .HasIndex(b => b.UserId);

            modelBuilder.Entity<Log>()
                        .HasIndex(b => b.Code);

            modelBuilder.Entity<MockupTemplate>()
                        .HasOne(l => l.User)
                        .WithMany(u => u.MockupTemplates)
                        .OnDelete(DeleteBehavior.SetNull);
        }
    }
}