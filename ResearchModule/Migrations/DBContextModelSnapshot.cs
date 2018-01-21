﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ResearchModule.Data;
using System;

namespace ResearchModule.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ResearchModule.Models.Author", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CoAuthor");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("ResearchModule.Models.FormWork", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FormName")
                        .IsRequired();

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.ToTable("FormWork");
                });

            modelBuilder.Entity("ResearchModule.Models.PA", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AId");

                    b.Property<long>("PId");

                    b.HasKey("Id");

                    b.ToTable("PA");
                });

            modelBuilder.Entity("ResearchModule.Models.Publication", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AuthorId");

                    b.Property<long>("FormWorkId");

                    b.Property<bool?>("IsTranslate");

                    b.Property<string>("Language");

                    b.Property<string>("PublicationName")
                        .IsRequired();

                    b.Property<long>("SectionId");

                    b.Property<string>("TranslateText");

                    b.Property<long>("TypePublicationId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("FormWorkId");

                    b.HasIndex("SectionId");

                    b.HasIndex("TypePublicationId");

                    b.ToTable("Publication");
                });

            modelBuilder.Entity("ResearchModule.Models.Section", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("SectionName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Section");
                });

            modelBuilder.Entity("ResearchModule.Models.TypePublication", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TypePublicationName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TypePublication");
                });

            modelBuilder.Entity("ResearchModule.Models.Publication", b =>
                {
                    b.HasOne("ResearchModule.Models.Author")
                        .WithMany("Publication")
                        .HasForeignKey("AuthorId");

                    b.HasOne("ResearchModule.Models.FormWork", "FormWork")
                        .WithMany("Publication")
                        .HasForeignKey("FormWorkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ResearchModule.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ResearchModule.Models.TypePublication", "TypePublication")
                        .WithMany("Publication")
                        .HasForeignKey("TypePublicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}