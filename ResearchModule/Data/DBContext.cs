using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ResearchModule.Components.Models;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Data
{
    public partial class DBContext : IdentityDbContext<User> //: DbContext
    {
        // Add-Migration InitialCreate
        // Update-Database
        // Remove-Migration
        private string option = "Data Source=w0044;Initial Catalog=Researches;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(option);
        }

        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Lastname).HasColumnType("nchar(100)");

                entity.Property(e => e.Name).HasColumnType("nchar(100)");

                entity.Property(e => e.Surname).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<User>()
                .HasOne(a => a.Author)
                .WithOne(u => u.User)
                .HasPrincipalKey<User>(u=>u.UserName);

            modelBuilder.Entity<PublicationType>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<PublicationType>()
                .HasMany(p => p.Publications)
                .WithOne(t => t.PublicationType)
                .HasForeignKey(p=>p.PublicationTypeId);

            modelBuilder.Entity<PA>()
                .HasKey(t => new { t.MultipleId, t.PublicationId });

            modelBuilder.Entity<PF>()
                .HasKey(t => new { t.MultipleId, t.PublicationId });

            modelBuilder.Entity<PA>()
               .HasOne(p => p.Publication)
               .WithMany(i => i.PAs)
               .HasForeignKey(pt => pt.PublicationId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PA>()
              .HasOne(p => p.Multiple)
              .WithMany(i => i.PAs)
              .HasForeignKey(pt => pt.MultipleId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PF>()
               .HasOne(p => p.Publication)
               .WithMany(i => i.PFs)
               .HasForeignKey(pt => pt.PublicationId);

            modelBuilder.Entity<PF>()
              .HasOne(p => p.Multiple)
              .WithMany(i => i.PFs)
              .HasForeignKey(pt => pt.MultipleId);

            modelBuilder.Entity<FileDetail>()
                .HasOne(p => p.Publication)
                .WithOne(o => o.PublicationFile)
                .HasForeignKey<Publication>(b=>b.PublicationFileId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<PublicationType> TypePublication { get; set; }
        public DbSet<Publication> Publication { get; set; }
        public DbSet<PA> PA { get; set; }
        public DbSet<PublicationFilters> PublicationFilter { get; set; }
        public DbSet<PF> PF { get; set; }
        public DbSet<FileDetail> FileDetail { get; set; }
    }
}
