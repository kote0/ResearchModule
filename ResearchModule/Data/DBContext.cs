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
        
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<PublicationType> TypePublication { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<PA> PA { get; set; }
        public virtual DbSet<PublicationFilter> PublicationFilter { get; set; }
        public virtual DbSet<PF> PF { get; set; }
        public virtual DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(options);
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
        }
        
    }
}
