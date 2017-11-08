using Microsoft.EntityFrameworkCore;
using ResearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Data
{
    public class DBContext : DbContext
    {
        private string options = "Data Source=w0044;Initial Catalog=Researches;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.SectionName).HasMaxLength(50);
            });
        }
        public DbSet<Section> Section { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<FormWork> FormWork { get; set; }
        public DbSet<TypePublication> TypePublication { get; set; }
        public DbSet<Publication> Publication { get; set; }
    }
}
