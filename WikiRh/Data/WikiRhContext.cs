﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WikiRh.Models;

namespace WikiRh.Data
{
    public class WikiRhContext : DbContext
    {
         public WikiRhContext(DbContextOptions<WikiRhContext> options) : base(options)
        {
        }
        public DbSet<WikiRh_Question> Questions { get; set; }
        public DbSet<WikiRh_Category> Categories { get; set; }
        public DbSet<WikiRh_File> Files { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WikiRh_Question>().ToTable("WikiRh_Question").HasKey(d => d.Id_quest);
            modelBuilder.Entity<WikiRh_Category>().ToTable("WikiRh_Category").HasKey(p => p.Id_cat);
            modelBuilder.Entity<WikiRh_File>().ToTable("WikiRh_File").HasKey(p => p.Id_file);

            modelBuilder.Entity<WikiRh_Question>()
                .HasOne(p => p.Categories)
                .WithMany(b => b.Questions)
                .HasForeignKey(p => p.Id_cat)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WikiRh_File>()
               .HasOne(p => p.Questions)
               .WithMany(b => b.Files)
               .HasForeignKey(p => p.Id_quest)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

