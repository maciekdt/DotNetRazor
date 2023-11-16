using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationList13X.Models;

namespace WebApplicationList13X.Data
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles);

            modelBuilder.Seed();
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<Category> Category { get; set; }
    }

    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "Fruits"
            },
            new Category()
            {
                Id = 2,
                Name = "Candy"
            },
            new Category()
            {
                Id = 3,
                Name = "Vegatables"
            }
            );
        }
    }
}
