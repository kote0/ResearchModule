using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Data
{
    public partial class DBContext : DbContext
    {
        private string options = "Data Source=w0044;Initial Catalog=Researches;Integrated Security=True;";

        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<FormWork> FormWork { get; set; }
        public virtual DbSet<TypePublication> TypePublication { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<PA> PA { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.LastName).HasColumnType("nchar(100)");

                entity.Property(e => e.Name).HasColumnType("nchar(100)");

                entity.Property(e => e.Surname).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<FormWork>(entity =>
            {
                entity.Property(e => e.FormName).HasColumnType("nchar(200)");

                entity.Property(e => e.ShortName).HasColumnType("nchar(100)");
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.Language).HasColumnType("nchar(100)");

                entity.Property(e => e.PublicationName)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.TranslateText).HasColumnType("nchar(400)");
                
                entity.HasOne(d => d.FormWork)
                    .WithMany(p => p.Publication)
                    .HasForeignKey(d => d.FormWorkId)
                    .HasConstraintName("FK_Publication_ToFormWork");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Publication)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_Publication_ToSection");

                entity.HasOne(d => d.TypePublication)
                    .WithMany(p => p.Publication)
                    .HasForeignKey(d => d.TypePublicationId)
                    .HasConstraintName("FK_Publication_ToTypePublication");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypePublication>(entity =>
            {
                entity.Property(e => e.TypePublicationName).HasColumnType("nchar(100)");
            });
        }
        
    }
}
