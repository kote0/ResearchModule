using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

            modelBuilder.Entity<PublicationType>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<PA>()
                .HasKey(t => new { t.AuthorId, t.PublicationId });

            modelBuilder.Entity<PF>()
                .HasKey(t => new { t.PublicationFilterId, t.PublicationId });

            modelBuilder.Entity<PA>()
               .HasOne(p => p.Publication)
               .WithMany(i => i.PAs)
               .HasForeignKey(pt=>pt.PublicationId)
               .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<PA>()
              .HasOne(p => p.Author)
              .WithMany(i => i.PAs)
              .HasForeignKey(pt => pt.AuthorId)
              .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<PF>()
               .HasOne(p => p.Publication)
               .WithMany(i => i.PFs)
               .HasForeignKey(pt => pt.PublicationId);

            modelBuilder.Entity<PF>()
              .HasOne(p => p.PublicationFilter)
              .WithMany(i => i.PFs)
              .HasForeignKey(pt => pt.PublicationFilterId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<PublicationType> TypePublication { get; set; }
        public DbSet<Publication> Publication { get; set; }
        public DbSet<PA> PA { get; set; }
        public DbSet<PublicationFilter> PublicationFilter { get; set; }
        public DbSet<PF> PF { get; set; }
        //public virtual DbSet<User> User { get; set; }
    }
}
